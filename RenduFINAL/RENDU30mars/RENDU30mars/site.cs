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

        // Constructeur pour initialiser la connexion à la base de données
        public Site()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=LIV;UID=root;PASSWORD=Thomas_mange1rat;";
            connection = new MySqlConnection(connectionString);
        }
        // Connexion BDD
        public void ConnexionBD()
        {
            try
            {
                // Ouverture de la connexion à la base de données
                connection.Open();
                Console.WriteLine("Connexion à la base de données réussie !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
            }
        }

        // Déconnexion de la base de données
        public void DeconnexionBD()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Déconnexion réussie !");
            }
        }





        //============== Programes qui font appel à la BDD============
        public void InscriptionClient(string idUtilisateur, string nom, string prenom, string adresse, string email, string motdepasse, string telephone)
        {
            try
            {
                Console.WriteLine("Tentative d'inscription du client...");

                string query = "INSERT INTO Utilisateur (Id_Utilisateur, Nom, Prenom, Adresse, Email, Mot_de_Passe, Numero_de_Telephone) " +
                               "VALUES (@Id_Utilisateur, @Nom, @Prenom, @Adresse, @Email, @Mot_de_Passe, @Numero_de_Telephone)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Paramètres pour la requête SQL
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Adresse", adresse);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Mot_de_Passe", motdepasse);
                    cmd.Parameters.AddWithValue("@Numero_de_Telephone", telephone);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Client inscrit avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Erreur lors de l'inscription du client.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'inscription du client : " + ex.Message);
            }
            finally
            {
                DeconnexionBD(); // Ferme la connexion même en cas d'erreur
            }
        }

        public void InscriptionCuisinier(string idUtilisateur, string specialite)
        {
            try
            {
                Console.WriteLine("Tentative d'inscription du cuisinier...");

                string query = "INSERT INTO Cuisinier (Id_Utilisateur, Specialite) " +
                               "VALUES (@Id_Utilisateur, @Specialite)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Paramètres pour la requête SQL
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);
                    cmd.Parameters.AddWithValue("@Specialite", specialite);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Cuisinier inscrit avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Erreur lors de l'inscription du cuisinier.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'inscription du cuisinier : " + ex.Message);
            }
            finally
            {
                DeconnexionBD(); // Ferme la connexion même en cas d'erreur
            }
        }

        public void PreparationPlat()
        {
            Console.WriteLine("Bienvenue dans l'espace préparation !");

            // Génération d'un ID unique pour le plat
            string idPlat = Guid.NewGuid().ToString();

            // Demande du nom du plat
            string nomPlat = DemanderTexte("Entrez le nom du plat : ");

            // Sélection du type de plat
            string typePlat = DemanderTypePlat("Quel est le type du plat ? (Entree, Plat Principal, Dessert)");

            // Demande du nombre de parts
            string nbParts = DemanderNbParts("Pour combien de personnes est ce plat ? ");

            // Demande de la nationalité
            string nationalite = DemanderTexte("Quelle est la nationalité du plat ? ");

            // Demande du régime alimentaire (optionnel)
            string regimeAlimentaire = DemanderTexte("Quel est le régime alimentaire du plat ? (ex: Végétarien, Sans gluten, etc.)");

            // Demande des ingrédients
            string ingredients = DemanderTexte2("Listez les ingrédients du plat (séparés par des virgules) : ");

            // Demande du prix par portion
            decimal prixParPortion = DemanderPrix("Quel est le prix par portion (€) ? ");

            // Définition des dates
            DateTime dateFabrication = DemanderDate("Quelle est la date de fabrication ? (aaaa-MM-jj HH:mm:ss)");
            DateTime datePeremption = DemanderDate("Quelle est la date de péremption ? (aaaa-MM-jj HH:mm:ss)");

            // Insertion dans la base de données
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
                    {
                        Console.WriteLine("Plat ajouté avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Erreur lors de l'ajout du plat.");
                    }
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

        public bool VerifierIdentifiants(string email, string motDePasse)
        {
            string query = "SELECT COUNT(*) FROM Utilisateur WHERE Email = @Email AND Mot_de_Passe = @Mot_dePasse";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Paramétrage de la requête
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Mot_dePasse", motDePasse);

            try
            {
                Console.WriteLine("Vérification de la connexion à la base de données...");
                // S'assurer que la connexion est ouverte avant d'exécuter la commande
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connexion fermée, tentative d'ouverture...");
                    connection.Open();  // Ouvrir la connexion si elle n'est pas déjà ouverte
                }

                Console.WriteLine("Exécution de la requête SQL...");
                // Exécution de la commande et récupération du résultat
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                Console.WriteLine($"Nombre d'utilisateurs trouvés : {count}");

                // Si l'utilisateur existe, retourner true
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la vérification des identifiants : " + ex.Message);
                return false;  // Retourner false en cas d'erreur
            }
            finally
            {
                // Toujours fermer la connexion après la requête
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();  // Fermer la connexion
                }
            }
        }

        public void CommandePlat()
        {
            Console.WriteLine("Bienvenue dans l'espace commande !");
            Console.WriteLine("\nVoici les offres disponibles ");
            Console.WriteLine("\nQuel type de cuisine recherchez-vous ?");
            string type = Console.ReadLine(); // Pas besoin de Convert.ToString()

            ConnexionBD(); // Connexion à la base de données

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"
        SELECT Nom, Type_Plat,Pour_combien_de_personnes, 
               Regime_Alimentaire, Ingrédients, Date_de_fabrication, 
               Date_de_Peremption, Prix_par_Portion 
        FROM Plats 
        WHERE Nationalite = @Nationalite;";

            command.Parameters.AddWithValue("@Nationalite", type);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("\nPlats disponibles pour la cuisine " + type + " :");
                Console.WriteLine("----------------------------------------------------------");

                while (reader.Read())
                {
                    Console.WriteLine($"Nom : {reader["Nom"]}");
                    Console.WriteLine($"Type de plat : {reader["Type_Plat"]}");

                    Console.WriteLine($"Portions : {reader["Pour_combien_de_personnes"]}");
                    Console.WriteLine($"Régime Alimentaire : {reader["Regime_Alimentaire"]}");
                    Console.WriteLine($"Ingrédients : {reader["Ingrédients"]}");
                    Console.WriteLine($"Date de fabrication : {reader["Date_de_fabrication"]}");
                    Console.WriteLine($"Date de péremption : {reader["Date_de_Peremption"]}");
                    Console.WriteLine($"Prix par portion : {reader["Prix_par_Portion"]} euros");
                    Console.WriteLine("----------------------------------------------------------");
                }
            }

            DeconnexionBD(); // Déconnexion propre
        }

        public void AfficherPlats()
        {
            try
            {
                string query = "SELECT Id_Plat, Nom, Type_Plat, Prix_par_Portion FROM Plats";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                // Exécution de la requête et récupération des résultats
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string idPlat = reader["Id_Plat"].ToString();
                    string nomPlat = reader["Nom"].ToString();
                    string typePlat = reader["Type_Plat"].ToString();
                    decimal prixPlat = Convert.ToDecimal(reader["Prix_par_Portion"]);

                    Console.WriteLine($"{idPlat} - {nomPlat} ({typePlat}) - {prixPlat} €");
                }

                reader.Close();  // Fermer le reader après avoir parcouru les résultats
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



                {
                    Client();  //  Redirige vers la méthode client
                }


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
                    {
                        Cuisinier();
                    }
                    else
                    {
                        CommandePlat();
                    }



                }
                else
                {
                    PreparationPlat();
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
                {
                    Console.WriteLine(" Erreur : Veuillez entrer un nombre valide (1 ou 2).");
                }

            } while (choix != 1 && choix != 2);
            return choix;

        }

        public void Client()
        {
            Console.WriteLine("INSCRIPTION CLIENT");

            // Demander le Nom
            string nom = DemanderTexte("Entrez votre nom : ");

            // Demander le Prénom
            string prenom = DemanderTexte("Entrez votre prénom : ");

            // Demander l'Adresse (doit contenir "Paris")
            string adresse = DemanderAdresse("Entrez votre adresse (adresse + code postale) : ");

            // Demander l'Email
            string email = DemanderTexte("Entrez votre adresse e-mail : ");

            // Demander le Numéro de téléphone (10 chiffres)
            string telephone = DemanderTelephone("Entrez votre numéro de téléphone (10 chiffres) : ");

            string motdepasse = DemanderMotdePasse("entrez un mot de passe (4 chiffres) : ");


            Console.WriteLine("\n Inscription réussie !");
            Console.WriteLine($"{prenom} {nom}");
            Console.WriteLine($"{adresse}");
            Console.WriteLine($"{email}");
            Console.WriteLine($"{telephone}");
            Console.WriteLine($"{motdepasse}");

            string idUtilisateur = Guid.NewGuid().ToString(); // Exemple avec un GUID pour l'id unique

            ConnexionBD();

            // Insérer l'utilisateur dans la base de données
            InscriptionClient(idUtilisateur, nom, prenom, adresse, email, motdepasse, telephone);

            // Déconnexion de la base de données
            DeconnexionBD();

            Console.WriteLine("Voulez-vous faire une commande ?");
            int choix3 = Choix();
            if (choix3 != 1)
            {
                CommandePlat();
            }
            else
            {
                Console.WriteLine("Appuyez sur une touche pour fermer...");
                Console.ReadKey();
                return;
            }

        }
        public void Cuisinier()
        {
            // Demande un ID utilisateur existant
            string idUtilisateur = DemanderId("Quel est votre id ?");

            Console.WriteLine("Spécialité ?");
            string specialite = Console.ReadLine(); // Pas besoin de Convert.ToString()

            Console.WriteLine($"{specialite}");

            // Connexion à la base de données
            ConnexionBD();

            // Inscription du cuisinier avec l'ID existant
            InscriptionCuisinier(idUtilisateur, specialite);

            // Déconnexion de la base de données
            DeconnexionBD();

            PreparationPlat();
        }

        // ================== MÉTHODES DE VALIDATION ==================

        private string DemanderTexte(string message)
        {
            string entree;
            do
            {
                Console.Write(message);
                entree = Console.ReadLine();

                // Si on demande une adresse e-mail, on s'assure qu'il y a un '@'
                if (message == "Entrez votre adresse e-mail : ")
                {
                    if (!string.IsNullOrWhiteSpace(entree) && entree.Contains("@"))
                        break; // Sortie de la boucle si l'entrée est valide
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(entree) && EstUnTexteValide(entree))
                        break; // Sortie de la boucle si l'entrée est valide pour un texte normal
                }

            } while (true); // Continue tant que l'entrée est invalide

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
            // Vérifie si l'adresse contient un code postal de Paris (75001 à 75020)
            for (int i = 1; i <= 20; i++)
            {
                string codePostal = GenererCodePostal(i); // Génère "75001" à "75020"
                if (adresse.Contains(codePostal))
                {
                    return true;
                }
            }
            return false;
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


        // Fonction pour générer un code postal de Paris de 75001 à 75020
        private string GenererCodePostal(int i)
        {
            string numero;

            if (i < 10)
            {
                numero = "0" + i;  // Ajoute un "0" devant (ex: 01, 02, ..., 09)
            }
            else
            {
                numero = i.ToString();  // Conversion normale (ex: 10, 11, ..., 20)
            }

            return "750" + numero;  // Assemble le code postal complet
        }

        private string DemanderTelephone(string message)
        {
            string telephone;
            do
            {
                Console.Write(message);
                telephone = Console.ReadLine();
                if (!EstUnNumeroValide(telephone))
                {
                    Console.WriteLine("Le numéro doit contenir exactement 10 chiffres !");
                }
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
                {
                    Console.WriteLine("Le numéro doit contenir exactement 4 chiffres !");
                }


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

                // Vérifier si l'entrée est un nombre entier
                if (int.TryParse(input, out nbParts))
                {
                    // Vérifier que le nombre de parts est compris entre 1 et 10
                    if (nbParts >= 1 && nbParts <= 10)
                    {
                        return input; // Retourne la saisie de l'utilisateur si valide
                    }
                    else
                    {
                        Console.WriteLine("Erreur : Le nombre de parts doit être entre 1 et 10.");
                    }
                }
                else
                {
                    Console.WriteLine("Erreur : Veuillez entrer un nombre valide.");
                }

            } while (true); // La boucle continue jusqu'à ce qu'une saisie valide soit faite
        }

        private string DemanderId(string message)
        {
            string idUtilisateur;

            do
            {
                Console.Write(message);
                idUtilisateur = Console.ReadLine();

                // Vérifier si l'ID a été saisi
                if (string.IsNullOrWhiteSpace(idUtilisateur))
                {
                    Console.WriteLine("Erreur : L'ID ne peut pas être vide.");
                    continue; // Redemande la saisie
                }

                // Vérifier si l'ID existe dans la base de données
                if (!VerifierId(idUtilisateur))
                {
                    Console.WriteLine("Erreur : Cet ID n'existe pas. Veuillez entrer un ID valide.");
                }
                else
                {
                    return idUtilisateur; // Retourne l'ID si valide
                }

            } while (true); // Continue jusqu'à une saisie correcte
        }

        
        // Demande un type de plat valide
        private string DemanderTypePlat(string message)
        {
            string[] typesValides = { "Entree", "Plat Principal", "Dessert" };
            string type;
            do
            {
                Console.Write(message);
                type = Console.ReadLine();
                if (typesValides.Contains(type, StringComparer.OrdinalIgnoreCase))
                {
                    return type;
                }
                Console.WriteLine("Type de plat invalide !");
            } while (true);
        }

        // Demande un prix valide
        private decimal DemanderPrix(string message)
        {
            decimal prix;
            do
            {
                Console.Write(message);
                if (decimal.TryParse(Console.ReadLine(), out prix) && prix >= 0)
                {
                    return prix;
                }
                Console.WriteLine("Veuillez entrer un prix valide !");
            } while (true);
        }

        // Demande une date valide
        private DateTime DemanderDate(string message)
        {
            DateTime date;
            do
            {
                Console.Write(message);
                if (DateTime.TryParse(Console.ReadLine(), out date))
                {
                    return date;
                }
                Console.WriteLine("Veuillez entrer une date valide (format: yyyy-MM-dd HH:mm:ss)");
            } while (true);
        }

        // ================== MÉTHODES UTILITAIRES ==================

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
                // Ouvrir la connexion à la base de données
                ConnexionBD();

                // Requête SQL pour vérifier l'existence de l'ID
                string query = "SELECT COUNT(*) FROM Utilisateur WHERE Id_Utilisateur = @Id_Utilisateur";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Ajouter le paramètre pour l'ID
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);

                    // Exécuter la requête et obtenir le résultat
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    // Retourner vrai si l'ID existe, faux sinon
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
                // Fermer la connexion
                DeconnexionBD();
            }
        }
    }
}