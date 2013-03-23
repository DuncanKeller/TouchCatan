using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    class Vertex
    {
        public List<Tile> adjacent = new List<Tile>();
        public Player space = null;
        public bool usable = true;
        public bool city = false;

        public List<Tile> Adjacent
        {
            get { return adjacent; }
        }

        public Vertex(Tile t)
        {
            adjacent.Add(t);
        }

        public void Add(Tile t)
        {
            adjacent.Add(t);
        }
    }
}
