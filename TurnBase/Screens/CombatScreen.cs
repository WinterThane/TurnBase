using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBase.Screens
{
    public class CombatScreen
    {
        Texture2D playerCombat;
        Texture2D enemyCombat;
        SpriteFont titleText;
        private string title = "Combat screen!";

        public CombatScreen(ContentManager content)
        {
            playerCombat = content.Load<Texture2D>("Player\\wizardCombat");
            enemyCombat = content.Load<Texture2D>("Enemy\\skeletonCombat");

            titleText = content.Load<SpriteFont>("Fonts\\debugfont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(titleText, title, new Vector2(Config.SCREEN_WIDTH / 2 - 20, 20), Color.Azure);

            spriteBatch.Draw(playerCombat, new Vector2(0, 20), Color.White);
            spriteBatch.Draw(enemyCombat, new Vector2(Config.SCREEN_WIDTH - enemyCombat.Width, 20), Color.White);
        }
    }
}
