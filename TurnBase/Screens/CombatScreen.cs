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
        public string combatText = "";

        String wrappedText = "";
        String typedText = "";
        double typedTextLength;
        int delayInMilliseconds = 50;
        bool isDoneDrawing = false;

        int playerInitiation;
        int enemyInitiation;

        int turns = 1;

        private string title = "Combat screen\nPress ESC to exit";

        Panel combatTextPanel; 

        public CombatScreen(ContentManager content, int playerInit, int enemyInit)
        {
            playerInitiation = playerInit;
            enemyInitiation = enemyInit;
            playerCombat = content.Load<Texture2D>("Player\\wizardCombat");
            enemyCombat = content.Load<Texture2D>("Enemy\\skeletonCombat");

            combatTextPanel = new Panel(new Point(playerCombat.Width + 80, 150), new Vector2(620, 400))
            {
                Texture = content.Load<Texture2D>("panel")
            };

            titleText = content.Load<SpriteFont>("Fonts\\debugfont");
            combatTextFont = content.Load<SpriteFont>("Fonts\\debugfont");
        }

        public void Update(GameTime gameTime)
        {   
            if(playerInitiation >= enemyInitiation)
            {
                combatText = "Players initiation is higher.\nTurn " + turns + ": Player starts.";
            }
            else
            {
                combatText = "Enemies initiation is higher.\nTurn " + turns + ": Enemy starts.";
            }

            wrappedText = WrapText(combatText);

            if (!isDoneDrawing)
            {
                if (delayInMilliseconds == 0)
                {
                    typedText = wrappedText;
                    isDoneDrawing = true;
                }
                else if (typedTextLength < wrappedText.Length)
                {
                    typedTextLength = typedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / delayInMilliseconds;

                    if (typedTextLength >= wrappedText.Length)
                    {
                        typedTextLength = wrappedText.Length;
                        isDoneDrawing = true;
                    }

                    typedText = wrappedText.Substring(0, (int)typedTextLength);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            combatTextPanel.Draw(spriteBatch);
            spriteBatch.DrawString(combatTextFont, typedText, new Vector2(combatTextPanel.Position.X + 5, combatTextPanel.Position.Y + 5), Color.White);

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
