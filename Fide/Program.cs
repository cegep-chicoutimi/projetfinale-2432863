using Tournoi;
using MySql.Data.MySqlClient;


namespace Fide
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestion des parties FIDE ===");
                Console.WriteLine("1. Juge - Saisir une partie");
                Console.WriteLine("2. Responsable - Consigner une partie");
                Console.WriteLine("3. Quitter");
                Console.Write("Choix : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        juge.Lancer();
                        break;
                    case "2":
                        responsable.Lancer();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Choix invalide.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}

