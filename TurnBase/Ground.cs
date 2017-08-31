using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase
{
    class Ground
    {
        public Vector2 Position;
        public Sprite Sprite;
        public float Depth;

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position, Depth);
        }
    }
}
