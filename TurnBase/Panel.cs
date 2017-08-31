using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase
{
    class Panel
    {
        public Texture2D Texture;
        public Vector2 Position;
        private Vector2 Size;

        public Panel(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    spriteBatch.Draw(Texture, new Vector2(Position.X + i,Position.Y + j), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
                }
            }   
        }
    }
}
