using System;
using System.Collections.Generic;
using System.Linq;

namespace RENDU30mars
{
    public class Noeud
    {
        private string nom;
        private List<Lien> listeliens;

        public Noeud(string nom)
        {
            this.nom = nom;
            this.listeliens = new List<Lien>(); // Initialisation de la liste
        }

        public string Nom
        {
            get { return this.nom; }
        }

        public List<Lien> Listeliens
        {
            get { return this.listeliens; }
        }

        public void AjouterLien(Noeud autreNoeud, int poids)
        {
            // Vérifier que le lien n'existe pas déjà pour éviter les doublons
            if (!listeliens.Any(l => (l.Noeud1 == this && l.Noeud2 == autreNoeud) || (l.Noeud1 == autreNoeud && l.Noeud2 == this)))
            {
                listeliens.Add(new Lien(this, autreNoeud, poids));
            }
        }
    }
}
