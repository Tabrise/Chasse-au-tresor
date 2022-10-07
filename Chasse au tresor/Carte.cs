using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chasse_au_tresor
{
    public class Carte
    {
        private (int x,int y) dimensions;
        private Aventurier a;
        private List<Terrain> contenuCase;
        private Terrain[,] map;
        public Carte()
        {
            lireEntrer();
            this.map = createMap();
        }
        #region executions
        public void ecrireSortie()
        {
            using (StreamWriter sw = new StreamWriter("C:\\Users\\emili\\source\\repos\\Chasse au tresor\\Chasse au tresor\\sortie.txt"))
            {
                try
                {
                    string[] lines = new string[contenuCase.Count + 2];

                    lines[0] = "C-" + dimensions.x + "-" + dimensions.y;
                    for (int i = 0; i < contenuCase.Count; i++)
                        lines[i + 1] = contenuCase[i].sortie();

                    lines[lines.Length - 1] = "A-" + a.Nom + "-" + a.Position.x + "-" + a.Position.y + "-" + a.Direction.ToString()[0] + "-" + a.tresor;
                    foreach (string line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    sw.Close();
                    Console.WriteLine("Sortie générée");
                }
            }
        }
        public void executerInstruction()
        {
            foreach (char c in a.Instruction)
            {
                if (c == 'D' || c == 'G')
                    a.Direction = a.tourner(c);
                else
                {
                    if (verifierTout(a.futurePosition()))
                    {
                        a.avancer();
                        if(map[a.Position.x, a.Position.y].Type == 'T' && caseTresor())
                            a.trouverTresor();
                    }
                }
            }
        }
        
        #endregion

        #region constructeur
        private void lireEntrer()
        {
            List<Terrain> dicCase = new List<Terrain>();
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\emili\\source\\repos\\Chasse au tresor\\Chasse au tresor\\test.txt"))
                {
                    string line;
                    int[] position = new int[0];
                    int i=0;
                    while ((line = sr.ReadLine()) != null && line.ToCharArray()[0]!='#')
                    {
                        string[] parts = line.Split('-');
                        if (parts.Length != 0)
                        {
                            switch (parts[0])
                            {
                                case "C":
                                    int x = Int32.Parse(parts[1]);
                                    int y = Int32.Parse(parts[2]);
                                    (int x, int y) dimensions = (x, y);
                                    this.dimensions = dimensions;
                                    break;

                                case "A":
                                    x = Int32.Parse(parts[2]);
                                    y = Int32.Parse(parts[3]);
                                    (int x, int y) debut = (x, y);
                                    this.a = new Aventurier(parts[1], debut, char.Parse(parts[4]), parts[5]);
                                    break;

                                //Dans ce cas ni carte ni aventurier; contenuCase de la carte est une montagne ou tresor
                                default:
                                    (int x, int y) coord = (Int32.Parse(parts[1]), Int32.Parse(parts[2]));
                                    if (verifierBordure(coord))
                                    {
                                        if (parts.Length == 4)
                                        {
                                            int tresor = Int32.Parse(parts[3]);
                                            dicCase.Add(new Terrain(char.Parse(parts[0]), coord, tresor));
                                        }
                                        else
                                            dicCase.Add(new Terrain(char.Parse(parts[0]), coord));
                                        i++;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Case en dehors du tableau non prise en compte");
                                    }
                                    break;
                            }
                        }      
                    }
                    contenuCase = dicCase;
                    //en cas d'érreur using ferme le fichier + libère la ressource
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pus être lus \n Erreur: " + e.Message);
            }
            finally
            {       
                Console.WriteLine("Fin de lecture du fichier");
            }
        }
        private Terrain[,] createMap()
        {
            Terrain[,] map = new Terrain[dimensions.x, dimensions.y];

            foreach(Terrain t in contenuCase)
                map[t.Coord.x, t.Coord.y] = t;

            for(int i = 0; i < dimensions.x; i++)
            {
                for(int j = 0; j < dimensions.y; j++)
                {
                    if (map[i,j] == null)
                        map[i, j] = new Terrain('P', (i, j));
                }
            }
            return map;
        }
        #endregion
        #region verification
        private bool verifierBordure((int x, int y) coord)
        {
            if (coord.x >= dimensions.x || coord.x < 0)
                return false;
            if (coord.y >= dimensions.y || coord.y < 0)
                return false;

            return true;
        }
        private bool verifierCase((int x, int y) coord)
        {
            if (map[coord.x, coord.y].Type == 'M')
                return false;

            return true;
        }
        private bool verifierTout((int x, int y) coord)
        {
            if (verifierBordure(coord) && verifierCase(coord))
                return true;

            return false;
        }
        private bool caseTresor()
        {
            for (int i = 0; i < contenuCase.Count; i++)
            {
                Terrain t = contenuCase[i];
                if (t.Type.Equals('T'))
                {
                    if (a.Position == t.Coord)
                    {
                        if (t.Tresor != 0)
                        {
                            t.Tresor--;
                            contenuCase[i] = t;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion
        internal class Terrain
        {
            private char type;
            private (int x, int y) coord;
            private int tresor;
            public Terrain(char type,(int x,int y)coord, int tresor = 0)
            { 
                this.type = type;
                this.coord = coord;
                this.tresor = tresor;
            }
            public char Type { get { return type; } }
            public (int x, int y) Coord { get { return coord; } }
            public int Tresor { get { return tresor; } set{ this.tresor=value; } }
            public string sortie()
            {
                if(Type.Equals('M'))
                    return Type + "-" + coord.x + "-" + coord.y;
                else
                    return Type + "-" + coord.x + "-" + coord.y + "-" + Tresor;
            }
        }
    }
}
