using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heuristics
{
    public class HeuristicsBase
    {
        public List<Tuple<double, double, double, double, double>> Iteracoes = new List<Tuple<double, double, double, double, double>>();
        public int[] solucao;

        #region var_declare
        [JsonProperty]
        /// <summary>
        /// número de talhões
        /// </summary>
        public static int n;
        [JsonProperty]
        /// <summary>
        /// não sei o que essa variável faz
        /// </summary>
        public static int nAf;
        [JsonProperty]
        /// <summary>
        /// número de talhões cortados por autofuste e talhadia
        /// </summary>
        public static int nTa;
        [JsonProperty]
        /// <summary>
        /// número de prescrições
        /// </summary>
        public static int m;
        [JsonProperty]
        /// <summary>
        /// horizonte de planejamento -- h = 3 * r
        /// </summary>
        public static int h;
        [JsonProperty]
        /// <summary>
        /// idade regulatória
        /// </summary>
        public static int r;

        [JsonProperty]
        /// <summary>
        /// volume mínimo
        /// </summary>
        public static double volMin = 100000;
        [JsonProperty]
        /// <summary>
        /// volume máximo
        /// </summary>
        public static double volMax = 300000;
        [JsonProperty]
        /// <summary>
        /// variação permitida da regulação de área
        /// </summary>
        public static double alfaRegArea = 0.20;
        [JsonProperty]
        /// <summary>
        /// área total da fazenda em hectaes
        /// </summary>
        public static double areaTotal = 1892.08;
        [JsonProperty]
        /// <summary>
        /// coeficiente da restrição de volume
        /// </summary>
        public static double alfa = 0.00001;
        [JsonProperty]
        /// <summary>
        /// coeficiente da restrição de adjacencia
        /// </summary>
        public static double beta = 0.001;
        [JsonProperty]
        /// <summary>
        /// coeficiente da restrição de regulação de área
        /// </summary>
        public static double gama = 0.1; // coeficientes

        [JsonProperty]
        /// <summary>
        /// vetor de talhões
        /// </summary>
        public static Talhao[] talhoes;
        [JsonProperty]
        /// <summary>
        /// matriz de VPLs
        /// </summary>
        public static double[,] mVPL;
        [JsonProperty]
        /// <summary>
        /// matriz que indica se o talhão i sobre a prescrição j será cortado no ano k
        /// </summary>
        public static bool[,,] mCorte;
        [JsonProperty]
        /// <summary>
        /// matriz que indica o volume do talhão i sobre a prescrição j no ano k
        /// </summary>
        public static double[,,] mVolume;
        [JsonProperty]
        /// <summary>
        /// matriz que indica se o talhão i é adjacente ao talhão j
        /// </summary>
        public static bool[,] mAdj;
        [JsonProperty]
        /// <summary>
        /// matriz que indica a distância do talhão i ao talhão j
        /// </summary>
        public static double[,] mDistancia;
        [JsonProperty]
        /// <summary>
        /// matriz que indica a área do talhao i na prescricao j com a idade regulatória k
        /// </summary>
        public static double[,,] mRegArea;

        [JsonProperty]
        public static double areaPorR;
        [JsonProperty]
        public static double areaPorR_maisAlfaReg;
        [JsonProperty]
        public static double areaPorR_menosAlfaReg;

        [JsonProperty]
        /// <summary>
        /// booleano que indica se a função de avaliação leva em consideração area de adjacencia ou IAC
        /// </summary>
        public static bool areaAdjacencia;

        public static Random rand;
        #endregion

        public HeuristicsBase()
        {
            rand = new Random();
        }

        /// <summary>
        /// Avalia uma solução
        /// <para>Retorno:</para>
        /// <para>Item1: Função de avaliação,</para>
        /// <para>Item2: Função objetivo,</para>
        /// <para>Item3: restricao de volume,</para>
        /// <para>Item4: area de quebra de adjacência,</para>
        /// <para>Item5: restrição de área de regulação.</para>
        /// </summary>
        /// <param name="solucao">Vetor contendo a prescrição para cada talhão</param>
        /// <returns>
        /// </returns>
        public Tuple<double, double, double, double, double> avaliar(int[] solucao)
        {
            Task<double>[] funcoes = new Task<double>[4];

            funcoes[0] = new Task<double>(() =>
            {
                double funcaoObjetivo = 0;

                for (int i = 0; i < n; i++)
                    funcaoObjetivo += mVPL[i, solucao[i]];

                return funcaoObjetivo;
            });

            // Calcula restrição de volume
            // O(h*n) ~ O(r*n)

            funcoes[1] = new Task<double>(() =>
            {
                double restricaoVolume = 0;

                for (int k = 0; k < h; k++)
                {
                    double volume = 0;

                    for (int i = 0; i < n; i++)
                        if (mCorte[i, solucao[i], k])
                            volume += mVolume[i, solucao[i], k];

                    if (volume < volMin)
                        restricaoVolume += (volMin - volume);
                    else if (volume > volMax)
                        restricaoVolume += (volume - volMax);
                }

                return restricaoVolume;
            });

            // Calcula restrição de adjacência
            // O(h * n^2) ~ O(r*n^2)

            funcoes[2] = new Task<double>(() =>
            {
                if (areaAdjacencia)
                {
                    double areaQuebra = 0;

                    for (int i = 0; i < n; i++)
                        for (int k = 0; k < h; k++)
                            if (mCorte[i, solucao[i], k])
                                foreach (int vizinho in talhoes[i].vizinhos)
                                    if (mCorte[vizinho, solucao[vizinho], k])
                                    {
                                        areaQuebra += talhoes[i].area;

                                        break;
                                    }

                    return areaQuebra;
                }
                else
                {
                    double IAC = 0;
                    double area2;
                    double distMaisProximo;
                    double areasCortadas = 0;

                    for (int i = 0; i < h; i++)
                        for (int j = 0; j < n; j++)
                            if (mCorte[j, solucao[j], i])
                            {
                                areasCortadas += talhoes[i].area * talhoes[i].area;

                                area2 = talhoes[j].area * talhoes[j].area;

                                distMaisProximo = Double.PositiveInfinity;

                                for(int k = 0; k < n; k++)
                                    if (k != j && mCorte[k, solucao[k], i])
                                        distMaisProximo = Math.Min(distMaisProximo, mDistancia[j, k]);

                                distMaisProximo /= 1000;

                                IAC += area2 / (distMaisProximo * distMaisProximo);
                            }
                    
                    return IAC / areasCortadas;
                }
            });

            // Calcula restrição de area de regulamentação
            // O(r*n)

            funcoes[3] = new Task<double>(() =>
            {
                double restricaoRegArea = 0;

                for (int k = 0; k < r; k++)
                {
                    double area = 0;

                    for (int i = 0; i < n; i++)
                        area += mRegArea[i, solucao[i], k];

                    if (area > areaPorR_maisAlfaReg)
                        restricaoRegArea += area - areaPorR_maisAlfaReg;
                    else if (area < areaPorR_menosAlfaReg)
                        restricaoRegArea += areaPorR_menosAlfaReg - area;
                }

                return restricaoRegArea;
            });

            foreach (var task in funcoes)
                task.Start();

            Task.WaitAll(funcoes);

            double funcaoAvalicao = funcoes[0].Result
                - funcoes[1].Result * funcoes[1].Result * alfa
                - funcoes[0].Result * funcoes[2].Result * beta
                - funcoes[3].Result * funcoes[3].Result * gama;

            return Tuple.Create<double, double, double, double, double>
                (funcaoAvalicao, funcoes[0].Result, funcoes[1].Result, funcoes[2].Result, funcoes[3].Result);
        }

        public int[] geraSolucaoAleatoria()
        {
            int[] solucao = new int[n];

            do
            {
                for (int i = 0; i < n; i++)
                    solucao[i] = rand.Next(0, nAf);
            } while (avaliar(solucao).Item1 < 0);

            return solucao;
        }

        public virtual void Run()
        {

        }
    }
}
