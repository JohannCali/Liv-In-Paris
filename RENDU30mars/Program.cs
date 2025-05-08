using System;
using MySql.Data.MySqlClient;

namespace Rendu30mars
{
    public class Program
    {
        // CHAÎNE DE CONNEXION A MODIFIER POUR CHAQUE UTILISATEUR !
        static string chaineDeConnexion = "SERVER=localhost;PORT=3306;DATABASE=LIV;UID=root;PASSWORD=MetTonMDP!!!!!;";

        // Méthode principale avec l'écran de sélection
        // Méthode pour afficher l'écran de connexion ou d'inscription
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
            site.DeconnexionBD();

            if (utilisateurValide)
            {
                Console.WriteLine("Connexion réussie !");
                return true;
            }
            else
            {
                // Affichage du message d'erreur
                Console.Clear();
                Console.WriteLine("Identifiants incorrects. Veuillez réessayer.");
                Console.WriteLine("Vous allez être redirigé dans 3 secondes...");

                // Attente de 3 secondes avant de retourner à l'écran de connexion
                System.Threading.Thread.Sleep(3000);

                // Retour à l'écran de connexion
                return false;
            }
        }



        // Inscription de l'utilisateur
        static void InscriptionUtilisateur()
        {
            Console.Clear();
            Console.WriteLine("=== Inscription ===");
            string prenom = DemanderTexte("Entrez votre prénom : ");
            string nom = DemanderTexte("Entrez votre nom : ");
            string adresse = DemanderTexte("Entrez votre adresse : ");
            string email = DemanderTexte("Entrez votre email : ");
            string telephone = DemanderTexte("Entrez votre numéro de téléphone : ");
            string motdepasse = DemanderTexte("Entrez votre mot de passe : ");

            string idUtilisateur = Guid.NewGuid().ToString(); // Générer un ID unique pour l'utilisateur

            Site site = new Site();
            site.ConnexionBD();
            site.InscriptionClient(idUtilisateur, nom, prenom, adresse, email, motdepasse, telephone);
            site.DeconnexionBD();

            Console.WriteLine("\nInscription réussie !");
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
                Console.WriteLine($"{item.Nom} - {item.Prix_par_Portion.ToString("C2")}");
            }

            decimal tva = total * 0.20m;  // Calcul de la TVA (20%)
            decimal prixFinal = total + tva;

            Console.WriteLine($"Total HT : {total.ToString("C2")}");
            Console.WriteLine($"TVA (20%) : {tva.ToString("C2")}");
            Console.WriteLine($"Prix final : {prixFinal.ToString("C2")}");

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
        Console.WriteLine($"{i + 1}. {panier[i].Nom} - {panier[i].Prix_par_Portion.ToString("C2")}");
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

        // Méthode pour supprimer un plat du panier (en mémoire et dans la base de données)
        public static void SupprimerPlatsDuPanier(List<Plat> panier)
        {
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                string query = @"
            DELETE FROM Plats
            WHERE Nom = @Nom";  // Assumes "Nom" is the unique identifier for a Plat, adjust if needed

                foreach (var plat in panier)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Nom", plat.Nom);
                    cmd.ExecuteNonQuery();
                }
            }

            // Maintenant, supprimer les plats du panier en mémoire
            panier.Clear();
            Console.WriteLine("Plats supprimés du panier et base de données.");
        }


        // Méthode pour créer la commande dans la base de données
        // Méthode pour créer une commande dans la BDD et vider le panier
        public static void CreerCommande(List<Plat> panier)
        {
            decimal total = 0;
            List<string> nomsPlats = new List<string>();

            // Calculer le total et obtenir les noms des plats
            foreach (var plat in panier)
            {
                total += plat.Prix_par_Portion;  // Calcul du total
                nomsPlats.Add(plat.Nom);  // Ajouter le nom du plat à la liste
            }

            decimal tva = total * 0.20m;  // Calcul de la TVA (20%)
            decimal prixFinal = total + tva;

            // Convertir la liste des noms de plats en une seule chaîne
            string nomPlats = string.Join(", ", nomsPlats);

            // Insérer la commande dans la base de données
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                string query = @"
        INSERT INTO Commandes (Quantite, Date_heure, Id_Utilisateur, Nom_Plat)
        VALUES (@Quantite, @Date_heure, @Id_Utilisateur, @Nom_Plat)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Quantite", panier.Count);  // La quantité est le nombre de plats différents
                cmd.Parameters.AddWithValue("@Date_heure", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id_Utilisateur", "UserID");  // Remplacer par l'ID de l'utilisateur connecté
                cmd.Parameters.AddWithValue("@Nom_Plat", nomPlats);  // Insérer les noms des plats

                cmd.ExecuteNonQuery();
            }

            // Vider le panier après la commande
            panier.Clear();

            Console.WriteLine($"Commande passée avec succès ! Total à payer : {prixFinal.ToString("C2")}");
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
                Console.WriteLine($"{i + 1}. {plats[i].Nom} - {plats[i].Prix_par_Portion} €");
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
            Console.WriteLine($"Prix : {plat.Prix_par_Portion} €");
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
                Console.WriteLine("3. Retour");

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
                        Console.WriteLine("Retour à l'écran principal...");
                        return;  // Retour à l'écran précédent
                    default:
                        Console.WriteLine("Choix invalide. Essayez à nouveau.");
                        break;
                }
            }
        }

        public static void CreerPlat()
        {
            Console.Clear();
            Console.WriteLine("=== Création d'un nouveau plat ===");
            // Demander les informations du plat
            string nomPlat = DemanderTexte("Entrez le nom du plat : ");
            string typePlat = DemanderTexte("Entrez le type du plat (Entrée, Plat Principal, Dessert) : ");
            decimal prix = Convert.ToDecimal(DemanderTexte("Entrez le prix par portion : "));
            string regimeAlimentaire = DemanderTexte("Entrez le régime alimentaire (Végétarien, Sans gluten, etc.) : ");
            int pourCombienDePersonnes = Convert.ToInt32(DemanderTexte("Combien de personnes pour ce plat ? "));
            // Demander la spécialité
            string specialite = ChoisirOuCreerSpecialite();  // Permet de choisir ou créer une spécialité
            // Créer le plat dans la base de données
            AjouterPlatDansBDD(nomPlat, typePlat, prix, regimeAlimentaire, pourCombienDePersonnes, specialite);
            Console.WriteLine("Plat créé avec succès !");
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

        public static void AjouterPlatDansBDD(string nomPlat, string typePlat, decimal prix, string regimeAlimentaire, int pourCombienDePersonnes, string specialite)
        {
            using (var connection = new MySqlConnection(chaineDeConnexion))
            {
                connection.Open();

                // Insérer le plat dans la base de données
                string query = @"
            INSERT INTO Plats (Nom, Type_Plat, Prix_par_Portion, Regime_Alimentaire, Pour_combien_de_personnes, Specialite)
            VALUES (@Nom, @Type_Plat, @Prix, @Regime_Alimentaire, @Pour_combien_de_personnes, @Specialite)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nom", nomPlat);
                cmd.Parameters.AddWithValue("@Type_Plat", typePlat);  // Utiliser Type_Plat ici
                cmd.Parameters.AddWithValue("@Prix", prix);
                cmd.Parameters.AddWithValue("@Regime_Alimentaire", regimeAlimentaire);  // Utiliser Regime_Alimentaire
                cmd.Parameters.AddWithValue("@Pour_combien_de_personnes", pourCombienDePersonnes);
                cmd.Parameters.AddWithValue("@Specialite", specialite);
                cmd.ExecuteNonQuery();
            }
        }

        public static void ConsulterListePlats()
        {
            Console.Clear();
            Console.WriteLine("=== Liste des plats disponibles ===");

            var plats = ObtenirPlatsDisponibles();  // Récupère la liste des plats dans la base de données

            foreach (var plat in plats)
            {
                Console.WriteLine($"{plat.Nom} - {plat.Prix_par_Portion} €");
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
