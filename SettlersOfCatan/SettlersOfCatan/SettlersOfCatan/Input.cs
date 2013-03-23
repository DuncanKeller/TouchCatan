using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Windows.Input.Manipulations;
using System.Windows.Input;
using Windows7.Multitouch;

namespace SettlersOfCatan
{
    static class Input
    {
        static List<float> x = new List<float>();
        static List<float> y = new List<float>();
        static List<bool> held = new List<bool>();

        public static bool Held
        {
            get
            {
                foreach (bool h in held)
                { if (h) { return true; } }
                return false;
            }
        }

        public static void Update()
        {
            x.Clear();
            y.Clear();
            held.Clear();

            x.Add(Mouse.GetState().X);
            y.Add(Mouse.GetState().Y);
            held.Add(Mouse.GetState().LeftButton == ButtonState.Pressed);


            //testing

            if(World.CurrentPlayer != null)
            {
                if (World.CurrentPlayer.BuildQueue.Count > 0)
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Building b = World.CurrentPlayer.BuildQueue.Peek();
                        foreach (Tile t in World.Tiles)
                        {
                            if (t != null)
                            {
                                int mx = Mouse.GetState().X;
                                int my = Mouse.GetState().Y;
                                Rectangle testR = World.Cam.ScreenToWorld(mx, my);

                                if (b == Building.settlement)
                                {
                                    foreach (VertexDirection dir in Enum.GetValues(typeof(VertexDirection)))
                                    {
                                        if (t.InsideSettlementRadius(testR.X, testR.Y, dir))
                                        {
                                            Vertex r = t.GetSettlement(dir);
                                            if (r != null)
                                            {
                                                if (r.space == null)
                                                {
                                                    r.space = World.CurrentPlayer;
                                                    t.SetSettlement(r, dir);
                                                    World.CurrentPlayer.BuildQueue.Dequeue();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (b == Building.road)
                                {
                                    foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
                                    {
                                        if (t.InsideRoadRadius(testR.X, testR.Y, dir))
                                        {
                                            Edge r = t.GetRoad(dir);
                                            if (r != null)
                                            {
                                                if (r.space == null)
                                                {
                                                    r.space = World.CurrentPlayer;
                                                    t.SetRoad(r, dir);
                                                    World.CurrentPlayer.BuildQueue.Dequeue();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool Overlapping(Rectangle r)
        {
            for (int i = 0; i < x.Count; i++)
            {
                if (r.Intersects(new Rectangle((int)x[i], (int)y[i], 1, 1)))
                {
                    return true;
                }
            }

            return false;
        }


        public static void TestDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            Rectangle r = World.Cam.ScreenToWorld(Mouse.GetState().X, Mouse.GetState().Y);
            sb.Draw(TM.tileTextures[TileType.sand], new Rectangle(r.X, r.Y, 5, 5) , Color.Red);
        }

    }
}
