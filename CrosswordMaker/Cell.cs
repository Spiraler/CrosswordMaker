using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordMaker
{
    internal class Cell
    {
        private bool white;
        private char thething;
        private int num;

        public Cell()
        {
            white = true;
            thething = ' ';
            num = 0;
        }

        public bool IsWhite()
        {
            return white;
        }

        public void IsWhite(bool x)
        {
            if (!x)
            {
                thething = ' ';
            }
            white = x;
        }

        public char C()
        {
            return thething;
        }

        public void C(char x)
        {
            thething = x;
        }

        public int N()
        {
            return num;
        }

        public void N(int x)
        {
            num = x;
        }
    }
}
