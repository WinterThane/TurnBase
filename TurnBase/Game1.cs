using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBase
{
    enum Turn
    {
        Player,
        Enemy
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int SCREEN_WIDTH = 1024;
        private const int SCREEN_HEIGHT = 400;
        private const float PLAYER_SPEED = 3.0f;
        private const float ENEMY_SPEED = 3.0f;
        private const int PROJECTILE_SPEED = 10;
        private const int LEFT_PANEL = 250;

        private string staticText = "Press Enter to start\nPress Space to change actor";
        private string actorText = "Current actor selected: ";

        private Rectangle playerAreaLimit;

        bool isRunning = false;

        Texture2D fillTexture, sky;
        Ground[] groundTexture = new Ground[100];
        Actor player, enemy;
        Shot fireball;
        Panel debugPanel;

        Panel buttonFireball;

        KeyboardState previousState;

        SpriteFont debugText;
        Vector2 debugActorPosition;

        private FrameCounter frames = new FrameCounter();
        private int mouseX, mouseY;

        Turn turn = Turn.Player;

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
            playerAreaLimit = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
            previousState = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fireball = new Shot();
            fireball.Velocity = new Point(PROJECTILE_SPEED, 0);
            fireball.Texture = Content.Load<Texture2D>("fireball");
            debugText = Content.Load<SpriteFont>("Fonts\\debugfont");

            fillTexture = Content.Load<Texture2D>("fill");
            sky = Content.Load<Texture2D>("cloudMap");

            LoadGround();
            LoadTextPanel();
            LoadPlayer();
            LoadButtonFireball();
            LoadEnemy();
        }

        private void LoadGround()
        {
            for (int i = 0; i < groundTexture.Length; i++)
            {
                groundTexture[i] = new Ground();
                groundTexture[i].Sprite = new Sprite(Content.Load<Texture2D>("grass"), 1, 1, 1);
                groundTexture[i].Position = new Vector2(64 * i, SCREEN_HEIGHT - 32);
                groundTexture[i].Depth = 0.9f;
            }
        }

        private void LoadTextPanel()
        {
            debugPanel = new Panel(new Point(0, 0), new Vector2(LEFT_PANEL, SCREEN_HEIGHT))
            {
                Texture = Content.Load<Texture2D>("panel")
            };
        }

        private void LoadButtonFireball()
        {
            buttonFireball = new Panel(new Point(LEFT_PANEL + 10, 10), new Vector2(50, 30))
            {
                Texture = Content.Load<Texture2D>("fireBtn")
            };
        }


        private void LoadPlayer()
        {
            player = new Actor()
            {
                Sprite = new Sprite(Content.Load<Texture2D>("Player\\player"), 8, 1, 1),
                Position = new Vector2(LEFT_PANEL + 200, SCREEN_HEIGHT - 80),
                Depth = 0.5f                
            };
        }

        private void LoadEnemy()
        {
            enemy = new Actor()
            {
                Sprite = new Sprite(Content.Load<Texture2D>("Enemy\\enemy"), 8, 1, 1),
                Position = new Vector2(SCREEN_WIDTH - 200, SCREEN_HEIGHT - 80),
                Depth = 0.5f
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
            var mouse = Mouse.GetState();
            mouseX = mouse.X;
            mouseY = mouse.Y;

            if (isRunning)
            {           
                if (keyState.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                {
                    SetTurn();
                }

                fireball.Update();
                EnterButton();

                MovementControl(gameTime, keyState, turn);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isRunning = true;
                ResetProjectile();
            }

            previousState = keyState;

            base.Update(gameTime);
        }

        private void MovementControl(GameTime gameTime, KeyboardState keyState, Turn setTurn)
        {
            var direction = Vector2.Zero;

            if (setTurn == Turn.Player)
            {
                //if (keyState.IsKeyDown(Keys.W))
                //{
                //    direction.Y--;
                //    player.Update(gameTime, keyState);
                //}
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

                ImposeMovingLimits(player);
                debugActorPosition = player.Position;
                player.Position += direction * PLAYER_SPEED;
            }
            else
            {
                //if (keyState.IsKeyDown(Keys.W))
                //{
                //    direction.Y--;
                //    enemy.Update(gameTime, keyState);
                //}
                if (keyState.IsKeyDown(Keys.S))
                {
                    direction.Y++;
                    enemy.Update(gameTime, keyState);
                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    direction.X--;
                    enemy.Update(gameTime, keyState);
                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    direction.X++;
                    enemy.Update(gameTime, keyState);
                }

                ImposeMovingLimits(enemy);
                debugActorPosition = enemy.Position;
                enemy.Position += direction * PLAYER_SPEED;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frames.Update(deltaTime);
            var fps = string.Format("FPS: {0}", frames.AverageFPS);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            
            spriteBatch.Draw(sky, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(debugText, staticText, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(debugText, actorText + turn.ToString(), new Vector2(10, 45), Color.White);
            spriteBatch.DrawString(debugText, "Position: " + debugActorPosition.ToString(), new Vector2(10, 65), Color.White);
            spriteBatch.DrawString(debugText, fps, new Vector2(10, 85), Color.White);
            spriteBatch.DrawString(debugText, string.Format("Mouse position: X:{0} Y:{1}", mouseX, mouseY), new Vector2(10, 105), Color.White);



            foreach (var ground in groundTexture)
            {
                ground.Draw(spriteBatch);
            }

            debugPanel.Draw(spriteBatch);
            player.Draw(spriteBatch);
            buttonFireball.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            fireball.Draw(spriteBatch);
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

        private void ImposeMovingLimits(Actor actor)
        {
            var location = actor.Position;

            if (location.X < playerAreaLimit.X + 16 + LEFT_PANEL)
                location.X = playerAreaLimit.X + 16 + LEFT_PANEL;

            if (location.X > (playerAreaLimit.Right - 16))
                location.X = (playerAreaLimit.Right - 16);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y > (playerAreaLimit.Bottom - 80))
                location.Y = (playerAreaLimit.Bottom - 80);

            actor.Position = location;
        }

        private void SetTurn()
        {
            if(turn == Turn.Player)
            {
                turn = Turn.Enemy;
            }
            else
            {
                turn = Turn.Player;
            }
        }

        private void ResetProjectile()
        {
            fireball.Position = new Point(fireball.Origin.X, 0);
        }

        public void EnterButton()
        {
            var mouse = Mouse.GetState();
            var mousePosition = new Point(mouse.X, mouse.Y); 
            Rectangle button = new Rectangle(buttonFireball.Position.X, buttonFireball.Position.Y, (int)buttonFireball.Size.X, (int)buttonFireball.Size.Y);

            if(button.Contains(mousePosition) && (turn == Turn.Player) && (mouse.LeftButton == ButtonState.Pressed) && (fireball.Position.X > SCREEN_WIDTH))
            {
                int projectileX = (int)player.Position.X;
                int projectileY = (int)player.Position.Y;
                fireball.Position = new Point(projectileX, projectileY);
            }
        }
    }
}
