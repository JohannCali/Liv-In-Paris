using RENDU30mars;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RENDU30mars
{
    public class Graphe<T>
    {
        public Dictionary<T, Noeud> Noeuds { get; set; }
        public List<Noeud> ListeAdjacence { get; set; }
        public bool[,] MatriceAdjacence { get; set; }

        public Graphe()
        {
            Noeuds = new Dictionary<T, Noeud>();
            ListeAdjacence = new List<Noeud>();
            MatriceAdjacence = new bool[333, 333];
        }

        public void ChargerDepuisFichier(string cheminFichier)
        {
            using (StreamReader lecteur = new StreamReader(cheminFichier))
            {
                lecteur.ReadLine(); // Ignorer l'en-tête
                string ligne;

                while ((ligne = lecteur.ReadLine()) != null)
                {
                    var colonnes = ligne.Split(',');
                    if (colonnes.Length < 6) continue;

                    string stationNom = colonnes[1].Trim();
                    string stationSuivante = colonnes[3].Trim();
                    int temps = ConvertirTemps(colonnes[4]);

                    // Ajouter les stations dans le dictionnaire
                    if (!Noeuds.ContainsKey((T)(object)stationNom))
                    {
                        Noeuds[(T)(object)stationNom] = new Noeud(stationNom);
                        ListeAdjacence.Add(Noeuds[(T)(object)stationNom]); // Ajout dans la liste d'adjacence
                    }

                    if (!Noeuds.ContainsKey((T)(object)stationSuivante))
                    {
                        Noeuds[(T)(object)stationSuivante] = new Noeud(stationSuivante);
                        ListeAdjacence.Add(Noeuds[(T)(object)stationSuivante]); // Ajout dans la liste d'adjacence
                    }

                    Noeud noeud1 = Noeuds[(T)(object)stationNom];
                    Noeud noeud2 = Noeuds[(T)(object)stationSuivante];

                    // Ajouter les liens dans la liste d'adjacence
                    noeud1.AjouterLien(noeud2, temps);
                    noeud2.AjouterLien(noeud1, temps); // Connexion bidirectionnelle

                    // Mettre à jour la matrice d'adjacence
                    int index1 = ListeAdjacence.IndexOf(noeud1);
                    int index2 = ListeAdjacence.IndexOf(noeud2);

                    MatriceAdjacence[index1, index2] = true;
                    MatriceAdjacence[index2, index1] = true; // Connexion bidirectionnelle
                }
            }
        }

        private int ConvertirTemps(string tempsStr)
        {
            if (string.IsNullOrWhiteSpace(tempsStr))
            {
                Console.WriteLine("Valeur manquante pour le temps.");
                return 0;
            }

            if (int.TryParse(tempsStr, NumberStyles.Any, CultureInfo.InvariantCulture, out int temps))
            {
                return temps;
            }

            Console.WriteLine($"Échec de la conversion de la valeur en int : {tempsStr}");
            return 0;
        }

        public void AfficherGraphe()
        {
            HashSet<string> liensAffiches = new HashSet<string>();

            foreach (var noeud in Noeuds.Values)
            {
                foreach (var lien in noeud.Listeliens)
                {
                    string idLien = $"{lien.Noeud1.Nom}-{lien.Noeud2.Nom}";
                    string idLienInverse = $"{lien.Noeud2.Nom}-{lien.Noeud1.Nom}";

                    // Vérifier si le lien n'a pas déjà été affiché
                    if (!liensAffiches.Contains(idLien) && !liensAffiches.Contains(idLienInverse))
                    {
                        Console.WriteLine($"{lien.Noeud1.Nom} <-> {lien.Noeud2.Nom} : {lien.Poids} min");
                        liensAffiches.Add(idLien);
                    }
                }
            }
        }

        public void AfficherMatriceAdjacence()
        {
            Console.WriteLine("\n===== Matrice d'Adjacence =====");

            // En-tête avec noms abrégés
            Console.Write("   ");
            foreach (var noeud in ListeAdjacence)
            {
                Console.Write($"{noeud.Nom.Substring(0, Math.Min(3, noeud.Nom.Length))} ");
            }
            Console.WriteLine();

            // Affichage des lignes de la matrice
            for (int i = 0; i < ListeAdjacence.Count; i++)
            {
                Console.Write($"{ListeAdjacence[i].Nom.Substring(0, Math.Min(3, ListeAdjacence[i].Nom.Length))} ");
                for (int j = 0; j < ListeAdjacence.Count; j++)
                {
                    Console.Write(MatriceAdjacence[i, j] ? " 1 " : " 0 ");
                }
                Console.WriteLine();
            }
        }

        public (Dictionary<T, double>, Dictionary<T, T>) Dijkstra(T depart)
        {
            var distances = new Dictionary<T, double>();
            var predecesseurs = new Dictionary<T, T>();
            var nonVisites = new HashSet<T>(Noeuds.Keys);

            foreach (var noeud in Noeuds.Keys)
            {
                distances[noeud] = double.PositiveInfinity;
            }

            distances[depart] = 0;

            while (nonVisites.Count > 0)
            {
                // Trouver le nœud non visité avec la plus petite distance
                T courant = default!;
                double minDistance = double.PositiveInfinity;

                foreach (var noeud in nonVisites)
                {
                    if (distances[noeud] < minDistance)
                    {
                        minDistance = distances[noeud];
                        courant = noeud;
                    }
                }

                // Si aucun chemin n'est trouvé, on s'arrête
                if (minDistance == double.PositiveInfinity)
                    break;

                nonVisites.Remove(courant);

                // Mettre à jour les distances pour les voisins
                foreach (var lien in Noeuds[courant].Listeliens)
                {
                    var voisin = lien.Noeud1.Nom.Equals(courant.ToString()) ? (T)(object)lien.Noeud2.Nom : (T)(object)lien.Noeud1.Nom;
                    var nouvelleDistance = distances[courant] + lien.Poids;

                    if (nouvelleDistance < distances[voisin])
                    {
                        distances[voisin] = nouvelleDistance;
                        predecesseurs[voisin] = courant;
                    }
                }
            }

            return (distances, predecesseurs);
        }

        public (Dictionary<string, float> distances, Dictionary<string, string> predecessors) BellmanFord(string source)
        {
            var distances = new Dictionary<string, float>();
            var predecessors = new Dictionary<string, string>();

            foreach (var noeud in Noeuds.Values)
            {
                distances[noeud.Nom] = float.PositiveInfinity;
                predecessors[noeud.Nom] = null;
            }
            distances[source] = 0;

            for (int i = 1; i < Noeuds.Count; i++)
            {
                foreach (var noeud in Noeuds.Values)
                {
                    foreach (var lien in noeud.Listeliens)
                    {
                        if (distances[lien.Noeud1.Nom] + lien.Poids < distances[lien.Noeud2.Nom])
                        {
                            distances[lien.Noeud2.Nom] = distances[lien.Noeud1.Nom] + lien.Poids;
                            predecessors[lien.Noeud2.Nom] = lien.Noeud1.Nom;
                        }
                    }
                }
            }

            return (distances, predecessors);
        }

        public List<string> ReconstruireChemin(Dictionary<string, string> predecessors, string destination)
        {
            var chemin = new List<string>();
            for (string noeud = destination; noeud != null; noeud = predecessors[noeud])
            {
                chemin.Insert(0, noeud);
            }
            return chemin;
        }

        // Algorithme de Floyd-Warshall
        public double[,] FloydWarshall()
        {
            int n = ListeAdjacence.Count;
            double[,] distances = new double[n, n];
            int[,] chemins = new int[n, n];

            // Initialisation
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        distances[i, j] = 0;
                    else
                        distances[i, j] = double.PositiveInfinity;

                    chemins[i, j] = -1;
                }
            }

            // Remplissage des distances directes entre les noeuds
            for (int i = 0; i < n; i++)
            {
                var noeud = ListeAdjacence[i];
                foreach (var lien in noeud.Listeliens)
                {
                    int j = ListeAdjacence.IndexOf(lien.Noeud2);
                    distances[i, j] = lien.Poids;
                    chemins[i, j] = i;
                }
            }

            // Algorithme de Floyd-Warshall
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            chemins[i, j] = chemins[k, j];
                        }
                    }
                }
            }

            return distances;
        }
    }
}
