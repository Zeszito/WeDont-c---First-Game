using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tecnicas.Magia;
using Tecnicas.Mapa;
using Tecnicas.NPC;

namespace Tecnicas
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //********************************************************************************************************************************
        private SoundEffect effect;
        //*******************************

        public static GeradorMapa GerMapas;
        public static GeraInimigos GerInimigos;
        public static Random RNG;
        public static GraphicsDeviceManager graphics;
        public static ContentManager sContent;
        public static SpriteBatch spriteBatch;
        Texture2D t, Light;
        Vector2 p;

        float timePassed;

        //Camera
        Texture2D teste;

        //Jogador 
        public static Player Jogador;

        Camera cam;

        Vector2 followPlayer;

        Texture2D Sangue;
        public static float espera = 0;

        //Coisas para MEnu//

        public static bool TelaMenu = true;
        private bool up = false;
        private bool down = false;

        private bool up1 = false;
        private bool down1 = false;

        private bool upf = false;
        private bool downf = false;

        private bool fullScreen = false;
        private bool som = false;
        private int pass = 0;

        private Texture2D fundo;

        public static List<Drop> drops;
        public static int Ninidead = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sContent = Content;
            p = new Vector2(0,0);
            //Fps do jogo
            this.IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 16); // 33ms = 30fps





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

            //TesteFogo = new Projectile();

            GerMapas = new GeradorMapa();
            GerInimigos = new GeraInimigos();
            RNG = new Random(DateTime.Now.Millisecond);
            timePassed = 0;
            t = new Texture2D(this.GraphicsDevice, 100, 100);

            Color[] cdata = new Color[100 * 100]; //Isto serve para algo?
            for(int i = 0; i < 10000; i++)
            {
                cdata[i] = Color.Red;
            }
            t.SetData<Color>(cdata);
            // TODO: Add your initialization logic here
            drops = new List<Drop>();

            //MAPA
            int conexoesNoMapa = GerMapas.Inicializa();
            while (conexoesNoMapa <= 3)
            {
                conexoesNoMapa = GerMapas.Inicializa();
            }

            //O iniVal é a quantidade de inimigos que vão ser spawned
            int iniVal = 25;
            GerInimigos.CriaMonstros(iniVal);

            Sangue = sContent.Load<Texture2D>("Poca");
            fundo = sContent.Load<Texture2D>("Menu");
            base.Initialize();
            //PlayerCutScene.introIn();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //********************************************************************************************************************************
            effect = Content.Load<SoundEffect>("Underwater");
            effect.Play();
            //******************************************************
            UI.LoadContent(); //UI todo lindo

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cam = new Camera(GraphicsDevice.Viewport);

            foreach(Drop d in drops)
            {
                d.LoadContent();
            }

            foreach (Inimigo Inim in GerInimigos.listaInimigos)
            {
                Inim.LoadContent(Inim.Name);
             
                if (Inim.Mruna != null)
                {

                    //Inim.Mruna.Projectile._ProjectileTexture = Game1.sContent.Load<Texture2D>("Speels/evilProjectile");
                }
            }

            GerMapas.LoadContent();
            Jogador = new Player(GerMapas.PoePlayer(Jogador));
            //Jogador.animacoes.SetSpriteAnimation(4, 0, 4, 1, 10);


            teste = sContent.Load<Texture2D>("P.PRINCIPAL-SpriteSheet");
   
            // TODO: use this.Content to load your game content here
            Texture2D[] tilesets = new Texture2D[2];

            for (int i = 0; i < 2; i++)
                tilesets[i] = Content.Load<Texture2D>("temp/tileset_" + i.ToString());

            Light = Content.Load<Texture2D>("temp/light");


            //TesteFogo.LoadContent("Speels/fire");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            espera += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TelaMenu == false)
            {
                foreach (Inimigo I in GerInimigos.listaInimigos)
                {
                    if (I.Isdead == false)
                    {
                        I.Update(gameTime);
                   
                          

                    }
                    else
                    {
                        I.UpdateDead(gameTime);
               
                    }
                }
                //PlayerCutScene.introUpdate(gameTime); //Ista faz a intro acontecer

            }
            foreach (Drop d in drops)
            {
                d.Update();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (IsActive)
            {
                //timePassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if (timePassed > 1.01f)
                //{
                //    timePassed -= 1.01f;
                //    mapa.Clear();
                //    mapa.GeraMapa(100, 0, Vector2.Zero, Vector2.Zero);
                //}

                KeyboardState k = Keyboard.GetState();
                MouseState m = Mouse.GetState();
                //Cria um vetor das posições do rato
                Vector2 m2 = new Vector2(m.X, m.Y);
                //Converte para serem coordenadas na tela
                Matrix inverse = Matrix.Invert(cam.get_transformation(GraphicsDevice));
                Vector2 mousePos = Vector2.Transform(new Vector2(m.X, m.Y), inverse);
                //E manda para o player na forma de um Vector
                Jogador.Update(mousePos, gameTime);
                if (TelaMenu == true)
                {
                    #region teclas mexe menu
                    if (pass < 0)
                    {
                        pass = 0;
                    }
                    if (pass > 4)
                    {
                        pass = 4;
                    }

                    if (k.IsKeyDown(Keys.F))
                    {
                        downf = true;
                        upf = false;

                    }

                    if (k.IsKeyUp(Keys.F) && downf == true && upf == false)
                    {
                        upf = true;

                    }

                    if (upf == true && downf == true)
                    {
                        if (fullScreen == false)
                        {
                            graphics.IsFullScreen = true;
                            fullScreen = true;
                            graphics.ApplyChanges();
                        }
                        else if (fullScreen == true)
                        {
                            graphics.IsFullScreen = false;
                            fullScreen = false;
                            graphics.ApplyChanges();
                        }
                        downf = false;
                        upf = false;


                    }
            
                    /////////////////////////////////////////77
                    if (k.IsKeyDown(Keys.Left))
                    {
                        down = true;
                        up = false;

                    }
                    if (k.IsKeyUp(Keys.Left))
                    {
                        up = true;
                    }

                    if (up == true && down == true)
                    {
                        up = false;
                        down = false;
                        pass--;

                    }
                    ///////////////////////////////
                    if (k.IsKeyDown(Keys.Right))
                    {
                        down1 = true;
                        up1 = false;


                    }
                    else if (k.IsKeyUp(Keys.Right))
                    {
                        up1 = true;
                    }

                    if (up1 == true && down1 == true)
                    {
                        up1 = false;
                        down1 = false;
                        pass++;

                    }


                }
                #endregion
                //if (k.IsKeyDown(Keys.Right))
                //    cam.Rotation += 0.1f;
                if (k.IsKeyDown(Keys.Up))
                    cam.Zoom *= 1f + 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (k.IsKeyDown(Keys.Down))
                    cam.Zoom /= 1f + 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if (k.IsKeyDown(Keys.F))
                //    cam.Rotation = cam.RotaOri;
                //Update(gameTime);
                //m.Update();
                cam.Move(Jogador.Position);
            }
            
            //TesteFogo.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (Jogador.luz == false && Jogador.vida>0)
            {
                GraphicsDevice.Clear(Color.Gray);
            }

            #region aviso dano

            if (Jogador.luz == true && Jogador.vida > 0)
            {


                var red = Color.Black;
                var black = Color.Red;


                // Gives you a color at half the distance between red and green
                var color3 = Color.Lerp(red, black, 0.5f);
                GraphicsDevice.Clear(color3);

                if (espera > 0.5) { espera = 0; Jogador.luz = false; }

            }
            #endregion

            // TODO: Add your drawing code here
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.Transform);
            //spriteBatch.Draw(t,p);
            #region desenha tela
            if (TelaMenu == true && Jogador.vida>0)
            {
                KeyboardState k = Keyboard.GetState();
                
                spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, null);
                spriteBatch.Draw(fundo, new Rectangle(0,0,800,480), Color.White);
                FontSupport.PrintStatus(null);

                if (pass == 0)
                {
                    FontSupport.Mensagem("Novo Jogo");
                    if (k.IsKeyDown(Keys.Enter))
                    {

                        TelaMenu = false;
                        if (TelaMenu == false)
                        {
                            FontSupport.sStatusLocation = new Vector2(200, 200);
                            FontSupport.Mensagem(" ");

                        }
                    }
                }
                else if (pass == 1)
                {
                    FontSupport.sStatusLocation = new Vector2(100, 200);
                    FontSupport.Mensagem("Opcoes: \n\nF-FullScreen\nOn/off");





                }
                else if (pass == 2)
                {
                    FontSupport.sStatusLocation = new Vector2(200, 200);
                    FontSupport.Mensagem("Creditos");
                    if (k.IsKeyDown(Keys.Enter))
                    {
                        FontSupport.sStatusLocation = Vector2.One * 5;
                        FontSupport.Mensagem("\nJogo by: \n\nLucas Santos \n\nMoises Moreira \n\nVasco Figueiredo \n\n2017\nEdj-IPCA");

                    }
                }
                else if (pass == 3)
                {
                    FontSupport.sStatusLocation = new Vector2(200, 200);
                    FontSupport.Mensagem("Sair");

                    if (k.IsKeyDown(Keys.Enter))
                    {
                        Exit();

                    }
                    #endregion
                }
                spriteBatch.End();
            }




            if(TelaMenu == false && Jogador.vida>0) { 
            //spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.get_transformation(GraphicsDevice));
            GerMapas.Draw();
            cam.get_transformation(GraphicsDevice);
            Jogador.Draw();
            
            //spriteBatch.Draw(teste, Vector2.Zero, Color.White);
            /*
            spriteBatch.Draw(
                texture: shitLight,
                position: Vector2.One * 2,
                //color: new Color(
                //    ((float)gameTime.TotalGameTime.TotalSeconds * 1.0f) % 1f,
                //    ((float)gameTime.TotalGameTime.TotalSeconds * 2.0f) % 1f,
                //    ((float)gameTime.TotalGameTime.TotalSeconds * 3.0f) % 1f,
                //    1f),
                color: Color.Black,
                scale: new Vector2(10f),
                origin: new Vector2(32));
                */
            //TesteFogo.Draw(spriteBatch);


            foreach (Inimigo Ini in GerInimigos.listaInimigos)
            {
                if (Ini.Isdead == false)
                {
                    Ini.Draw(spriteBatch);
                }
                else
                {
                    spriteBatch.Draw(Sangue, Ini.Position, Color.White);

                }
            }

                foreach (Drop d in drops)
                {
                    d.Draw();
                }



                followPlayer = cam.centre;
       
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, null);
            FontSupport.PrintStatus(null);
            FontSupport.PrintCoins(null);
            UI.UiDraw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
           else if (Jogador.vida <= 0)
            {
                KeyboardState k = Keyboard.GetState();
          
                spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, null);

                if (som == false)
                {
                    GraphicsDevice.Clear(Color.Gray);
               
                  
                    som = true;
                }
                FontSupport.PrintStatus(null);
                FontSupport.sStatusLocation = new Vector2(200, 200);
                FontSupport.Mensagem("Ha!ha!ha!\nOjogo\nTerminou!");
                

            





                if (k.IsKeyDown(Keys.Enter))
                {
                    Exit();

                }
                spriteBatch.End();
            }
        }
    }
}
