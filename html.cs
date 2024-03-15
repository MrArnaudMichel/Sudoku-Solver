using System;
using System.Collections.Generic;
using System.IO;

// Classe qui permet de générer du code HTML à partir d'une grille de sudoku
public class html
{
    static void Main()
    {
        // On charge la grille et on l'enregistre en HTML
        save();
		// On concatène les trois fichiers HTML en un seul
		concaFile();
    }

	private static string[] listegrille = new string[]
		{
		"/grillefile/grille_facile",
		"/grillefile/solution_facile",
		"/grillefile/grille_normal",
		"/grillefile/solution_normal",
		"/grillefile/grille_difficile",
		"/grillefile/solution_difficile"
		};

    // Méthode qui enregistre une grille de sudoku en HTML dans un fichier
    public static void save()
    {
        // Tableau contenant les balises HTML pour construire une grille
        string[] balises = new string[]
        {
            "<table", // 0
            "</table>", // 1
            "<tbody>", // 2
            "</tbody>", // 3
            "<tr>", // 4
            "</tr>", // 5
            "<td>", // 6
            "</td>", // 7
			"<div>", // 8
        };
    // Tableau contenant les noms de classes CSS pour chaque grille et solution
        string[] classes = new string[]
        {
            "grille-facile", // 0
            "solution-facile", // 1
            "grille-moyen", // 2
            "solution-moyen", // 3
            "grille-difficile", // 4
            "solution-difficile", // 5
			"_facile", // 6
			"_moyen", // 7
			"_difficile" // 8
        };
		int compteur_id = 6; // Compteur qui servira à donner un nom unique aux boutons d'affichage des solutions
		// On efface le contenu du fichier html2.html
		File.WriteAllText(Directory.GetCurrentDirectory() + "/html2.html", string.Empty);
		// On ouvre le contenu du fichier html2.html
        StreamWriter sw =  File.AppendText(Directory.GetCurrentDirectory() + "/html2.html");
		// Pour chaque grille de Sudoku
        for (int k = 0; k < 6; k++)
        {
	        if (k % 2 == 0)
	        {
		        sw.WriteLine("<div>");
	        }
	        Console.WriteLine(listegrille[k]);
			int[,] grille = LoadGrille(listegrille[k]);
            sw.WriteLine(balises[0] + " class=\""+classes[k]+"\">");

            sw.WriteLine(balises[2]);
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                sw.WriteLine(balises[4]);
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    sw.WriteLine(balises[6] + grille[i, j] + balises[7]);
                }

                sw.WriteLine(balises[5]);
            }

            sw.WriteLine(balises[3]);
            sw.WriteLine(balises[1]);
			if (k % 2 == 0){
				sw.WriteLine("<input type=\"checkbox\" name=\"grille" +classes[compteur_id] + "\" id=\"grille" +classes[compteur_id] + "\"><button><label for=\"grille" +classes[compteur_id] + "\">Afficher la solution</label></button>");
				compteur_id++;
			}

			if (k % 2 == 1)
			{
				sw.WriteLine("</div>");
			}
			
        }

        sw.Close();		
    }
        
    public static int[,] LoadGrille(string cheminFichier)
    {
        // On défini un entier testchar
        int testchar;
        // On lit le contenu du fichier en utilisant la méthode File.ReadAllLines
        string[] lignes = File.ReadAllLines(Directory.GetCurrentDirectory() + cheminFichier);

        // On détermine la longueur de la grille en prenant la longueur de la première ligne (on suppose que toutes les lignes ont la même longueur)
        int longueurGrille = lignes[0].Length;

        // On crée une nouvelle grille de la bonne taille
        int[,] grille = new int[lignes.Length, longueurGrille];

        // On parcourt chaque ligne de la grille
        for (int i = 0; i < lignes.Length; i++)
        {
            // On parcourt chaque caractère de la ligne
            for (int j = 0; j < longueurGrille; j++)
            {
                // On convertit le caractère en entier
                testchar = (int)(lignes[i][j]);
                // Si la valeur est un caractère
                if (testchar > 58)
                {
                    // Et on lui affecte la valeur
                    grille[i, j] = testchar - 55;
                }
                // Sinon
                else
                {
                    // On lui affecte l'entier
                    grille[i, j] = int.Parse(lignes[i][j].ToString());
                }
            }
        }
		Console.WriteLine(cheminFichier);
        return grille;
    }

	public static void concaFile()
	{
		if (!File.Exists("html1.html") || !File.Exists("html2.html") || !File.Exists("html3.html"))
        	{
         	   Console.WriteLine("Un des fichiers n'existe pas.");
         	   return;
        	}

        	// Ouvre les fichiers en lecture
        	using (StreamReader sr1 = new StreamReader("html1.html"))
        	using (StreamReader sr2 = new StreamReader("html2.html"))
        	using (StreamReader sr3 = new StreamReader("html3.html"))
        	{
			File.WriteAllText("index.html", string.Empty);
            // Ouvre le fichier index.html en écriture
            using (StreamWriter sw = new StreamWriter("index.html"))
            {
                // Concatène les fichiers html dans index.html
                sw.Write(sr1.ReadToEnd());
                sw.Write(sr2.ReadToEnd());
                sw.Write(sr3.ReadToEnd());
            }
		}
	}
}