using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.IO;



namespace Rendu30mars
{
    public class Noeud
    {
        public string Nom { get; set; }
        public List<Lien> Liens { get; set; }

        public Noeud(string nom)
        {
            Nom = nom;
            Liens = new List<Lien>();
        }

        public void AjouterLien(Noeud autreNoeud, double temps)
        {
            // Vérifier que le lien n'existe pas déjà pour éviter les doublons
            if (!Liens.Any(l => (l.Noeud1 == this && l.Noeud2 == autreNoeud) || (l.Noeud1 == autreNoeud && l.Noeud2 == this)))
            {
                Liens.Add(new Lien(this, autreNoeud, temps));
            }
        }
    }

}
