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
    public class GRASP : HeuristicsBase
    {
        double alfaGrasp;
        int numIteracoesLocal;
        int numIteracoesGuloso;
        int opt;
        int tipo;

        public GRASP(double alfaGrasp = 0.05, int numIteracoesLocal = 100, int numIteracoesGuloso = 0, int opt = 1, int tipo = 1)
        {
            this.alfaGrasp = alfaGrasp;
            this.numIteracoesLocal = numIteracoesLocal;
            this.numIteracoesGuloso = numIteracoesGuloso;
            this.opt = opt;
            this.tipo = tipo;
        }

        int selecionaPresc(ref int[] solucao, int pos)
        {
            int prescAntiga = solucao[pos];

            if (!minimizar)
            {
                double[] probVPL = new double[m];

                probVPL = probVPL.Select((p, idx) => Math.Abs(mVPL[pos, idx] - mVPL[pos, prescAntiga])).ToArray();

                var soma = probVPL.Aggregate(0.0, (acc, p) => p + acc);

                probVPL = probVPL.Select(p => p / soma).ToArray();

                var r = rand.NextDouble();

                soma = 0;

                for (int j = 0; j < m; j++)
                {
                    soma += probVPL[j];

                    if (soma >= r)
                        return j;
                }

                return m - 1;
            }
            else
            {
                double[] probCustos = new double[m];

                probCustos = probCustos.Select((p, idx) => Math.Abs(mCustosMedios[pos, idx] - mCustosMedios[pos, prescAntiga])).ToArray();

                var soma = probCustos.Aggregate(0.0, (acc, p) => p + acc);

                probCustos = probCustos.Select(p => soma - p).ToArray();

                soma = probCustos.Aggregate(0.0, (acc, p) => p + acc);

                probCustos = probCustos.Select(p => p / soma).ToArray();

                var r = rand.NextDouble();

                soma = 0;

                for (int j = 0; j < m; j++)
                {
                    soma += probCustos[j];

                    if (soma >= r)
                        return j;
                }

                return m - 1;
            }
        }

        bool pertuba(ref int[] solucao, int vizinhanca)
        {
            int[] novaSolucao;
            var valorAtual = avaliar((int[])solucao.Clone()).Item1;

            for (int i = 0; i < numIteracoesLocal; i++)
            {
                novaSolucao = (int[])solucao.Clone();

                int rndPosicao = rand.Next(n);

                int rndPosicao2;

                if (opt == 3)
                {
                    if (vizinhanca == 0) // 1-OPT
                    {
                        novaSolucao[rndPosicao] = selecionaPresc(ref novaSolucao, rndPosicao);
                    }
                    else if (vizinhanca == 1) // 2-OPT
                    {
                        do
                            rndPosicao2 = rand.Next(n);
                        while (rndPosicao2 == rndPosicao);

                        Task<int>[] funcoes = new Task<int>[2];

                        funcoes[0] = new Task<int>(() => selecionaPresc(ref novaSolucao, rndPosicao));
                        funcoes[1] = new Task<int>(() => selecionaPresc(ref novaSolucao, rndPosicao2));

                        foreach (var task in funcoes)
                            task.Start();

                        Task.WaitAll(funcoes);

                        novaSolucao[rndPosicao] = funcoes[0].Result;
                        novaSolucao[rndPosicao2] = funcoes[1].Result;
                    }
                    else // Random
                    {
                        novaSolucao[rndPosicao] = rand.Next(m);
                    }
                }
                else if (opt == 2)
                {
                    do
                        rndPosicao2 = rand.Next(n);
                    while (rndPosicao2 == rndPosicao);

                    Task<int>[] funcoes = new Task<int>[2];

                    funcoes[0] = new Task<int>(() => selecionaPresc(ref novaSolucao, rndPosicao));
                    funcoes[1] = new Task<int>(() => selecionaPresc(ref novaSolucao, rndPosicao2));

                    foreach (var task in funcoes)
                        task.Start();

                    Task.WaitAll(funcoes);

                    novaSolucao[rndPosicao] = funcoes[0].Result;
                    novaSolucao[rndPosicao2] = funcoes[1].Result;
                }
                else
                {
                    novaSolucao[rndPosicao] = selecionaPresc(ref novaSolucao, rndPosicao);
                }

                double valorNovo = avaliar(novaSolucao).Item1;

                if ((valorNovo >= valorAtual && !minimizar) ||
                    (valorNovo <= valorAtual && minimizar))
                {
                    solucao = novaSolucao;

                    return true;
                }
            }

            return false;
        }

        public override void Run()
        {
            solucao = geraSolucaoGulosa(tipo, alfaGrasp);

            if (opt == 3)
            {
                int k = 0, k_max = 3;

                while (k != k_max)
                {
                    for (var i = 0; i < numIteracoesLocal; i++)
                        while (pertuba(ref solucao, k)) ;
                    k++;
                }

                Iteracoes.Add(avaliar(solucao));

                for (var i = 0; i < numIteracoesGuloso; i++)
                {
                    var novaSolucao = geraSolucaoGulosa(tipo, alfaGrasp);

                    k = 0;
                    while (k != k_max)
                    {
                        for (var j = 0; j < numIteracoesLocal; j++)
                        {
                            while (pertuba(ref novaSolucao, k)) ;
                        }
                        k++;
                    }

                    if ((avaliar(novaSolucao).Item1 > avaliar(solucao).Item1 && !minimizar) ||
                        (avaliar(novaSolucao).Item1 < avaliar(solucao).Item1 && minimizar))
                        solucao = novaSolucao;

                    Iteracoes.Add(avaliar(novaSolucao));
                }

                if (!minimizar)
                    Iteracoes = Iteracoes.OrderBy(p => p.Item1).ToList();
                else
                    Iteracoes = Iteracoes.OrderBy(p => -p.Item1).ToList();
            }
            else
            {
                int k = 0;
                
                for (var i = 0; i < numIteracoesLocal; i++)
                    while (pertuba(ref solucao, k)) ;

                Iteracoes.Add(avaliar(solucao));

                for (var i = 0; i < numIteracoesGuloso; i++)
                {
                    var novaSolucao = geraSolucaoGulosa(tipo, alfaGrasp);

                    for (var j = 0; j < numIteracoesLocal; j++)
                        while (pertuba(ref novaSolucao, k)) ;
                     

                    if ((avaliar(novaSolucao).Item1 > avaliar(solucao).Item1 && !minimizar) ||
                        (avaliar(novaSolucao).Item1 < avaliar(solucao).Item1 && minimizar))
                        solucao = novaSolucao;

                    Iteracoes.Add(avaliar(novaSolucao));
                }
                
                if(!minimizar)
                    Iteracoes = Iteracoes.OrderBy(p => p.Item1).ToList();
                else
                    Iteracoes = Iteracoes.OrderBy(p => -p.Item1).ToList();
            }
        }
    }
}
