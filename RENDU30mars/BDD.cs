using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RENDU30mars
{
    internal class BDD
    {
        //string connectionString = "SERVER=localhost;PORT=3306; DATABASE=Projet_LivinParis;UID=nouvel_utilisateur;PASSWORD=mot_de_passe_secure";
        //MySqlConnection connection = new MySqlConnection(connectionString);
        //connection.Open();

        //MySqlCommand command = connection.CreateCommand();
        //command.CommandText = "SELECT ID_Client, ID_Utilisateur FROM Client;"; // exemple de requete bien-sur !

        //MySqlDataReader reader;
        //reader = command.ExecuteReader();

        ///* exemple de manipulation du resultat */
        //while (reader.Read())                           // parcours ligne par ligne
        //{
        //    string currentRowAsString = "";
        //    for (int i = 0; i<reader.FieldCount; i++)    // parcours cellule par cellule
        //    {
        //        string valueAsString = reader.GetValue(i).ToString();
        //        currentRowAsString += valueAsString + ", ";

        //    }
        //    Console.WriteLine(currentRowAsString);    // affichage de la ligne (sous forme d'une "grosse" string) sur la sortie standard
        //    string currentRowAsString2 = "";
        //    string id_client = reader["ID_Client"].ToString();
        //    string id_utilisateur = reader["ID_Utilisateur"].ToString();// recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
        //    currentRowAsString2 += id_client + ", " + id_utilisateur + ", ";
        //    Console.WriteLine(currentRowAsString2);
        //}
        //reader.Close();
        //command.Dispose();

        //MySqlCommand command2 = connection.CreateCommand();
        //command2.CommandText = "INSERT INTO Utilisateur(ID_utilisateur, Mot_de_passe_utilisateur, Nom_utilisateur, Prénom_utilisateur, Adresse_utilisateur, Num_tel_utilisateur, Utilisateur_est_entreprise, Nom_entreprise) VALUES('115', 'pass123', 'Poe', 'Allan', '15 Rue Cardinet, Paris', '0690123956', FALSE, NULL)"; ; // exemple de requete bien-sur !
        //command2.ExecuteNonQuery();
        //command2.Dispose();
        //connection.Close();


    }
}
