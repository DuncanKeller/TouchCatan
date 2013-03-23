using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SettlersOfCatan
{
    enum VertexDirection
    {
        TopRight,
        Right,
        BottomRight,
        BottomLeft,
        Left,
        TopLeft
    }

    enum EdgeDirection 
    {
        Top ,
        TopRight,
        BottomRight,
        Bottom,
        BottomLeft,
        TopLeft
    }

    enum TileType
    {
        wheat,
        brick,
        ore,
        sand, 
        forest,
        plain,
        water,
        port
    }

    class Tile
    {

        protected int xindex;
        protected int yindex;
        protected Dictionary<VertexDirection, Vertex> settlements;
        protected Dictionary<EdgeDirection, Edge> roads;
        protected TileType type;
        protected bool robber = false;
        protected int frequencyNum = 0;

        protected int width;
        protected int height;
        protected Texture2D backTex;
        protected Texture2D frontTex;
        protected Dictionary<EdgeDirection, Tile> tiles = new Dictionary<EdgeDirection,Tile>();

        public static Dictionary<TileType, Resource> tileToResource = new Dictionary<TileType, Resource>();

        public Point Center
        {
            get
            {
                return new Point(Pos.X + (width / 2), Pos.Y + (height / 2));
            }
        }

        public Point Pos
        {
            get
            {
                return new Point(xindex * (width - 100) + (yindex * 285),
                    (((yindex * height) - (xindex * (height / 2))) - (yindex * 127)) + (xindex * 12));
            }
        }

        public int Frequency
        {
            get { return frequencyNum; }
        }

        public TileType Type
        {
            get { return type; }
        }

        public Tile(int x, int y, TileType t)
        {
            settlements = new Dictionary<VertexDirection, Vertex>();
            roads = new Dictionary<EdgeDirection, Edge>();
            type = t;
            xindex = x;
            yindex = y;

            if (type != TileType.port &&
                type != TileType.water)
            {
                backTex = TM.backTextures[type];
                if (backTex != null)
                {
                    width = backTex.Width - 2;
                    height = backTex.Height - 60;
                }
                frontTex = TM.frontTextures[type];
            }
        }

        public virtual void Init()
        {

        }

        public void Update()
        {
            
        }

        public void DistributeResources()
        {
            foreach (VertexDirection dir in Enum.GetValues(typeof(VertexDirection)))
            {
                if (settlements[dir].space != null)
                {
                    if (settlements[dir].city)
                    {
                        settlements[dir].space.Resources[tileToResource[type]] += 3;
                    }
                    else
                    {
                        settlements[dir].space.Resources[tileToResource[type]]++;
                    }
                }
            }
        }

        public void SetAdjacency(EdgeDirection dir, Tile t)
        {
            tiles[dir] = t;
        }

        public static TileType RandomType()
        {
            TileType testType;
            do
            {
                int num = Config.rand.Next(6);
                switch (num)
                {
                    case 0:
                        testType = TileType.brick;
                        break;
                    case 1:
                        testType = TileType.forest;
                        break;
                    case 2:
                        testType = TileType.ore;
                        break;
                    case 3:
                        testType = TileType.plain;
                        break;
                    case 4:
                        testType = TileType.wheat;
                        break;
                    default:
                        testType = TileType.sand;
                        break;
                }
            }
            while (World.remainingPieces[testType] <= 0);
            World.remainingPieces[testType]--;

            return testType;
        }

        public void SetFrequency(int i)
        {
            frequencyNum = i;
        }

        public void SetSettlement(Vertex s, VertexDirection d)
        {
            settlements[d] = s;
        }

        public void SetRoad(Edge e, EdgeDirection d)
        {
            roads[d] = e;
        }

        public Vertex GetSettlement(VertexDirection d)
        {
            if (!settlements.ContainsKey(d))
            { return null; }
            return settlements[d];
        }

        public Edge GetRoad(EdgeDirection d)
        {
            if (!roads.ContainsKey(d))
            { return null; }
            return roads[d];
        }

        public bool InsideSettlementRadius(int x, int y, VertexDirection d)
        {
            float testDist = 50;
            int testx = Center.X;
            int testy = Center.Y;

            if (d == VertexDirection.TopRight)
            {
                testx += (int)(width / 3.8);
                testy -= (int)(height / 3.8f);
            }
            else if (d == VertexDirection.Right)
            {
                testx += (int)(width / 2.1);
                testy += (int)(height / 5);
            }
            else if (d == VertexDirection.BottomRight)
            {
                testx += (int)(width / 3.8);
                testy += (int)(height / 1.5f);
            }
            else if (d == VertexDirection.BottomLeft)
            {
                testx -= (int)(width / 3.8);
                testy += (int)(height / 1.5f);
            }
            else if (d == VertexDirection.Left)
            {
                testx -= (int)(width / 2.1);
                testy += (int)(height / 5);
            }
            else
            {
                testx -= (int)(width / 3.8);
                testy -= (int)(height / 3.8f);
            }

            double dist = Math.Sqrt(Math.Pow(x - testx, 2) + Math.Pow(y - testy, 2));

            if (dist < testDist)
            {
                return true;
            }
            return false;
        }

        public bool InsideRoadRadius(int x, int y, EdgeDirection d)
        {
            float testDist = 50;
            int testx = Center.X;
            int testy = Center.Y;

            if (d == EdgeDirection.Top)
            {
                testy -= (int)(height / 3.8f);
            }
            else if (d == EdgeDirection.TopRight)
            {
                testx += width / 3;
            }
            else if (d == EdgeDirection.BottomRight)
            {
                testx += (int)(width / 2.8f);
                testy += (int)(height / 2.4);
            }
            else if (d == EdgeDirection.Bottom)
            {
                testy += (int)(height / 1.5f);
            }
            else if (d == EdgeDirection.BottomLeft)
            {
                testx -= (int)(width / 2.8f);
                testy += (int)(height / 2.4);
            }
            else
            {
                testx -= width / 3;
            }

            double dist = Math.Sqrt(Math.Pow(x - testx, 2) + Math.Pow(y - testy, 2));
            
            if (dist < testDist)
            {
                return true;
            }
            return false;
        }

        public void DrawSettlements(SpriteBatch sb)
        {
            

            if (type != TileType.port && type != TileType.water)
            {
                if (tiles[EdgeDirection.Top] != null)
                {
                    if (tiles[EdgeDirection.Top].Type == TileType.water ||
                        tiles[EdgeDirection.Top].Type == TileType.port)
                    {
                        if (settlements[VertexDirection.TopLeft].space != null)
                        {
                            Color c = settlements[VertexDirection.TopLeft].space.Color;
                            Texture2D t = TM.pieces[PieceType.settlement];
                            sb.Draw(t, new Rectangle(Center.X - (int)(width / 3.8) - t.Width / 2,
                                (int)(Center.Y - (height / 3.8) - t.Height / 1.5), t.Width, t.Height), c);
                        }
                        if (settlements[VertexDirection.TopRight].space != null)
                        {
                            Color c = settlements[VertexDirection.TopRight].space.Color;
                            Texture2D t = TM.pieces[PieceType.settlement];
                            sb.Draw(t, new Rectangle((Center.X + (int)(width / 3.8)) - t.Width / 2,
                                (int)(Center.Y - (height / 3.8) - t.Height / 1.5), t.Width, t.Height), c);
                        }
                    }
                }
            }
        }

        public void DrawRoads(SpriteBatch sb)
        {
            if (type != TileType.port && type != TileType.water)
            {
                if (tiles[EdgeDirection.Top] != null)
                {
                    if (tiles[EdgeDirection.Top].Type == TileType.water ||
                        tiles[EdgeDirection.Top].Type == TileType.port)
                    {
                        if (roads[EdgeDirection.Top].space != null)
                        {
                            Color c = roads[EdgeDirection.Top].space.Color;
                            Texture2D t = TM.roadTextures[EdgeDirection.Top];
                            sb.Draw(t, new Rectangle(Center.X - t.Width / 2,
                                Pos.Y + height / 6, t.Width, t.Height), c);
                        }
                    }
                }
                if(tiles[EdgeDirection.TopLeft] != null)
                {
                    if (tiles[EdgeDirection.TopLeft].type == TileType.water ||
                        tiles[EdgeDirection.TopLeft].Type == TileType.port)
                    {
                        if (roads[EdgeDirection.TopLeft].space != null)
                        {
                            Color c = roads[EdgeDirection.TopLeft].space.Color;
                            Texture2D t = TM.roadTextures[EdgeDirection.TopLeft];
                            sb.Draw(t, new Rectangle(((int)(Center.X - (width / 2.2))),
                               Pos.Y + height / 4, t.Width, t.Height), c);
                        }
                    }
                }
                if (tiles[EdgeDirection.TopRight] != null)
                {
                    if (tiles[EdgeDirection.TopRight].Type == TileType.water ||
                        tiles[EdgeDirection.TopRight].Type == TileType.port)
                    {
                        if (roads[EdgeDirection.TopRight].space != null)
                        {
                            Color c = roads[EdgeDirection.TopRight].space.Color;
                            Texture2D t = TM.roadTextures[EdgeDirection.TopRight];
                            sb.Draw(t, new Rectangle(Center.X + width / 2,
                                Pos.Y + height / 4, t.Width, t.Height), c);
                        }
                    }
                }
            }
        }

        public void DrawFront(SpriteBatch sb)
        {
            if (frontTex != null)
            {
                sb.Draw(frontTex, new Rectangle(Pos.X - 1, Pos.Y, frontTex.Width, frontTex.Height), Color.White);
            }


            if (type != TileType.port && type != TileType.water)
            {
                if (roads[EdgeDirection.BottomLeft].space != null)
                {
                    Color c = roads[EdgeDirection.BottomLeft].space.Color;
                    Texture2D t = TM.roadTextures[EdgeDirection.BottomLeft];
                    sb.Draw(t, new Rectangle((Center.X - (width / 2)) + 15,
                       Center.Y + height / 4, t.Width, t.Height), c);
                }
                if (roads[EdgeDirection.Bottom].space != null)
                {
                    Color c = roads[EdgeDirection.Bottom].space.Color;
                    Texture2D t = TM.roadTextures[EdgeDirection.Bottom];
                    sb.Draw(t, new Rectangle(Center.X - t.Width / 2,
                        (Pos.Y + height) + (t.Height / 2) - 4, t.Width, t.Height), c);
                }
                if (roads[EdgeDirection.BottomRight].space != null)
                {
                    Color c = roads[EdgeDirection.BottomRight].space.Color;
                    Texture2D t = TM.roadTextures[EdgeDirection.BottomRight];
                    sb.Draw(t, new Rectangle((Pos.X + width) - t.Width - 15,
                       Center.Y + height / 4, t.Width, t.Height), c);
                }

                if (settlements[VertexDirection.BottomLeft].space != null)
                {
                    Color c = settlements[VertexDirection.BottomLeft].space.Color;
                    Texture2D t = TM.pieces[PieceType.settlement];
                    sb.Draw(t, new Rectangle(Center.X - (int)(width / 3.8) - t.Width / 2,
                        (Center.Y  + (int)((height / 1.5) - t.Height / 1.5)), t.Width, t.Height), c);
                }
                if (settlements[VertexDirection.BottomRight].space != null)
                {
                    Color c = settlements[VertexDirection.BottomRight].space.Color;
                    Texture2D t = TM.pieces[PieceType.settlement];
                    sb.Draw(t, new Rectangle((Center.X + (int)(width / 3.8)) - t.Width / 2,
                        (Center.Y + (int)((height / 1.5) - t.Height / 1.5)), t.Width, t.Height), c);
                }

                if (settlements[VertexDirection.Right].space != null ||
                    settlements[VertexDirection.BottomRight].space != null ||
                    settlements[VertexDirection.BottomLeft].space != null ||
                    settlements[VertexDirection.Left].space != null ||
                    settlements[VertexDirection.TopLeft].space != null ||
                    settlements[VertexDirection.TopRight].space != null )
                {
                    int cunt = 3;
                }


                if (type == TileType.brick)
                {
                    int forg = 4;
                }
                
            }
            
        }

        public void DrawBack(SpriteBatch sb)
        {
            int xpos = Pos.X;
            int ypos = Pos.Y;

            Color c = Color.Red;

            if (backTex != null)
            {
                sb.Draw(backTex, new Rectangle(xpos - 1, ypos, backTex.Width, backTex.Height), Color.White);
            }

            
        }


    }
}
