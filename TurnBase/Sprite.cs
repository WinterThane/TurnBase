using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBase
{
    class Sprite
    {
        public Texture2D Texture;

        private int tilesX;
        private int tilesY;
        private int tileWidth;
        private int tileHeight;
        private int frame;
        private int frameCount;

        private double frameInterval;
        private double frameTimeRemaining;

        public int Width { get { return tileWidth; } }
        public int Height { get { return tileHeight; } }

        public Rectangle FrameBounds
        {
            get
            {
                int x = frame % tilesX * tileWidth;
                int y = frame / tilesX * tileHeight;
                return new Rectangle(x, y, tileWidth, tileHeight);
            }
        }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(tileWidth / 2, tileHeight / 2);
            }
        }

        public Sprite(Texture2D texture, int tilesX, int tilesY, double frameRate)
            : this(texture, tilesX, tilesY, frameRate, tilesX * tilesY) { }

        public Sprite(Texture2D texture, int tilesX, int tilesY, double frameRate, int frameCount)
        {
            Texture = texture;
            this.tilesX = tilesX;
            this.tilesY = tilesY;
            tileWidth = texture.Width / tilesX;
            tileHeight = texture.Height / tilesY;
            this.frameCount = frameCount;
            frameInterval = 1 / frameRate;
            frameTimeRemaining = frameInterval;
        }

        public void Update(GameTime gameTime, KeyboardState key)
        {
            frameTimeRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
            int time = ((int)gameTime.TotalGameTime.Milliseconds / 200);
            var direction = key.GetPressedKeys();

            if(direction[0] == Keys.W)
            {
                if (time % 2 == 0)
                    frame = 0;
                else
                    frame = 1;
            }

            if (direction[0] == Keys.S)
            {
                if (time % 2 == 0)
                    frame = 2;
                else
                    frame = 3;
            }

            if (direction[0] == Keys.A)
            {
                if (time % 2 == 0)
                    frame = 4;
                else
                    frame = 5;
            }

            if (direction[0] == Keys.D)
            {
                if (time % 2 == 0)
                    frame = 6;
                else
                    frame = 7;
            }           
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float depth)
        {
            spriteBatch.Draw(Texture, position, FrameBounds, Color.White, 0f, Origin, 1f, SpriteEffects.None, depth);
        }
    }
}
