using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rendu30mars;
using System.Collections.Generic;

namespace Rendu30mars.Tests
{
    [TestClass]
    public class GrapheTests
    {
        private Graphe<string> graphe;

        [TestInitialize]
        public void Setup()
        {
            graphe = new Graphe<string>();
            // Initialiser le graphe avec des données de test
            graphe.Noeuds = new Dictionary<string, Noeud>
            {
                { "A", new Noeud("A") },
                { "B", new Noeud("B") },
                { "C", new Noeud("C") },
                { "D", new Noeud("D") }
            };

            graphe.Noeuds["A"].AjouterLien(graphe.Noeuds["B"], 1);
            graphe.Noeuds["A"].AjouterLien(graphe.Noeuds["C"], 4);
            graphe.Noeuds["B"].AjouterLien(graphe.Noeuds["C"], 2);
            graphe.Noeuds["B"].AjouterLien(graphe.Noeuds["D"], 5);
            graphe.Noeuds["C"].AjouterLien(graphe.Noeuds["D"], 1);
        }

        [TestMethod]
        public void TestDijkstra()
        {
            var (distances, predecessors) = graphe.Dijkstra("A");

            Assert.AreEqual(0, distances["A"]);
            Assert.AreEqual(1, distances["B"]);
            Assert.AreEqual(3, distances["C"]);
            Assert.AreEqual(4, distances["D"]);

            CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, graphe.ReconstruireChemin(predecessors, "D").ToArray());
        }

        [TestMethod]
        public void TestBellmanFord()
        {
            var (distances, predecessors) = graphe.BellmanFord("A");

            Assert.AreEqual(0, distances["A"]);
            Assert.AreEqual(1, distances["B"]);
            Assert.AreEqual(3, distances["C"]);
            Assert.AreEqual(4, distances["D"]);

            CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, graphe.ReconstruireChemin(predecessors, "D").ToArray());
        }

        [TestMethod]
        public void TestFloydWarshall()
        {
            double[,] distances = graphe.FloydWarshall();

            Assert.AreEqual(0, distances[0, 0]);
            Assert.AreEqual(1, distances[0, 1]);
            Assert.AreEqual(3, distances[0, 2]);
            Assert.AreEqual(4, distances[0, 3]);

            Assert.AreEqual(1, distances[1, 0]);
            Assert.AreEqual(0, distances[1, 1]);
            Assert.AreEqual(2, distances[1, 2]);
            Assert.AreEqual(5, distances[1, 3]);

            Assert.AreEqual(4, distances[2, 0]);
            Assert.AreEqual(2, distances[2, 1]);
            Assert.AreEqual(0, distances[2, 2]);
            Assert.AreEqual(1, distances[2, 3]);

            Assert.AreEqual(4, distances[3, 0]);
            Assert.AreEqual(5, distances[3, 1]);
            Assert.AreEqual(1, distances[3, 2]);
            Assert.AreEqual(0, distances[3, 3]);
        }
    }
}
