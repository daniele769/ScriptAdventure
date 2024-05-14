using Godot;
using NLog.Filters;
using System;
using System.Collections.Generic;

public partial class new_script : Node
{
    public int[,] matrice = new int[6, 6];

    public void InizializzaMatrice()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                matrice[i, j] = 0;
            }
        }
    }
    public void AggiungiNave(int x, int y)
    {
        matrice[x, y] = 1;
    }

    public void DistruggiNave(int x, int y)
    {
        if(matrice[x, y] == 1)
        {
            matrice[x, y] = -1;
            Console.WriteLine("Colpito!");
            return;
        }
        Console.WriteLine("Mancato!");
    }

}
