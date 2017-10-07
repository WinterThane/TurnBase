using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase.Screens
{
    public class TitleScreen
    {
        Texture2D titleScreen;
        SpriteFont titleText;

        private bool doFadeIn = true;
        private bool doFadeOut = false;

        private string instuctions =
            "Press Enter or click Enter Game to begin\n" +
            "Walking A S D (jump not implemented)\n" +
            "Click Fire to shoot a fireball\n" +
            "Press Escape to exit back to title screen";

        Panel enterGame = new Panel(new Point(Config.SCREEN_WIDTH / 2, Config.SCREEN_HEIGHT / 2), new Vector2(113, 28), "Enter Game");
        Panel exitGame = new Panel(new Point(Config.SCREEN_WIDTH / 2, Config.SCREEN_HEIGHT - 50), new Vector2(113, 28), "Exit to DOS");

        public Panel EnterGame
        {
            get { return enterGame; }
        }

        public Panel ExitGame
        {
            get { return exitGame; }
        }

        public TitleScreen(ContentManager content)
        {
            titleScreen = content.Load<Texture2D>("Screens\\titleScreen");
            titleText = content.Load<SpriteFont>("Fonts\\debugfont");
            enterGame.Texture = content.Load<Texture2D>("panel");
            exitGame.Texture = content.Load<Texture2D>("panel");
        }

        public void Update(GameTime gameTime)
        {
            //if (doFadeIn == true && doFadeOut == false)
            //{
            //    FadeIn();
            //}

            //if (doFadeIn == false && doFadeOut == true)
            //{
            //    FadeOut();
            //}
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            enterGame.Draw(spriteBatch);
            exitGame.Draw(spriteBatch);
            spriteBatch.DrawString(titleText, instuctions + " \nAlpha: " + Config.ALPHA_FADE.ToString("#.###") + " \nfade_in: " + doFadeIn.ToString() + " \nfade_out: " + doFadeOut.ToString(), new Vector2(10, 10), Color.White * Config.ALPHA_FADE);
            spriteBatch.DrawString(titleText, enterGame.Text, new Vector2(enterGame.Position.X + 10, enterGame.Position.Y + 5), Color.Red * Config.ALPHA_FADE);
            spriteBatch.DrawString(titleText, exitGame.Text, new Vector2(exitGame.Position.X + 10, exitGame.Position.Y + 5), Color.Red * Config.ALPHA_FADE);
            spriteBatch.Draw(titleScreen, Vector2.Zero, null, Color.White * Config.ALPHA_FADE, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }

        private void FadeIn()
        {
            if (Config.ALPHA_FADE <= 1f)
            {
                Config.ALPHA_FADE += Config.ALPHA_FADE_SPEED;
                if(Config.ALPHA_FADE >= 1f)
                {
                    doFadeIn = false;
                    doFadeOut = true;
                }
            }
        }

        private void FadeOut()
        {
            if (Config.ALPHA_FADE >= 0f)
            {
                Config.ALPHA_FADE -= Config.ALPHA_FADE_SPEED;
                if(Config.ALPHA_FADE <= 0f)
                {
                    doFadeOut = false;
                    doFadeIn = true;
                }
            }
        }
    }
}
