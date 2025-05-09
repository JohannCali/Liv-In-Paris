using Rendu30mars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RENDU30mars
{
    public class Lien
    {
        private Noeud noeud1;
        private Noeud noeud2;
        private int poids;

        public Lien(Noeud noeud1, Noeud noeud2, int poids)
        {
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.poids = poids;
        }

        public Noeud Noeud1
        {
            get { return this.noeud1; }
        }

        public Noeud Noeud2
        {
            get { return this.noeud2; }
        }

        public int Poids
        {
            get { return this.poids; }
        }
    }
}
