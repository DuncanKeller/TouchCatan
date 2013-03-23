using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SettlersOfCatan
{
    class World
    {
        static Camera cam;
        static List<List<Tile>> tiles = new List<List<Tile>>();
        public static Dictionary<TileType, int> remainingPieces = new Dictionary<TileType, int>();
        static List<Player> players = new List<Player>();
        static int currentPlayer = 0;

        public static Player CurrentPlayer
        {
            get
            {
                if (currentPlayer >= 0 && currentPlayer < players.Count)
                {
                    return players[currentPlayer];
                }
                return null;
            }
            set { players[currentPlayer] = value; }
        }

        public static Camera Cam
        {
            get { return cam; }
        }

        public static List<Tile> Tiles
        {
            get
            {
                List<Tile> tileList = new List<Tile>();
                foreach (List<Tile> t in tiles)
                {
                    tileList.AddRange(t);
                }
                return tileList;
            }
        }

        public static void Init()
        {
            players.Add(new Player(0, Color.Orange));
            CurrentPlayer.BuildQueue.Enqueue(Building.settlement);
            CurrentPlayer.BuildQueue.Enqueue(Building.settlement);
            CurrentPlayer.BuildQueue.Enqueue(Building.settlement);

            cam = new Camera();

            StreamReader sr = new StreamReader("map.txt");
            Config.xNum = Int32.Parse(sr.ReadLine());
            Config.yNum = Int32.Parse(sr.ReadLine());

            remainingPieces[TileType.brick] = 4;
            remainingPieces[TileType.forest] = 4;
            remainingPieces[TileType.ore] = 4;
            remainingPieces[TileType.plain] = 4;
            remainingPieces[TileType.forest] = 4;
            remainingPieces[TileType.wheat] = 4;
            remainingPieces[TileType.sand] = 1;

            for (int x = 0; x < Config.xNum; x++)
            {
                string line = sr.ReadLine();
                tiles.Add(new List<Tile>());

                for (int y = 0; y < Config.yNum; y++)
                {
                    char c = line[y];

                    switch (c)
                    {
                        case '-':
                            tiles[x].Add(new Tile(x, y, TileType.water));
                            break;
                        case '+':
                            tiles[x].Add(new Port(x, y));
                            break;
                        case 'x':
                            tiles[x].Add(null);
                            break;
                        case 'o':
                            tiles[x].Add(new Tile(x, y, Tile.RandomType()));
                            break;
                    }
                }
            }

            sr.Close();

            // adjacency 
            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        tiles[x][y].SetAdjacency(EdgeDirection.Top, GetTile(x + 1, y - 1));
                        tiles[x][y].SetAdjacency(EdgeDirection.TopRight, GetTile(x + 1, y));
                        tiles[x][y].SetAdjacency(EdgeDirection.BottomRight, GetTile(x, y + 1));
                        tiles[x][y].SetAdjacency(EdgeDirection.Bottom, GetTile(x - 1, y + 1));
                        tiles[x][y].SetAdjacency(EdgeDirection.BottomLeft, GetTile(x - 1, y));
                        tiles[x][y].SetAdjacency(EdgeDirection.TopLeft, GetTile(x, y - 1));
                    }
                }
            }

            // link roads / settlements
            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        if (tiles[x][y].Type != TileType.water &&
                            tiles[x][y].Type != TileType.port)
                        {
                            SetSettlement(x, y, VertexDirection.TopRight);
                            SetSettlement(x, y, VertexDirection.Right);
                            SetSettlement(x, y, VertexDirection.BottomRight);
                            SetSettlement(x, y, VertexDirection.BottomLeft);
                            SetSettlement(x, y, VertexDirection.Left);
                            SetSettlement(x, y, VertexDirection.TopLeft);

                            SetRoad(x, y, EdgeDirection.Top);
                            SetRoad(x, y, EdgeDirection.TopRight);
                            SetRoad(x, y, EdgeDirection.BottomRight);
                            SetRoad(x, y, EdgeDirection.Bottom);
                            SetRoad(x, y, EdgeDirection.BottomLeft);
                            SetRoad(x, y, EdgeDirection.TopLeft);
                        }
                    }
                }
            }

            // init
            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        tiles[x][y].Init();
                    }
                }
            }

        }

        private static void SetSettlement(int x, int y, VertexDirection d)
        {
            int xMod = 0;
            int yMod = 0;
            int xMod2 = 0;
            int yMod2 = 0;
            VertexDirection newDir;
            VertexDirection newDir2;

            if (d == VertexDirection.TopRight)
            {
                xMod = 1;
                newDir = VertexDirection.Left;
                xMod2 = 1;
                yMod2 = -1;
                newDir2 = VertexDirection.BottomRight;

            }
            else if (d == VertexDirection.Right)
            {
                xMod = 1;
                newDir = VertexDirection.BottomLeft;
                yMod2 = 1;
                newDir2 = VertexDirection.TopLeft;
            }
            else if (d == VertexDirection.BottomRight)
            {
                yMod = 1;
                newDir = VertexDirection.Left;
                xMod2 = -1;
                yMod2 = 1;
                newDir2 = VertexDirection.TopRight;
            }
            else if (d == VertexDirection.BottomLeft)
            {
                xMod = -1;
                yMod = 1;
                newDir = VertexDirection.TopLeft;
                xMod2 = -1;
                newDir2 = VertexDirection.Right;
            }
            else if (d == VertexDirection.Left)
            {
                xMod = -1;
                newDir = VertexDirection.TopRight;
                yMod2 = -1;
                newDir2 = VertexDirection.BottomRight;
            }
            else
            {
                yMod = -1;
                newDir = VertexDirection.Right;
                xMod2 = 1;
                yMod2 = -1;
                newDir2 = VertexDirection.BottomLeft;
            }

            if (x + xMod >= 0 && x + xMod < Config.xNum &&
                y + yMod >= 0 && y + yMod < Config.yNum)
            {
                if (tiles[x + xMod][y + yMod] != null)
                {
                    if (tiles[x + xMod][y + yMod].GetSettlement(newDir) != null)
                    {
                        tiles[x][y].SetSettlement(tiles[x + xMod][y + yMod].GetSettlement(newDir), d);
                        tiles[x + xMod][y + yMod].GetSettlement(newDir).Add(tiles[x][y]);
                    }
                    else
                    {
                        if (x + xMod2 >= 0 && x + xMod2 < Config.xNum &&
                            y + yMod2 >= 0 && y + yMod2 < Config.yNum)
                        {
                            if (tiles[x + xMod2][y + yMod2] != null)
                            {
                                if (tiles[x + xMod2][y + yMod2].GetSettlement(newDir2) != null)
                                {
                                    tiles[x][y].SetSettlement(tiles[x + xMod2][y + yMod2].GetSettlement(newDir2), d);
                                    tiles[x][y].GetSettlement(d).Add(tiles[x][y]);
                                }
                                else
                                {
                                    tiles[x][y].SetSettlement(new Vertex(tiles[x][y]), d);
                                }
                            }
                        }
                    }
                }
            }

          
        }

        private static void SetRoad(int x, int y, EdgeDirection d)
        {
            int xMod = 0;
            int yMod = 0;
            EdgeDirection newDir;

            if (d == EdgeDirection.Top)
            {
                xMod = 1;
                yMod = -1;
                newDir = EdgeDirection.Bottom;
            }
            else if (d == EdgeDirection.TopRight)
            {
                xMod = 1;
                newDir = EdgeDirection.BottomLeft;
            }
            else if (d == EdgeDirection.BottomRight)
            {
                yMod = 1;
                newDir = EdgeDirection.TopLeft;
            }
            else if (d == EdgeDirection.Bottom)
            {
                xMod = -1;
                yMod = 1;
                newDir = EdgeDirection.Top;
            }
            else if (d == EdgeDirection.BottomLeft)
            {
                xMod = -1;
                newDir = EdgeDirection.TopRight;
            }
            else
            {
                yMod = -1;
                newDir = EdgeDirection.BottomRight;
            }

            if (x + xMod >= 0 && x + xMod < Config.xNum &&
                y + yMod >= 0 && y + yMod < Config.yNum)
            {
                if (tiles[x + xMod][y + yMod].GetRoad(newDir) != null)
                {
                    tiles[x][y].SetRoad(tiles[x + xMod][y + yMod].GetRoad(newDir), d);
                }
                else
                {
                    tiles[x][y].SetRoad(new Edge(), d);
                }
            }
        }

        public static Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= Config.xNum ||
                y < 0 || y >= Config.yNum)
            {
                return null;
            }

            return tiles[x][y];

        }

        public static void Update()
        {
            UpdateCam();
            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        tiles[x][y].Update();
                    }
                }
            }

        }

        public static void AddResources()
        {
            int[] dice = new int[2];
            for (int i = 0; i < 2; i++)
            {
                dice[i] = Config.rand.Next(1, 7);
            }
            int sum = dice[0] + dice[1];

            foreach (Tile t in Tiles)
            {
                if (t != null)
                {
                    if (t.Frequency == sum)
                    {
                        t.DistributeResources();
                    }
                }
            }
        }

        public static void NextTurn()
        {
            currentPlayer++;
            if (currentPlayer >= players.Count)
            {
                currentPlayer = 0;
                AddResources();
            }
        }

        public static void UpdateCam()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            { cam._pos.X -= 10; }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            { cam._pos.X += 10; }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            { cam._pos.Y -= 10; }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            { cam._pos.Y += 10; }

        }

        public static void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, cam.get_transformation(g));

            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        tiles[x][y].DrawBack(sb);
                    }
                }
            }

            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        tiles[x][y].DrawRoads(sb);
                        tiles[x][y].DrawSettlements(sb);
                    }
                }
            }

            for (int x = tiles.Count - 1; x >= 0; x--)
            {
                for (int y = 0; y < tiles[x].Count; y++)
                {
                    if (tiles[x][y] != null)
                    {
                        tiles[x][y].DrawFront(sb);
                    }
                }
            }
            Input.TestDraw(sb);
            sb.End();
        }

    }
}
