using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TowerDef
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font, endFont;

        float aspectRatio;
        public int cash = 100;

        public static Matrix viewMatrix = Matrix.Identity;
        public static Matrix projectionMatrix = Matrix.Identity;


        Map map;

        ObjectContainer objectContainer;

        EnemyManager enemyManager;

        Position selectedField;
        KeyboardState oldState, newState;

        GameCamera gameCamera;
        Gui gui;
        ObjectType selectedMenuItem;

        Vector3 pCameraPosition = new Vector3(0, 50, -200);
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 500;
            //this.graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            gameCamera = new GameCamera();
            selectedMenuItem = ObjectType.TOWER;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            objectContainer = new ObjectContainer(Content);
            map = new Map(GlobalGameConstants.mapWidth, GlobalGameConstants.mapHeight, Content, objectContainer);
            enemyManager = new EnemyManager(Content, objectContainer);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            map.loadContent();
            selectedField = map.selectedObject.positionOnMap;

            font = Content.Load<SpriteFont>("myFont");
            endFont = Content.Load<SpriteFont>("endFont");
            gui = new Gui(spriteBatch);
            gui.loadContent(this.Content);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameCamera.update(aspectRatio);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                if (map.isMovePossibe(selectedField.x, selectedField.y + 1))
                    selectedField.y++;
            }
            else if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                if (map.isMovePossibe(selectedField.x, selectedField.y - 1))
                    selectedField.y--;
            }
            else if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                if (map.isMovePossibe(selectedField.x - 1, selectedField.y))
                    selectedField.x--;
            }
            else if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                if (map.isMovePossibe(selectedField.x + 1, selectedField.y))
                    selectedField.x++;
            }
            else if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                if (cash >= 50)
                {
                    map.putFieldType(selectedField, selectedMenuItem);
                    objectContainer.addBullet(selectedField, selectedMenuItem);
                    cash -= 50;
                }
            }
            else if (newState.IsKeyDown(Keys.D1) && oldState.IsKeyUp(Keys.D1))
            {
                gui.select(1);
                selectedMenuItem = ObjectType.TOWERSINGLE;
                gui.update();
            }
            else if (newState.IsKeyDown(Keys.D2) && oldState.IsKeyUp(Keys.D2))
            {
                gui.select(2);
                selectedMenuItem = ObjectType.TOWERSINGLE_R;
                gui.update();
            }
            else if (newState.IsKeyDown(Keys.D3) && oldState.IsKeyUp(Keys.D3))
            {
                gui.select(3);
                selectedMenuItem = ObjectType.TOWERSINGLE_U;
                gui.update();
            }
            else if (newState.IsKeyDown(Keys.D4) && oldState.IsKeyUp(Keys.D4))
            {
                gui.select(4);
                selectedMenuItem = ObjectType.TOWERSINGLE_D;
                gui.update();
            }

            oldState = newState;

            map.update(selectedField);
            cash = objectContainer.update(cash);
            enemyManager.update();
            
            // Setup the Camera's View matrix
            Matrix sViewMatrix = Matrix.CreateLookAt(pCameraPosition, new Vector3(0, 50, 0), Vector3.Up);

            // Setup the Camera's Projection matrix by specifying the field of view (1/4 pi), aspect ratio, and the near and far clipping planes
            Matrix sProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height, 1, 10000);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            map.draw(aspectRatio);
            objectContainer.draw();

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            gui.draw();
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Cash: " + "\n" + cash, new Vector2(10, 410), Color.Tan);
            spriteBatch.End();

            if (cash < 0)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(endFont, "YOU LOST ", new Vector2(200, 180), Color.Red);
                spriteBatch.DrawString(font, "Press ESC to exit", new Vector2(400, 350), Color.Red);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

    }
}
