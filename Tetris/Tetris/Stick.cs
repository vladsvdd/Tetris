﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Stick : Figure
    {
        public Stick(int x, int y, char symbol)
        {
            points[0] = new Point(x, y, symbol);
            points[1] = new Point(x, y + 1, symbol);
            points[2] = new Point(x, y + 2, symbol);
            points[3] = new Point(x, y + 3, symbol);
        }
    }
}
