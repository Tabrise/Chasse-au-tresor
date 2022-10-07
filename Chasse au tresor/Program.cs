using System;
using System.Collections;
using System.Collections.Generic;
namespace Chasse_au_tresor
{
    class Program
    {
        static void Main(string[] args)
        {
            Carte carte = new Carte();
            carte.executerInstruction();
            carte.ecrireSortie();
        }
    }

}