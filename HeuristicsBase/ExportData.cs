using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Heuristics
{
    public class ExportData : HeuristicsBase
    {
        public static void Export(string filename)
        {
            try
            {
                var writer = new StreamWriter(filename, false, Encoding.ASCII);

                writer.Write(JsonConvert.SerializeObject(new HeuristicsBase()));

                writer.Close();
            }
            catch
            {
            }
        }

        public static void ExportResults(string filename, int [] solucao, TimeSpan tempoDeExecucao, List <string> parametrosHeuristica)
        {
            Excel.Application app = new Excel.Application();

            app.EnableSound = false;

            Excel.Workbook workbook = app.Workbooks.Add();


            Excel.Worksheet worksheet1 = workbook.Worksheets.Add();
            worksheet1.Name = "Parametros da Execucao";

            int cont = 1;

            worksheet1.Cells[cont, 1] = "Idade Regulatória";
            worksheet1.Cells[cont, 2] = r;
            cont++;
            worksheet1.Cells[cont, 1] = "Hp";
            worksheet1.Cells[cont, 2] = h;
            cont++;
            worksheet1.Cells[cont, 1] = "Volume Mínimo";
            worksheet1.Cells[cont, 2] = volMin;
            cont++;
            worksheet1.Cells[cont, 1] = "Volume Máximo";
            worksheet1.Cells[cont, 2] = volMax;
            cont++;
            worksheet1.Cells[cont, 1] = "% Variação Área";
            worksheet1.Cells[cont, 2] = alfaRegArea;
            cont++;
            worksheet1.Cells[cont, 1] = "% Variação Volume +";
            worksheet1.Cells[cont, 2] = alfaRegVol;
            cont++;
            worksheet1.Cells[cont, 1] = "% Variação Volume -";
            worksheet1.Cells[cont, 2] = betaRegVol;
            cont++;
            worksheet1.Cells[cont, 1] = "Alfa";
            worksheet1.Cells[cont, 2] = alfa;
            cont++;
            worksheet1.Cells[cont, 1] = "Beta";
            worksheet1.Cells[cont, 2] = beta;
            cont++;
            worksheet1.Cells[cont, 1] = "Gama";
            worksheet1.Cells[cont, 2] = gama;
            cont++;
            worksheet1.Cells[cont, 1] = "Restrição de Adjacência";
            worksheet1.Cells[cont, 2] = areaAdjacencia?"Área de Adjacência":"IAC";
            cont++;
            worksheet1.Cells[cont, 1] = "Green Up";
            worksheet1.Cells[cont, 2] = greenUp;
            cont += 2;
            
            worksheet1.Cells[cont, 1] = "Função Objetivo (R$)";
            worksheet1.Cells[cont, 2] = fObjetivoFinal;
            cont++;
            worksheet1.Cells[cont, 1] = "Restrição de Regulação de Área (ha)";
            worksheet1.Cells[cont, 2] = rAreaFinal;
            cont++;
            worksheet1.Cells[cont, 1] = "Restrição de Volume (m³)";
            worksheet1.Cells[cont, 2] = rVolumeFinal;
            cont++;
            worksheet1.Cells[cont, 1] = "Restrição de Adjacência";
            worksheet1.Cells[cont, 2] = rAdjFinal;
            cont += 2;

            worksheet1.Cells[cont, 1] = "Heurística Escolhida";
            worksheet1.Cells[cont, 2] = parametrosHeuristica[0];

            cont += 2;

            worksheet1.Cells[cont, 1] = "Parâmetros da Heurística";
            cont++;

            for(int i = 1; i < parametrosHeuristica.Count; i += 2)
            {
                worksheet1.Cells[cont, 1] = parametrosHeuristica[i];
                worksheet1.Cells[cont, 2] = parametrosHeuristica[i + 1];
                cont++;
            }

            cont++;

            worksheet1.Cells[cont, 1] = "Tempo de Execução";
            worksheet1.Cells[cont, 2] = tempoDeExecucao.ToString(@"hh\:mm\:ss");
            cont++;

            Excel.Worksheet worksheet2 = workbook.Worksheets.Add();
            worksheet2.Name = "Custos e Receitas";

            cont = 1;

            double colheitaTotal = 0;
            double baldeioTotal = 0;
            double transporteTotal = 0;
            double silviculturaTotal = 0;
            double implantacaoTotal = 0;
            double anterioresTotal = 0;
            double VPLTotal = 0;
            double receitaTotal = 0;

            for (int i = 0; i < n; i++)
            {
                colheitaTotal += mColheita[i, solucao[i]] * talhoes[i].area;
                baldeioTotal += mBaldeio[i, solucao[i]] * talhoes[i].area; ;
                transporteTotal += mTransporte[i, solucao[i]] * talhoes[i].area; ;
                silviculturaTotal += mSilvicultura[i, solucao[i]] * talhoes[i].area; ;
                implantacaoTotal += mImplantacao[i, solucao[i]] * talhoes[i].area; ;
                anterioresTotal += mAnteriores[i, solucao[i]] * talhoes[i].area;
                receitaTotal += mReceita[i, solucao[i]] * talhoes[i].area;
                VPLTotal += mVPL[i, solucao[i]];
            }

            worksheet2.Cells[cont, 1] = "Custos";
            worksheet2.Cells[cont, 2] = "Valor (R$)";
            cont++;


            worksheet2.Cells[cont, 1] = "Colheita";
            worksheet2.Cells[cont, 2] = colheitaTotal;
            cont++;
            worksheet2.Cells[cont, 1] = "Baldeio";
            worksheet2.Cells[cont, 2] = baldeioTotal;
            cont++;
            worksheet2.Cells[cont, 1] = "Transporte";
            worksheet2.Cells[cont, 2] = transporteTotal;
            cont++;
            worksheet2.Cells[cont, 1] = "Silvicultura";
            worksheet2.Cells[cont, 2] = silviculturaTotal;
            cont++;
            worksheet2.Cells[cont, 1] = "Implantação";
            worksheet2.Cells[cont, 2] = implantacaoTotal;
            cont++;
            worksheet2.Cells[cont, 1] = "Anteriores";
            worksheet2.Cells[cont, 2] = anterioresTotal;
            cont += 2;

            worksheet2.Cells[cont, 1] = "Receita Total (R$)";
            worksheet2.Cells[cont, 2] = receitaTotal;

            cont += 2;

            worksheet2.Cells[cont, 1] = "VPL";
            worksheet2.Cells[cont, 2] = VPLTotal;


            Excel.Worksheet worksheet3 = workbook.Worksheets.Add();
            worksheet3.Name = "Area Regulada";

            cont = 1;

            worksheet3.Cells[cont, 1] = "Periodo";
            worksheet3.Cells[cont, 2] = "Area (ha)";
            cont++;

            for(int i=0; i < r ; i++)
            {
                worksheet3.Cells[cont, 1] = (i + 1).ToString();
                worksheet3.Cells[cont, 2] = solucao.Select((p, idx) => mRegArea[idx, p, i]).Sum();
                cont++;
            }

            Excel.Worksheet worksheet4 = workbook.Worksheets.Add();
            worksheet4.Name = "Volume Anual";

            cont = 1;

            worksheet4.Cells[cont, 1] = "Periodo";
            worksheet4.Cells[cont, 2] = "Volume (m3)";
            cont++;



            worksheet4.Cells[cont, 1] = "Media";
            worksheet4.Cells[cont, 2] = volumeMedio;
            cont++;

            for(int i = 0; i < h; i++)
            {
                worksheet4.Cells[cont, 1] = (i + 1).ToString();
                worksheet4.Cells[cont, 2] = solucao.Select((p, idx) => mVolume[idx, p, i]).Sum();
                cont++;
            }

            Excel.Worksheet worksheet5 = workbook.Worksheets.Add();
            worksheet5.Name = "Restricao Adjacencia";

            cont = 1;

            worksheet5.Cells[cont, 1] = "Periodo";
            worksheet5.Cells[cont, 2] = "Adjacencia";
            cont++;

            double [] soma = new double[h];

            if (areaAdjacencia)
            {
                for (int i = 0; i < n; i++)
                    for (int k = 0; k < h; k++)
                    {
                        if (mCorte[i, solucao[i], k])
                            foreach (int vizinho in talhoes[i].vizinhos)
                                if (mCorte[vizinho, solucao[vizinho], k])
                                {
                                    soma[k] += talhoes[k].area;
                                    break;
                                }
                    }
            }
            else
            {
                for (int i = 0; i < h; i++)
                {
                    double IAC, area2, distMaisProximo, areasCortadas;
                    IAC = areasCortadas = 0;

                    for (int j = 0; j < n; j++)
                        if (mCorte[j, solucao[j], i])
                        {
                            areasCortadas += talhoes[j].area * talhoes[j].area;
                            area2 = talhoes[j].area * talhoes[j].area;

                            distMaisProximo = Double.PositiveInfinity;

                            for (int k = 0; k < n; k++)
                                if (k != j && mCorte[k, solucao[k], i])
                                    distMaisProximo = Math.Min(distMaisProximo, mDistancia[j, k]);

                            distMaisProximo /= 1000;

                            IAC += area2 / (distMaisProximo * distMaisProximo);
                        }

                    soma[i] = IAC / areasCortadas;
                }
            }

            for(int i = 0; i < h; i++)
            {
                worksheet5.Cells[cont, 1] = (i + 1).ToString();
                worksheet5.Cells[cont, 2] = soma[i];
                cont++;
            }
            
            Excel.Worksheet worksheet6 = workbook.Worksheets.Add();
            worksheet6.Name = "mCorte";

            cont = 1;

            for (int i = 0; i < h; i++)
            {
                worksheet6.Cells[cont, i + 1] = i;
            }

            cont++;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (mCorte[i, solucao[i], j])
                        worksheet6.Cells[cont, j + 1] = 1;
                    else
                        worksheet6.Cells[cont, j + 1] = 0;
                }
                cont++;
            }

            Excel.Worksheet worksheet7 = workbook.Worksheets.Add();
            worksheet7.Name = "Solução";

            cont = 2;

            worksheet7.Cells[1, 1] = "Talhao";
            worksheet7.Cells[1, 2] = "Alternativa";

            for (int i = 0; i < n; i++)
            {
                worksheet7.Cells[cont, 1] = (i + 1).ToString();
                worksheet7.Cells[cont, 2] = solucao[i].ToString();
                cont++;
            }


            workbook.SaveAs(filename);
            workbook.Close(0);

            app.Quit();
        }
    }
}
