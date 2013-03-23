using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SettlersOfCatan
{

    enum PieceType
    {
        settlement,
        city,
        robber
    }

    static class TM
    {
        public static Dictionary<TileType, Texture2D> tileTextures = new Dictionary<TileType, Texture2D>();
        public static Dictionary<TileType, Texture2D> backTextures = new Dictionary<TileType, Texture2D>();
        public static Dictionary<TileType, Texture2D> frontTextures = new Dictionary<TileType, Texture2D>();

        public static Dictionary<VertexDirection, Texture2D> portTextures = new Dictionary<VertexDirection, Texture2D>();
        public static Dictionary<EdgeDirection, Texture2D> roadTextures = new Dictionary<EdgeDirection, Texture2D>();

        public static Dictionary<PieceType, Texture2D> pieces = new Dictionary<PieceType, Texture2D>();

        public static Texture2D blank;

        public static void Init(ContentManager c)
        {
            Config.rand = new Random();

            blank = c.Load<Texture2D>("blank");

            pieces.Add(PieceType.settlement, c.Load<Texture2D>("pieces\\settlement"));

            tileTextures.Add(TileType.plain, c.Load<Texture2D>("tiles\\pasture"));
            tileTextures.Add(TileType.brick, c.Load<Texture2D>("tiles\\quarry"));
            tileTextures.Add(TileType.forest, c.Load<Texture2D>("tiles\\forest"));
            tileTextures.Add(TileType.ore, c.Load<Texture2D>("tiles\\mountain"));
            tileTextures.Add(TileType.sand, c.Load<Texture2D>("tiles\\desert"));
            tileTextures.Add(TileType.wheat, c.Load<Texture2D>("tiles\\wheat"));
            tileTextures.Add(TileType.water, null);

            backTextures.Add(TileType.plain, c.Load<Texture2D>("tiles\\pasture"));
            backTextures.Add(TileType.brick, c.Load<Texture2D>("tiles\\quarry-back"));
            backTextures.Add(TileType.forest, c.Load<Texture2D>("tiles\\forest-back"));
            backTextures.Add(TileType.ore, c.Load<Texture2D>("tiles\\mountain-back"));
            backTextures.Add(TileType.sand, c.Load<Texture2D>("tiles\\desert"));
            backTextures.Add(TileType.wheat, c.Load<Texture2D>("tiles\\wheat"));

            frontTextures.Add(TileType.plain, null);
            frontTextures.Add(TileType.brick, c.Load<Texture2D>("tiles\\quarry-front"));
            frontTextures.Add(TileType.forest, c.Load<Texture2D>("tiles\\forest-front"));
            frontTextures.Add(TileType.ore, c.Load<Texture2D>("tiles\\mountain-front"));
            frontTextures.Add(TileType.sand, null);
            frontTextures.Add(TileType.wheat, null);

            portTextures.Add(VertexDirection.TopRight, c.Load<Texture2D>("tiles\\port-1"));
            portTextures.Add(VertexDirection.Right, c.Load<Texture2D>("tiles\\port-2"));
            portTextures.Add(VertexDirection.BottomRight, c.Load<Texture2D>("tiles\\port-3"));
            portTextures.Add(VertexDirection.BottomLeft, c.Load<Texture2D>("tiles\\port-4"));
            portTextures.Add(VertexDirection.Left, c.Load<Texture2D>("tiles\\port-5"));
            portTextures.Add(VertexDirection.TopLeft, c.Load<Texture2D>("tiles\\port-6"));

            roadTextures.Add(EdgeDirection.Top, c.Load<Texture2D>("pieces\\road-bottom"));
            roadTextures.Add(EdgeDirection.Bottom, c.Load<Texture2D>("pieces\\road-bottom"));
            roadTextures.Add(EdgeDirection.TopLeft, c.Load<Texture2D>("pieces\\road-forward"));
            roadTextures.Add(EdgeDirection.BottomRight, c.Load<Texture2D>("pieces\\road-forward"));
            roadTextures.Add(EdgeDirection.BottomLeft, c.Load<Texture2D>("pieces\\road-backward"));
            roadTextures.Add(EdgeDirection.TopRight, c.Load<Texture2D>("pieces\\road-backward"));
        }

    }
}
