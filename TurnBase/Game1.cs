using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TurnBase.Enemies;

namespace TurnBase
{
    enum Turn
    {
        Player,
        Enemy
    }

    enum GameStates
    {
        TitleScreen,
        MainMenu,
        TravelScreen,
        CombatScreen,
        EndOfGame,
        Paused
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private string staticText = "Press Space to change actor";
        private string actorText = "Current actor selected: ";

        private Rectangle playerAreaLimit;

        bool isRunning = false;

        Texture2D fillTexture, sky;
        Ground[] groundTexture = new Ground[100];
        Player player;
        Texture2D playerHPbar;
        Panel playerStatsPanel;
        string playerDamage;
        Skeleton skeleton;
        Shot fireball;
        Panel debugPanel;

        Panel buttonFireball;

        KeyboardState previousState;

        SpriteFont debugText;
        SpriteFont playerStats;
        Vector2 debugActorPosition;

        Screens.TitleScreen titleScreen;
        Screens.CombatScreen combatScreen;

        private FrameCounter frames = new FrameCounter();
        private double elapsedTime = 0d;
        private int mouseX, mouseY;

        private bool isPaused = true;

        Turn turn = Turn.Player;
        GameStates CurrentGameState = GameStates.TitleScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = Config.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Config.SCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            playerAreaLimit = new Rectangle(0, 0, Config.SCREEN_WIDTH, Config.SCREEN_HEIGHT);
            previousState = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fireball = new Shot();
            fireball.Velocity = new Point(Config.PROJECTILE_SPEED, 0);
            fireball.Texture = Content.Load<Texture2D>("fireball");

            debugText = Content.Load<SpriteFont>("Fonts\\debugfont");
            playerStats = Content.Load<SpriteFont>("Fonts\\playerstats");

            fillTexture = Content.Load<Texture2D>("fill");
            sky = Content.Load<Texture2D>("cloudMap");
            playerHPbar = Content.Load<Texture2D>("healthbar");

            titleScreen = new Screens.TitleScreen(Content);
            
            LoadGround();
            LoadTextPanel();
            LoadPlayer();
            LoadPlayerStatsPanel();
            LoadButtonFireball();
            LoadEnemy();

            combatScreen = new Screens.CombatScreen(Content, player.Initination, skeleton.Initination);
        }

        private void LoadGround()
        {
            for (int i = 0; i < groundTexture.Length; i++)
            {
                groundTexture[i] = new Ground();
                groundTexture[i].Sprite = new Sprite(Content.Load<Texture2D>("grass"), 1, 1, 1);
                groundTexture[i].Position = new Vector2(64 * i, Config.SCREEN_HEIGHT - 32);
                groundTexture[i].Depth = 0.9f;
            }
        }

        private void LoadTextPanel()
        {
            debugPanel = new Panel(new Point(0, 0), new Vector2(Config.LEFT_PANEL, Config.SCREEN_HEIGHT), null)
            {
                Texture = Content.Load<Texture2D>("panel")
            };
        }

        private void LoadButtonFireball()
        {
            buttonFireball = new Panel(new Point(Config.LEFT_PANEL + 10, (int)playerStatsPanel.Size.Y + 20), new Vector2(50, 30), null)
            {
                Texture = Content.Load<Texture2D>("fireBtn")
            };
        }

        private void LoadPlayer()
        {
            player = new Player()
            {
                Sprite = new Sprite(Content.Load<Texture2D>("Player\\player"), 8, 1, 1),
                Position = new Vector2(Config.LEFT_PANEL + 200, Config.SCREEN_HEIGHT - 80),
                Depth = 0.5f,
                Name = "Winter"
            };
        }

        private void LoadPlayerStatsPanel()
        {
            playerStatsPanel = new Panel(new Point(Config.LEFT_PANEL + 10, 10), new Vector2(200, 125), null)
            {
                Texture = Content.Load<Texture2D>("panel")
            };
        }

        private void LoadEnemy()
        {
            skeleton = new Skeleton()
            {
                Sprite = new Sprite(Content.Load<Texture2D>("Enemy\\enemy"), 8, 1, 1),
                Position = new Vector2(Config.SCREEN_WIDTH - 200, Config.SCREEN_HEIGHT - 80),
                Depth = 0.5f,
                Name = "Skeleton"
            };
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            switch (CurrentGameState)
            {
                case GameStates.TitleScreen:
                    if (keyState.IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
                        Exit();

                    UpdateTitleScreen(gameTime);
                    break;

                case GameStates.TravelScreen:
                    UpdateTravelScreen(gameTime);
                    break;

                case GameStates.CombatScreen:
                    UpdateCombatScreen(gameTime);
                    break;
            }

            previousState = keyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case GameStates.TitleScreen:
                    DrawTitleScreen(gameTime);
                    break;

                case GameStates.TravelScreen:
                    DrawTravelScreen(gameTime);
                    break;

                case GameStates.CombatScreen:
                    DrawCombatScreen(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }


        //methods called in main Update function
        #region Update Methods

        private void UpdateTitleScreen(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            //var mouse = Mouse.GetState();

            var mouse = Mouse.GetState();
            var mousePosition = new Point(mouse.X, mouse.Y);
            Rectangle EnterGamebutton = new Rectangle(titleScreen.EnterGame.Position.X, titleScreen.EnterGame.Position.Y, (int)titleScreen.EnterGame.Size.X, (int)titleScreen.EnterGame.Size.Y);
            Rectangle ExitGamebutton = new Rectangle(titleScreen.ExitGame.Position.X, titleScreen.ExitGame.Position.Y, (int)titleScreen.ExitGame.Size.X, (int)titleScreen.ExitGame.Size.Y);

            if ((keyState.IsKeyDown(Keys.Enter) && !previousState.IsKeyDown(Keys.Enter)) || (EnterGamebutton.Contains(mousePosition) && (mouse.LeftButton == ButtonState.Pressed)))
            {
                CurrentGameState = GameStates.TravelScreen;
                isRunning = true;
                ResetProjectile();
            }

            if (ExitGamebutton.Contains(mousePosition) && (mouse.LeftButton == ButtonState.Pressed))
            {
                Exit();
            }
        }

        private void UpdateTravelScreen(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
                CurrentGameState = GameStates.TitleScreen;

            if (Actor.CheckCollision(player, skeleton))
            {
                player.Position.X = skeleton.Position.X - 50;
                CurrentGameState = GameStates.CombatScreen;
                return;
            }

            if (keyState.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
            {
                if (isPaused)
                {
                    isPaused = false;
                }
                else
                {
                    isPaused = true;
                }
            }

            if (isRunning)
            {
                elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                mouseX = mouse.X;
                mouseY = mouse.Y;

                if (keyState.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                {
                    SetTurn();
                }

                fireball.Update();
                FireFireBallButton();

                MovementControl(gameTime, keyState, turn);
            }

            previousState = keyState;
        }

        private void UpdateCombatScreen(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
            {
                combatScreen.combatText = "";
                CurrentGameState = GameStates.TravelScreen;
            }

            combatScreen.Update(gameTime);
        }

        private void SetTurn()
        {
            if (turn == Turn.Player)
            {
                turn = Turn.Enemy;
            }
            else
            {
                turn = Turn.Player;
            }
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
                player.Position += direction * Config.PLAYER_SPEED;
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
                    skeleton.Update(gameTime, keyState);
                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    direction.X--;
                    skeleton.Update(gameTime, keyState);
                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    direction.X++;
                    skeleton.Update(gameTime, keyState);
                }

                ImposeMovingLimits(skeleton);
                debugActorPosition = skeleton.Position;
                skeleton.Position += direction * Config.PLAYER_SPEED;
            }
        }

        private void ImposeMovingLimits(Actor actor)
        {
            var location = actor.Position;

            if (location.X < playerAreaLimit.X + 16 + Config.LEFT_PANEL)
                location.X = playerAreaLimit.X + 16 + Config.LEFT_PANEL;

            if (location.X > (playerAreaLimit.Right - 16))
                location.X = (playerAreaLimit.Right - 16);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y > (playerAreaLimit.Bottom - 80))
                location.Y = (playerAreaLimit.Bottom - 80);

            actor.Position = location;
        }

        private void ResetProjectile()
        {
            fireball.Position = new Point(fireball.Origin.X, 0);
        }

        public void FireFireBallButton()
        {
            var mouse = Mouse.GetState();
            var mousePosition = new Point(mouse.X, mouse.Y);
            Rectangle button = new Rectangle(buttonFireball.Position.X, buttonFireball.Position.Y, (int)buttonFireball.Size.X, (int)buttonFireball.Size.Y);

            if (button.Contains(mousePosition) && (turn == Turn.Player) && (mouse.LeftButton == ButtonState.Pressed) && (fireball.Position.X > Config.SCREEN_WIDTH))
            {
                int projectileX = (int)player.Position.X;
                int projectileY = (int)player.Position.Y;
                fireball.Position = new Point(projectileX, projectileY);
            }
        }

        #endregion


        //methods called in main Draw function
        #region Draw Methods

        private void DrawTitleScreen(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            titleScreen.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawTravelScreen(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frames.Update(deltaTime);
            var fps = string.Format("FPS: {0}", frames.AverageFPS);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            spriteBatch.Draw(sky, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.3f, SpriteEffects.None, 1f);

            DrawDebugStats(fps);
            DrawPlayerStats();

            spriteBatch.DrawString(debugText, player.Health.ToString() + "%", new Vector2(player.Position.X - 16, player.Position.Y - 45), Color.Black);

            foreach (var ground in groundTexture)
            {
                ground.Draw(spriteBatch);
            }

            debugPanel.Draw(spriteBatch);

            player.Draw(spriteBatch);
            DrawPlayerHealthBar();
            playerStatsPanel.Draw(spriteBatch);

            buttonFireball.Draw(spriteBatch);
            skeleton.Draw(spriteBatch);
            fireball.Draw(spriteBatch);
            DebugDraw();

            spriteBatch.End();
        }

        private void DrawCombatScreen(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            combatScreen.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawPlayerHealthBar()
        {
            //hp current bar
            spriteBatch.Draw(playerHPbar, new Rectangle((int)player.Position.X - 15, (int)player.Position.Y - 29, (int)(playerHPbar.Width * ((double)player.Health / 100)), 8), new Rectangle(0, 10, playerHPbar.Width, 10), Color.Red);
            //hp bar box
            spriteBatch.Draw(playerHPbar, new Rectangle((int)player.Position.X - 16, (int)player.Position.Y - 30, playerHPbar.Width, 10), new Rectangle(0, 0, playerHPbar.Width, 10), Color.White);
            // missing hp
            spriteBatch.Draw(playerHPbar, new Rectangle((int)player.Position.X - 16, (int)player.Position.Y - 30, playerHPbar.Width, 10), new Rectangle(0, 10, playerHPbar.Width, 10), Color.Gray);
        }

        private void DrawDebugStats(string fps)
        {
            spriteBatch.DrawString(debugText, staticText, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(debugText, actorText + turn.ToString(), new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(debugText, "Position: " + debugActorPosition.ToString(), new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(debugText, fps, new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(debugText, string.Format("Mouse position: X:{0} Y:{1}", mouseX, mouseY), new Vector2(10, 90), Color.White);
            spriteBatch.DrawString(debugText, "Elapsed time: " + elapsedTime.ToString("#.####"), new Vector2(10, 110), Color.White);
        }

        private void DrawPlayerStats()
        {
            spriteBatch.DrawString(playerStats, "Player: " + player.Name, new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 5), Color.White);
            spriteBatch.DrawString(playerStats, "Level: " + player.Level.ToString(), new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 20), Color.White);
            spriteBatch.DrawString(playerStats, "Experience: " + player.Experience.ToString(), new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 35), Color.White);
            spriteBatch.DrawString(playerStats, "Strength: " + player.Strength.ToString(), new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 50), Color.White);
            spriteBatch.DrawString(playerStats, "Dexterity: " + player.Dexterity.ToString(), new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 65), Color.White);
            spriteBatch.DrawString(playerStats, "Intelligence: " + player.Intelligence.ToString(), new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 80), Color.White);
            playerDamage = player.Damage(player.MinMeleeDamage, player.MaxMeleeDamage, player.Strength);
            spriteBatch.DrawString(playerStats, playerDamage, new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 95), Color.White);
            spriteBatch.DrawString(playerStats, "Magic damage: unknown", new Vector2(playerStatsPanel.Position.X + 10, playerStatsPanel.Position.Y + 110), Color.White);
        }

        private void DebugDraw()
        {
            Rectangle collisionRect = Actor.Intersect(player.Bounds, skeleton.Bounds);

            if (Actor.CheckCollision(player, skeleton))
                spriteBatch.Draw(fillTexture, collisionRect, new Color(255, 0, 0, 128));
            else
                spriteBatch.Draw(fillTexture, collisionRect, new Color(0, 255, 0, 128));

            //spriteBatch.Draw(player.Sprite.Texture, Vector2.Zero, player.NormalizeBounds(collisionRect), Color.White);
            //spriteBatch.Draw(enemy.Sprite.Texture, Vector2.Zero, enemy.NormalizeBounds(collisionRect), Color.White);
        }

        #endregion

    }
}
