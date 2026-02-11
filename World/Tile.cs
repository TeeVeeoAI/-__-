using Microsoft.Xna.Framework;
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
        public Texture2D Texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Tint {get; set; } = Color.White;

        public Tile(int x, int y, TileType type)
        {
            X = x;
            Y = y;
            Type = type;
            IsWalkable = type != TileType.Wall && type != TileType.Water;
        }

        public void Draw(SpriteBatch spriteBatch, int tileWidth, int tileHeight)
        {
            if (Texture != null)
            {
                Vector2 position = new Vector2(X * tileWidth, Y * tileHeight);
                
                if (SourceRectangle.HasValue)
                {
                    spriteBatch.Draw(Texture, position, SourceRectangle.Value, Tint);
                }
                else
                {
                    spriteBatch.Draw(Texture, position, Tint);
                }
            }
        }
    }
}