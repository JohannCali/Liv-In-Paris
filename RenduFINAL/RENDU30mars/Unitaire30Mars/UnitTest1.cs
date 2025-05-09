using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rendu30mars;
using System.Collections.Generic;

namespace TestsUnitaires
{
    [TestClass]
    public class GrapheTests
    {
        [TestMethod]
        public void TestBellmanFord_SimpleGraph()
        {
            // Arrange
            var graphe = new Graphe();

            var a = new Noeud("A");
            var b = new Noeud("B");
            var c = new Noeud("C");

            a.AjouterLien(b, 1);
            b.AjouterLien(c, 2);
            a.AjouterLien(c, 5);

            graphe.Noeuds.Add("A", a);
            graphe.Noeuds.Add("B", b);
            graphe.Noeuds.Add("C", c);

            // Act
            var (distances, predecessors) = graphe.BellmanFord("A");

            // Assert
            Assert.AreEqual(0, distances["A"]);
            Assert.AreEqual(1, distances["B"]);
            Assert.AreEqual(3, distances["C"]);
            Assert.AreEqual("B", predecessors["C"]);
        }

        [TestMethod]
        public void TestFloydWarshall_SimpleGraph()
        {
            // Arrange
            var graphe = new Graphe();

            var a = new Noeud("A");
            var b = new Noeud("B");
            var c = new Noeud("C");

            a.AjouterLien(b, 1);
            b.AjouterLien(c, 2);
            a.AjouterLien(c, 5);

            graphe.ListeAdjacence.Add(a);
            graphe.ListeAdjacence.Add(b);
            graphe.ListeAdjacence.Add(c);

            // Act
            var distances = graphe.FloydWarshall();

            // Assert
            Assert.AreEqual(0, distances[0, 0]);
            Assert.AreEqual(1, distances[0, 1]);
            Assert.AreEqual(3, distances[0, 2]); // A -> B -> C = 1 + 2
        }
    }
}
