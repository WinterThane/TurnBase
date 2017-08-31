using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase
{
    class Shot
    {
        private Point position;
        private Point velocity;
        private Texture2D texture;

        #region properties

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Point Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Point Origin
        {
            get
            {
                return new Point((texture.Width / 2), (texture.Height / 2));
            }
        }

        #endregion

        public void Update()
        {
            position = new Point((position.X + velocity.X), (position.Y + velocity.Y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 positionVector = new Vector2(position.X, position.Y);
            Vector2 originVector = new Vector2(Origin.X, Origin.Y);

            spriteBatch.Draw(texture, positionVector, null, Color.White, 0.0f, originVector, 2.0f, SpriteEffects.None, 0.5f);
        }
    }
}
