using System;
using System.IO;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using Rendu30mars;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;

namespace RENDU30mars
{


    public class Program
    {
        static void Main()
        {

            //Site site = new Site();
            //site.affichage();
            //site.Client();
            //site.CommandePlat();

            // Création d'une instance de Graphe
            Graphe<string> graphe = new Graphe<string>();

            // Chargement du graphe depuis un fichier
            graphe.ChargerDepuisFichier("metro_paris.csv");

            // Exemple d'utilisation de l'algorithme de Bellman-Ford
            string nomNoeudSource = "Tuileries"; // Remplacez par le nom du nœud source
            string nomNoeudCible = "Pigalle"; // Remplacez par le nom du nœud cible

            var (distances, predecessors) = graphe.BellmanFord(nomNoeudSource); 

            // Affichage du plus court chemin vers le nœud cible
            if (distances.ContainsKey(nomNoeudCible))
            {
                Console.WriteLine($"Station de départ : {nomNoeudSource}");
                Console.WriteLine($"Station d'arrivée : {nomNoeudCible}");
                Console.WriteLine($"Le chemin le plus court est : {string.Join(" -> ", graphe.ReconstruireChemin(predecessors, nomNoeudCible))} et prend {distances[nomNoeudCible]} minutes.");
            }
            else
            {
                Console.WriteLine($"Aucun chemin trouvé entre {nomNoeudSource} et {nomNoeudCible}.");
            }

            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();

            //Graphe<string> graphe = new Graphe<string>();

            //// Chargement du graphe depuis un fichier
            //graphe.ChargerDepuisFichier("metro_paris.csv");

            //// Exemple d'utilisation de l'algorithme de Dijkstra
            //string nomNoeudSource = "Tuileries"; // Remplacez par le nom du nœud source
            //string nomNoeudCible = "Pigalle"; // Remplacez par le nom du nœud cible

            //var resultatDijkstra = graphe.Dijkstra(nomNoeudSource);

            //// Affichage du plus court chemin vers le nœud cible
            //if (resultatDijkstra.ContainsKey(nomNoeudCible))
            //{
            //    Console.WriteLine($"Le plus court chemin de {nomNoeudSource} à {nomNoeudCible} est de {resultatDijkstra[nomNoeudCible]} minutes.");

            //    // Reconstruction du chemin
            //    var chemin = ReconstruireChemin(graphe.Noeuds, nomNoeudSource, nomNoeudCible);
            //    Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", chemin));
            //}
            //else
            //{
            //    Console.WriteLine($"Aucun chemin trouvé entre {nomNoeudSource} et {nomNoeudCible}.");
            //}

            //Console.WriteLine("Appuyez sur une touche pour quitter...");
            //Console.ReadKey();
        }

        static List<string> ReconstruireChemin(Dictionary<string, Noeud> noeuds, string source, string cible)
        {
            var chemin = new List<string>();
            var predecesseurs = new Dictionary<string, string>();

            // Initialisation des prédecesseurs
            foreach (var noeud in noeuds.Keys)
            {
                predecesseurs[noeud] = null;
            }

            predecesseurs[source] = null;

            // Simulation de Dijkstra pour obtenir les prédecesseurs
            var distances = new Graphe<string>().Dijkstra(source);

            // Reconstruction du chemin
            string etape = cible;
            while (etape != null)
            {
                chemin.Add(etape);
                etape = predecesseurs[etape];
            }

            chemin.Reverse(); // Inverser pour avoir le chemin de la source à la cible
            return chemin;
        }


        //public static void DessinerGraphe(bool[,] adjMatrix, int n, string filePath)
        //{
        //    int width = 500, height = 500;
        //    int rayon = 10;
        //    float centreX = width / 2, centreY = height / 2;
        //    float rayonCercle = Math.Min(width, height) / 2 - 50;
        //    SKBitmap bitmap = new SKBitmap(width, height);
        //    SKCanvas canvas = new SKCanvas(bitmap);
        //    canvas.Clear(SKColors.White);

        //    SKPaint paintNoeud = new SKPaint
        //    {
        //        Color = SKColors.Blue,
        //        IsAntialias = true,
        //        Style = SKPaintStyle.Fill
        //    };

        //    SKPaint paintArete = new SKPaint
        //    {
        //        Color = SKColors.Black,
        //        StrokeWidth = 2,
        //        IsAntialias = true
        //    };

        //    SKPaint paintTexte = new SKPaint
        //    {
        //        Color = SKColors.Black,
        //        IsAntialias = true,
        //        TextSize = 16,
        //        TextAlign = SKTextAlign.Center
        //    };

        //    SKPoint[] positions = new SKPoint[n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        float angle = i * 2 * (float)Math.PI / n;
        //        positions[i] = new SKPoint(
        //            centreX + rayonCercle * (float)Math.Cos(angle),
        //            centreY + rayonCercle * (float)Math.Sin(angle)
        //        );
        //    }

        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = i + 1; j < n; j++)
        //        {
        //            if (adjMatrix[i, j])
        //            {
        //                canvas.DrawLine(positions[i].X, positions[i].Y, positions[j].X, positions[j].Y, paintArete);
        //            }
        //        }
        //    }

        //    for (int i = 0; i < n; i++)
        //    {
        //        canvas.DrawCircle(positions[i], rayon, paintNoeud);
        //        canvas.DrawText(i.ToString(), positions[i].X, positions[i].Y - rayon - 5, paintTexte);
        //    }

        //    using (var image = SKImage.FromBitmap(bitmap))
        //    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        //    using (var stream = File.OpenWrite(filePath))
        //    {
        //        data.SaveTo(stream);
        //        Console.WriteLine($"Image sauvegardée à l'emplacement : {filePath}");
        //    }
        //}        
    }
}

