using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heuristics
{
    public class Talhao
    {
        public int id { get; set; }
        public double area { get; set; }
        public int idade { get; set; }
        public string regime { get; set; }
        public List<int> vizinhos { get; set; }

        public Talhao()
        {
            vizinhos = new List<int>();
        }
    }
}
