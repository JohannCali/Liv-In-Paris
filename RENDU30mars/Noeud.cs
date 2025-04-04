using System;
using System.Collections.Generic;
using System.Linq;

namespace Rendu30mars
{
    public class Noeud
    {
        private string nom; /// nom du noeud (ex : nom de station)
        private List<Lien> listeliens; /// tous les liens connectés à ce noeud

        public Noeud(string nom)
        {
            this.nom = nom;
            this.listeliens = new List<Lien>(); /// on initialise la liste vide des connexions
        }
    
        public string Nom
        {
            get { return this.nom; } /// permet de récupérer le nom du noeud
        }
    
        public List<Lien> Listeliens
        {
            get { return this.listeliens; } /// récupère tous les liens associés à ce noeud
        }
    
        public void AjouterLien(Noeud autreNoeud, int poids)
        {
            /// vérifie si un lien entre les deux noeuds existe déjà (dans un sens ou dans l'autre)
            if (!listeliens.Any(l => (l.Noeud1 == this && l.Noeud2 == autreNoeud) || (l.Noeud1 == autreNoeud && l.Noeud2 == this)))
            {
                listeliens.Add(new Lien(this, autreNoeud, poids)); /// sinon on crée le lien
            }
        }
    }
}