using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnicas.Mapa;
using Tecnicas.NPC;

namespace Tecnicas
{
    public enum Tipo { Moises, Lucas, Vasco };

    public class Facas
    {
        //Variaveis basicas
        private Vector2 mDirecao, mPosicao, mSize, mPosicaoOriginal;
        private Texture2D mSprite;
        private float mRotacao;
        //Qual de nos é que é????
        public Tipo tipo;

        //Variaveis Para os ataques
        public bool IsVisible, IsNoAr, IniciaContador;
        private const float timer = 1;
        private float countdown = timer;
        private int speed = 10, contador = 25;

        //Retangulos
        public Rectangle Rec { get; set; }
        //Animacoes
        private SpritePrimitive espada;

        public Rectangle Colider;

        //Hitbox à mostra
        Texture2D whiteRectangle;
        public int dano = 10;
        //--------------------------------------------
        //sound
        private SoundEffect _SwordSwing;
        private SoundEffect _Metal;
        private bool Is_playing;
        private bool blocker;
        float volume = 0.4f;
        float pitch = 0.3f;
        float pan = 0.0f;
        //--------------------------------------

        //Construtor vazio. Acho que nao vai ser usado
        public Facas()
        {
            //Vazio
        }

        //Load da Sprite. Isto ainda está incompleto visto que vai ter várias sprites etc
        public void LoadImage()
        {
            //**************************************************************************************************************************
            //NOVA IMAGEM
            espada = new SpritePrimitive("MISC-SpriteSheet", mPosicao, new Vector2(160, 160), 4, 7, 0);
            //APAGAR DEPOIS
            mSprite = Game1.sContent.Load<Texture2D>("sword");
            //sound
            _SwordSwing = Game1.sContent.Load<SoundEffect>("SSwo.Swing");
            _Metal = Game1.sContent.Load<SoundEffect>("Metal");
            //**************************************************************************************************************************
            //sprites para mostrar o colider maroto
            whiteRectangle = new Texture2D(Game1.spriteBatch.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

        //Melee Attack duh
        public void MeleeAttack(Vector2 posicao, Vector2 direcao, float rotacao)
        {
            IsVisible = true;
            if ((IsVisible) && (!IsNoAr)) //Não deixa usar a faca caso esta esteja "no ar" ,ou seja, se a atiramos
            {
                mPosicao = posicao;
                mDirecao = direcao;
                mRotacao = rotacao;
            }
        }

        //Ranged Attack
        public void RangedAttack(Vector2 posicao, Vector2 direcao, float rotacao)
        {
            IsVisible = true;
            if ((IsVisible) && (!IsNoAr)) //Confirma que ainda temos a faca
            {
                IsNoAr = true;
                mPosicao = posicao;
                mDirecao = direcao;
                mRotacao = rotacao;
                IniciaContador = true;  //Este contador serve apenas para delimitar o tempo que ele "voa"
            }
        }

        //Physical Mechanic
        public int PhysicalMechanic()
        {
            if (tipo == Tipo.Lucas)
            {
                return 1;
            }
            return 0;
        }

        //Este é o Update. Mas como sou Edgy chamei de Contadores. Processa-me moisés
        public void Contadores(GameTime gameTime)
        {
            //Isto é a máquina de estados da faca
            //O estado principal é se esta no Ar
            //Caso nao esteja, se tiver visivel, quer dizer que alguem a chamou, logo ele vai por a faca no centro do player, mas depois conforme a direção, vai puxa-la
            //400 pixeis nesse lado. Desta maneira, a faca aparece à frente do player, roda com ele, e segue-o caso este se mexa
            //E depois vai diminuir o contador. Este contador é responsavel pelo tempo em que a faca se mantem ativa.
            if (!IsNoAr)
            {
                if (IsVisible)
                {
                    mPosicao = Game1.Jogador.Position + Vector2.Normalize(mDirecao) * 18;
                    contador--;
                }

                if (contador < 0)
                {
                    IsVisible = false;
                    contador = 50;
                }
            }
            else
            //Caso a faca esteja no ar, vai checar se IniciaContador está true ou não. O IniciaContador vai limitar o tempo em que a faca "voa". Se tiver true
            //Vai andar a uma certa speed numa direcao e vai ligar um contador. Quando este acabar IniciaContador vira false
            //Se IniciaContador estiver falso, a faca para
            {

                if (IniciaContador == true)
                {
                    bool colisao = false, colisao2 = false;
                    foreach (Tile t in Game1.GerMapas.UniversalTiles)
                    {
                        colisao = t.ChecaColisao(t.Colider, Colider);
                        if (colisao == true && t.existe == true)
                        {
                            break;
                        }
                    }
                    foreach (Inimigo ini in Game1.GerInimigos.listaInimigos)
                    {
                        colisao2 = Colider.Intersects(ini.Colider);
                        if (colisao2 == true && ini.Isdead == false)
                        {
                            break;
                        }
                    }
                    if (colisao == false && colisao2 == false)
                    {
                        speed = 10;
                        mPosicao += Vector2.Normalize(mDirecao) * speed;
                    }
                    else
                    {
                        IniciaContador = false;
                        countdown = timer;
                    }
                    float temptimer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    countdown -= temptimer;

                    if (countdown < 0)
                    {
                        IniciaContador = false;
                        countdown = timer;
                    }
                }
                if (IniciaContador == false)
                {
                    speed = 0;
                    mPosicao += Vector2.Normalize(mDirecao) * speed;
                }

                //No entanto, para o jogador voltar a pegar na faca, temos de checar a colisão. Ele pega no Player declarado na Game1 e vai ao metodo ChecaColisaoPlayer que se encontra dentro
                //do player
                if (Game1.Jogador.ChecaColisaoPlayer(Colider) == true)
                {
                    if ((IsNoAr) && (IniciaContador == false))
                    {
                        Colider.X = 0;
                        Colider.Y = 0;
                        IsNoAr = false;
                        IsVisible = false;
                    }
                }
                //=================================================================================
                //ANIMACOES / sound
                espada.Update();
                if (IniciaContador == true)
                {
                    espada.SetSpriteAnimation(2, 0, 2, 3, 3);
                    if (espada.mCurrentColumn % 2 == 0) _SwordSwing.Play(volume, pitch, pan);
                }
                else
                {
                    espada.SetSpriteAnimation(0, 2, 0, 2, 0);
                    //Previne que seja tocado mais que 1 vez
                    Is_playing = true;
                    if (espada.mCurrentColumn == 2)
                    {
                        Is_playing = false;
                        blocker = false;
                    }
                    if (blocker == true) Is_playing = false;
                    if (Is_playing == true)
                    {
                        _Metal.Play(volume + 0.6f, pitch, pan);
                        blocker = true;
                    }
                }
                //=================================================================================
            }

        }


        public void Draw()
        {
            //Caso esteja visivel desenha lol xd hehe
            //CREIEI MAIS UM RECTANGULO
            if (IsVisible && IsNoAr)
            {
                Vector2 centro = new Vector2(50, 50);
                //Retangulo da sprite
                Rectangle Rec = new Rectangle(0, 0, 150, 150);
                //Retangulo da colisão que vai seguir a faca
                Colider = new Rectangle(0, 0, 75, 75);
                Rec.X += (int)mPosicao.X;
                Rec.Y += (int)mPosicao.Y;


                Colider.X += (int)mPosicao.X - 33;
                Colider.Y += (int)mPosicao.Y - 33;
                //Desenha a sprite
                espada.Draw(Rec);
                //Game1.spriteBatch.Draw(mSprite, mPosicao, Rec, Color.White, mRotacao, centro, 1.0f, SpriteEffects.None, 1);
                //Desenha o colider. 
                //Game1.spriteBatch.Draw(whiteRectangle, Colider, Color.Chocolate);

            }
            else if (IsVisible)
            {
                Vector2 centro = new Vector2(50, 50);
                //Retangulo da sprite
                Rectangle Rec = new Rectangle(0, 0, 150, 150);
                //Retangulo da colisão que vai seguir a faca
                Colider = new Rectangle(0, 0, 75, 75);
                Rec.X += (int)mPosicao.X;
                Rec.Y += (int)mPosicao.Y;

                if (Game1.Jogador.lado == 1)
                {
                    Colider.X += (int)mPosicao.X + 10;
                    Colider.Y += (int)mPosicao.Y - 25;
                }
                if (Game1.Jogador.lado == 2)
                {
                    Colider.X += (int)mPosicao.X - 20;
                    Colider.Y += (int)mPosicao.Y + 45;

                }
                if (Game1.Jogador.lado == 3)
                {
                    Colider.X += (int)mPosicao.X - 80;
                    Colider.Y += (int)mPosicao.Y - 25;

                }
                if (Game1.Jogador.lado == 4)
                {
                    Colider.X += (int)mPosicao.X - 20;
                    Colider.Y += (int)mPosicao.Y - 80;

                }

                //Desenha a sprite
                espada.Draw(Rec);
                //Game1.spriteBatch.Draw(mSprite, mPosicao, Rec, Color.White, mRotacao, centro, 1.0f, SpriteEffects.None, 1);
                //Desenha o colider. 
               // Game1.spriteBatch.Draw(whiteRectangle, Colider, Color.Chocolate);

            }
        }
    }
}
