using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ____.World
{
    public class Map
    {
        private Tile[,] tiles;
        private int width;
        private int height;
        private int tileWidth;
        private int tileHeight;

        public int Width => width;
        public int Height => height;
        public int TileWidth => tileWidth;
        public int TileHeight => tileHeight;

        public Map(int width, int height, int tileWidth = 32, int tileHeight = 32)
        {
            this.width = width;
            this.height = height;
            this.tileWidth = width;
            this.tileHeight = tileHeight;

            tiles = new Tile[width, height];
            InitializeMap();
        }

        private void InitializeMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x,y] = new Tile(x, y, TileType.Grass);
                }
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return null;

            return tiles[x,y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                tiles[x, y] = tile;
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera, GraphicsDevice graphicsDevice)
        {
            // Get camera bounds in world space
            Rectangle cameraBounds = GetCameraBounds(camera, graphicsDevice);

            // Calculate which tiles are visible
            int startX = Math.Max(0, cameraBounds.Left / tileWidth);
            int endX = Math.Min(width, (cameraBounds.Right / tileWidth) + 1);
            int startY = Math.Max(0, cameraBounds.Top / tileHeight);
            int endY = Math.Min(height, (cameraBounds.Bottom / tileHeight) + 1);

            // Only draw visible tiles
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    tiles[x, y]?.Draw(spriteBatch, tileWidth, tileHeight);
                }
            }
        }

        private Rectangle GetCameraBounds(Camera2D camera, GraphicsDevice graphicsDevice)
        {
            // Invert the camera transformation to get world bounds from screen bounds
            Matrix inverseTransform = Matrix.Invert(camera.Get_transformation());
            
            // Get screen corners
            Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverseTransform);
            Vector2 topRight = Vector2.Transform(new Vector2(graphicsDevice.Viewport.Width, 0), inverseTransform);
            Vector2 bottomLeft = Vector2.Transform(new Vector2(0, graphicsDevice.Viewport.Height), inverseTransform);
            Vector2 bottomRight = Vector2.Transform(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), inverseTransform);

            // Find min/max bounds
            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }

        public Vector2 TileToWorld(int tileX, int tileY)
        {
            return new Vector2(tileX * tileWidth, tileY * tileHeight);
        }

        public Point WorldToTile(Vector2 worldPosition)
        {
            return new Point(
                (int)(worldPosition.X / tileWidth),
                (int)(worldPosition.Y / tileHeight)
            );
        }
    }
}