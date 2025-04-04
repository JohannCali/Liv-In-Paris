using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendu30mars
{
    internal class Site
    {
        private MySqlConnection connection;

        /// initialisation de la connexion à la BDD
        public Site()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=LIV;UID=nouvel_utilisateur;PASSWORD=mot_de_passe_secure";
            connection = new MySqlConnection(connectionString);
        }

        /// ouverture de la BDD
        public void ConnexionBD()
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connexion à la base de données réussie !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
            }
        }

        /// fermeture propre de la BDD
        public void DeconnexionBD()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Déconnexion réussie !");
            }
        }

        public void InscriptionClient(string idUtilisateur, string nom, string prenom, string adresse, string email, string motdepasse, string telephone)
        {
            try
            {
                Console.WriteLine("Tentative d'inscription du client...");

                string query = "INSERT INTO Utilisateur (Id_Utilisateur, Nom, Prenom, Adresse, Email, Mot_de_Passe, Numero_de_Telephone) " +
                               "VALUES (@Id_Utilisateur, @Nom, @Prenom, @Adresse, @Email, @Mot_de_Passe, @Numero_de_Telephone)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Adresse", adresse);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Mot_de_Passe", motdepasse);
                    cmd.Parameters.AddWithValue("@Numero_de_Telephone", telephone);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine("Client inscrit avec succès !");
                    else
                        Console.WriteLine("Erreur lors de l'inscription du client.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'inscription du client : " + ex.Message);
            }
            finally
            {
                DeconnexionBD();
            }
        }

        public void InscriptionCuisinier(string idUtilisateur, string specialite)
        {
            try
            {
                Console.WriteLine("Tentative d'inscription du cuisinier...");

                string query = "INSERT INTO Cuisinier (Id_Utilisateur, Specialite) VALUES (@Id_Utilisateur, @Specialite)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);
                    cmd.Parameters.AddWithValue("@Specialite", specialite);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine("Cuisinier inscrit avec succès !");
                    else
                        Console.WriteLine("Erreur lors de l'inscription du cuisinier.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'inscription du cuisinier : " + ex.Message);
            }
            finally
            {
                DeconnexionBD();
            }
        }

        public void PreparationPlat()
        {
            Console.WriteLine("Bienvenue dans l'espace préparation !");

            string idPlat = Guid.NewGuid().ToString();
            string nomPlat = DemanderTexte("Entrez le nom du plat : ");
            string typePlat = DemanderTypePlat("Quel est le type du plat ? (Entree, Plat Principal, Dessert)");
            string nbParts = DemanderNbParts("Pour combien de personnes est ce plat ? ");
            string nationalite = DemanderTexte("Quelle est la nationalité du plat ? ");
            string regimeAlimentaire = DemanderTexte("Quel est le régime alimentaire du plat ? ");
            string ingredients = DemanderTexte2("Listez les ingrédients du plat (séparés par des virgules) : ");
            decimal prixParPortion = DemanderPrix("Quel est le prix par portion (€) ? ");
            DateTime dateFabrication = DemanderDate("Date de fabrication ? (aaaa-MM-jj HH:mm:ss) ");
            DateTime datePeremption = DemanderDate("Date de péremption ? (aaaa-MM-jj HH:mm:ss) ");

            ConnexionBD();
            try
            {
                string query = "INSERT INTO Plats (Id_Plat, Nom, Type_Plat, Pour_combien_de_personnes, Nationalite, Regime_Alimentaire, Ingrédients, Date_de_fabrication, Date_de_Peremption, Prix_par_Portion) " +
                               "VALUES (@Id_Plat, @Nom, @Type_Plat, @Pour_combien_de_personnes, @Nationalite, @Regime_Alimentaire, @Ingrédients, @Date_de_fabrication, @Date_de_Peremption, @Prix_par_Portion)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id_Plat", idPlat);
                    cmd.Parameters.AddWithValue("@Nom", nomPlat);
                    cmd.Parameters.AddWithValue("@Type_Plat", typePlat);
                    cmd.Parameters.AddWithValue("@Pour_combien_de_personnes", nbParts);
                    cmd.Parameters.AddWithValue("@Nationalite", nationalite.ToLower());
                    cmd.Parameters.AddWithValue("@Regime_Alimentaire", regimeAlimentaire);
                    cmd.Parameters.AddWithValue("@Ingrédients", ingredients);
                    cmd.Parameters.AddWithValue("@Date_de_fabrication", dateFabrication);
                    cmd.Parameters.AddWithValue("@Date_de_Peremption", datePeremption);
                    cmd.Parameters.AddWithValue("@Prix_par_Portion", prixParPortion);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine("Plat ajouté avec succès !");
                    else
                        Console.WriteLine("Erreur lors de l'ajout du plat.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout du plat : " + ex.Message);
            }
            finally
            {
                DeconnexionBD();
            }
        }

        public void CommandePlat()
        {
            Console.WriteLine("Bienvenue dans l'espace commande !");
            Console.WriteLine("Quel type de cuisine recherchez-vous ?");
            string type = Console.ReadLine();

            ConnexionBD();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
            SELECT Nom, Type_Plat, Pour_combien_de_personnes, 
                   Regime_Alimentaire, Ingrédients, Date_de_fabrication, 
                   Date_de_Peremption, Prix_par_Portion 
            FROM Plats 
            WHERE Nationalite = @Nationalite";

            command.Parameters.AddWithValue("@Nationalite", type);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("\nPlats disponibles :");
                while (reader.Read())
                {
                    Console.WriteLine($"Nom : {reader["Nom"]}");
                    Console.WriteLine($"Type : {reader["Type_Plat"]}");
                    Console.WriteLine($"Portions : {reader["Pour_combien_de_personnes"]}");
                    Console.WriteLine($"Régime : {reader["Regime_Alimentaire"]}");
                    Console.WriteLine($"Ingrédients : {reader["Ingrédients"]}");
                    Console.WriteLine($"Fabrication : {reader["Date_de_fabrication"]}");
                    Console.WriteLine($"Péremption : {reader["Date_de_Peremption"]}");
                    Console.WriteLine($"Prix : {reader["Prix_par_Portion"]} €");
                    Console.WriteLine("------------------------------");
                }
            }

            DeconnexionBD();
        }

                public void AfficherPlats()
        {
            try
            {
                string query = "SELECT Id_Plat, Nom, Type_Plat, Prix_par_Portion FROM Plats";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string idPlat = reader["Id_Plat"].ToString();
                    string nomPlat = reader["Nom"].ToString();
                    string typePlat = reader["Type_Plat"].ToString();
                    decimal prixPlat = Convert.ToDecimal(reader["Prix_par_Portion"]);

                    Console.WriteLine($"{idPlat} - {nomPlat} ({typePlat}) - {prixPlat} €");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }

        public void affichage()
        {
            Console.WriteLine("BIENVENUE SUR LIVINPARIS");
            Console.WriteLine("vous êtes-vous déjà inscrit ?");
            int choix = Choix();

            if (choix == 2)
            {
                Client(); /// si pas inscrit, inscription client
            }
            else
            {
                Console.WriteLine("Etes-vous cuisinier ?");
                int choix2 = Choix();

                if (choix2 == 2)
                {
                    Console.WriteLine("Voulez-vous être cuisinier ?");
                    int choix3 = Choix();

                    if (choix3 == 1)
                        Cuisinier(); /// inscription en tant que cuisinier
                    else
                        CommandePlat(); /// redirige vers la commande
                }
                else
                {
                    PreparationPlat(); /// déjà cuisinier : aller en préparation
                }
            }
        }

        public int Choix()
        {
            int choix;
            do
            {
                Console.WriteLine("Choisissez : oui (1) ou non (2) ?");
                string saisie = Console.ReadLine();

                if (!int.TryParse(saisie, out choix) || (choix != 1 && choix != 2))
                    Console.WriteLine("Erreur : Veuillez entrer 1 ou 2.");
            } while (choix != 1 && choix != 2);

            return choix;
        }

        public void Client()
        {
            Console.WriteLine("INSCRIPTION CLIENT");

            string nom = DemanderTexte("Entrez votre nom : ");
            string prenom = DemanderTexte("Entrez votre prénom : ");
            string adresse = DemanderAdresse("Entrez votre adresse (adresse + code postale) : ");
            string email = DemanderTexte("Entrez votre adresse e-mail : ");
            string telephone = DemanderTelephone("Entrez votre numéro de téléphone (10 chiffres) : ");
            string motdepasse = DemanderMotdePasse("Entrez un mot de passe (4 chiffres) : ");

            Console.WriteLine("\nInscription réussie !");
            Console.WriteLine($"{prenom} {nom}");
            Console.WriteLine($"{adresse}");
            Console.WriteLine($"{email}");
            Console.WriteLine($"{telephone}");
            Console.WriteLine($"{motdepasse}");

            string idUtilisateur = Guid.NewGuid().ToString(); /// ID unique

            ConnexionBD();
            InscriptionClient(idUtilisateur, nom, prenom, adresse, email, motdepasse, telephone);
            DeconnexionBD();

            Console.WriteLine("Voulez-vous faire une commande ?");
            int choix3 = Choix();
            if (choix3 != 1)
                CommandePlat();
            else
            {
                Console.WriteLine("Appuyez sur une touche pour fermer...");
                Console.ReadKey();
                return;
            }
        }

        public void Cuisinier()
        {
            string idUtilisateur = DemanderId("Quel est votre id ?");
            Console.WriteLine("Spécialité ?");
            string specialite = Console.ReadLine();
            Console.WriteLine($"{specialite}");

            ConnexionBD();
            InscriptionCuisinier(idUtilisateur, specialite);
            DeconnexionBD();

            PreparationPlat();
        }

        /// ---------------- MÉTHODES DE VALIDATION ----------------

        private string DemanderTexte(string message)
        {
            string entree;
            do
            {
                Console.Write(message);
                entree = Console.ReadLine();

                if (message == "Entrez votre adresse e-mail : ")
                {
                    if (!string.IsNullOrWhiteSpace(entree) && entree.Contains("@"))
                        break;
                }
                else if (!string.IsNullOrWhiteSpace(entree) && EstUnTexteValide(entree))
                {
                    break;
                }
            } while (true);
            return entree;
        }

        private string DemanderAdresse(string message)
        {
            string adresse;
            do
            {
                Console.Write(message);
                adresse = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(adresse) || !ContientCodePostalParis(adresse));
            return adresse;
        }

        private bool ContientCodePostalParis(string adresse)
        {
            for (int i = 1; i <= 20; i++)
            {
                string codePostal = GenererCodePostal(i);
                if (adresse.Contains(codePostal))
                    return true;
            }
            return false;
        }

        private string GenererCodePostal(int i)
        {
            string numero = i < 10 ? "0" + i : i.ToString();
            return "750" + numero;
        }

        private string DemanderTexte2(string message)
        {
            string input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        private string DemanderTelephone(string message)
        {
            string telephone;
            do
            {
                Console.Write(message);
                telephone = Console.ReadLine();
                if (!EstUnNumeroValide(telephone))
                    Console.WriteLine("Le numéro doit contenir exactement 10 chiffres !");
            } while (!EstUnNumeroValide(telephone));
            return telephone;
        }

        private string DemanderMotdePasse(string message)
        {
            string motdepasse;
            do
            {
                Console.Write(message);
                motdepasse = Console.ReadLine();
                if (!EstUnNumeroValide2(motdepasse))
                    Console.WriteLine("Le mot de passe doit contenir exactement 4 chiffres !");
            } while (!EstUnNumeroValide2(motdepasse));
            return motdepasse;
        }

        private string DemanderNbParts(string message)
        {
            string input;
            int nbParts;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (int.TryParse(input, out nbParts) && nbParts >= 1 && nbParts <= 10)
                    return input;
                Console.WriteLine("Erreur : Le nombre de parts doit être entre 1 et 10.");
            } while (true);
        }

        private string DemanderId(string message)
        {
            string idUtilisateur;
            do
            {
                Console.Write(message);
                idUtilisateur = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(idUtilisateur))
                {
                    Console.WriteLine("Erreur : L'ID ne peut pas être vide.");
                    continue;
                }

                if (!VerifierId(idUtilisateur))
                {
                    Console.WriteLine("Erreur : ID inexistant.");
                }
                else
                {
                    return idUtilisateur;
                }

            } while (true);
        }

        private string DemanderTypePlat(string message)
        {
            string[] typesValides = { "Entree", "Plat Principal", "Dessert" };
            string type;
            do
            {
                Console.Write(message);
                type = Console.ReadLine();
                if (typesValides.Contains(type, StringComparer.OrdinalIgnoreCase))
                    return type;
                Console.WriteLine("Type de plat invalide !");
            } while (true);
        }

        private decimal DemanderPrix(string message)
        {
            decimal prix;
            do
            {
                Console.Write(message);
                if (decimal.TryParse(Console.ReadLine(), out prix) && prix >= 0)
                    return prix;
                Console.WriteLine("Veuillez entrer un prix valide !");
            } while (true);
        }

        private DateTime DemanderDate(string message)
        {
            DateTime date;
            do
            {
                Console.Write(message);
                if (DateTime.TryParse(Console.ReadLine(), out date))
                    return date;
                Console.WriteLine("Format attendu : yyyy-MM-dd HH:mm:ss");
            } while (true);
        }

        /// ---------------- MÉTHODES UTILITAIRES ----------------

        private bool EstUnTexteValide(string texte)
        {
            return texte.All(char.IsLetter);
        }

        private bool EstUnNumeroValide(string numero)
        {
            return numero.Length == 10 && numero.All(char.IsDigit);
        }

        private bool EstUnNumeroValide2(string numero)
        {
            return numero.Length == 4 && numero.All(char.IsDigit);
        }

        private bool VerifierId(string idUtilisateur)
        {
            try
            {
                ConnexionBD();
                string query = "SELECT COUNT(*) FROM Utilisateur WHERE Id_Utilisateur = @Id_Utilisateur";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la vérification de l'ID : " + ex.Message);
                return false;
            }
            finally
            {
                DeconnexionBD();
            }
        }
    }
}
