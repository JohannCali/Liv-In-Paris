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
        private Noeud noeud1; /// extrémité 1 du lien
        private Noeud noeud2; /// extrémité 2 du lien
        private int poids; /// valeur associée au lien (temps de trajet, distance...)
    
        public Lien(Noeud noeud1, Noeud noeud2, int poids)
        {
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.poids = poids;
        }
    
        public Noeud Noeud1
        {
            get { return this.noeud1; } /// getter pour la 1ère extrémité
        }
    
        public Noeud Noeud2
        {
            get { return this.noeud2; } /// getter pour la 2ème extrémité
        }
    
        public int Poids
        {
            get { return this.poids; } /// getter pour le poids du lien
        }
    }
}
