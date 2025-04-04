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
        public Dictionary<T, Noeud> Noeuds { get; set; } /// dictionnaire qui associe chaque valeur T à un objet Noeud
        public List<Noeud> ListeAdjacence { get; set; } /// liste utilisée pour parcourir les sommets dans l’ordre d’ajout
        public bool[,] MatriceAdjacence { get; set; } /// matrice booléenne pour représenter les connexions entre noeuds

        public Graphe()
        {
            Noeuds = new Dictionary<T, Noeud>();
            ListeAdjacence = new List<Noeud>();
            MatriceAdjacence = new bool[333, 333]; /// taille fixe pour simplifier, on suppose 333 noeuds max
        }

        public void ChargerDepuisFichier(string cheminFichier)
        {
            using (StreamReader lecteur = new StreamReader(cheminFichier))
            {
                lecteur.ReadLine(); /// on ignore la 1ère ligne (en-tête CSV)
                string ligne;

                while ((ligne = lecteur.ReadLine()) != null)
                {
                    var colonnes = ligne.Split(','); /// on découpe la ligne CSV
                    if (colonnes.Length < 6) continue; /// ligne invalide ou incomplète

                    string stationNom = colonnes[1].Trim(); /// nom de la station actuelle
                    string stationSuivante = colonnes[3].Trim(); /// nom de la station suivante
                    int temps = ConvertirTemps(colonnes[4]); /// temps de trajet entre les deux

                                                             /// conversion générique pour le dictionnaire
                    T key1 = (T)(object)stationNom;
                    T key2 = (T)(object)stationSuivante;

                    /// ajout si pas déjà existant
                    if (!Noeuds.ContainsKey(key1))
                    {
                        Noeuds[key1] = new Noeud(stationNom);
                        ListeAdjacence.Add(Noeuds[key1]);
                    }

                    if (!Noeuds.ContainsKey(key2))
                    {
                        Noeuds[key2] = new Noeud(stationSuivante);
                        ListeAdjacence.Add(Noeuds[key2]);
                    }

                    Noeud noeud1 = Noeuds[key1];
                    Noeud noeud2 = Noeuds[key2];

                    noeud1.AjouterLien(noeud2, temps); /// ajout du lien aller
                    noeud2.AjouterLien(noeud1, temps); /// ajout du lien retour

                    int index1 = ListeAdjacence.IndexOf(noeud1);
                    int index2 = ListeAdjacence.IndexOf(noeud2);

                    /// mise à jour de la matrice d’adjacence
                    MatriceAdjacence[index1, index2] = true;
                    MatriceAdjacence[index2, index1] = true;
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
            HashSet<string> liensAffiches = new HashSet<string>(); /// évite les doublons

            foreach (var noeud in Noeuds.Values)
            {
                foreach (var lien in noeud.Listeliens)
                {
                    string idLien = $"{lien.Noeud1.Nom}-{lien.Noeud2.Nom}";
                    string idLienInverse = $"{lien.Noeud2.Nom}-{lien.Noeud1.Nom}";

                    if (!liensAffiches.Contains(idLien) && !liensAffiches.Contains(idLienInverse))
                    {
                        Console.WriteLine($"{lien.Noeud1.Nom} <-> {lien.Noeud2.Nom} : {lien.Poids} min");
                        liensAffiches.Add(idLien); /// on mémorise le lien pour pas le répéter
                    }
                }
            }
        }

        public void AfficherMatriceAdjacence()
        {
            Console.WriteLine("\n===== Matrice d'Adjacence =====");

            Console.Write("   ");
            foreach (var noeud in ListeAdjacence)
            {
                Console.Write($"{noeud.Nom.Substring(0, Math.Min(3, noeud.Nom.Length))} "); /// abréviation du nom pour gain de place
            }
            Console.WriteLine();

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

        public (Dictionary<string, float> distances, Dictionary<string, string> predecessors) Dijkstra(T source)
        {
            var distances = new Dictionary<string, float>(); /// stocke la distance minimale trouvée
            var predecessors = new Dictionary<string, string>(); /// permet de reconstruire le chemin
            var nonVisites = new HashSet<T>(Noeuds.Keys); /// ensemble des sommets encore à visiter

            foreach (var noeud in Noeuds.Values)
            {
                distances[noeud.Nom] = float.PositiveInfinity;
                predecessors[noeud.Nom] = null;
            }
            distances[source.ToString()] = 0; /// point de départ

            while (nonVisites.Count > 0)
            {
                T courant = default(T);
                float minDistance = float.PositiveInfinity;

                /// on choisit le noeud non visité avec la plus petite distance connue
                foreach (var noeud in nonVisites)
                {
                    if (distances[noeud.ToString()] < minDistance)
                    {
                        minDistance = distances[noeud.ToString()];
                        courant = noeud;
                    }
                }

                if (EqualityComparer<T>.Default.Equals(courant, default(T))) break; /// plus rien d’atteignable

                nonVisites.Remove(courant);

                /// mise à jour des distances pour les voisins
                foreach (var lien in Noeuds[courant].Listeliens)
                {
                    var voisin = lien.Noeud1.Nom == courant.ToString() ? lien.Noeud2.Nom : lien.Noeud1.Nom;
                    var nouvelleDistance = distances[courant.ToString()] + lien.Poids;

                    if (nouvelleDistance < distances[voisin])
                    {
                        distances[voisin] = nouvelleDistance;
                        predecessors[voisin] = courant.ToString();
                    }
                }
            }

            return (distances, predecessors);
        }

        public (Dictionary<string, float> distances, Dictionary<string, string> predecessors) BellmanFord(T source)
        {
            var distances = new Dictionary<string, float>();
            var predecessors = new Dictionary<string, string>();

            foreach (var noeud in Noeuds.Values)
            {
                distances[noeud.Nom] = float.PositiveInfinity;
                predecessors[noeud.Nom] = null;
            }
            distances[source.ToString()] = 0;

            /// on répète |V|-1 fois le processus de relaxation des arêtes
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
            /// on remonte les prédécesseurs depuis la destination jusqu’à la source
            for (string noeud = destination; noeud != null; noeud = predecessors[noeud])
            {
                chemin.Insert(0, noeud); /// on insère au début pour avoir le chemin dans le bon sens
            }
            return chemin;
        }

        public double[,] FloydWarshall()
        {
            int n = ListeAdjacence.Count;
            double[,] distances = new double[n, n]; /// matrice des distances minimales
            int[,] chemins = new int[n, n]; /// matrice des précédents (utile si on veut reconstruire les chemins plus tard)

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

            /// initialisation des distances connues (liens directs)
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

            /// on applique l’algorithme pour chaque triplet (i, j, k)
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            chemins[i, j] = chemins[k, j]; /// mise à jour du chemin optimal
                        }
                    }
                }
            }

            return distances;
        }
    }
}
