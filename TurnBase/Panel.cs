using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase
{
    public class Panel
    {
        public Texture2D Texture;
        public Point Position;
        public Vector2 Size;
        public string Text = "";
        private Rectangle textBox;

        public Panel(Point position, Vector2 size)
        {
            Position = position;
            Size = size;
            textBox = new Rectangle(Position.X, Position.Y, (int)Size.X, (int)Size.Y);
        }

        public Panel(Point position, Vector2 size, string text)
        {
            Position = position;
            Size = size;
            Text = text;
            textBox = new Rectangle(Position.X, Position.Y, (int)Size.X, (int)Size.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, null, textBox, null, null, 0f, null, Color.White, SpriteEffects.None, 0.5f); 
        }
    }
}
