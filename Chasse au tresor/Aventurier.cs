using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chasse_au_tresor
{
    public class Aventurier
    {
        private string nom;
        private string instruction;
        private (int x,int y) position;
        private orientation direction;
        private int nbTresor;

        public Aventurier(string nom, (int x,int y) position, char direction, string instruction)
        {
            this.nom = nom;
            this.instruction = instruction;
            this.position = position;
            this.direction = parseOrienation(direction);
            nbTresor = 0;
        }
        public string Nom { get { return nom; } }
        public string Instruction { get { return instruction; } }
        public (int x, int y) Position { get { return position; } set { this.position = value; } }
        public orientation Direction { get { return direction; } set { this.direction = value; } }
        public int tresor { get { return nbTresor; } }

        #region enum orientation
        public enum orientation
        {
            Nord,
            Est,
            Sud,
            Ouest
        }
        public static orientation parseOrienation(char c)
        {
            orientation o1 = new orientation();
            switch (c)
            {
                case 'N':
                    o1 = orientation.Nord;
                    return o1;
                case 'E':
                    o1 = orientation.Est;
                    return o1;
                case 'S':
                    o1 = orientation.Sud;
                    return o1;
                case 'O':
                    o1 = orientation.Ouest;
                    return o1;
            }
            return o1;
        }
        #endregion
        
        #region action
        public void avancer()
        {
            position = futurePosition();
        }

        public (int x, int y) futurePosition ()
        {
            switch (direction)
            {
                case orientation.Nord:
                    return (position.x, position.y - 1);
                case orientation.Sud:
                    return (position.x, position.y + 1);
                case orientation.Est:
                    return (position.x + 1, position.y);
                case orientation.Ouest:
                    return (position.x - 1, position.y);
            }
            return(0,0);
        }
        public orientation tourner(char newDirection)
        {
            switch (direction)
            {
                case orientation.Nord:
                    if(newDirection.Equals('D'))
                        return orientation.Est;
                    return orientation.Ouest;

                case orientation.Sud:
                    if (newDirection.Equals('D'))
                        return orientation.Ouest;
                    return orientation.Est;

                case orientation.Est:
                    if (newDirection.Equals('D'))
                        return orientation.Sud;
                    return orientation.Nord;

                case orientation.Ouest:
                    if (newDirection.Equals('D'))
                        return orientation.Nord;
                    return orientation.Sud;                    
            }
            return orientation.Nord;
        }
        #endregion
        public void trouverTresor()
        {
            nbTresor++;
        }
    }
}
