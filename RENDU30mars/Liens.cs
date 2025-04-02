using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendu30mars
{
    public class Lien
    {
        public Noeud Noeud1 { get; set; }
        public Noeud Noeud2 { get; set; }
        public double Temps { get; set; }

        public Lien(Noeud noeud1, Noeud noeud2, double temps)
        {
            Noeud1 = noeud1;
            Noeud2 = noeud2;
            Temps = temps;
        }
    }
}
