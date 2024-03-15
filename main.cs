using System;
using System.Collections.Generic;
using System.IO;

public class sudoku
{
    static void Main(string[] args)
    {
		// Affiche les Arguments passé en lanceant le Programme
        Console.WriteLine("Arguments: ");
        foreach (string arg in args)
        {
            Console.Write(arg + " ");
        }
		// Fais un saut de ligne
        Console.WriteLine();
		// Lance la fontion run
        run();
        
    }

	// Méthode qui charge la grille et la résout
    public static void run()
    {
        System.Console.WriteLine("----- Lancement -----");
		
		// il faut spécifier le chemin relatif depuis le ficher vers la grille (Attention au format de la grille)
        int[,] sudoku = LoadGrille("grid");

		// Lance la résolution
        ResoSudoku(sudoku);

        System.Console.WriteLine("----- Terminé -----");
    }
    
    // Méthode qui résout une grille de sudoku
    public static void ResoSudoku(int[,] grille)
    {
        // Si la grille est nulle ou vide, on ne fait rien
        if (grille == null || grille.Length == 0)
            return;
        // On appelle la méthode qui résout la grille de manière itérative
        SolverIter(grille);
    }

    
    // Méthode qui résout la grille de sudoku passée en paramètre de manière itérative
    public static void SolverIter(int[,] grille)
    {
        int[,] ngrille = copie(grille); // On copie la grille pour pouvoir la modifier sans affecter l'original
        int i = 0, j = 0; // Variables qui stockent les indices de la case en cours d'analyse
        // Tant que les indices sont dans les limites de la grille
        while (i < grille.GetLength(0) && j < grille.GetLength(1))
        {
            // Si la case est vide
            if (grille[i, j] == 0)
            {
                // On essaie de placer une valeur dans la case
                if (mettre(ref ngrille, i, j, ngrille[i, j] + 1))
                {
                    // Si la placement a réussi, on passe à la case suivante
                    if (j == grille.GetLength(1) - 1)
                    {
                        i++;
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                }
                else
                {
                    // Si le placement a échoué, on remet la case à 0 et on recule d'une case
                    ngrille[i, j] = 0;
                    if (j == 0)
                    {
                        i--;
                        j = ngrille.GetLength(0) - 1;
                    }
                    else
                    {
                        j--;
                    }
                    // Tant que la case est déjà remplie, on continue de reculer
                    while (grille[i, j] != 0)
                    {
                        if (j == 0)
                        {
                            i--;
                            j = ngrille.GetLength(0) - 1;
                        }
                        else
                        {
                            j--;
                        }
                    }
                }
            }
            else
            {
                // Si la case n'est pas vide, on passe à la case suivante
                if (j == grille.GetLength(1) - 1)
                {
                    i++;
                    j = 0;
                }
                else
                {
                    j++;
                }
            }
        }
        // On affiche la grille résolue
        affiche(ngrille);
    }

    
    // Méthode qui essaie de placer une valeur dans la case spécifiée par les indices i et j de la grille
    public static bool mettre(ref int[,] grille, int i, int j, int k)
    {
        bool res = false; // Variable qui stockera le résultat de la tentative de placement
        // Pour chaque valeur possible (de k à la longueur de la grille)
        for (int l = k; l <= grille.GetLength(0); l++)
        {
            // Si la valeur peut être placée dans la case
            if (estPlacable(grille, i, j, l))
            {
                // On place la valeur dans la case et on met res à vrai
                grille[i, j] = l;
                res = true;
                break; // On sort de la boucle
            }
        }
        return res; // On retourne le résultat de la tentative de placement
    }

    
    // Méthode qui vérifie si la case spécifiée par les indices i et j de la grille est vide ou non
    public static bool checkback(int[,] grille, int i, int j)
    {
        bool res = false; // Variable qui stockera le résultat de la vérification
        // Si la colonne spécifiée par j est la dernière colonne de la grille
        if (j == grille.GetLength(0) - 1)
        {
            // Si la case n'est pas vide
            if (grille[i, j] != 0)
            {
                res = true; // La case n'est pas vide, on met res à vrai
            }
        }
        else
        {
            // Si la colonne spécifiée par j n'est pas la dernière colonne de la grille
            // et que la case n'est pas vide
            if (grille[i, j] != 0)
            {
                res = true; // La case n'est pas vide, on met res à vrai
            }
        }
        return res; // On retourne le résultat de la vérification
    }


    
    private static bool SolverRecu(int[,] grille)
    {
        // Pour chaque ligne de la grille
        for (int i = 0; i < grille.GetLength(0); i++)
        {
            // Pour chaque colonne de la grille
            for (int j = 0; j < grille.GetLength(1); j++)
            {
                // Si la case est vide (0)
                if (grille[i, j] == 0)
                {
                    // Pour chaque valeur possible (de 1 à la longueur de la grille)
                    for (int k = 1; k < grille.GetLength(0) + 1; k++)
                    {
                        // Si la valeur peut être placée dans la case
                        if (estPlacable(grille, i, j, k))
                        {
                            // On place la valeur dans la case et on appelle récursivement la méthode sur la grille modifiée
                            grille[i, j] = k;
                            if (SolverRecu(grille))
                                return true; // Si la méthode retourne vrai, on propage cette valeur jusqu'à la fin de l'appel récursif
                            else
                                grille[i, j] = 0; // Si la méthode retourne faux, on remet la case à 0 et on essaie une autre valeur
                        }
                    }
                    return false; // Si aucune valeur ne peut être placée dans la case, on retourne faux
                }
            }
        }

        // Si on a parcouru toute la grille et qu'aucune case n'est vide, cela signifie que la grille est complète
        // On affiche la grille et on retourne vrai
        for (int i = 0; i < grille.GetLength(0); i++)
        {
            for (int j = 0; j < grille.GetLength(1); j++)
            {
                Console.Write(grille[i, j] + " ");
            }
            Console.WriteLine();
        }
        return true;
    }

    
    private static bool estPlacable(int[,] tab, int row, int col, int val)
    {
        // On met la variable retour a true
        bool retour = true;
        // Pour chaque longueur
        for (int i = 0; i < tab.GetLength(0) && retour; i++)
        {
            // Si la valeur est dans la ligne
            if (tab[i, col] != 0 && tab[i, col] == val)
            {
                // Alors on met retour a faux
                retour = false;   
            }
            // Si la valeur est dans la colonne
            if (tab[row, i] != 0 && tab[row, i] == val)
            {
                // Alors on met retour a faux
                retour = false;
            }
            // Si la valeur est présente dans le bloc contenant la case
            if (tab[
                    (int)Math.Sqrt(tab.GetLength(0)) * (row / (int)Math.Sqrt(tab.GetLength(0))) +
                    i / (int)Math.Sqrt(tab.GetLength(0)),
                    (int)Math.Sqrt(tab.GetLength(0)) * (col / (int)Math.Sqrt(tab.GetLength(0))) +
                    i % (int)Math.Sqrt(tab.GetLength(0))] == val && tab[
                    (int)Math.Sqrt(tab.GetLength(0)) * (row / (int)Math.Sqrt(tab.GetLength(0))) +
                    i / (int)Math.Sqrt(tab.GetLength(0)),
                    (int)Math.Sqrt(tab.GetLength(0)) * (col / (int)Math.Sqrt(tab.GetLength(0))) +
                    i % (int)Math.Sqrt(tab.GetLength(0))] != 0)
            {
                // Alors on met retour a faux
                retour = false;
            }
        }
        return retour;
    }

    public static int[,] evolue(int[,] grille)
    {
        //On copie la grille précedente grace à la fonction copie()
        int[,] newgrille = copie(grille);
        // On déclare un entier k ainsi qu'un compteur
        int k, compteur;
        // On défini un break
        bool breaker = false;
        // On parcourt chaque ligne de la grille
        for (int i = 0; i < grille.GetLength(0) && !breaker; i++)
        {
            // On parcourt chaqur colonne du tableau
            for (int j = 0; j < grille.GetLength(1) && !breaker; j++)
            {
                // On affecte a 0 la variable compteur et un entier compris entre 1 et la longueur du tablea a k
                compteur = 0;
                k = random.Next(1, grille.GetLength(0) + 1);
                // Tant que la fonction estPlacable renvoie false et que compteur < 50 (Pour éviter que l'algorythme tourne en boucle car pas de solution)
                while (!estPlacable(newgrille, i, j, k) || compteur < 100)
                {
                    // On réaffecte la valeur de k et on incrémente le compteur
                    k = random.Next(1, grille.GetLength(0) + 1);
                    compteur++;
                }
                // Si le compteur est inférieur a 50 (On a trouvé une Solution)
                if (compteur < 50)
                {
                    // On affecte la case i,j la valeur k
                    newgrille[i, j] = k;
                }
                // Sinon
                else
                {
                    // Pas de solution trouvée
                    breaker = true;
                }
            }
        }
        // On renvoie la grille
        return newgrille;
    }
    
    // On déclare une variable random statique afin de ne pas avoir à la recréer à chaque appel de la fonction
    private static Random random = new Random();

    // La fonction prend en paramètre une grille sous forme de tableau 2D d'entiers
    public static void Naif(int[,] grille)
    {
        // On parcourt chaque ligne de la grille
        for (int i = 0; i < grille.GetLength(0); i++)
        {
            // On parcourt chaque colonne de la grille
            for (int j = 0; j < grille.GetLength(1); j++)
            {
                // Si l'élément de la grille à cette position est égal à 0, on y met un nombre aléatoire compris entre 1 et la largeur de la grille (la méthode GetLength(0) retourne la longueur de la première dimension du tableau, c'est-à-dire le nombre de lignes)
                if (grille[i, j] == 0)
                {
                    grille[i, j] = random.Next(1, grille.GetLength(0) + 1);
                }
            }
        }
    }

    public static bool verifgrid(int[,] tab)
    {
        // On initialise la variable de résultat à true
        bool res = true;

        // On parcourt chaque ligne de la grille
        for (int i = 0; i < tab.GetLength(0) && res; i++)
        {
            // On parcourt chaque colonne de la grille
            for (int j = 0; j < tab.GetLength(1) && res; j++)
            {
                // Si l'élément de la grille à cette position n'est pas valide selon la fonction estPlacable, on met res à false
                if (!estPlacable(tab, i, j, tab[i, j])) res = false;
            }
        }

        // On retourne le résultat final
        return res;
    }

    public static int[,] copie(int[,] tab)
    {
        // On crée un tableau des dimension du tableau en parametre
        int[,] ntab = new Int32[tab.GetLength(0), tab.GetLength(1)];
        // On parcourt chaque ligne du tableau
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            // On parcourt chaque colonne du sous tableau
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                // On affecte la valeur du tableau a la position i,j la valeur du tableau passé en parametre à la position i,j
                ntab[i, j] = tab[i, j];
            }
        }

        return ntab;
    }
    
    public static int[,] LoadGrille(string cheminFichier)
    {
        // On défini un entier testchar
        int testchar;
        // On lit le contenu du fichier en utilisant la méthode File.ReadAllLines
        string[] lignes = File.ReadAllLines(cheminFichier);

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
        return grille;
    }

    public static bool verif_path(string path, string fic = "")
    {
        bool verif = false;
        if (File.Exists(path + fic))
        {
            Console.WriteLine("Le fichier existe");
            verif = true;
        }

        return verif;
    }
    public static void affiche(int[,] tab)
    {
        // On parcourt chaque ligne du tableau
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            // On parcourt chaque colonne du tableau
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                // Si la valeur du tableau est supérieur a 9 alors on la transforme en son caractere
                if (tab[i, j] > 9)
                {
                    Console.Write((char)(tab[i, j] + 55) + "\t");
                }
                // Sinon on l'affiche simplement
                else
                {
                    Console.Write(tab[i, j] + "\t");   
                }
				if ((j + 1) % (Math.Sqrt(tab.GetLength(0))) == 0){
					Console.Write("\t");
				}
            }	
            // On saute ue ligne
            Console.WriteLine("");
			if ((i + 1) % (Math.Sqrt(tab.GetLength(0))) == 0){
					Console.WriteLine();
			}
        }
    }
}
