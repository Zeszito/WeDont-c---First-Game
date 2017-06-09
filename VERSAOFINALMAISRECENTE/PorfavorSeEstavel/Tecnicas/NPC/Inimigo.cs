using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnicas.Magia;

namespace Tecnicas.NPC
{
    public class Inimigo
    {
        //=======================================================================================
        //EM FALTA:
        //fazer coliders dos ataques dos mobs
        //refazer condicao do ataque dos ranged mobs
        //fazer animacao morrer, depois de perceber como caralho o mob morre
        //CHUPAR UMA PISSA QUE CHEGUE AO ESTOMAGO
        //=======================================================================================
        #region Variaveis
        public string Name;
        public EnemyType eT;
        public EnemyFaction eF;
        public Estado Es;
        //talvez nao necessario
        private Texture2D Texture;

        public Runa Mruna = new Runa();
        public Vector2 Position;
        public bool Isdead;
        public int Health;
        private int MaxHealth;
        private int Attack;
        bool ativo = false;
        bool Atacar = false;

        public Rectangle Colider;

        //ANIMACOES
        private SpritePrimitive animacoes;
        private SpritePrimitive sombra;
        private Rectangle recSombra;
        private bool Idle, Walking;
        private bool topSide, botSide, rightSide, leftSide;
        private Vector2 Final;
        private float rotation;

        public Runa RangedRuna;
        int contadorSpell;

        public enum EnemyType
        {
            Melee,
            Ranged,
            Boss
        }
        public enum EnemyFaction
        {
            Evil,
            Holy,
            Neutral
        }
        public enum Estado
        {
            Atacando,
            Idle,
            Atirando,
            Morrendo
        }
        #endregion
        //=======================================================================================
        public Inimigo() {
            Random r = new Random();
            int i = r.Next(2);
            if (i==0)
            {
                RangedRuna = new Runa(tipo.Fire);
            }
            else
            if (i == 1)
            {
                RangedRuna = new Runa(tipo.Ice);
            }
            contadorSpell = 150;
            Atacar = false;
        }                                                                     // esta merda secalhar tambem nao
        //=======================================================================================
        public void LoadContent(string Name)
        {
            //Texture = Game1.sContent.Load<Texture2D>("inimigos/" + Name);                      // esta merda secalhar nao e preciso

        }
        //=======================================================================================
        public void Update(GameTime gameTime) //vai mover o inimigo
        {
            //___________________________________________________________________________________
            //Atacar = false;
            //COSMETICS
            sombra.SetSpriteAnimation(1, 1, 1, 1, 0);
            sombra.Update();
            //OBTER O ANGULO PARA ANIMAR CORRETAMENTE
            Final       = Game1.Jogador.Position - Position;
            rotation    = (float)Math.Atan2(Final.Y, Final.X);
            //DISTANCIA
            float x = Vector2.Distance(Position, Game1.Jogador.Position);
            //ESTADO PARA ANIMACAO IDLE FUNCIONAR
            Walking = false;
            //Atacar  = false;
            Debug.Print("" + x);                                                  //dafuq is dis?
            //===================================================================================
            //VERIFICA DISTANCIA PARA AGRO
            if (Vector2.Distance(Position, Game1.Jogador.Position) < 1500)          ativo =  true;
            if (Vector2.Distance(Position, Game1.Jogador.Position) > 1500)          ativo = false;
            //VERIFICA DISTANCIA PARA ATACAR_MELEE
            if (eT == EnemyType.Melee && Vector2.Distance(Position, Game1.Jogador.Position) < 75)
                Atacar = true;


            if (Vector2.Distance(Position, Game1.Jogador.Position) < 575 && eT == EnemyType.Ranged)
                Atacar = false;
            else
            if (Vector2.Distance(Position, Game1.Jogador.Position) > 575 && eT == EnemyType.Ranged)
                Atacar = true;


            ChecaColisaoComProjeteis();
            ChecaColisao();
            if (Health <= 0)
            {
                Random r = new Random();
                int whatup = r.Next(4);
                if (whatup == 0)
                {
                    Drop d = new Drop(tipo.Fire, Position);
                    Game1.drops.Add(d);
                }
                else if (whatup == 1)
                {
                    Drop d = new Drop(tipo.Ice, Position);
                    Game1.drops.Add(d);
                }
                Isdead = true;
                Game1.Ninidead++; //ADicionei aqui
            }
           //VERIFICA DISTANCIA PARA ATACAR_RANGED /*alterar condiçao de quand os ranged atacam*/
            if (eT == EnemyType.Ranged && Vector2.Distance(Position, Game1.Jogador.Position) < 575)
            {
                //Atacar = true;

                if (contadorSpell == 150)
                {
                    Final = Game1.Jogador.Position - Position;
                    rotation = (float)Math.Atan2(Final.Y, Final.X);

                    RangedRuna.Ativacao(Position + (Vector2.Normalize(Final) * 10), Final, rotation, true);
                    contadorSpell = 0;
                }
            }

            if (contadorSpell < 150)
                contadorSpell++;

            if (contadorSpell >= 150)
            {
                contadorSpell = 150;
            }
          

            RangedRuna.Update(gameTime);
            //===================================================================================
            //SE O MOB ESTIVER AGRADO REALIZA UMA SERIE DE VERIFICAÇOES
            if (ativo == true)
            {
                //_______________________________________________________________________________
                #region MELEE
                if (eT == EnemyType.Melee && Atacar == false)
                {
                    //Basico para caralho 
                    if (Position.X < Game1.Jogador.Position.X) Position.X += 1;
                    if (Position.Y < Game1.Jogador.Position.Y) Position.Y += 1;
                    if (Position.X > Game1.Jogador.Position.X) Position.X -= 1;
                    if (Position.Y > Game1.Jogador.Position.Y) Position.Y -= 1;

                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        animacoes.SetSpriteAnimation(6, 0, 6, 3, 10);         animacoes.Update();
                        topSide = false; botSide = false;   leftSide = false;   rightSide = true;
                        Walking = true;
                    }
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        animacoes.SetSpriteAnimation(3, 0, 3, 3, 10);         animacoes.Update();
                        topSide = false;  botSide = true;  leftSide = false;   rightSide = false;
                        Walking = true;
                    }
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        animacoes.SetSpriteAnimation(9, 0, 9, 3, 10);         animacoes.Update();
                        topSide = false;  botSide = false;  leftSide = true;   rightSide = false;
                        Walking = true;
                    }
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        animacoes.SetSpriteAnimation(0, 0, 0, 3, 10);         animacoes.Update();
                        topSide = true; botSide = false;   leftSide = false;   rightSide = false;
                        Walking = true;
                    }
                }
                #endregion
                //===============================================================================
                #region RANGED
                if (eT == EnemyType.Ranged && Atacar == false)
                {
                    if (Position.X < Game1.Jogador.Position.X) Position.X += 1;
                    if (Position.Y < Game1.Jogador.Position.Y) Position.Y += 1;
                    if (Position.X > Game1.Jogador.Position.X) Position.X -= 1;
                    if (Position.Y > Game1.Jogador.Position.Y) Position.Y -= 1;

                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        animacoes.SetSpriteAnimation(6, 0, 6, 3, 10);         animacoes.Update();
                        topSide = false;  botSide = false;   leftSide = false;  rightSide = true;
                        Walking = true;
                    }
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        animacoes.SetSpriteAnimation(3, 0, 3, 3, 10);         animacoes.Update();
                        topSide = false;  botSide = true;   leftSide = false;  rightSide = false;
                        Walking = true;
                    }
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        animacoes.SetSpriteAnimation(9, 0, 9, 3, 10);         animacoes.Update();
                        topSide = false;  botSide = false;  leftSide = true;   rightSide = false;
                        Walking = true;
                    }
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        animacoes.SetSpriteAnimation(0, 0, 0, 3, 10);         animacoes.Update();
                        topSide = true;  botSide = false;   leftSide = false;  rightSide = false;
                        Walking = true;
                    }
                }
                #endregion
                //===============================================================================
                #region BOSS
                if (eT == EnemyType.Boss && Atacar == false)
                {
                    if (Position.X < Game1.Jogador.Position.X) Position.X += 1;
                    if (Position.Y < Game1.Jogador.Position.Y) Position.Y += 1;
                    if (Position.X > Game1.Jogador.Position.X) Position.X -= 1;
                    if (Position.Y > Game1.Jogador.Position.Y) Position.Y -= 1;
                    if (rotation > -0.75 && rotation <= 0.75)
                    {
                        animacoes.SetSpriteAnimation(6, 0, 6, 3, 10);
                        animacoes.Update();
                        Walking = true;
                    }
                    if (rotation > 0.75 && rotation <= 2.25)
                    {
                        animacoes.SetSpriteAnimation(3, 0, 3, 3, 10);
                        animacoes.Update();
                        Walking = true;
                    }
                    if ((rotation > 2.25 && rotation < 3.2) || (rotation <= -2.25 && rotation > -3.2))
                    {
                        animacoes.SetSpriteAnimation(9, 0, 9, 3, 10);
                        animacoes.Update();
                        Walking = true;
                    }
                    if (rotation > -2.25 && rotation <= -0.75)
                    {
                        animacoes.SetSpriteAnimation(0, 0, 0, 3, 10);
                        animacoes.Update();
                        Walking = true;
                    }
                }
                #endregion
                //===============================================================================
            }
            //===================================================================================
            //IDLE ANIMATION
            #region IDLE
            if (Walking == false && Atacar == false)
            {
                if(rightSide == true)
                {
                    animacoes.SetSpriteAnimation(8, 0, 8, 1, 10);             animacoes.Update();
                    Idle = true;
                }
                if (botSide == true)
                {
                    animacoes.SetSpriteAnimation(5, 0, 5, 1, 10);             animacoes.Update();
                    Idle = true;
                }
                if (leftSide == true)
                {
                    animacoes.SetSpriteAnimation(11, 0, 11, 1, 10);           animacoes.Update();
                    Idle = true;
                }
                if (topSide == true)
                {
                    animacoes.SetSpriteAnimation(2, 0, 2, 1, 10);             animacoes.Update();
                    Idle = true;
                }
            }
            #endregion
            //ATTACK ANIMATION
            #region ATTACK
            //___________________________________________________________________________________
            if(Atacar == true)
            {
                if (rotation > -0.75 && rotation <= 0.75)
                {
                    animacoes.SetSpriteAnimation(7, 0, 7, 3, 10);             animacoes.Update();
                    Atacar = true;
                }
                if (rotation > 0.75 && rotation <= 2.25)
                {
                    animacoes.SetSpriteAnimation(4, 0, 4, 3, 10);             animacoes.Update();
                    Atacar = true;
                }
                if ((rotation > 2.25 && rotation < 3.2) ||(rotation <= -2.25 && rotation > -3.2))
                {
                    animacoes.SetSpriteAnimation(10, 0, 10, 3, 10);           animacoes.Update();
                    Atacar = true;
                }
                if (rotation > -2.25 && rotation <= -0.75)
                {
                    animacoes.SetSpriteAnimation(1, 0, 1, 3, 10);             animacoes.Update();
                    Atacar = true;
                }
            }
            #endregion
            //DEAD
            #region
            if (Isdead == true)
            {
                if (animacoes.mCurrentColumn != 3)
                {
                    animacoes.SetSpriteAnimation(4, 0, 4, 3, 0);              animacoes.Update();
                }
            }
            #endregion
            //===================================================================================
        }

        public void ChecaColisaoComProjeteis()
        {
            //Verifica se morreu
            bool colisao = false, colisao2 = false; ; int imunidade = 10;
            foreach (Projectile p in Game1.Jogador.PlayerRuna.iceSpikes)
            {
                colisao = Colider.Intersects(p.mColider);
                if (colisao == true && p.IsVisible == true)
                {
                    break;
                }
            }
            foreach (Projectile p in Game1.Jogador.PlayerRuna.fireBalls)
            {
                colisao2 = Colider.Intersects(p.mColider);
                if (colisao2 == true && p.IsVisible == true)
                {
                    break;
                }
            }
            if (colisao == true && imunidade == 10 || colisao2 == true && imunidade == 10)
            {
                Health -= 40;
                imunidade = 0;
            }

            if (imunidade == 0)
            {
                imunidade++;
            }

            if (imunidade >= 10)
            {
                imunidade = 10;
            }
        }

        public void ChecaColisao()
        {
            bool colisao = false; int imunidade = 10;
            colisao = Colider.Intersects(Game1.Jogador.f1.Colider);
      
            if (colisao == true && imunidade == 10)
            {
                Health -= 40;
                imunidade = 0;
            }

            if (imunidade == 0)
            {
                imunidade++;
            }

            if (imunidade >= 10)
            {
                imunidade = 10;
            }
     
        }



        //=======================================================================================
        public void UpdateDead(GameTime gameTime)                           //vai mover o inimigo
        {
            Position.X = Position.X;
            Position.Y = Position.Y;

        }
        //=======================================================================================
        public void Draw(SpriteBatch spriteBatch)
        {
            //CREIEI MAIS UM RECTANGULO
            //Rectangulo Sombra
            recSombra = new Rectangle((int)Position.X - 15, (int)Position.Y + 65, sombra.mSpriteWidth, sombra.mSpriteHeight);
            //Retangulo da sprite           
            Colider = new Rectangle((int)Position.X - 75, (int)Position.Y - 68, animacoes.mSpriteWidth, animacoes.mSpriteHeight);
            Rectangle desenho = new Rectangle((int)Position.X, (int)Position.Y, animacoes.mSpriteWidth, animacoes.mSpriteHeight);
            //DRAW===============================================================================
            sombra.Draw(recSombra);                                                      //SOMBRA
            animacoes.Draw(desenho);                                                    //INIMIGO
            //spriteBatch.Draw(Texture, Position, Color.White);
            //spriteBatch.Draw(animacoes.image, Colider, Color.Chocolate);
            //===================================================================================
            RangedRuna.Draw();
        }
        //=======================================================================================
        #region Tipos_De_Inimigos
        //_______________________________________________________________________________________
        public Inimigo Melee(Vector2 PosicaoIni)
        {
            Inimigo este = new Inimigo();
            este.animacoes = new SpritePrimitive("P.E.MELEE-SpriteSheet", PosicaoIni, new Vector2(160, 160), 4, 12, 0);
            este.sombra    = new SpritePrimitive("MISC-SpriteSheet"     , PosicaoIni, new Vector2(160, 160), 4, 8,  0);
            este.MaxHealth = 40;
            este.Health = 100;
            este.Attack = 20;
            este.eT = EnemyType.Melee;
            este.eF = EnemyFaction.Evil;
            este.Position = PosicaoIni;
            este.Isdead = false;
            Mruna = null;
            return este;
        }
        public Inimigo Melee2(Vector2 PosicaoIni)
        {
            Inimigo este = new Inimigo();
            este.animacoes = new SpritePrimitive("P.E.MELEE-SpriteSheet", PosicaoIni, new Vector2(160, 160), 4, 12, 0);
            este.sombra    = new SpritePrimitive("MISC-SpriteSheet"     , PosicaoIni, new Vector2(160, 160), 4, 8,  0);
            este.MaxHealth = 40;
            este.Health = 100;
            este.Attack = 20;
            este.eT = EnemyType.Melee;
            este.eF = EnemyFaction.Evil;
            este.Position = PosicaoIni;
            este.Isdead = false;
            Mruna = null;
            return este;
        }
        public Inimigo Ranged(Vector2 PosicaoIni)
        {
            Inimigo este = new Inimigo();
            este.animacoes = new SpritePrimitive("P.E.RANGED-SpriteSheet", PosicaoIni, new Vector2(160, 160), 4, 12, 0);
            este.sombra    = new SpritePrimitive("MISC-SpriteSheet"      , PosicaoIni, new Vector2(160, 160), 4, 8,  0);
            este.MaxHealth = 40;
            este.Health = 100;
            este.Attack = 20;
            este.eT = EnemyType.Ranged;
            este.eF = EnemyFaction.Evil;
            este.Position = PosicaoIni;
            este.Isdead = false;
            return este;
        }
        #endregion
        //=======================================================================================
        public Vector2 CalculaDistancia(Player p)
        {
            Vector2 vec;
            vec.X = p.Position.X - this.Position.X;
            vec.Y = p.Position.Y - this.Position.Y;


            return vec;
        }
        //=======================================================================================
    }
}

