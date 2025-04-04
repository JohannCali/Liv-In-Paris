
using System;
using System.IO;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using Rendu30mars;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;

namespace Rendu30mars
{
    public class Program
    {
        static void Main()
        {
            /// exemple de tests avec la base de données (désactivés pour l’instant)
            /// Site site = new Site();
            /// site.affichage();
            /// site.Client();
            /// site.CommandePlat();

            Graphe<string> graphe = new Graphe<string>();
            graphe.ChargerDepuisFichier("metro_paris.csv"); /// chargement des stations et connexions depuis un fichier CSV

            /// test Dijkstra entre deux stations
            string nomNoeudSource = "Tuileries";
            string nomNoeudCible = "Pigalle";

            var (distancesDijkstra, predecessorsDijkstra) = graphe.Dijkstra(nomNoeudSource);
            Console.WriteLine($"\nDijkstra - Distance minimale de {nomNoeudSource} à {nomNoeudCible} : {distancesDijkstra[nomNoeudCible]}");
            Console.WriteLine($"Chemin : {string.Join(" -> ", graphe.ReconstruireChemin(predecessorsDijkstra, nomNoeudCible))}");

            /// test Bellman-Ford entre les mêmes stations
            var (distancesBellmanFord, predecessorsBellmanFord) = graphe.BellmanFord(nomNoeudSource);
            Console.WriteLine($"\nBellman-Ford - Distance minimale de {nomNoeudSource} à {nomNoeudCible} : {distancesBellmanFord[nomNoeudCible]}");
            Console.WriteLine($"Chemin : {string.Join(" -> ", graphe.ReconstruireChemin(predecessorsBellmanFord, nomNoeudCible))}");

            /// exécution de Floyd-Warshall pour toutes les paires
            double[,] distancesFloydWarshall = graphe.FloydWarshall();
            Console.WriteLine("\nMatrice des distances les plus courtes (Floyd-Warshall) :");

            for (int i = 0; i < distancesFloydWarshall.GetLength(0); i++)
            {
                for (int j = 0; j < distancesFloydWarshall.GetLength(1); j++)
                {
                    /// affichage INF si les deux noeuds ne sont pas connectés
                    Console.Write(distancesFloydWarshall[i, j] == double.PositiveInfinity ? "INF\t" : $"{distancesFloydWarshall[i, j]}\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();
        }

        /// méthode pour dessiner un graphe à partir d’une matrice d’adjacence (optionnel)
        public static void DessinerGraphe(bool[,] adjMatrix, int n, string filePath)
        {
            int width = 500, height = 500;
            int rayon = 10;
            float centreX = width / 2, centreY = height / 2;
            float rayonCercle = Math.Min(width, height) / 2 - 50;

            SKBitmap bitmap = new SKBitmap(width, height);
            SKCanvas canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White); /// fond blanc

            var paintNoeud = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var paintArete = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                IsAntialias = true
            };

            var paintTexte = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                TextSize = 16,
                TextAlign = SKTextAlign.Center
            };

            /// placement des noeuds en cercle
            SKPoint[] positions = new SKPoint[n];
            for (int i = 0; i < n; i++)
            {
                float angle = i * 2 * (float)Math.PI / n;
                positions[i] = new SKPoint(
                    centreX + rayonCercle * (float)Math.Cos(angle),
                    centreY + rayonCercle * (float)Math.Sin(angle)
                );
            }

            /// tracé des arêtes
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (adjMatrix[i, j])
                    {
                        canvas.DrawLine(positions[i].X, positions[i].Y, positions[j].X, positions[j].Y, paintArete);
                    }
                }
            }

            /// tracé des noeuds + leur identifiant
            for (int i = 0; i < n; i++)
            {
                canvas.DrawCircle(positions[i], rayon, paintNoeud);
                canvas.DrawText(i.ToString(), positions[i].X, positions[i].Y - rayon - 5, paintTexte);
            }

            /// sauvegarde de l’image PNG
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(filePath))
            {
                data.SaveTo(stream);
                Console.WriteLine($"Image sauvegardée à l'emplacement : {filePath}");
            }
        }
    }
}
