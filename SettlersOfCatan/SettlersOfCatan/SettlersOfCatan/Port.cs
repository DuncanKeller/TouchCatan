using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    class Port : Tile
    {
        public Port(int x, int y)
            : base(x, y, TileType.port)
        {
            //figure out direction
           
        }

        private bool ValidTile(Tile t)
        {
            if (t == null)
            {
                return false;
            }
            if (t.Type == TileType.water)
            {
                return false;
            }
            return true;
        }

        public override void Init()
        {
            base.Init();
            VertexDirection dir = VertexDirection.TopLeft;

            if (ValidTile(tiles[EdgeDirection.Top]) &&
                ValidTile(tiles[EdgeDirection.TopRight]))
            {
                dir = VertexDirection.TopRight;
            }
            else if (ValidTile(tiles[EdgeDirection.TopRight]) &&
               ValidTile(tiles[EdgeDirection.BottomRight]))
            {
                dir = VertexDirection.Right;
            }
            else if (ValidTile(tiles[EdgeDirection.BottomRight]) &&
               ValidTile(tiles[EdgeDirection.Bottom]))
            {
                dir = VertexDirection.BottomRight;
            }
            else if (ValidTile(tiles[EdgeDirection.Bottom]) &&
               ValidTile(tiles[EdgeDirection.BottomLeft] ))
            {
                dir = VertexDirection.BottomLeft;
            }
            else if (ValidTile(tiles[EdgeDirection.BottomLeft]) &&
               ValidTile(tiles[EdgeDirection.TopLeft]))
            {
                dir = VertexDirection.Left;
            }

            backTex = TM.portTextures[dir];
            if (backTex != null)
            {
                width = backTex.Width - 2;
                height = backTex.Height - 60;
            }
        }
    }
}
