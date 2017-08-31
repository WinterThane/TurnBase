using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TurnBase
{
    class Actor
    {
        const int ALPHA_THRESHOLD = 48;

        public Vector2 Position;
        public Sprite Sprite;

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X - (int)Sprite.Origin.X, (int)Position.Y - (int)Sprite.Origin.Y, Sprite.Width, Sprite.Height);
            }
        }

        public void Update(GameTime gameTime, KeyboardState key)
        {
            Sprite.Update(gameTime, key);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        public Rectangle NormalizeBounds(Rectangle rect)
        {
            return new Rectangle(rect.X - (int)Position.X + Sprite.FrameBounds.X + (int)Sprite.Origin.X,
                                 rect.Y - (int)Position.Y + Sprite.FrameBounds.Y + (int)Sprite.Origin.Y,
                                 rect.Width,
                                 rect.Height);
        }

        public static Rectangle Intersect(Rectangle boundsA, Rectangle boundsB)
        {
            int x1 = Math.Max(boundsA.Left, boundsB.Left);
            int y1 = Math.Max(boundsA.Top, boundsB.Top);
            int x2 = Math.Min(boundsA.Right, boundsB.Right);
            int y2 = Math.Min(boundsA.Bottom, boundsB.Bottom);

            int width = x2 - x1;
            int height = y2 - y1;

            if (width > 0 && height > 0)
                return new Rectangle(x1, y1, width, height);
            else
                return Rectangle.Empty;
        }

        public static bool CheckCollision(Actor actorA, Actor actorB)
        {
            Rectangle collisionRect = Intersect(actorA.Bounds, actorB.Bounds);
            if (collisionRect == Rectangle.Empty)
                return false;

            int pixelCount = collisionRect.Width * collisionRect.Height;

            Color[] pixelsA = new Color[pixelCount];
            Color[] pixelsB = new Color[pixelCount];

            actorA.Sprite.Texture.GetData<Color>(0, actorA.NormalizeBounds(collisionRect), pixelsA, 0, pixelCount);
            actorB.Sprite.Texture.GetData<Color>(0, actorB.NormalizeBounds(collisionRect), pixelsB, 0, pixelCount);

            for (int i = 0; i < pixelCount; i++)
            {
                if (pixelsA[i].A > ALPHA_THRESHOLD && pixelsB[i].A > ALPHA_THRESHOLD)
                    return true;
            }
            return false;
        }
    }
}
