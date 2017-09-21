using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TurnBase.Screens
{
    public class CombatScreen
    {
        Texture2D playerCombat;
        Texture2D enemyCombat;
        SpriteFont titleText;
        SpriteFont combatTextFont;
        private string combatText = "combat text here ** combat text here ** combat text here ** combat text here ** combat text here ** combat text here ** combat text here ** ";
        private string title = "Combat screen\nPress ESC to exit";

        Panel combatTextPanel; 

        public CombatScreen(ContentManager content)
        {
            playerCombat = content.Load<Texture2D>("Player\\wizardCombat");
            enemyCombat = content.Load<Texture2D>("Enemy\\skeletonCombat");

            combatTextPanel = new Panel(new Point(playerCombat.Width + 80, 150), new Vector2(620, 400))
            {
                Texture = content.Load<Texture2D>("panel")
            };

            titleText = content.Load<SpriteFont>("Fonts\\debugfont");
            combatTextFont = content.Load<SpriteFont>("Fonts\\debugfont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            combatTextPanel.Draw(spriteBatch);
            spriteBatch.DrawString(combatTextFont, WrapText(combatText), new Vector2(combatTextPanel.Position.X + 5, combatTextPanel.Position.Y + 5), Color.White);

            spriteBatch.DrawString(titleText, title, new Vector2(Config.SCREEN_WIDTH / 2 - 50, 20), Color.Azure);

            spriteBatch.Draw(playerCombat, new Vector2(10, 20), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(enemyCombat, new Vector2(Config.SCREEN_WIDTH - enemyCombat.Width - 10, 20), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        private String WrapText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (combatTextFont.MeasureString(line + word).Length() > combatTextPanel.Size.X)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }
    }
}
