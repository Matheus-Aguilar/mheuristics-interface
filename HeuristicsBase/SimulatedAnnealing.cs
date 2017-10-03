using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heuristics
{
    public class SimulatedAnnealing : HeuristicsBase
    {
        double t = 100000;
        double tf = 1000;
        double taxaResf = 0.95;
        int contIteracao = 100;
        int cont = 0;
        int opt = 2;

        int selecionaPresc(ref int[] solucao, int pos)
        {
            int prescAntiga = solucao[pos];

            double[] probVPL = new double [m];

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

        double metropolis(double valorAtual, double valorNovo)
        {
            double delta = valorAtual - valorNovo;

            return Math.Exp(-delta / t);
        }

        void pertubaEAvalia()
        {
            int[] novaSolucao = (int[]) solucao.Clone();

            int rndPosicao = rand.Next(n);

            int rndPosicao2;

            if (opt == 2)
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

            Task<double>[] avaliacoes = new Task<double>[2];

            avaliacoes[0] = new Task<double>(() => avaliar(solucao).Item1);
            avaliacoes[1] = new Task<double>(() => avaliar(novaSolucao).Item1);

            foreach (var task in avaliacoes)
                task.Start();

            Task.WaitAll(avaliacoes);

            double valorNovo = avaliacoes[1].Result, valorAtual = avaliacoes[0].Result;

            if (valorNovo <= valorAtual)
            {
                double z = metropolis(valorAtual, valorNovo);
                double r = rand.NextDouble();
                
                if(r < z)
                {
                    solucao = novaSolucao;
                    
                    cont++;

                    if (cont >= contIteracao)
                    {
                        Iteracoes.Add(avaliar(solucao));

                        t = t * taxaResf;

                        cont = 0;
                    }
                }
            }
            else
            {
                solucao = novaSolucao;

                cont++;

                if (cont >= contIteracao)
                {
                    Iteracoes.Add(avaliar(solucao));

                    t = t * taxaResf;

                    cont = 0;
                }
            }
        }

        public SimulatedAnnealing(double t, double tf, double taxaResf, int contIteracao, int opt)
        {
            this.t = t;
            this.tf = tf;
            this.taxaResf = taxaResf;
            this.contIteracao = contIteracao;
            this.opt = opt;
        }

        public override void Run()
        {
            solucao = geraSolucaoAleatoria();

            while (t > tf)
            {
                pertubaEAvalia();
            }
        }
    }
}
