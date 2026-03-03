using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ____.World
{
    public enum TileType
    {
        Grass,
        Water,
        Stone,
        Wall
    }

    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TileType Type { get; set; }
        public bool IsWalkable { get; set; }
        public Texture2D texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Tint {get; set; } = Color.White;

        public Tile(int x, int y, TileType type)
        {
            X = x;
            Y = y;
            Type = type;
            IsWalkable = type != TileType.Wall && type != TileType.Water;
        }

        public void LoadContent(ContentManager contentManager)
        {
            GraphicsDevice gd = Game1.Instance.GraphicsDevice;
            switch (Type)
            {
                case TileType.Grass:
                    texture = contentManager.Load<Texture2D>("grass");
                    break;
                case TileType.Water:
                    texture = contentManager.Load<Texture2D>("water");
                    break;
                case TileType.Stone:
                    texture = contentManager.Load<Texture2D>("stone");
                    break;
                case TileType.Wall:
                    texture = contentManager.Load<Texture2D>("wall");
                    break;
                default:
                    texture = new Texture2D(gd, 1, 1);
                    texture.SetData(new[] { Color.White });
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int tileWidth, int tileHeight, Vector2 mapPosition)
        {
            if (texture != null)
            {
                Vector2 position = mapPosition + new Vector2(X * tileWidth, Y * tileHeight);


                if (SourceRectangle.HasValue)
                {
                    spriteBatch.Draw(texture, position, SourceRectangle.Value, Tint);
                }
                else
                {
                    spriteBatch.Draw(texture, position, Tint);
                }
            }
        }
    }
}
