using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chasse_au_tresor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chasse_au_tresor.Aventurier;

namespace Chasse_au_tresor.Tests
{
    [TestClass()]
    public class AventurierTests
    {
        Aventurier a = new Aventurier("Testeur", (1, 1), 'E', "AD");

        [TestMethod()]
        public void parseOrienationTest()
        {
            orientation o1 = orientation.Est;
            Assert.AreEqual(o1, a.Direction);
        }

        [TestMethod()]
        public void futurePositionTest()
        {
            (int x, int y) position= (1, 1);
            a.avancer();
            position = (position.x + 1,position.y);
            Assert.AreEqual(position, a.Position,"Position différente");
        }

        [TestMethod()]
        public void tournerTest()
        {
            orientation o1 = a.Direction;
            char[] newDirection = a.Instruction.ToCharArray();
            for(int i=0; i < newDirection.Length; i++)
            {
                if (newDirection[i] == 'D' || newDirection[i] == 'G')
                {
                    o1 = a.tourner(newDirection[i]);
                    a.Direction = a.tourner(newDirection[i]);
                }
            }
            Assert.AreEqual(o1,a.Direction,"Mauvaise direction");
        }
    }
}