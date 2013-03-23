using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SettlersOfCatan
{
    enum Building
    {
        settlement,
        city,
        road
    }

    enum Resource
    {
        brick,
        ore,
        wheat,
        sheep,
        wood
    }

    class Player
    {
        Queue<Building> buildQueue = new Queue<Building>();
        Dictionary<Resource, int> resources = new Dictionary<Resource, int>();
        Color color;
        int index;

        public Color Color
        {
            get { return color; }
        }

        public Queue<Building> BuildQueue
        {
            get { return buildQueue; }
            set { buildQueue = value; }
        }

        public Dictionary<Resource, int> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        public Player(int i, Color c)
        {
            index = i;
            color = c;
        }

        
    }
}
