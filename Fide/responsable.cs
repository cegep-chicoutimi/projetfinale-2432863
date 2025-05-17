using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Tournoi
{
    class responsable
    {
        public static void Lancer()
        {
            Console.Write("Entrez le nom du fichier à lire (sans .txt) : ");
            string nomFichier = Console.ReadLine();
            string chemin = $"{nomFichier}.txt";

            if (!File.Exists(chemin))
            {
                Console.WriteLine("Fichier introuvable.");
                return;
            }

            string[] lignes = File.ReadAllLines(chemin);
            Console.WriteLine("\n--- Résumé de la partie ---");

            string nom1 = lignes[0];
            string nom2 = lignes[1];
            string id1 = lignes[2];
            string id2 = lignes[3];

            Console.WriteLine(nom1);
            Console.WriteLine(nom2);
            Console.WriteLine($"ID {id1} vs ID {id2}");

            Console.WriteLine("\nÉtat final de la partie :");
            for (int i = 4; i < lignes.Length; i++)
            {
                Console.WriteLine(lignes[i]);
            }

            // Déterminer qui a gagné
            int gagnantId = nom1.Contains("(W)") ? int.Parse(id1) : int.Parse(id2);
            int perdantId = nom1.Contains("(W)") ? int.Parse(id2) : int.Parse(id1);

            // Connexion à la base
            string connStr = "server=localhost;user=dev-2432863;password=Biscuit8851;database=h25_dad_projet3_2432863;";
            using MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand("EnregistrerVictoire", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idGagnant", gagnantId);
            cmd.Parameters.AddWithValue("@idPerdant", perdantId);
            var retour = new MySqlParameter("@totalVictoires", MySqlDbType.Int32);
            retour.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(retour);

            cmd.ExecuteNonQuery();

            Console.WriteLine($"\nLe joueur gagnant a maintenant {retour.Value} victoire(s).");
        }
    }
}

