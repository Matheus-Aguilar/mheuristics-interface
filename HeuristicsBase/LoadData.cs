using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace Heuristics
{
    public class LoadClass : HeuristicsBase
    {
        #region loading_functions
        /// <summary>
        /// Obrigatório ser chamado antes de qualquer heuristica
        /// </summary>
        /// <param name="n"></param>
        /// <param name="nAf"></param>
        /// <param name="nTa"></param>
        /// <param name="m"></param>
        /// <param name="r"></param>
        /// <param name="volMin"></param>
        /// <param name="volMax"></param>
        /// <param name="alfaRegArea"></param>
        /// <param name="areaTotal"></param>
        /// <param name="alfa"></param>
        /// <param name="beta"></param>
        /// <param name="gama"></param>
        public static void LoadData(int n = 91, int nAf = 72, int nTa = 0, int m = 72, int r = 6)
        {
            HeuristicsBase.n = n;
            HeuristicsBase.nAf = nAf;
            HeuristicsBase.nTa = nTa;
            HeuristicsBase.m = m;
            HeuristicsBase.h = 3 * r;
            HeuristicsBase.r = r;

            talhoes = new Talhao[n];
            mVPL = new double[n, m];
            mCorte = new bool[n, m, h];
            mVolume = new double[n, m, h];
            mAdj = new bool[n, n];
            mDistancia = new double[n, n];
            mRegArea = new double[n, m, r];
        }

        public static void LoadParams(double volMin = 100000, double volMax = 300000, double alfaRegArea = 0.20,
            double areaTotal = 1892.08, double alfa = 0.00001, double beta = 0.001, double gama = 0.1)
        {
            HeuristicsBase.volMin = volMin;
            HeuristicsBase.volMax = volMax;
            HeuristicsBase.alfaRegArea = alfaRegArea;
            HeuristicsBase.areaTotal = areaTotal;
            HeuristicsBase.alfa = alfa;
            HeuristicsBase.beta = beta;
            HeuristicsBase.gama = gama;

            areaPorR = areaTotal / r;
            areaPorR_maisAlfaReg = areaPorR * (1 + alfaRegArea);
            areaPorR_menosAlfaReg = areaPorR * (1 - alfaRegArea);
        }

        public static void LoadXLSX(string filename)
        {
            Excel.Application app = new Excel.Application();

            app.EnableSound = false;

            Excel.Workbook workbook = app.Workbooks.Open(filename,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets["Dados"];

            int n = Convert.ToInt32(worksheet.Cells[2, 1].Value2);
            int m = nAf = Convert.ToInt32(worksheet.Cells[4, 1].Value2);
            int r = Convert.ToInt32(worksheet.Cells[6, 1].Value2);
            int nTa = 0;

            LoadData(n, nAf, nTa, m, r);

            worksheet = (Excel.Worksheet)workbook.Sheets["Prescrições"];

            Excel.Range range = worksheet.get_Range(CellFormat(2, 1), CellFormat(n * m + 2, 10));

            object[,] values = range.Value2;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mVPL[i, j] = Convert.ToDouble(values[i * m + j + 1, 10]);

            for (int i = 0; i < n; i++)
            {
                talhoes[i] = new Talhao();

                talhoes[i].area = Convert.ToDouble(values[i * m + 1, 2]);
                talhoes[i].id = Convert.ToInt32(values[i * m + 1, 1]);
                talhoes[i].idade = Convert.ToInt32(values[i * m + 1, 3]);
                talhoes[i].regime = (string)values[i * m + 12, 4];
            }

            worksheet = (Excel.Worksheet)workbook.Sheets["mCorte"];

            range = worksheet.get_Range(CellFormat(2, 1), CellFormat(n * m + 2, h));

            values = range.Value2;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    for (int k = 0; k < h; k++)
                        mCorte[i, j, k] = Convert.ToInt32(Convert.ToDouble(values[i * m + j + 1, k + 1])) == 1;

            worksheet = (Excel.Worksheet)workbook.Sheets["mRegArea"];

            range = worksheet.get_Range(CellFormat(2, 1), CellFormat(n * m + 2, r));

            values = range.Value2;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    for (int k = 0; k < r; k++)
                        mRegArea[i, j, k] = Convert.ToDouble(values[i * m + j + 1, k + 1]);

            worksheet = (Excel.Worksheet)workbook.Sheets["mVolume"];

            range = worksheet.get_Range(CellFormat(2, 3), CellFormat(n * m + 2, h + 2));

            values = range.Value2;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    for (int k = 0; k < h; k++)
                        mVolume[i, j, k] = Convert.ToDouble(values[i * m + j + 1, k + 1]);

            worksheet = (Excel.Worksheet)workbook.Sheets["mAdj"];

            range = worksheet.get_Range(CellFormat(2, 3), CellFormat(n + 2, n + 2));

            values = range.Value2;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (Convert.ToInt32(Convert.ToDouble(values[i + 1, j + 1])) == 1)
                        talhoes[i].vizinhos.Add(j);

                    mAdj[i,j] = Convert.ToInt32(Convert.ToDouble(values[i + 1, j + 1])) == 1;
                }

            worksheet = (Excel.Worksheet)workbook.Sheets["mDistancia"];

            range = worksheet.get_Range(CellFormat(2, 1), CellFormat(n * n + 2, 3));

            values = range.Value2;

            int n2 = n * n;

            for (int i = 0; i < n2; i++)
                mDistancia[Convert.ToInt32(values[i + 1, 1]) - 1, Convert.ToInt32(values[i + 1, 2]) - 1] = Convert.ToDouble(values[i + 1, 3]);

            worksheet = (Excel.Worksheet)workbook.Sheets["Dados"];

            double volMin = Convert.ToDouble(worksheet.Cells[8, 1].Value2);
            double volMax = Convert.ToDouble(worksheet.Cells[10, 1].Value2);
            double alfaRegArea = Convert.ToDouble(worksheet.Cells[12, 1].Value2);
            double alfa = Convert.ToDouble(worksheet.Cells[14, 1].Value2);
            double beta = Convert.ToDouble(worksheet.Cells[16, 1].Value2);
            double gama = Convert.ToDouble(worksheet.Cells[18, 1].Value2);

            double areaTotal = talhoes.Aggregate(0.0, (acc, p) => acc + p.area);

            LoadParams(volMin, volMax, alfaRegArea, areaTotal, alfa, beta, gama);

            workbook.Close(0);

            app.Quit();
        }
        #endregion

        private static string CellFormat(int row, int column)
        {
            string col = "";

            while (column != 0)
            {
                col = col + Convert.ToChar((int)column % 26 + 64);
                column /= 26;
            }

            return col + row;
        }

        public static void LoadJSON(string filename)
        {
            var reader = new StreamReader(filename, Encoding.ASCII);

            string json;

            try
            {
                json = reader.ReadToEnd();

                JsonConvert.DeserializeObject<HeuristicsBase>(json);
            }
            catch
            {

            }

            reader.Close();
        }
    }
}
