using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ____.Camera;
using ____.Systems.LoadData.LoadWorld;
using ____.Systems.SaveData.SaveWorld;
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
        private Vector2 position;

        public int Width => width;
        public int Height => height;
        public int TileWidth => tileWidth;
        public int TileHeight => tileHeight;
        public Vector2 Position => position;
        public static Map CurrentMap { get; private set; }
        public Rectangle Rec;

        public Map(int width, int height, int tileWidth = 640, int tileHeight = 640)
        {
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.position = new(
                -(width * tileWidth) / 2f,
                -(height * tileHeight) / 2f
            );
            Rec = new(position.ToPoint(), new(width * tileWidth, height * tileHeight));

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
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x,y].LoadContent(contentManager);
                }
            }
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
            CurrentMap = this;
        }


        public void Draw(SpriteBatch spriteBatch, Camera2D camera, GraphicsDevice graphicsDevice)
        {
            // Get camera bounds in world space
            Rectangle cameraBounds = GetCameraBounds(camera, graphicsDevice);

            // Calculate which tiles are visible
            int startX = Math.Max(0, (int)Math.Floor((cameraBounds.Left - position.X) / tileWidth));
            int endX = Math.Min(width, (int)Math.Floor((cameraBounds.Right - position.X) / tileWidth) + 1);
            int startY = Math.Max(0, (int)Math.Floor((cameraBounds.Top - position.Y) / tileHeight));
            int endY = Math.Min(height, (int)Math.Floor((cameraBounds.Bottom - position.Y) / tileHeight) + 1);

            // Only draw visible tiles
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    tiles[x, y]?.Draw(spriteBatch, tileWidth, tileHeight, position);
                }
            }

        }

        private Rectangle GetCameraBounds(Camera2D camera, GraphicsDevice graphicsDevice)
        {
            // Invert the camera transformation to get world bounds from screen bounds
            Matrix inverseTransform = Matrix.Invert(camera.Get_transformation());
            
            // Get screen corners
            Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverseTransform);
            Vector2 topRight = Vector2.Transform(new(graphicsDevice.Viewport.Width, 0), inverseTransform);
            Vector2 bottomLeft = Vector2.Transform(new(0, graphicsDevice.Viewport.Height), inverseTransform);
            Vector2 bottomRight = Vector2.Transform(new(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), inverseTransform);

            // Find min/max bounds
            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            return new((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }

        public Vector2 TileToWorld(int tileX, int tileY)
        {
            return position + new Vector2(tileX * tileWidth, tileY * tileHeight);
        }

        public Point WorldToTile(Vector2 worldPosition)
        {
            return new(
                (int)Math.Floor((worldPosition.X - position.X) / tileWidth),
                (int)Math.Floor((worldPosition.Y - position.Y) / tileHeight)
            );
        }

        public bool IsWalkable(Rectangle rect)
        {
            // Convert the rectangle to tile coordinates
            Point topLeft = WorldToTile(new(rect.Left, rect.Top));
            Point bottomRight = WorldToTile(new(rect.Right - 1, rect.Bottom - 1)); // Subtract 1 to avoid off-by-one errors

            // Check all tiles that the rectangle covers
            for (int x = topLeft.X; x <= bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    Tile tile = GetTile(x, y);
                    if (tile == null || !tile.IsWalkable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Map Load(string filePath = null)
        {
            return LoadMap.Load(filePath);
        }

        public static void Save(Map map, string filePath = null)
        {
            SaveMap.Save(map, filePath);
        }

        public void Save(string filePath = null)
        {
            SaveMap.Save(this, filePath);
        }
    }
}
