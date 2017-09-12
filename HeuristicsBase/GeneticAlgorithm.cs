using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heuristics
{
    public class GeneticAlgorithm : HeuristicsBase
    {
        int populacaoInicial;
        double taxaCruzamento;
        double taxaMutacao;
        int numIteracoes;
        int contIteracoes;

        public GeneticAlgorithm(int populacaoInicial = 20, double taxaCruzamento = 0.5, double taxaMutacao = 0.05, int numIteracoes = 200)
        {
            this.populacaoInicial = populacaoInicial;
            this.taxaCruzamento = taxaCruzamento;
            this.taxaMutacao = taxaMutacao;
            this.numIteracoes = numIteracoes;
            this.contIteracoes = 0;
        }

        private double[] gerarProbabilidadesIniciais(int[][] solucoesIniciais)
        {
            double soma = 0;
            double[] probSolucoes = new double[n];

            for (int i = 0; i < populacaoInicial; i++)
            {
                probSolucoes[i] = avaliar(solucoesIniciais[i]).Item1;
                soma += probSolucoes[i];
            }

            for (int i = 0; i < populacaoInicial; i++)
                probSolucoes[i] /= soma;

            return probSolucoes;
        }

        private void mutacao(int[] solucao)
        {
            int aux = Convert.ToInt32(n * taxaMutacao);
            int[] flag = new int[aux];
            int cont = 0;

            for (int i = 0; i < aux; i++)
            {
                int rndPosicao;
                bool l = false;
                do
                {
                    l = false;
                    rndPosicao = rand.Next(0, n); // vai 0 ate n-1
                    for (int j = 0; j < cont; j++)
                    {
                        if (rndPosicao == flag[j])
                        {
                            l = true;
                            break;
                        }
                    }
                } while (l);
                flag[cont] = rndPosicao;
                cont++;

                int prescAntigo = solucao[rndPosicao];

                int rndPresc;
                do
                {
                    rndPresc = rand.Next(0, m);  // seleciona de forma randomica
                } while (rndPresc == solucao[rndPosicao]);

                solucao[rndPosicao] = rndPresc;
            }
        }

        private void crossover(int[][] solucoesIniciais, int i1, int i2)
        {
            int[] filho1 = new int[n], filho2 = new int[n];

            int i;

            for (i = 0; i < (int)n * taxaCruzamento; i++)
            {
                filho1[i] = solucoesIniciais[i1][i];
                filho2[i] = solucoesIniciais[i2][i];
            }

            for (; i < n; i++)
            {
                filho1[i] = solucoesIniciais[i2][i];
                filho2[i] = solucoesIniciais[i1][i];
            }

            mutacao(filho1);
            mutacao(filho2);

            List<int[]> solucoes = new List<int[]>();

            solucoes.Add(solucoesIniciais[i1]);
            solucoes.Add(solucoesIniciais[i2]);
            solucoes.Add(filho1);
            solucoes.Add(filho2);

            //Task<double>[] tasks = new Task<double>[4];

            Task<double>[] tasks = solucoes.Select(p => new Task<double>(() => avaliar(p).Item1)).ToArray();

            foreach (Task task in tasks)
                task.Start();

            Task.WaitAll(tasks);

            int melhorPai = tasks[0].Result > tasks[1].Result ? 0 : 1;
            int melhorFilho = tasks[2].Result > tasks[3].Result ? 2 : 3;
            
            if (tasks[melhorFilho].Result > tasks[melhorPai].Result)
            {
                Iteracoes.Add(avaliar(solucoes[melhorFilho]));
                
                tasks = solucoesIniciais.Select(p => new Task<double>(() => avaliar(p).Item1)).ToArray();

                foreach (Task task in tasks)
                    task.Start();

                Task.WaitAll(tasks);

                int min = 0;

                for(int k = 1; k < tasks.Length; k++)
                    if (tasks[k].Result < tasks[min].Result)
                        min = k;

                solucoesIniciais[min] = solucoes[melhorFilho];

                contIteracoes++;
            }
        }

        public void Run()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int[][] solucoesIniciais = new int[populacaoInicial][];
            double[] probSolucoes;

            for (int i = 0; i < populacaoInicial; i++)
                solucoesIniciais[i] = geraSolucaoAleatoria();

            while (contIteracoes < numIteracoes)
            {
                probSolucoes = gerarProbabilidadesIniciais(solucoesIniciais);

                int index1 = 0, index2 = 0;

                double random1 = rand.NextDouble();
                double soma = 0;

                for (int j = 0; j < populacaoInicial; j++)
                {
                    soma += probSolucoes[j];

                    if (random1 <= soma)
                    {
                        index1 = j;
                        break;
                    }
                }

                do
                {
                    double random2 = rand.NextDouble();
                    soma = 0;

                    for (int j = 0; j < populacaoInicial; j++)
                    {
                        soma += probSolucoes[j];
                        if (random2 <= soma)
                        {
                            index2 = j;
                            break;
                        }
                    }
                } while (index1 == index2);

                crossover(solucoesIniciais, index1, index2);
            }

            var maior = avaliar(solucoesIniciais[0]);
            int Imaior = 0;

            for (int k = 1; k < populacaoInicial; k++)
            {
                var aux2 = avaliar(solucoesIniciais[k]);
                if (maior.Item1 < aux2.Item1)
                {
                    maior = aux2;
                    Imaior = k;
                }
            }

            watch.Stop();

            var writer = new StreamWriter(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + @"\solucaoGA.txt", false);

            writer.WriteLine("Solução:");

            for (int i = 0; i < n; i++)
                writer.WriteLine(solucoesIniciais[Imaior][i]);

            writer.WriteLine();

            writer.WriteLine("FA: " + maior.Item1);
            writer.WriteLine("FO: " + maior.Item2);
            writer.WriteLine("RV: " + maior.Item3);
            writer.WriteLine("RA: " + maior.Item4);
            writer.WriteLine("RR: " + maior.Item5);

            writer.WriteLine("Tempo gasto: " + watch.ElapsedMilliseconds / 1000.0 + "s");

            writer.Close();

            Process.Start(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + @"\solucaoGA.txt");
        }
    }
}
