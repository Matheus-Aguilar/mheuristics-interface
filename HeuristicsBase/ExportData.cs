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

        
        public static void ExportResults(string filename, int [] solucao)
        {
            Excel.Application app = new Excel.Application();

            app.EnableSound = false;

            Excel.Workbook workbook = app.Workbooks.Add();

            Excel.Worksheet worksheet = workbook.Worksheets.Add();

            int cont = 3;

            worksheet.Cells[1, 1] = "Simulacao Heuristica";
            worksheet.Cells[2, 1] = "Talhao";
            worksheet.Cells[2, 2] = "Alternativa";

            for (int i = 0; i < n; i++)
            {
                worksheet.Cells[cont, 1] = (i+1).ToString();
                worksheet.Cells[cont, 2] = solucao[i].ToString();
                cont++;
            }
            
            cont++; //saltando uma linha

            worksheet.Cells[cont, 1] = "Area Regulada";
            cont++;
            worksheet.Cells[cont, 1] = "Periodo";
            worksheet.Cells[cont, 2] = "Area (ha)";
            cont++;

            for(int i=0; i < r ; i++)
            {
                worksheet.Cells[cont, 1] = (i + 1).ToString();
                worksheet.Cells[cont, 2] = solucao.Select((p, idx) => mRegArea[idx, p, i]).Sum();
                cont++;
            }

            cont++;

            worksheet.Cells[cont, 1] = "Volume Anual";
            cont++;
            worksheet.Cells[cont, 1] = "Periodo";
            cont++;
            worksheet.Cells[cont, 2] = "Volume (m3)";
            cont++;



            worksheet.Cells[cont, 1] = "Media";
            worksheet.Cells[cont, 2] = volumeMedio;
            cont++;

            for(int i = 0; i < h; i++)
            {
                worksheet.Cells[cont, 1] = (i + 1).ToString();
                worksheet.Cells[cont, 2] = solucao.Select((p, idx) => mVolume[idx, p, i]).Sum();
                cont++;
            }

            cont++;

            worksheet.Cells[cont, 1] = "Restricao Adjacencia";
            cont++;
            worksheet.Cells[cont, 1] = "Periodo";
            worksheet.Cells[cont, 2] = "Adjacencia";
            cont++;

            double [] soma = new double[h];
            
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


            for(int i = 0; i < h; i++)
            {
                worksheet.Cells[cont, 1] = (i + 1).ToString();
                worksheet.Cells[cont, 2] = soma[i];
                cont++;
            }

            worksheet.Name = "Resultados";
            workbook.SaveAs(filename);

            workbook.Close(0);

            app.Quit();
        }
    }
}
