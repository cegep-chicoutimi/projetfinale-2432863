using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Tournoi
{
    class juge
    {
        public static void Lancer()
        {
            Console.WriteLine("--- ENTRÉE DES DONNÉES DE LA PARTIE ---");

            // Saisie joueur blanc
            Console.Write("Nom joueur blanc : ");
            string nomBlanc = Console.ReadLine();
            Console.Write("Prénom joueur blanc : ");
            string prenomBlanc = Console.ReadLine();
            Console.Write("ID joueur blanc : ");
            int idBlanc = int.Parse(Console.ReadLine());

            // Saisie joueur noir
            Console.Write("Nom joueur noir : ");
            string nomNoir = Console.ReadLine();
            Console.Write("Prénom joueur noir : ");
            string prenomNoir = Console.ReadLine();
            Console.Write("ID joueur noir : ");
            int idNoir = int.Parse(Console.ReadLine());

            // Saisie du gagnant
            Console.Write("Qui a gagné ? (b pour blanc, n pour noir) : ");
            string gagnant = Console.ReadLine().ToLower();

            int gagnantId = gagnant == "b" ? idBlanc : idNoir;
            int perdantId = gagnant == "b" ? idNoir : idBlanc;

            // Saisie de l'état final du plateau
            Console.WriteLine("Entrer la position finale (8 lignes, de haut en bas) :");
            string[] plateau = new string[8];
            for (int i = 0; i < 8; i++)
            {
                plateau[i] = Console.ReadLine();
            }

            // Connexion MySQL
            string connStr = "server=localhost;user=root;password=tonmdp;database=ta_base;";
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            // Vérifie si joueurs déjà existants sinon les insère
            InsererOuMettreAJourJoueur(conn, idBlanc, nomBlanc, prenomBlanc);
            InsererOuMettreAJourJoueur(conn, idNoir, nomNoir, prenomNoir);

            // Mise à jour des victoires/défaites
            using var cmd = new MySqlCommand("EnregistrerVictoire", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idGagnant", gagnantId);
            cmd.Parameters.AddWithValue("@idPerdant", perdantId);
            cmd.ExecuteNonQuery();

            Console.WriteLine("\nPartie enregistrée avec succès !");
            Console.WriteLine("État final du plateau :");
            foreach (var ligne in plateau)
                Console.WriteLine(ligne);
        }

        // Petite fonction pour insérer ou mettre à jour un joueur
        static void InsererOuMettreAJourJoueur(MySqlConnection conn, int id, string nom, string prenom)
        {
            string query = "INSERT INTO fide_joueurs (idJoueur, Nom, Prenom, Victoires, Defaites) " +
                           "VALUES (@id, @nom, @prenom, 0, 0) " +
                           "ON DUPLICATE KEY UPDATE Nom=@nom, Prenom=@prenom;";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.ExecuteNonQuery();
        }
    }
}
