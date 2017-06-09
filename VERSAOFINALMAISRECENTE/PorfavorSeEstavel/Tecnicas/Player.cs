using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Tecnicas.Magia;
using Tecnicas.NPC;
using Tecnicas.Mapa;
using Microsoft.Xna.Framework.Audio;
using Tecnicas.Animations;

namespace Tecnicas
{
    public class Player
    {
        //=======================================================================================
        #region Variaveis basicas -> tipo textura, vetores da posicao e tamanho e retangulos

        //Scale da sprite. Kinda inutil mas pode vir a ser util se quisermos por cheats ou whatever
        Vector2 tamanho;
        
        //Posicao. Visto que a faca usa-a tem de ter getters e setters. 
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Rotação
        private float rotation;

        //Retangulo da sprite e do Colider. O colider tem getters e setters porque, guess what, outra classes precisam dele.
        private Rectangle RecOri;
        private Rectangle coliderPlayer;

        public Rectangle ColiderPlayer
        {
            get { return coliderPlayer; }
            set { coliderPlayer = value; }
        }

        public bool luz = false;
        #endregion
        //=======================================================================================
        #region Variaveis especificas
        //Textura para desenhar o colider -> apenas para testes
        Texture2D whiteRectangle;
        Vector2 weee;
      
        //Facas e os estados Atacando e MecInCoolDown que é bastante importante. O resto é timers
        public Facas f1 = new Facas();
        Vector2 Final;
        private bool Atacando = false;
        private const float timer = 1;
        private float countdown = timer;
        //Walking
        private bool Walking = false;
        //Dash
        private bool MecInCoolDown = false;
        private static int contadorTempo = 50;
        private int contador = contadorTempo;
        //Shield
        Rectangle recShield;
        private bool OnShield = false;

        //Parry
        Rectangle recParry;
        private bool OnParry = false;

        //Runas
        private List<Runa> PlayerRunes;
        //Construtor recebe nome (para a sprite) e a posicao dele

        public Runa PlayerRuna;

        //Coins
        public int coins;

        //esta na cut scene
        public bool inCutscene = true;

        //Player
        public int vida;
        //playvsInimigos
        private float timeDestiro, timeDestiro2;
        int number = Game1.RNG.Next(1, 4);
        //PAssar o lado
        public int lado = 0;
        //SPRITE PRIMITIVE
        private SpritePrimitive animacoes;
        private SpritePrimitive espada;
        private SpritePrimitive sombra; // na verdade so 1 imagem;
        private SpritePrimitive stepEffects;
        private Rectangle recSombra;        //Rectangulo para posicao da sombra
        private Rectangle recSteps;
        private bool Idle = true;
        //SOUND
        //private SoundEffect _SwordSwing;
        private SoundEffect _FootSteps;
        private SoundEffect _SwordSwing;
        private SoundEffect _Dash;
        private bool Is_playing;
        private bool blocker;
        float volume = 0.4f;
        float pitch = 0.3f;
        float pan = 0.0f;
        #endregion

        #region Spells
        int contadorSpell = 50;
        #endregion

        //Variavel dash
        List<AfterImages> DashAfterImages;
        //=======================================================================================
        public Player(/*string nome,*/ Vector2 posicao)
        {
            PlayerRunes = new List<Runa>();
            position = posicao;
            tamanho = new Vector2(5,5);
            //texIni = Game1.sContent.Load<Texture2D>(nome);

            f1.LoadImage();
            f1.tipo = Tipo.Lucas;
            //Textura do colider -> teste apenas
            whiteRectangle = new Texture2D(Game1.spriteBatch.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            //PlayerSpell = null; //começa sem spell.
            DashAfterImages = new List<AfterImages>();
            foreach(AfterImages after in DashAfterImages)
            {
                after.LoadImage();
            }

            timeDestiro = 10;
            vida = 100;
            coins = 200;

            PlayerRuna = new Runa();
            PlayerRuna.NoPlayer = true;

            //ANIMACOES==============================================================================================
            animacoes  = new SpritePrimitive("P-PrincipalSpriteSheet"   , position, new Vector2(160, 160), 8, 21, 0);
            espada     = new SpritePrimitive("F-PrincipalSpriteSheet"   , position, new Vector2(160, 160), 8, 21, 0);
            sombra     = new SpritePrimitive("MISC-SpriteSheet"         , position, new Vector2(160, 160), 4, 7 , 0);
            stepEffects= new SpritePrimitive("MISC-SpriteSheet"         , position, new Vector2(160, 160), 4, 7 , 0);
            //=======================================================================================================
            //SOUND
            Is_playing = false;
            //erro aqui talvez
            _SwordSwing = Game1.sContent.Load<SoundEffect>("SSwo.Swing");
            _FootSteps  = Game1.sContent.Load<SoundEffect>("LFootStep");
            _Dash       = Game1.sContent.Load<SoundEffect>("Dash");
            SoundEffect.MasterVolume =1f;
        }
        //=======================================================================================   
        public void Update(Vector2 posRato, GameTime gameTime)
        {
            //___________________________________________________________________________________
            //COSMETIC STUFF
            if (f1.IsNoAr == true) espada.SetSpriteAnimation(0, 7, 0, 7, 10);  //SPRITE INVISIVEL
            sombra      .SetSpriteAnimation(1, 1, 1, 1, 1 );                     sombra.Update();
            stepEffects .SetSpriteAnimation(3, 0, 3, 2, 10);
            if (Walking == false)                                              //SPRITE INVISIVEL
            { stepEffects.SetSpriteAnimation(7, 2, 7, 3, 10);
              stepEffects.mCurrentColumn = 3;               }

            _FootSteps.CreateInstance().IsLooped = false;

            Walking = false;
            Idle = false;
            //===================================================================================
            Vector2 speed = Vector2.Zero;
            //===================================================================================
            #region Movimentos basicos do player das 4 direções
            //___________________________________________________________________________________
            if(Game1.TelaMenu == false) { 
            KeyboardState keyState = Keyboard.GetState();
            int vel = 10;
            if (inCutscene == true)
            {
                if (OnParry == false && Atacando == false)
                {
                    //Maquina de Estados
                    #region Maquina de Estados
                    if (keyState.IsKeyDown(Keys.D))
                    {
                        Walking = true;
                        //position.X += vel;
                        if (f1.IsNoAr == false)
                        {
                            espada.SetSpriteAnimation(12, 0, 12, 7, 5);
                            espada.Update();
                        }
                        animacoes.SetSpriteAnimation(12, 0, 12, 7, 5);
                        animacoes.Update();
                        stepEffects.Update();
                    }
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        Walking = true;
                        //position.X -= vel;
                        if (f1.IsNoAr == false)
                        {
                            espada.SetSpriteAnimation(17, 0, 17, 7, 5);
                            espada.Update();
                        }
                        animacoes.SetSpriteAnimation(17, 0, 17, 7, 5);
                        animacoes.Update();
                        stepEffects.Update();
                    }
                    if (keyState.IsKeyDown(Keys.W))
                    {
                        Walking = true;
                        //position.Y -= vel;
                        if (f1.IsNoAr == false)
                        {
                            espada.SetSpriteAnimation(2, 0, 2, 7, 10);
                            espada.Update();
                        }
                        animacoes.SetSpriteAnimation(2, 0, 2, 7, 10);
                        animacoes.Update();
                        stepEffects.Update();
                    }
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        Walking = true;
                        //position.Y += vel;
                        if (f1.IsNoAr == false)
                        {
                            espada.SetSpriteAnimation(7, 0, 7, 7, 10);
                            espada.Update();
                        }
                        animacoes.SetSpriteAnimation(7, 0, 7, 7, 10);
                        animacoes.Update();
                        stepEffects.Update();
                    }
                    //***************************************************************************
                    //SOUND
                    if(keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.S))
                    {
                        if (animacoes.mCurrentColumn == 2 || animacoes.mCurrentColumn == 4 || animacoes.mCurrentColumn == 6 || animacoes.mCurrentColumn == 0)
                            _FootSteps.Play(volume+0.2f,pitch,pan);
                    }
                    if(keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.D))
                    {
                        if (animacoes.mCurrentColumn == 2 || animacoes.mCurrentColumn == 6)
                            _FootSteps.Play(volume+0.2f,pitch,pan);
                    }
                    //***************************************************************************

                    #endregion

                    //ACTUAL MOVING
                    #region Actual Moving
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        bool colis = false;
                        Rectangle r = new Rectangle();
                        Rectangle v = coliderPlayer;
                        v.X -= vel;
                        foreach (Tile t in Game1.GerMapas.UniversalTiles)
                        {
                            colis = t.ChecaColisao(t.Colider, v);
                            if (colis == true && t.existe == true)
                            {
                                r = t.Colider;
                                break;
                            }
                        }
                        if ((v.Left < r.Left && colis == true) || colis == false)
                        {
                            position.X -= vel;
                        }
                    }

                    if (keyState.IsKeyDown(Keys.D))
                    {
                        bool colis = false;
                        Rectangle r = new Rectangle();
                        Rectangle v = coliderPlayer;
                        v.Width += vel;
                        foreach (Tile t in Game1.GerMapas.UniversalTiles)
                        {
                            colis = t.ChecaColisao(t.Colider, v);
                            if (colis == true && t.existe == true)
                            {
                                r = t.Colider;
                                break;
                            }
                        }
                        if ((v.Right > r.Right && colis == true) || colis == false)
                        {
                            position.X += vel;
                        }
                    }

                    if (keyState.IsKeyDown(Keys.W))
                    {
                        bool colis = false;
                        Rectangle r = new Rectangle();
                        Rectangle v = coliderPlayer;
                        v.Y -= vel;
                        foreach (Tile t in Game1.GerMapas.UniversalTiles)
                        {
                            colis = t.ChecaColisao(t.Colider, v);
                            if (colis == true && t.existe == true)
                            {
                                r = t.Colider;
                                break;
                            }
                        }
                        if ((v.Top < r.Top && colis == true) || colis == false)
                        {
                            position.Y -= vel;
                        }
                    }

                    if (keyState.IsKeyDown(Keys.S))
                    {
                        bool colis = false;
                        Rectangle r = new Rectangle();
                        Rectangle v = coliderPlayer;
                        v.Height += vel;
                        foreach (Tile t in Game1.GerMapas.UniversalTiles)
                        {
                            colis = t.ChecaColisao(t.Colider, v);
                            if (colis == true && t.existe == true)
                            {
                                r = t.Colider;
                                break;
                            }
                        }
                        if ((v.Left == r.Bottom && colis == true) || colis == false)
                        {
                            position.Y += vel;
                        }
                    }
                }
                #endregion
                if (keyState.IsKeyDown(Keys.F1))
                    f1.tipo = Tipo.Lucas;
                if (keyState.IsKeyDown(Keys.F2))
                    f1.tipo = Tipo.Vasco;
                if (keyState.IsKeyDown(Keys.F3))
                    f1.tipo = Tipo.Moises;
                #endregion
            //===================================================================================
            #region Personagem aponta para o rato
                //___________________________________________________________________________________
                //Subtrai posição do rato à posicao do player, originando um vetor, que usa para calcular a rotação necessária
                Final = posRato - position;
                rotation = (float)Math.Atan2(Final.Y, Final.X);
                if (Atacando == false)
                {
                    if (Walking == false)
                    {
                        if (rotation > -0.75 && rotation <= 0.75)
                        {
                            lado = 1;
                            if (f1.IsNoAr == false)
                            {
                                espada.SetSpriteAnimation(14, 0, 14, 7, 10);
                                espada.Update();
                            }
                            animacoes.SetSpriteAnimation(14, 0, 14, 7, 10);
                            animacoes.Update();
                            Idle = true;
                        }
                        if (rotation > 0.75 && rotation <= 2.25)
                        {
                            lado = 2;
                            if (f1.IsNoAr == false)
                            {
                                espada.SetSpriteAnimation(9, 0, 9, 7, 10);
                                espada.Update();
                            }
                            animacoes.SetSpriteAnimation(9, 0, 9, 7, 10);
                            animacoes.Update();
                            Idle = true;
                        }
                        if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                        {
                            lado = 3;
                            if (f1.IsNoAr == false)
                            {
                                espada.SetSpriteAnimation(19, 0, 10, 7, 10);
                                espada.Update();
                            }
                            animacoes.SetSpriteAnimation(19, 0, 19, 7, 10);
                            animacoes.Update();
                            Idle = true;
                        }
                        if (rotation > -2.25 && rotation <= -0.75)
                        {
                            lado = 4;
                            if (f1.IsNoAr == false)
                            {
                                espada.SetSpriteAnimation(4, 0, 4, 7, 10);
                                espada.Update();
                            }
                            animacoes.SetSpriteAnimation(4, 0, 4, 7, 10);
                            animacoes.Update();
                            Idle = true;
                        }
                    }
                }
                #endregion
            //===================================================================================
            #region Ataque com facas
            //___________________________________________________________________________________
                MouseState mouse = Mouse.GetState();
                //Lado esquedo,, ataque básico___________________________________________________
                if ((mouse.LeftButton == ButtonState.Pressed) && OnParry == false && f1.IsNoAr == false)
                {
                    if (Atacando == false)
                    {
                        Idle = false;
                        f1.MeleeAttack(position + (Vector2.Normalize(Final) * 10), Final, rotation);
                        Atacando = true;
                    }
                }
                //Lado direito, lança faca________________________________________________________
                if ((mouse.RightButton == ButtonState.Pressed) && OnParry == false && f1.IsNoAr == false)
                {
                    if (Atacando == false)
                    {
                        Idle = false;
                        f1.RangedAttack(position + (Vector2.Normalize(Final) * 10), Final, rotation);
                        Atacando = true;
                    }
                }
                //Isto faz parte da máquina de estados do player. Não pode atacar enquanto Atacando for true
                if (Atacando == true)
                {
                    float temptimer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    countdown -= temptimer;

                    if (countdown < 0)
                    {
                        Atacando = false;
                        countdown = timer;
                    }
                }
                //ANIMACOES ATACAR ATIRAR
                #region ANIMACOES
                //ATACAR
                if (f1.IsVisible == true && f1.IsNoAr == false)
                {
                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        espada.SetSpriteAnimation(11, 0, 11, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(11, 0, 11, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        espada.SetSpriteAnimation(6, 0, 6, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(6, 0, 6, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        espada.SetSpriteAnimation(16, 0, 16, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(16, 0, 16, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        espada.SetSpriteAnimation(1, 0, 1, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(1, 0, 1, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    //SOUND----------------------------------------------------------------------------------
                    //algoritmo para o som apenas tocar uma vez e nao fazer stack
                    Is_playing = true;
                    if (animacoes.mCurrentColumn == 7)
                    {
                        Is_playing = false;
                        blocker = false;
                    }
                    if (blocker == true) Is_playing = false;
                    if (Is_playing == true)
                    {
                        _SwordSwing.Play(volume, pitch, pan);
                        blocker = true;
                    }
                    //_______________________________________________________________________________________
                }
                //ATIRAR
                if (f1.IsVisible == true && f1.IniciaContador == true)
                {
                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        espada.SetSpriteAnimation(10, 0, 10, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(10, 0, 10, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        espada.SetSpriteAnimation(5, 0, 5, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(5, 0, 5, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        espada.SetSpriteAnimation(15, 0, 15, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(15, 0, 15, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        espada.SetSpriteAnimation(0, 0, 0, 7, 5);
                        if (animacoes.mCurrentColumn == 7) espada.mCurrentColumn = 7;
                        else espada.Update();
                        animacoes.SetSpriteAnimation(0, 0, 0, 7, 5);
                        if (animacoes.mCurrentColumn == 7) animacoes.mCurrentColumn = 7;
                        else animacoes.Update();
                    }
                }

                #endregion
                #endregion
                //===================================================================================
                #region Mecanicas
                //_______________________________________________________________________________
                //Lucas aka Dash /*PARECE QUE TEM EBOLA MAS FUNCIONA*/ JA NAO TEM; KINDA
                #region Dash
                Texture2D dashShadow = whiteRectangle;
                if (f1.tipo == Tipo.Lucas)
                {
                    if (keyState.IsKeyDown(Keys.Space) && MecInCoolDown == false && contador == contadorTempo)
                    {
                        MecInCoolDown = true;
                        contador = 0;
                        _Dash.Play(volume,pitch,pan);

                    }
                    //DASH
                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        #region Direita
                        if (MecInCoolDown == true)
                        {
                            if (contador <= 1)
                            {
                                weee = Final;
                                dashShadow = animacoes.image;
                            }
                            contador++;
                            Rectangle r = coliderPlayer;
                            r.Width += 40;
                            bool colis = ChecaColisaoComParedes(r);
                            if (contador < 15 && colis == false)
                            {
                                AfterImages dash = new AfterImages();
                                if (colis == false)
                                {
                                    speed = Vector2.Normalize(weee) * 40;
                                    position += speed;
                                }
                                else
                                {
                                    speed = Vector2.Zero;
                                    position += speed;
                                }
                                #region Direcao do Dash
                                if (rotation > -0.75 && rotation <= 0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 110, position.Y - 80), 3);
                                }
                                else
                               if (rotation > 0.75 && rotation <= 2.25)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 2);
                                }
                                else
                               if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 1);
                                }
                                else
                               if (rotation > -2.25 && rotation <= -0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 60), 0);
                                }
                                #endregion
                                DashAfterImages.Add(dash);
                            }

                            if (contador == 19)
                            {
                                speed = Vector2.Zero;
                                position += speed;
                            }

                            if (contador >= contadorTempo - 1)
                            {
                                DashAfterImages.Clear();
                                contador = contadorTempo;
                                MecInCoolDown = false;
                            }
                        }
                        #endregion
                    }
                    else
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        #region Baixo
                        if (MecInCoolDown == true)
                        {
                            if (contador <= 1)
                            {
                                weee = Final;
                                dashShadow = animacoes.image;
                            }
                            contador++;
                            Rectangle r = coliderPlayer;
                            r.Height += 40;
                            bool colis = ChecaColisaoComParedes(r);
                            if (contador < 15 && colis == false)
                            {
                                AfterImages dash = new AfterImages();
                                if (colis == false)
                                {
                                    speed = Vector2.Normalize(weee) * 40;
                                    position += speed;
                                }
                                else
                                {
                                    speed = Vector2.Zero;
                                    position += speed;
                                }
                                #region Direcao do Dash
                                if (rotation > -0.75 && rotation <= 0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 110, position.Y - 80), 3);
                                }
                                else
                               if (rotation > 0.75 && rotation <= 2.25)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 2);
                                }
                                else
                               if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 1);
                                }
                                else
                               if (rotation > -2.25 && rotation <= -0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 60), 0);
                                }
                                #endregion
                                DashAfterImages.Add(dash);
                            }

                            if (contador == 19)
                            {
                                speed = Vector2.Zero;
                                position += speed;
                            }

                            if (contador >= contadorTempo - 1)
                            {
                                DashAfterImages.Clear();
                                contador = contadorTempo;
                                MecInCoolDown = false;
                            }
                        }
                        #endregion
                    }
                    else
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        #region Esquerda
                        if (MecInCoolDown == true)
                        {
                            if (contador <= 1)
                            {
                                weee = Final;
                                dashShadow = animacoes.image;
                            }
                            contador++;
                            Rectangle r = coliderPlayer;
                            r.X -= 40;
                            bool colis = ChecaColisaoComParedes(r);
                            if (contador < 15 && colis == false)
                            {
                                AfterImages dash = new AfterImages();
                                if (colis == false)
                                {
                                    speed = Vector2.Normalize(weee) * 40;
                                    position += speed;
                                }
                                else
                                {
                                    speed = Vector2.Zero;
                                    position += speed;
                                }
                                #region Direcao do Dash
                                if (rotation > -0.75 && rotation <= 0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 110, position.Y - 80), 3);
                                }
                                else
                               if (rotation > 0.75 && rotation <= 2.25)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 2);
                                }
                                else
                               if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 1);
                                }
                                else
                               if (rotation > -2.25 && rotation <= -0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 60), 0);
                                }
                                #endregion
                                DashAfterImages.Add(dash);
                            }

                            if (contador == 19)
                            {
                                speed = Vector2.Zero;
                                position += speed;
                            }

                            if (contador >= contadorTempo - 1)
                            {
                                DashAfterImages.Clear();
                                contador = contadorTempo;
                                MecInCoolDown = false;
                            }
                        }
                        #endregion
                    }
                    else
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        #region Cima
                        if (MecInCoolDown == true)
                        {
                            if (contador <= 1)
                            {
                                weee = Final;
                                dashShadow = animacoes.image;
                            }
                            contador++;
                            Rectangle r = coliderPlayer;
                            r.Y -= 40;
                            bool colis = ChecaColisaoComParedes(r);
                            if (contador < 15 && colis == false)
                            {
                                AfterImages dash = new AfterImages();
                                if (colis == false)
                                {
                                    speed = Vector2.Normalize(weee) * 40;
                                    position += speed;
                                }
                                else
                                {
                                    speed = Vector2.Zero;
                                    position += speed;
                                }
                                #region Direcao do Dash
                                if (rotation > -0.75 && rotation <= 0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 110, position.Y - 80), 3);
                                }
                                else
                               if (rotation > 0.75 && rotation <= 2.25)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 2);
                                }
                                else
                               if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 80), 1);
                                }
                                else
                               if (rotation > -2.25 && rotation <= -0.75)
                                {
                                    dash = new AfterImages(new Vector2(position.X - 80, position.Y - 60), 0);
                                }
                                #endregion
                                DashAfterImages.Add(dash);
                            }

                            if (contador == 19)
                            {
                                speed = Vector2.Zero;
                                position += speed;
                            }

                            if (contador >= contadorTempo - 1)
                            {
                                DashAfterImages.Clear();
                                contador = contadorTempo;
                                MecInCoolDown = false;
                            }
                        }
                        #endregion
                    }
                    

                    foreach(AfterImages after in DashAfterImages)
                    {
                        after.FadeAway();
                    }
                    //ANIMACOES DASH
                    #region Animacoes
                    if (MecInCoolDown == true)
                    {
                        if (rotation > -0.75 && rotation <= 0.75)
                        {
                            espada.mCurrentColumn = 0;
                            espada.SetSpriteAnimation(13, 0, 13, 0, 10);
                            espada.Update();
                            animacoes.SetSpriteAnimation(13, 0, 13, 0, 10);
                            animacoes.mCurrentColumn = 0;
                            animacoes.Update();
                        }
                        if (rotation > 0.75 && rotation <= 2.25)
                        {
                            espada.mCurrentColumn = 0;
                            espada.SetSpriteAnimation(8, 0, 8, 0, 10);
                            espada.Update();
                            animacoes.mCurrentColumn = 0;
                            animacoes.SetSpriteAnimation(8, 0, 8, 0, 10);
                            animacoes.Update();
                        }
                        if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                        {
                            espada.mCurrentColumn = 0;
                            espada.SetSpriteAnimation(18, 0, 18, 0, 10);
                            espada.Update();
                            animacoes.SetSpriteAnimation(18, 0, 18, 0, 10);
                            animacoes.mCurrentColumn = 0;
                            animacoes.Update();
                        }
                        if (rotation > -2.25 && rotation <= -0.75)
                        {
                            espada.mCurrentColumn = 0;
                            espada.SetSpriteAnimation(3, 0, 3, 0, 10);
                            espada.Update();
                            animacoes.mCurrentColumn = 0;
                            animacoes.SetSpriteAnimation(3, 0, 3, 0, 10);
                            animacoes.Update();
                        }
                    }
                    #endregion
                }
                #endregion
                //Parry aka Vasco 
                #region Parry
                else if (f1.tipo == Tipo.Vasco)
                {
                    if (keyState.IsKeyDown(Keys.Space) && OnParry == false && contador == contadorTempo)
                    {
                        OnParry = true;
                    }
                    if (OnParry == true)
                    {
                        float temptimer = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        countdown -= temptimer;
                        if (countdown < 0)
                        {
                            OnParry = false;
                            countdown = timer;
                            contador = 0;
                        }
                        //ANIMACOES PARRY
                        #region Animacoes
                        if (rotation > -0.75 && rotation <= 0.75)
                        {
                            if (f1.IsNoAr == false)
                            {
                                espada.mCurrentColumn = 4;
                                espada.SetSpriteAnimation(11, 4, 11, 4, 10);
                                espada.Update();
                            }
                            animacoes.SetSpriteAnimation(11, 4, 11, 4, 10);
                            animacoes.mCurrentColumn = 4;
                            animacoes.Update();
                        }
                        if (rotation > 0.75 && rotation <= 2.25)
                        {
                            espada.mCurrentColumn = 3;
                            espada.SetSpriteAnimation(6, 3, 6, 3, 10);
                            espada.Update();
                            animacoes.mCurrentColumn = 3;
                            animacoes.SetSpriteAnimation(6, 3, 6, 3, 10);
                            animacoes.Update();
                        }
                        if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                        {
                            espada.mCurrentColumn = 4;
                            espada.SetSpriteAnimation(16, 4, 16, 4, 10);
                            espada.Update();
                            animacoes.SetSpriteAnimation(16, 4, 16, 4, 10);
                            animacoes.mCurrentColumn = 4;
                            animacoes.Update();
                        }
                        if (rotation > -2.25 && rotation <= -0.75)
                        {
                            espada.mCurrentColumn = 3;
                            espada.SetSpriteAnimation(1, 3, 1, 3, 10);
                            espada.Update();
                            animacoes.mCurrentColumn = 3;
                            animacoes.SetSpriteAnimation(1, 3, 1, 3, 10);
                            animacoes.Update();
                        }
                        #endregion
                    }
                    if (contador < contadorTempo)
                    {
                        contador++;

                        if (contador > contadorTempo - 1)
                        {
                            contador = contadorTempo;
                        }
                    }
                }
                #endregion
                //Shield aka Moises
                #region Shield
                else if (f1.tipo == Tipo.Moises)
                {
                    if (keyState.IsKeyDown(Keys.Space) && OnShield == false && contador == contadorTempo)
                    {
                        OnShield = true;
                        
                    }
                    if (keyState.IsKeyUp(Keys.Space) && OnShield == true && contador == contadorTempo)
                    {
                        OnShield = false;
                        contador = 0;
                    }
                    if (OnParry == false && contador == 0)
                    {
                        float temptimer = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        countdown -= temptimer;
                        if (countdown < 0)
                        {
                            OnShield = false;
                            countdown = timer;
                            contador = 0;
                        }
                        
                    }
                    if (contador < contadorTempo)
                    {
                        contador++;

                        if (contador > contadorTempo - 1)
                        {
                            contador = contadorTempo;
                        }
                    }
                }
                if (OnShield == true)
                {
                    //ANIMACOES SHIELD // MESMAS QUE PARRY
                    #region Animacoes
                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        espada.mCurrentColumn = 4;
                        espada.SetSpriteAnimation(11, 4, 11, 4, 10);
                        animacoes.SetSpriteAnimation(11, 4, 11, 4, 10);
                        animacoes.mCurrentColumn = 4;
                    }
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        espada.mCurrentColumn = 3;
                        espada.SetSpriteAnimation(6, 3, 6, 3, 10);
                        espada.Update();
                        animacoes.mCurrentColumn = 3;
                        animacoes.SetSpriteAnimation(6, 3, 6, 3, 10);
                        animacoes.Update();
                    }
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        espada.mCurrentColumn = 4;
                        espada.SetSpriteAnimation(16, 4, 16, 4, 10);
                        espada.Update();
                        animacoes.SetSpriteAnimation(16, 4, 16, 4, 10);
                        animacoes.mCurrentColumn = 4;
                        animacoes.Update();
                    }
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        espada.mCurrentColumn = 3;
                        espada.SetSpriteAnimation(1, 3, 1, 3, 10);
                        espada.Update();
                        animacoes.mCurrentColumn = 3;
                        animacoes.SetSpriteAnimation(1, 3, 1, 3, 10);
                        animacoes.Update();
                    }
                    #endregion
                }
                #endregion
                #endregion
            //===================================================================================
            #region usaruna
                //___________________________________________________________________________________
                if (keyState.IsKeyDown(Keys.Q) && contadorSpell == 50)
                {
                    PlayerRuna.Ativacao(position + (Vector2.Normalize(Final) * 10), Final, rotation, false);
                    contadorSpell = 0;
                }

                if (contadorSpell < 50)
                {
                    contadorSpell++;
                }

                if (contadorSpell >= 50)
                {
                    contadorSpell = 50;
                }

            }
            PlayerRuna.Update(gameTime);
                #endregion



                //===================================================================================
                #region Mata Monstro
                //Vai recebr a lista de monstros
                //WORK U«IN POGRESS

                if (ChecaDanoMagia() == true)
                {
                    vida = vida - 50;
                }

                foreach (Inimigo A in Game1.GerInimigos.listaInimigos)
                {              

                    timeDestiro += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (A != null)
                {
                    if (timeDestiro > 30)
                    {  //tem 5 segundo de invenerabilidade
                        if (A.Colider.Intersects(coliderPlayer) && A.Isdead == false)
                        {
                            luz = true;
                            Game1.espera = 0; //HERE
                            if (timeDestiro > 6)
                            {
                                timeDestiro = 0;
                            }
                           
                            vida = vida - 25;
                        }
                    }

                    if (f1.Colider.Intersects(A.Colider) && A.Isdead == false)
                    {

                        A.Health = A.Health - f1.dano;
                      

                            if (A.Health < 0)
                            {
                            A.Isdead = true;
                     

                            }
                    }

                
                    }

            }
            }
            #endregion
            //===================================================================================
            f1.Contadores(gameTime);                                 //Executa o "Update" da faca
            //===================================================================================
        }
        //=======================================================================================
        public bool ChecaColisaoPlayer(Rectangle Oposto)
        {
            if(coliderPlayer.Intersects(Oposto))
            {
                return true;
            }

            return false;
        }

        public bool ChecaColisaoComParedes(Rectangle colider)
        {
            bool colis = false;
            foreach (Tile t in Game1.GerMapas.UniversalTiles)
            {
                colis = t.ChecaColisao(t.Colider, colider);
                if (colis == true && t.existe == true)
                    return true;
            }

            return false;
        }

        public bool ChecaDanoMagia()
        {
            bool colis = false, colis2 = false;
            foreach(Inimigo ini in Game1.GerInimigos.listaInimigos)
            {
                foreach(Projectile p in ini.RangedRuna.fireBalls)
                {
                    colis = coliderPlayer.Intersects(p.mColider);
                    break;
                }
            }
            foreach (Inimigo ini in Game1.GerInimigos.listaInimigos)
            {
                foreach (Projectile p in ini.RangedRuna.iceSpikes)
                {
                    colis2 = coliderPlayer.Intersects(p.mColider);
                    break;
                }
            }

            if (colis2 == false && colis == false)
                return false;

            return true;
        }
        //=======================================================================================
        public virtual void Draw()
        {
            foreach (AfterImages after in DashAfterImages)
            {
                after.Draw();
            }
            //===============================================================================
            //Centro e Retangulo da sprite
            Vector2 centro = new Vector2(animacoes.image.Width/ animacoes.mNumColumn, animacoes.image.Height/animacoes.mNumRow);
            RecOri      = new Rectangle((int)position.X,(int)position.Y,160, 160);
            recParry    = new Rectangle(20, 20,                       80,80);
            recShield   = new Rectangle(10, 20, 140, 80);
            recSombra   = new Rectangle(RecOri.X, RecOri.Y + 80,    sombra.mSpriteWidth         , sombra.mSpriteHeight);
            recSteps    = new Rectangle(RecOri.X, RecOri.Y + 55,    stepEffects.mSpriteWidth    , stepEffects.mSpriteHeight);
            //===============================================================================
            //Retangulo do Colider. Ele segue o player, obviamente
            coliderPlayer = new Rectangle(-35, 30 , 70,50);
            coliderPlayer.X += (int)position.X;
            coliderPlayer.Y += (int)position.Y;
            //===============================================================================
            //Desenha animacoes
            sombra.Draw(recSombra);                                                  //SOMBRA
            stepEffects.Draw(recSteps);                                              //EFFECT
            espada.Draw(RecOri);                                                     //ESPADA
            animacoes.Draw(RecOri);                                                  //PLAYER
            //Desenha o colider para bug testing
            //Game1.spriteBatch.Draw(whiteRectangle, coliderPlayer, Color.Chocolate);
            //===============================================================================
            //Desenha runa
            PlayerRuna.Draw();
            //===============================================================================
            //Desenha runa Inimigos
           
            //===============================================================================
            //Desenha parry
            if (OnParry == true)
            {
                Game1.spriteBatch.Draw(whiteRectangle, position + (Vector2.Normalize(Final) * 20), recParry, Color.Red, rotation-(float)Math.PI/2, new Vector2(40,40), 1.0f, SpriteEffects.None, 1);
            }
            //===============================================================================
            //Desenha shield
            if (OnShield == true)
            {
                Game1.spriteBatch.Draw(whiteRectangle, position + (Vector2.Normalize(Final) * 20), recShield, Color.Green, rotation - (float)Math.PI / 2, new Vector2(70, 40), 1.0f, SpriteEffects.None, 1);
            }

            //===============================================================================
            //Desenha a faca.
            f1.Draw();

        }
        //=======================================================================================
    }
}
