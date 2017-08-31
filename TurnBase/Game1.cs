using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBase
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int SCREEN_WIDTH = 1024;
        private const int SCREEN_HEIGHT = 764;
        const float PLAYER_SPEED = 3.0f;
        const float ENEMY_SPEED = 3.0f;

        Texture2D fillTexture;
        Ground[] groundTexture = new Ground[100];
        Actor player;
        Actor enemy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fillTexture = Content.Load<Texture2D>("fill");

            LoadGround();
            LoadPlayer();
            LoadEnemy();
        }

        private void LoadGround()
        {
            for (int i = 0; i < groundTexture.Length; i++)
            {
                groundTexture[i] = new Ground();
                groundTexture[i].Sprite = new Sprite(Content.Load<Texture2D>("grass"), 1, 1, 1);
                groundTexture[i].Position = new Vector2(64 * i, SCREEN_HEIGHT - 32);
            }
        }

        private void LoadPlayer()
        {
            player = new Actor()
            {
                Sprite = new Sprite(Content.Load<Texture2D>("Player\\player"), 8, 1, 1),
                Position = new Vector2(200, SCREEN_HEIGHT - 80)
            };
        }

        private void LoadEnemy()
        {
            enemy = new Actor()
            {
                Sprite = new Sprite(Content.Load<Texture2D>("Enemy\\enemy"), 8, 1, 1),
                Position = new Vector2(SCREEN_WIDTH - 200, SCREEN_HEIGHT - 80)
            };
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyState = Keyboard.GetState();
            var direction = Vector2.Zero;

            if(keyState.IsKeyDown(Keys.W))
            {
                direction.Y--;
                player.Update(gameTime, keyState);
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                direction.Y++;
                player.Update(gameTime, keyState);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                direction.X--;
                player.Update(gameTime, keyState);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                direction.X++;
                player.Update(gameTime, keyState);
            }

            player.Position += direction * PLAYER_SPEED;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach (var ground in groundTexture)
            {
                ground.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            DebugDraw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DebugDraw()
        {
            Rectangle collisionRect = Actor.Intersect(player.Bounds, enemy.Bounds);

            if (Actor.CheckCollision(player, enemy))
                spriteBatch.Draw(fillTexture, collisionRect, new Color(255, 0, 0, 128));
            else
                spriteBatch.Draw(fillTexture, collisionRect, new Color(0, 255, 0, 128));

            //spriteBatch.Draw(player.Sprite.Texture, Vector2.Zero, player.NormalizeBounds(collisionRect), Color.White);
            //spriteBatch.Draw(enemy.Sprite.Texture, Vector2.Zero, enemy.NormalizeBounds(collisionRect), Color.White);
        }
    }
}
