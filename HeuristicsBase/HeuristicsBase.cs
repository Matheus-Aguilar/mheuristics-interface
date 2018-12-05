using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heuristics
{
    public class HeuristicsBase
    {
        public List<Tuple<double, double, double, double, double, double>> Iteracoes = new List<Tuple<double, double, double, double, double, double>>();
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
        /// variação permitida do volume por ano, para cima
        /// </summary>
        public static double alfaRegVol = 0.20;
        [JsonProperty]
        /// <summary>
        /// variação permtida do volume por ano, para baixo
        /// </summary>
        public static double betaRegVol = 0.20;
        [JsonProperty]
        /// <summary>
        /// volume médio dos talhões
        /// </summary>
        public static double volumeMedio = 2559.789;
        [JsonProperty]
        /// <summary>
        /// penalidade de volume do ano 0, calculada com base no volume médio
        /// </summary>
        public static double penalidadeInicial = 0;
        [JsonProperty]
        /// <summary>
        /// área total da fazenda, em hectares
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
        /// coeficiente da restrição de adjacencia
        /// </summary>
        public static int greenUp = 1;

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
        /// matriz de custos de colheita
        /// </summary>
        public static double[,] mColheita;
        [JsonProperty]
        /// <summary>
        /// matriz de custos de baldeio
        /// </summary>
        public static double[,] mBaldeio;
        [JsonProperty]
        /// <summary>
        /// matriz de custos de transporte
        /// </summary>
        public static double[,] mTransporte;
        [JsonProperty]
        /// <summary>
        /// matriz de custos de silvicultura
        /// </summary>
        public static double[,] mSilvicultura;
        [JsonProperty]
        /// <summary>
        /// matriz de custos de implantação
        /// </summary>
        public static double[,] mImplantacao;
        [JsonProperty]
        /// <summary>
        /// matriz de custos anteriores
        /// </summary>
        public static double[,] mAnteriores;
        [JsonProperty]
        /// <summary>
        /// matriz de custos médios de produção
        /// </summary>
        public static double[,] mCustosMedios;
        [JsonProperty]
        /// <summary>
        /// matriz de receitas
        /// </summary>
        public static double[,] mReceita;
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
        /// <summary>
        /// valor final da função objetivo
        /// </summary>
        public static double fObjetivoFinal;
        [JsonProperty]
        /// <summary>
        /// valor final da restrição de adjacência
        /// </summary>
        public static double rAdjFinal;
        [JsonProperty]
        /// <summary>
        /// valor final da restrição de área
        /// </summary>
        public static double rAreaFinal;
        [JsonProperty]
        /// <summary>
        /// valor final da restrição de volume
        /// </summary>
        public static double rVolumeFinal;

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
        [JsonProperty]
        /// <summary>
        /// booleano que indica se a função objetivo deve maximizar ou minimizar o resultado
        /// </summary>
        public static bool minimizar;

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
        public Tuple<double, double, double, double, double, double> avaliar(int[] solucao, int n = 0)
        {
            if (n == 0)
                n = HeuristicsBase.n;

            Task<double>[] funcoes = new Task<double>[4];

            funcoes[0] = new Task<double>(() =>
            {
                double funcaoObjetivo = 0;

                if(!minimizar)
                    for (int i = 0; i < n; i++)
                        funcaoObjetivo += mVPL[i, solucao[i]];
                else
                    for (int i = 0; i < n; i++)
                        funcaoObjetivo += mCustosMedios[i, solucao[i]];

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

                double volumeAnterior = 0;
                
                //Calculando o volume do ano zero
                for (int i = 0; i < n; i++)
                    if (mCorte[i, solucao[i], 0])
                        volumeAnterior += mVolume[i, solucao[i], 0];

                //Calculando a penalidade do primeiro ano, com base na média dos volumes
                if (volumeAnterior <= (1 - alfaRegVol) * volumeMedio)
                    restricaoVolume += (((1 - alfaRegVol) * volumeMedio) - volumeAnterior);
                else if (volumeAnterior >= (1 + betaRegVol) * volumeMedio)
                    restricaoVolume += (volumeAnterior - ((1 + betaRegVol) * volumeMedio));

                HeuristicsBase.penalidadeInicial = restricaoVolume;

                for (int k=1; k < h-1; k++)
                {
                    double volume = 0;

                    for (int i=0; i < n; i++)
                        if (mCorte[i, solucao[i], k])
                            volume += mVolume[i, solucao[i], k];

                    if (volume <= (1 - alfaRegVol) * volumeAnterior)
                        restricaoVolume += (((1 - alfaRegVol) * volumeAnterior) - volume);
                    else if(volume >= (1 + betaRegVol) * volumeAnterior)
                        restricaoVolume += (volume-((1 + betaRegVol) * volumeAnterior));

                    volumeAnterior = volume;
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
                                for(int j = 0; j < greenUp; j++)
                                    foreach (int vizinho in talhoes[i].vizinhos)
                                        if (mCorte[vizinho, solucao[vizinho], (k + j) % h])
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
                                areasCortadas += talhoes[i].area; // * talhoes[i].area;

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

            double funcaoAvalicao;

            if(!minimizar)
                funcaoAvalicao = funcoes[0].Result
                    - funcoes[1].Result * funcoes[1].Result * alfa
                    - funcoes[2].Result * funcoes[2].Result * beta
                    - funcoes[3].Result * funcoes[3].Result * gama;
            else
                funcaoAvalicao = funcoes[0].Result
                    + funcoes[1].Result * funcoes[1].Result * alfa
                    + funcoes[2].Result * funcoes[2].Result * beta
                    + funcoes[3].Result * funcoes[3].Result * gama;

            return Tuple.Create<double, double, double, double, double, double>
                (funcaoAvalicao, funcoes[0].Result, funcoes[1].Result, funcoes[2].Result, funcoes[3].Result, HeuristicsBase.penalidadeInicial);
        }

        public int[] geraSolucaoAleatoria()
        {
            int[] solucao = new int[n];

                do
                    solucao = solucao.Select(_ => rand.Next(nAf)).ToArray();
                while (avaliar(solucao).Item1 < 0);

            return solucao;
        }

        /// <summary>
        /// Gera solução gulosa, por tipo (1: cardinalidade, 2: valor) e com aleatoriedade(0 -> menos aleatório)
        /// </summary>
        /// <param name="tipo">1: cardinalidade, 2: valor</param>
        /// <param name="alfa">Aleatoriedade</param>
        /// <returns></returns>
        public int[] geraSolucaoGulosa(int tipo, double alfa)
        {
            int[] sol = new int[n];

            for (int i = 0; i < n; i++)
            {
                var rcl = new List<Tuple<int, double>>();

                for (int j = 0; j < nAf; j++)
                {
                    sol[i] = j;

                    var fo = avaliar(sol, i + 1).Item1;

                    rcl.Add(new Tuple<int, double>(j, fo));
                }

                if (tipo == 1)
                {
                    if(!minimizar)
                        rcl = rcl.OrderBy(p => -p.Item2).ToList();
                    else
                        rcl = rcl.OrderBy(p => p.Item2).ToList();

                    sol[i] = rcl[rand.Next(Convert.ToInt32(alfa * rcl.Count)) + 1].Item1;
                }
                else
                {
                    var cMin = rcl.Min(p => p.Item2);
                    var cMax = rcl.Max(p => p.Item2);
                    
                    if (!minimizar)
                    {
                        double limite = cMax - alfa * Math.Abs(cMax - cMin);
                        rcl = rcl.Where(p => p.Item2 > limite).ToList();
                    }
                    else
                    {
                        double limite = cMin + alfa * Math.Abs(cMax - cMin);
                        rcl = rcl.Where(p => p.Item2 < limite).ToList();
                    }

                    sol[i] = rcl[rand.Next(rcl.Count)].Item1;
                }
            }

            return sol;
        }

        public virtual void Run()
        {

        }
    }
}
