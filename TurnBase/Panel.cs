using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase
{
    class Panel
    {
        public Texture2D Texture;
        public Point Position;
        public Vector2 Size;
        private Rectangle textBox;

        public Panel(Point position, Vector2 size)
        {
            Position = position;
            Size = size;
            textBox = new Rectangle(Position.X, Position.Y, (int)Size.X, (int)Size.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, null, textBox, null, null, 0f, null, Color.White, SpriteEffects.None, 0.5f); 
        }
    }
}
