using System;
using MySql.Data.MySqlClient;
using RENDU30mars;

namespace Rendu30mars
{
    public class Program
    {
        // CHAÎNE DE CONNEXION A MODIFIER POUR CHAQUE UTILISATEUR !
        static string chaineDeConnexion = "SERVER=localhost;PORT=3306;DATABASE=LIV;UID=root;PASSWORD=VOTREmdp";

        // Méthode principale avec l'écran de sélection
        // Méthode pour afficher l'écran de connexion ou d'inscription



        // Ajout d'une méthode pour calculer le chemin entre le client et le cuisinier
        public static void CalculerCheminClientVersCuisinier(string stationClient, string stationCuisinier)
        {
            // Créer un objet Graphe pour les stations
            Graphe<string> graphe = new Graphe<string>();

            // Charger le graphe des stations (à partir d'un fichier ou base de données)
            graphe.ChargerDepuisFichier("metro_paris.csv"); // Fichier csv adéquat
            // Utiliser l'algorithme de Dijkstra pour obtenir le plus court chemin
            var resultatDijkstra = graphe.Dijkstra(stationClient);

            // Vérifier si le chemin existe
            if (resultatDijkstra.distances.ContainsKey(stationCuisinier))
            {
                float distance = resultatDijkstra.distances[stationCuisinier];
                var chemin = graphe.ReconstruireChemin(resultatDijkstra.predecessors, stationCuisinier);

                // Affichage du résultat
                Console.WriteLine($"Le plus court chemin de {stationClient} à {stationCuisinier} est de {distance} min.");
                Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", chemin));
            }
            else
            {
                Console.WriteLine($"Aucun chemin trouvé entre {stationClient} et {stationCuisinier}.");
            }
        }

        static bool EcranConnexionOuInscription()
        {
            Console.Clear();
            Console.WriteLine("=== Bienvenue ===");
            Console.WriteLine("1. Se connecter");
            Console.WriteLine("2. S'inscrire");
            Console.WriteLine("3. Quitter");
            Console.Write("Choisissez une option (1-3) : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    return ConnexionUtilisateur();  // Appel à la méthode de connexion
                case "2":
                    InscriptionUtilisateur();  // Inscription d'un nouvel utilisateur
                    return true;  // Une fois inscrit, on passe directement à l'écran principal
                case "3":
                    Console.WriteLine("Merci et à bientôt !");
                    Environment.Exit(0);  // Quitte le programme immédiatement
                    return false;  // Cette ligne ne sera jamais atteinte, car le programme se ferme immédiatement
                default:
                    Console.WriteLine("Choix invalide, veuillez réessayer.");
                    return false;
            }
        }

        // Méthode principale avec l'écran de sélection
        static void Main()
        {
            bool continuer = true;

            // Initialisation de la connexion
            bool estConnecte = false;
            while (!estConnecte)
            {
                estConnecte = EcranConnexionOuInscription();  // Demander l'option jusqu'à ce qu'un utilisateur se connecte
            }

            // Une fois connecté, choisir le mode Client ou Cuisinier
            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== Bienvenue sur Liv'In Paris ===");
                Console.WriteLine("1. Mode Client");
                Console.WriteLine("2. Mode Cuisinier");
                Console.WriteLine("3. Se déconnecter");
                Console.WriteLine("4. Quitter");
                Console.Write("Choisissez une option (1-4) : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ModeClient();  // Lancer le mode client
                        break;
                    case "2":
                        ModeCuisinier();  // Lancer le mode cuisinier
                        break;
                    case "3":
                        SeDeconnecter();  // Lancer la déconnexion
                        return;  // Quitter la boucle et retourner à l'écran de connexion
                    case "4":
                        continuer = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide, veuillez réessayer.");
                        break;
                }
            }
        }


        // Méthode pour la déconnexion et retourner à l'écran de connexion
        static void SeDeconnecter()
        {
            Console.Clear();
            Console.WriteLine("Vous êtes maintenant déconnecté !");
            Console.WriteLine("Redirection vers l'écran de connexion...");
            // Attendre que l'utilisateur appuie sur une touche pour revenir à l'écran de connexion
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            Main();  // Redémarrer la méthode principale pour revenir à l'écran de connexion
        }


        // Connexion de l'utilisateur
        // Méthode pour la connexion de l'utilisateur
        static bool ConnexionUtilisateur()
        {
            Console.Clear();
            Console.WriteLine("=== Connexion ===");
            Console.Write("Entrez votre email : ");
            string email = Console.ReadLine();
            Console.Write("Entrez votre mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.WriteLine($"Tentative de connexion avec Email: {email} et Mot de Passe: {motDePasse}");

            // Vérification des identifiants dans la base de données
            Site site = new Site();
            site.ConnexionBD();
            bool utilisateurValide = site.VerifierIdentifiants(email, motDePasse);

            if (utilisateurValide)
            {
                // Récupérer l'ID de l'utilisateur connecté depuis la base de données
                IdUtilisateurConnecte = ObtenirIdUtilisateur(email);  // Ajoute une méthode dans ton `Site` qui récupère l'ID
                Console.WriteLine("Connexion réussie !");
                site.DeconnexionBD();
                return true;
            }
            else
            {
                site.DeconnexionBD();
                Console.WriteLine("Identifiants incorrects. Veuillez réessayer.");
                Console.WriteLine("Vous allez être redirigé dans 3 secondes...");
                System.Threading.Thread.Sleep(3000);
                return false;
            }
        }

        public static string IdUtilisateurConnecte { get; set; } // Stocke l'ID de l'utilisateur actuellement connecté pour certaines fonctions utiles

        public static string ObtenirIdUtilisateur(string email)
        {
            string idUtilisateur = string.Empty;

            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Requête pour récupérer l'ID de l'utilisateur en fonction de son email
                string query = "SELECT Id_Utilisateur FROM Utilisateur WHERE Email = @Email";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    idUtilisateur = reader["Id_Utilisateur"].ToString();  // Récupérer l'ID de l'utilisateur
                }

                reader.Close();
            }

            return idUtilisateur;
        }


        // Inscription de l'utilisateur
        // Méthode pour l'inscription de l'utilisateur
        public static void InscriptionUtilisateur()
        {
            Console.Clear();
            Console.WriteLine("=== Inscription ===");

            // Demander les informations de l'utilisateur
            string prenom = DemanderTexte("Entrez votre prénom : ");
            string nom = DemanderTexte("Entrez votre nom : ");
            string adresse = DemanderTexte("Entrez votre adresse : ");
            string email = DemanderTexte("Entrez votre email : ");
            string telephone = DemanderTexte("Entrez votre numéro de téléphone : ");
            string motdepasse = DemanderTexte("Entrez votre mot de passe : ");

            // Demander le code postal et la station de métro
            string codePostal = DemanderTexte("Entrez votre code postal : ");
            string stationMetro = DemanderTexte("Entrez la station de métro la plus proche de chez vous : ");

            string idUtilisateur = Guid.NewGuid().ToString(); // Générer un ID unique pour l'utilisateur

            Site site = new Site();
            site.ConnexionBD();

            // Inscription du client avec les nouveaux champs (code postal et station de métro)
            InscriptionClient(idUtilisateur, nom, prenom, adresse, email, motdepasse, telephone, codePostal, stationMetro);

            site.DeconnexionBD();

            Console.WriteLine("\nInscription réussie !");
        }

        public static void InscriptionClient(string idUtilisateur, string nom, string prenom, string adresse, string email, string motdepasse, string telephone, string codePostal, string stationMetro)
        {
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Insérer l'utilisateur avec les nouveaux champs dans la base de données
                string query = @"
            INSERT INTO Utilisateur (Id_Utilisateur, Nom, Prenom, Adresse, Email, Mot_de_Passe, Numero_de_Telephone, Code_Postal, Station)
            VALUES (@Id_Utilisateur, @Nom, @Prenom, @Adresse, @Email, @Mot_de_Passe, @Numero_de_Telephone, @Code_Postal, @Station)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id_Utilisateur", idUtilisateur);
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Adresse", adresse);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Mot_de_Passe", motdepasse);
                cmd.Parameters.AddWithValue("@Numero_de_Telephone", telephone);
                cmd.Parameters.AddWithValue("@Code_Postal", codePostal);  // Ajout du code postal
                cmd.Parameters.AddWithValue("@Station", stationMetro);   // Ajout de la station de métro

                cmd.ExecuteNonQuery();
            }
        }


        // Mode Client
        public static void ModeClient()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== Mode Client ===");
                Console.WriteLine("1. Consulter la liste des spécialités");
                Console.WriteLine("2. Consulter le panier");
                Console.WriteLine("3. Retour arrière");  // Option retour
                Console.Write("Choisissez une option (1-3) : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ConsulterSpecialites();  // Afficher la liste des spécialités
                        break;
                    case "2":
                        ConsulterPanier();  // Afficher le panier
                        break;
                    case "3":
                        return;  // Retourner à l'écran précédent immédiatement
                    default:
                        Console.WriteLine("Choix invalide, veuillez réessayer.");
                        break;
                }
            }
        }

        public static List<Plat> panier = new List<Plat>();  // Le panier est maintenant une variable globale

        public static List<Plat> ObtenirPlatsParSpecialite(string specialite)
        {
            var plats = new List<Plat>();

            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Récupérer les plats disponibles pour une spécialité donnée
                string query = @"
            SELECT Nom, Type_Plat, Prix_par_Portion, Regime_Alimentaire, Pour_combien_de_personnes
            FROM Plats
            WHERE Specialite = @Specialite";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Specialite", specialite);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var plat = new Plat
                    {
                        Nom = reader["Nom"].ToString(),
                        Type_Plat = reader["Type_Plat"].ToString(),
                        Prix_par_Portion = Convert.ToDecimal(reader["Prix_par_Portion"]),
                        Regime_Alimentaire = reader["Regime_Alimentaire"].ToString(),
                        Pour_combien_de_personnes = Convert.ToInt32(reader["Pour_combien_de_personnes"]),
                        Specialite = specialite
                    };
                    plats.Add(plat);
                }

                reader.Close();
            }

            return plats;
        }


        // Classe pour représenter un plat
        public class Plat
        {
            public string Nom { get; set; }
            public string Type_Plat { get; set; }
            public decimal Prix_par_Portion { get; set; }
            public string Regime_Alimentaire { get; set; }
            public int Pour_combien_de_personnes { get; set; }
            public string Specialite { get; set; }
            public string ID_Cuisinier { get; set; }
        }

        // Méthode pour ajouter un plat au panier (en mémoire)
        public static void AjouterAuPanier(Plat plat)
        {
            // Vérifie si le plat est déjà dans le panier
            var platExist = panier.FirstOrDefault(p => p.Nom == plat.Nom);
            if (platExist == null)
            {
                panier.Add(plat);  // Ajoute le plat au panier
                Console.WriteLine("Plat ajouté au panier !");
            }
            else
            {
                Console.WriteLine("Ce plat est déjà dans votre panier.");
            }
        }

        public static void ConsulterPanier()
        {
            Console.Clear();
            Console.WriteLine("=== Panier ===");

            var panier = ObtenirPanier();  // Cette fonction récupère les plats dans le panier (en mémoire)
            decimal total = 0;

            // Affiche les plats du panier
            if (panier.Count == 0)
            {
                Console.WriteLine("Votre panier est vide !");
                Console.WriteLine("Retour à l'écran principal...");
                Console.ReadKey();
                return;
            }

            // Afficher chaque plat avec son prix
            foreach (var item in panier)
            {
                total += item.Prix_par_Portion;  // Un seul plat par entrée
                Console.WriteLine($"{item.Nom} - {item.Prix_par_Portion} euros");
            }

            decimal tva = total * 0.20m;  // Calcul de la TVA (20%)
            decimal prixFinal = total + tva;

            Console.WriteLine($"Total HT : {total} euros");
            Console.WriteLine($"TVA (20%) : {tva} euros");
            Console.WriteLine($"Prix final : {prixFinal} euros");

            // Menu d'options
            Console.WriteLine("\nQue souhaitez-vous faire ?");
            Console.WriteLine("1. Retirer un plat");
            Console.WriteLine("2. Commander");
            Console.WriteLine("3. Retour");
            Console.Write("Choisissez une option (1-3) : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    RetirerPlatDuPanier(panier);  // Retirer un plat du panier
                    break;
                case "2":
                    Commander(panier);  // Passer la commande
                    break;
                case "3":
                    return;  // Retourner à l'écran précédent
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    break;
            }
        }

        public static void RetirerPlatDuPanier(List<Plat> panier)
        {
            Console.Clear();
            Console.WriteLine("=== Retirer un plat du panier ===");

            // Si le panier est vide, retourner immédiatement
            if (panier.Count == 0)
            {
                Console.WriteLine("Votre panier est vide. Retour à l'écran précédent...");
                Console.ReadKey();
                return;
            }

            // Afficher les plats dans le panier
            for (int i = 0; i < panier.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {panier[i].Nom} - {panier[i]} euros");
            }

            Console.WriteLine("Choisissez le plat à retirer : ");
            int choixPlat = int.Parse(Console.ReadLine()) - 1;

            // Vérifier si l'index choisi est valide
            if (choixPlat >= 0 && choixPlat < panier.Count)
            {
                // Retirer le plat du panier
                panier.RemoveAt(choixPlat);
                Console.WriteLine("Plat retiré du panier !");
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }

            Console.WriteLine("Retour au panier...");
            Console.ReadKey();
        }

        public static List<Plat> ObtenirPanier()
        {
            // Retourne le panier actuel (stocké en mémoire)
            return panier;
        }

        public static void SupprimerPlatsDuPanier(List<Plat> panier)
        {
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                try
                {
                    connection.Open();  // Ouverture de la connexion
                    Console.WriteLine("Connexion à la base de données réussie.");

                    string query = @"
                DELETE FROM Plats
                WHERE Nom = @Nom AND ID_Cuisinier = @ID_Cuisinier";

                    foreach (var plat in panier)
                    {
                        // Affichage des paramètres
                        Console.WriteLine($"Tentative de suppression du plat : {plat.Nom} (Cuisinier ID: {plat.ID_Cuisinier})");

                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Nom", plat.Nom);
                        cmd.Parameters.AddWithValue("@ID_Cuisinier", plat.ID_Cuisinier);

                        // Vérification avant d'exécuter la requête
                        Console.WriteLine($"Exécution de la requête : {cmd.CommandText}");
                        Console.WriteLine($"Paramètres - Nom: {plat.Nom}, Cuisinier ID: {plat.ID_Cuisinier}");

                        // Exécution de la requête
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Vérification du nombre de lignes affectées
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Plat '{plat.Nom}' supprimé de la base de données.");
                        }
                        else
                        {
                            Console.WriteLine($"Aucun plat trouvé avec le nom '{plat.Nom}' et ID Cuisinier {plat.ID_Cuisinier}. Suppression échouée.");
                        }
                    }

                    // Vérifier si les plats ont bien été supprimés en base de données
                    string verificationQuery = @"
                SELECT COUNT(*) FROM Plats WHERE Nom IN (@Nom) AND ID_Cuisinier IN (@IdCuisinier)";
                    MySqlCommand verificationCmd = new MySqlCommand(verificationQuery, connection);
                    verificationCmd.Parameters.AddWithValue("@Nom", panier[0].Nom);  // Utilisation du nom du premier plat du panier
                    verificationCmd.Parameters.AddWithValue("@IdCuisinier", panier[0].ID_Cuisinier);  // Utilisation du cuisinier du premier plat

                    int count = Convert.ToInt32(verificationCmd.ExecuteScalar());

                    Console.WriteLine($"Vérification après suppression : {count} plat(s) restant(s) avec ce nom et ID_Cuisinier.");

                    // Si count == 0, les plats ont bien été supprimés, sinon il y a eu un problème.
                    if (count == 0)
                    {
                        Console.WriteLine("Plats bien supprimés.");
                    }
                    else
                    {
                        Console.WriteLine("Problème lors de la suppression des plats.");
                    }

                    // Maintenant, supprimer les plats du panier en mémoire
                    panier.Clear();
                    Console.WriteLine("Panier vidé.");
                }
                catch (Exception ex)
                {
                    // Capture toute exception et l'affiche pour mieux comprendre le problème
                    Console.WriteLine($"Erreur lors de la suppression du plat : {ex.Message}");
                }
            }
        }


        public static void CreerCommande(List<Plat> panier)
        {
            decimal total = 0;

            // Récupérer l'ID du client depuis la variable globale
            string idClient = IdUtilisateurConnecte;

            // Calculer le total des prix et obtenir les informations nécessaires
            foreach (var plat in panier)
            {
                total += plat.Prix_par_Portion;  // Calcul du total
            }

            decimal tva = total * 0.20m;  // Calcul de la TVA (20%)
            decimal prixFinal = total + tva;

            // Récupérer l'ID du cuisinier et les stations
            string idCuisinier = null;
            string stationClient = null;
            string stationCuisinier = null;

            // Obtenir l'ID du cuisinier et les stations du client et du cuisinier
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Récupérer l'ID du cuisinier depuis la table Plats (en fonction du nom du plat)
                string queryCuisinier = @"
            SELECT ID_Cuisinier 
            FROM Plats
            WHERE Nom = @NomPlat
            LIMIT 1";

                MySqlCommand cmdCuisinier = new MySqlCommand(queryCuisinier, connection);
                cmdCuisinier.Parameters.AddWithValue("@NomPlat", panier[0].Nom);  // Utilise le nom du premier plat du panier (en supposant que tous les plats sont du même cuisinier)
                MySqlDataReader readerCuisinier = cmdCuisinier.ExecuteReader();
                if (readerCuisinier.Read())
                {
                    idCuisinier = readerCuisinier["ID_Cuisinier"].ToString();
                }
                readerCuisinier.Close();

                // Récupérer la station du client
                string queryClient = @"
            SELECT Station 
            FROM Utilisateur
            WHERE Id_Utilisateur = @IdUtilisateur";

                MySqlCommand cmdClient = new MySqlCommand(queryClient, connection);
                cmdClient.Parameters.AddWithValue("@IdUtilisateur", idClient);
                MySqlDataReader readerClient = cmdClient.ExecuteReader();
                if (readerClient.Read())
                {
                    stationClient = readerClient["Station"].ToString();
                }
                readerClient.Close();

                // Récupérer la station du cuisinier
                string queryCuisinierStation = @"
            SELECT Station 
            FROM Utilisateur
            WHERE Id_Utilisateur = @IdCuisinier";

                MySqlCommand cmdCuisinierStation = new MySqlCommand(queryCuisinierStation, connection);
                cmdCuisinierStation.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                MySqlDataReader readerCuisinierStation = cmdCuisinierStation.ExecuteReader();
                if (readerCuisinierStation.Read())
                {
                    stationCuisinier = readerCuisinierStation["Station"].ToString();
                }
                readerCuisinierStation.Close();
            }

            // Appeler la méthode pour calculer et afficher le chemin entre la station du client et celle du cuisinier
            CalculerCheminClientVersCuisinier(stationCuisinier, stationClient);

            // Insérer la commande dans la base de données
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Insérer chaque plat du panier comme une nouvelle ligne dans Commandes
                foreach (var plat in panier)
                {
                    string query = @"
                INSERT INTO Commandes (Quantite, Date_heure, Id_Utilisateur, Nom_Plat, Id_Cuisinier, Station_Client, Station_Cuisinier)
                VALUES (@Quantite, @Date_heure, @Id_Utilisateur, @Nom_Plat, @ID_Cuisinier, @Station_Client, @Station_Cuisinier)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Quantite", 1);  // Une quantité par plat (car chaque plat est inséré individuellement)
                    cmd.Parameters.AddWithValue("@Date_heure", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id_Utilisateur", idClient);  // ID du client
                    cmd.Parameters.AddWithValue("@Nom_Plat", plat.Nom);  // Insérer le nom du plat
                    cmd.Parameters.AddWithValue("@ID_Cuisinier", idCuisinier);  // ID du cuisinier
                    cmd.Parameters.AddWithValue("@Station_Client", stationClient);  // Station du client
                    cmd.Parameters.AddWithValue("@Station_Cuisinier", stationCuisinier);  // Station du cuisinier
                    cmd.ExecuteNonQuery();
                }
            }

            // **Appeler ici la méthode pour supprimer les plats du panier de la base de données**
            SupprimerPlatsDuPanier(panier);

            // Vider le panier après la commande
            panier.Clear();
            Console.WriteLine($"Total à payer : {prixFinal} euros");

            // Affichage du résultat dans un nouvel écran
            AfficherResultatChemin(stationClient, stationCuisinier);
        }


        public static void AfficherResultatChemin(string stationClient, string stationCuisinier)
        {
            // Créer un objet Graphe pour les stations
            Graphe<string> graphe = new Graphe<string>();

            // Charger le graphe des stations (à partir d'un fichier ou base de données)
            graphe.ChargerDepuisFichier("metro_paris.csv"); // Fichier csv adéquat

            // Utiliser l'algorithme de Dijkstra pour obtenir le plus court chemin
            var resultatDijkstra = graphe.Dijkstra(stationClient);

            // Vérifier si le chemin existe
            if (resultatDijkstra.distances.ContainsKey(stationCuisinier))
            {
                float distance = resultatDijkstra.distances[stationCuisinier];
                var chemin = graphe.ReconstruireChemin(resultatDijkstra.predecessors, stationCuisinier);

                // Affichage du résultat dans un écran dédié
                Console.Clear();
                Console.WriteLine("=== Résultat du calcul de chemin ===");
                Console.WriteLine($"Le plus court chemin de {stationClient} à {stationCuisinier} est de {distance} min.");
                Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", chemin));
                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu principal...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Aucun chemin trouvé entre {stationClient} et {stationCuisinier}.");
            }
        }

        public static void ConsulterSpecialites()
        {
            Console.Clear();
            Console.WriteLine("=== Liste des spécialités ===");

            var specialites = ObtenirSpecialitesDisponibles();  // Cette méthode récupère les spécialités valides (avec des plats et quantité > 0)

            // Afficher les spécialités disponibles
            for (int i = 0; i < specialites.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {specialites[i]}");
            }
            Console.WriteLine("0. Retour arrière");  // Option pour revenir en arrière

            Console.WriteLine("Choisissez une spécialité (par numéro) ou tapez 0 pour revenir : ");
            int choixSpecialite = int.Parse(Console.ReadLine()) - 1;

            if (choixSpecialite >= 0 && choixSpecialite < specialites.Count)
            {
                ConsulterPlats(specialites[choixSpecialite]);  // Afficher les plats de la spécialité choisie
            }
            else if (choixSpecialite == -1)
            {
                Console.WriteLine("Retour à l'écran précédent...");
                Console.ReadKey();  // Attendre que l'utilisateur appuie sur une touche
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }


        public static List<string> ObtenirSpecialitesDisponibles()
        {
            var specialites = new List<string>();

            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Requête pour récupérer les spécialités distinctes
                string query = "SELECT DISTINCT Specialite FROM Plats WHERE Specialite IS NOT NULL AND Specialite != ''";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    specialites.Add(reader["Specialite"].ToString());
                }

                reader.Close();
            }

            return specialites;
        }


        public static void ConsulterPlats(string specialite)
        {
            Console.Clear();
            Console.WriteLine($"=== Plats de la spécialité : {specialite} ===");

            // Récupérer les plats disponibles pour cette spécialité
            var plats = ObtenirPlatsParSpecialite(specialite);

            if (plats.Count == 0)
            {
                Console.WriteLine("Aucun plat disponible dans cette spécialité.");
                Console.WriteLine("Retour à l'écran des spécialités...");
                Console.ReadKey();
                return;
            }

            // Afficher les plats disponibles pour la spécialité
            for (int i = 0; i < plats.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {plats[i].Nom} - {plats[i].Prix_par_Portion} euros");
            }

            Console.WriteLine("Choisissez un plat pour voir les détails (par numéro) ou tapez 0 pour revenir : ");
            int choixPlat = int.Parse(Console.ReadLine()) - 1;

            if (choixPlat >= 0 && choixPlat < plats.Count)
            {
                AfficherDetailsPlat(plats[choixPlat]);  // Afficher les détails du plat sélectionné
            }
            else if (choixPlat == -1)
            {
                Console.WriteLine("Retour à l'écran des spécialités...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }

        public static void AfficherDetailsPlat(Plat plat)
        {
            Console.Clear();
            Console.WriteLine($"=== Détails du plat : {plat.Nom} ===");
            Console.WriteLine($"Type de plat : {plat.Type_Plat}");
            Console.WriteLine($"Prix : {plat.Prix_par_Portion} euros");
            Console.WriteLine($"Régime alimentaire : {plat.Regime_Alimentaire}");
            Console.WriteLine($"Pour combien de personnes : {plat.Pour_combien_de_personnes}");
            Console.WriteLine($"Spécialité : {plat.Specialite}");
            Console.WriteLine("1. Ajouter au panier");
            Console.WriteLine("2. Retour à la liste des plats");

            string choix = Console.ReadLine();
            if (choix == "1")
            {
                AjouterAuPanier(plat);  // Ajouter ce plat au panier
                Console.WriteLine("Plat ajouté au panier !");
            }
            else if (choix == "2")
            {
                Console.WriteLine("Retour à la liste des plats...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }

        public static void Commander(List<Plat> panier)
        {
            Console.Clear();
            Console.WriteLine("=== Passer la commande ===");

            // Si le panier est vide, afficher un message et retourner
            if (panier.Count == 0)
            {
                Console.WriteLine("Votre panier est vide. Impossible de passer la commande.");
                Console.WriteLine("Retour à l'écran principal...");
                Console.ReadKey();
                return;
            }

            // Créer la commande dans la BDD
            CreerCommande(panier);

            // Vider le panier après la commande
            panier.Clear();

            Console.WriteLine("Commande passée avec succès !");
            Console.WriteLine("Retour à l'écran principal...");
            Console.ReadKey();
        }

        // Mode Cuisinier
        public static void ModeCuisinier()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Mode Cuisinier ===");
                Console.WriteLine("1. Créer un plat");
                Console.WriteLine("2. Consulter la liste des plats disponibles");
                Console.WriteLine("3. Consulter mes commandes");  // Nouvelle option pour consulter les commandes
                Console.WriteLine("4. Retour");

                Console.Write("Choisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        CreerPlat();  // Permet de créer un plat
                        break;
                    case "2":
                        ConsulterListePlats();  // Affiche la liste des plats
                        break;
                    case "3":
                        ConsulterCommandesCuisinier();  // Consulter les commandes du cuisinier
                        break;
                    case "4":
                        Console.WriteLine("Retour à l'écran principal...");
                        return;  // Retour à l'écran précédent
                    default:
                        Console.WriteLine("Choix invalide. Essayez à nouveau.");
                        break;
                }
            }
        }

        public static void ConsulterCommandesCuisinier()
        {
            string idCuisinier = IdUtilisateurConnecte;  // L'ID du cuisinier actuellement connecté

            // Récupérer les commandes du cuisinier
            List<Commande> commandes = new List<Commande>();

            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                string queryCommandes = @"
            SELECT c.Nom_Plat, c.Station_Client, c.Station_Cuisinier, u.Nom, u.Prenom, u.Adresse
            FROM Commandes c
            INNER JOIN Utilisateur u ON c.Id_Utilisateur = u.Id_Utilisateur
            WHERE c.Id_Cuisinier = @IdCuisinier";

                MySqlCommand cmdCommandes = new MySqlCommand(queryCommandes, connection);
                cmdCommandes.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                MySqlDataReader readerCommandes = cmdCommandes.ExecuteReader();

                while (readerCommandes.Read())
                {
                    Commande commande = new Commande
                    {
                        NomPlat = readerCommandes["Nom_Plat"].ToString(),
                        StationClient = readerCommandes["Station_Client"].ToString(),
                        StationCuisinier = readerCommandes["Station_Cuisinier"].ToString(),
                        NomDestinataire = readerCommandes["Nom"].ToString(),
                        PrenomDestinataire = readerCommandes["Prenom"].ToString(),
                        AdresseDestinataire = readerCommandes["Adresse"].ToString()
                    };

                    commandes.Add(commande);
                }
            }

            // Afficher les commandes et le chemin
            foreach (var commande in commandes)
            {
                Console.Clear();
                Console.WriteLine("=== Commande à livrer ===");
                Console.WriteLine($"Destinataire : {commande.NomDestinataire} {commande.PrenomDestinataire}");
                Console.WriteLine($"Plat à livrer : {commande.NomPlat}");
                Console.WriteLine($"Adresse de livraison : {commande.AdresseDestinataire}");

                // Affichage du résultat du calcul du chemin
                CalculerCheminClientVersCuisinier(commande.StationCuisinier, commande.StationClient);

                Console.WriteLine("\nAppuyez sur une touche pour consulter la commande suivante...");
                Console.ReadKey();
            }
        }

        // Classe pour représenter une commande
        public class Commande
        {
            public string NomPlat { get; set; }
            public string StationClient { get; set; }
            public string StationCuisinier { get; set; }
            public string NomDestinataire { get; set; }
            public string PrenomDestinataire { get; set; }
            public string AdresseDestinataire { get; set; }
        }

        public static void CreerPlat()
        {
            Console.Clear();
            Console.WriteLine("=== Création d'un nouveau plat ===");

            // Demander les informations du plat
            string nomPlat = DemanderTexte("Entrez le nom du plat : ");
            string typePlat = DemanderTypePlat();
            decimal prix = Convert.ToDecimal(DemanderTexte("Entrez le prix par portion : "));
            string regimeAlimentaire = DemanderTexte("Entrez le régime alimentaire (Végétarien, Sans gluten, etc.) : ");
            int pourCombienDePersonnes = Convert.ToInt32(DemanderTexte("Combien de personnes pour ce plat ? "));

            // Demander la spécialité
            string specialite = ChoisirOuCreerSpecialite();  // Permet de choisir ou créer une spécialité

            // Demander la liste des ingrédients
            List<string> ingredients = new List<string>();
            while (true)
            {
                string ingredient = DemanderTexte("Entrez un ingrédient (ou tapez '0' pour terminer) : ");
                if (ingredient == "0") break;
                ingredients.Add(ingredient);
            }

            // Demander la date de fabrication et la date de péremption
            DateTime dateFabrication = DemanderDate("Entrez la date de fabrication (JJ/MM/AAAA) : ");
            DateTime datePeremption = DemanderDate("Entrez la date de péremption (JJ/MM/AAAA) : ");

            // Récupérer l'ID du cuisinier connecté
            string idCuisinier = IdUtilisateurConnecte;

            if (string.IsNullOrEmpty(idCuisinier))
            {
                Console.WriteLine("Erreur : L'ID du cuisinier n'a pas pu être récupéré.");
                return;
            }

            Console.WriteLine($"ID du cuisinier récupéré : {idCuisinier}");

            // Créer le plat dans la base de données
            AjouterPlatDansBDD(nomPlat, typePlat, prix, regimeAlimentaire, pourCombienDePersonnes, specialite, idCuisinier, ingredients, dateFabrication, datePeremption);
            Console.WriteLine("Plat créé avec succès !");
        }

        // Méthode pour demander une date avec validation du format JJ/MM/AAAA
        public static DateTime DemanderDate(string message)
        {
            DateTime date;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    Console.WriteLine("Format invalide. Veuillez entrer la date au format JJ/MM/AAAA.");
                }
            }
        }


        public static void AjouterPlatDansBDD(string nomPlat, string typePlat, decimal prix, string regimeAlimentaire, int pourCombienDePersonnes, string specialite, string idCuisinier, List<string> ingredients, DateTime dateFabrication, DateTime datePeremption)
        {
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Insertion du plat avec les informations fournies
                string query = @"
        INSERT INTO Plats (Nom, Type_Plat, Prix_par_Portion, Regime_Alimentaire, Pour_combien_de_personnes, Specialite, ID_Cuisinier, Date_de_fabrication, Date_de_Peremption, Ingrédients)
        VALUES (@Nom, @Type_Plat, @Prix, @Regime_Alimentaire, @Pour_combien_de_personnes, @Specialite, @ID_Cuisinier, @Date_de_fabrication, @Date_de_Peremption, @Ingrédients)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nom", nomPlat);
                cmd.Parameters.AddWithValue("@Type_Plat", typePlat);
                cmd.Parameters.AddWithValue("@Prix", prix);
                cmd.Parameters.AddWithValue("@Regime_Alimentaire", regimeAlimentaire);
                cmd.Parameters.AddWithValue("@Pour_combien_de_personnes", pourCombienDePersonnes);
                cmd.Parameters.AddWithValue("@Specialite", specialite);
                cmd.Parameters.AddWithValue("@ID_Cuisinier", idCuisinier);  // Ajout de l'ID_Cuisinier
                cmd.Parameters.AddWithValue("@Date_de_fabrication", dateFabrication);
                cmd.Parameters.AddWithValue("@Date_de_Peremption", datePeremption);

                // Convertir la liste des ingrédients en un format compatible avec la base de données (par exemple une chaîne séparée par des virgules)
                string ingredientsStr = string.Join(", ", ingredients);
                cmd.Parameters.AddWithValue("@Ingrédients", ingredientsStr);

                cmd.ExecuteNonQuery();
            }
        }
        public static string DemanderTypePlat()
        {
            string typePlat = string.Empty;

            while (true)
            {
                Console.WriteLine("Choisissez le type de plat :");
                Console.WriteLine("1. Entrée");
                Console.WriteLine("2. Plat principal");
                Console.WriteLine("3. Dessert");
                Console.Write("Entrez 1, 2 ou 3 pour choisir le type de plat : ");

                string choix = Console.ReadLine();

                // Validation du choix
                if (choix == "1")
                {
                    typePlat = "Entrée";
                    break;
                }
                else if (choix == "2")
                {
                    typePlat = "Plat principal";
                    break;
                }
                else if (choix == "3")
                {
                    typePlat = "Dessert";
                    break;
                }
                else
                {
                    Console.WriteLine("Choix invalide. Veuillez entrer 1, 2 ou 3.");
                }
            }

            return typePlat;
        }

        public static string ChoisirOuCreerSpecialite()
        {
            Console.Clear();
            Console.WriteLine("=== Choisir ou créer une spécialité ===");
            var specialites = ObtenirSpecialitesDisponibles();  // Récupérer les spécialités existantes
            // Si des spécialités existent, on les affiche
            if (specialites.Count > 0)
            {
                Console.WriteLine("Spécialités existantes :");
                for (int i = 0; i < specialites.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {specialites[i]}");
                }
            }
            // Demander au cuisinier de choisir ou de créer une nouvelle spécialité
            Console.WriteLine("\nOu entrez un nouveau nom de spécialité :");
            Console.WriteLine("Entrez un numéro pour choisir ou tapez un nouveau nom pour créer une nouvelle spécialité.");
            string choix = Console.ReadLine();
            // Si le choix est un nombre, on assigne la spécialité existante
            if (int.TryParse(choix, out int numeroChoix) && numeroChoix >= 1 && numeroChoix <= specialites.Count)
            {
                return specialites[numeroChoix - 1];  // Retourner la spécialité sélectionnée
            }
            else
            {
                return choix;  // Si un texte libre est saisi, on crée une nouvelle spécialité
            }
        }

        public static string ChoisirSpecialite()
        {
            Console.Clear();
            Console.WriteLine("=== Choisir une spécialité ===");

            var specialites = ObtenirSpecialitesDisponibles();  // Cette méthode récupère les spécialités existantes

            if (specialites.Count == 0)
            {
                Console.WriteLine("Aucune spécialité disponible. Veuillez en créer une.");
                return string.Empty;  // Retourne une chaîne vide si aucune spécialité n'est disponible
            }

            // Afficher les spécialités disponibles
            for (int i = 0; i < specialites.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {specialites[i]}");
            }

            Console.Write("Choisissez une spécialité (par numéro) : ");
            int choix = int.Parse(Console.ReadLine()) - 1;

            if (choix >= 0 && choix < specialites.Count)
            {
                return specialites[choix];  // Retourner la spécialité choisie
            }
            else
            {
                Console.WriteLine("Choix invalide. Retour à l'écran principal.");
                return string.Empty;
            }
        }

        public static void ConsulterListePlats()
        {
            Console.Clear();
            Console.WriteLine("=== Liste des plats disponibles ===");

            var plats = ObtenirPlatsDisponibles();  // Récupère la liste des plats dans la base de données

            foreach (var plat in plats)
            {
                Console.WriteLine($"{plat.Nom} - {plat.Prix_par_Portion} euros");
            }

            Console.WriteLine("Appuyez sur une touche pour revenir...");
            Console.ReadKey();
        }

        public static List<Plat> ObtenirPlatsDisponibles()
        {
            var plats = new List<Plat>();

            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Requête pour récupérer tous les plats de la base de données
                string query = @"
            SELECT Nom, Type_Plat, Prix_par_Portion, Regime_Alimentaire, Pour_combien_de_personnes, Specialite
            FROM Plats";  // Ici on ne filtre rien, on récupère tous les plats

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var plat = new Plat
                    {
                        Nom = reader["Nom"].ToString(),
                        Type_Plat = reader["Type_Plat"].ToString(),
                        Prix_par_Portion = Convert.ToDecimal(reader["Prix_par_Portion"]),
                        Regime_Alimentaire = reader["Regime_Alimentaire"].ToString(),
                        Pour_combien_de_personnes = Convert.ToInt32(reader["Pour_combien_de_personnes"]),
                        Specialite = reader["Specialite"].ToString()
                    };
                    plats.Add(plat);
                }
                reader.Close();
            }
            return plats;
        }

        // Méthodes utilitaires pour demander du texte à l'utilisateur
        static string DemanderTexte(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
