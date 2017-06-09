using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnicas.Mapa;
using Tecnicas.NPC;

namespace Tecnicas.Magia
{
    public class Projectile
    {
        public Vector2 mPosicao;
        public Vector2 mDirecao;
        Texture2D mSprite;
        Texture2D whiteRectangle;
        public Rectangle mRec;
        public Rectangle mColider;

        tipo qualSou;
        public bool IsVisible;
        int speed;
        float mRotacao;
        public bool EdoInimigo;

        private const float timer = 1;
        private float countdown = timer;

        public void Proj(tipo t, Vector2 posicao, Vector2 direcao, float rotacao, bool e)
        {
            mPosicao = posicao;
            qualSou = t;
            IsVisible = true;
            mDirecao = direcao;
            mRotacao = rotacao;
            whiteRectangle = new Texture2D(Game1.spriteBatch.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            EdoInimigo = e;
        }

        public void LoadContent(tipo t)
        {
            if (t==tipo.Fire)
            {
                mSprite = Game1.sContent.Load<Texture2D>("fire");
            }
            if (t==tipo.Ice)
            {
                mSprite = Game1.sContent.Load<Texture2D>("ice");
            }
        }

        public void Update(GameTime gameTime)
        {
            if (EdoInimigo == false)
                {
                bool colisao = false, colisao2 = false;
                if (IsVisible == true)
                {
                    foreach (Tile t in Game1.GerMapas.UniversalTiles)
                    {
                        colisao = t.ChecaColisao(t.Colider, mColider);
                        if (colisao == true && t.existe == true)
                        {
                            break;
                        }
                    }
                    foreach (Inimigo ini in Game1.GerInimigos.listaInimigos)
                    {
                        colisao2 = mColider.Intersects(ini.Colider);
                        if (colisao2 == true && ini.Isdead == false)
                        {
                            break;
                        }
                    }
                    if (colisao == false && colisao2 == false)
                    {
                        speed = 20;
                        mPosicao += Vector2.Normalize(mDirecao) * speed;

                        float temptimer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                        countdown -= temptimer;

                        if (countdown < 0)
                        {
                            IsVisible = false;
                            countdown = timer;
                        }
                    }
                    else
                    {
                        IsVisible = false;
                        countdown = 1;
                    }


                }
                if (IsVisible == false)
                {
                    speed = 0;
                    mPosicao += Vector2.Normalize(mDirecao) * speed;
                    mColider.X = 0;
                    mColider.Y = 0;
                }
            }
            else if (EdoInimigo == true)
            {
                bool colisao = false, colisao2 = false;
                if (IsVisible == true)
                {
                    foreach (Tile t in Game1.GerMapas.UniversalTiles)
                    {
                        colisao = t.ChecaColisao(t.Colider, mColider);
                        if (colisao == true && t.existe == true)
                        {
                            break;
                        }
                    }
                    colisao2 = mColider.Intersects(Game1.Jogador.ColiderPlayer);

                    if (colisao2 == true)
                    {
                        Game1.Jogador.vida -= 25;
                        Game1.Jogador.luz = true;
                        Game1.espera = 0;
                    }

                    if (colisao == false && colisao2 == false)
                    {
                        speed = 20;
                        mPosicao += Vector2.Normalize(mDirecao) * speed;

                        float temptimer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                        countdown -= temptimer;

                        if (countdown < 0)
                        {
                            IsVisible = false;
                            countdown = timer;
                        }
                    }
                    else
                    {
                        
                        IsVisible = false;
                        countdown = 1;
                    }


                }
            }
            if (IsVisible == false)
            {
                speed = 0;
                mPosicao += Vector2.Normalize(mDirecao) * speed;
                mColider.X = 0;
                mColider.Y = 0;
            }
        }

        public void Draw()
        {
            if (IsVisible == true)
            {
                mColider = new Rectangle(-35, 30, 70, 50);
                mColider.X += (int)mPosicao.X;
                mColider.Y += (int)mPosicao.Y;
                Rectangle loc = new Rectangle(0,0, mSprite.Width, mSprite.Height);
                Vector2 centro = new Vector2(mSprite.Width / 2, mSprite.Height / 2);
                Game1.spriteBatch.Draw(mSprite, mPosicao, loc, Color.White, mRotacao - (float)Math.PI/2, centro, 1.0f, SpriteEffects.None, 1);
                //Game1.spriteBatch.Draw(whiteRectangle, mColider, Color.Chocolate);
                //Game1.spriteBatch.Draw(mSprite, mPosicao, loc, Color.White, 0f, centro, SpriteEffects.None, 0f);
            }
            //Game1.spriteBatch.Draw(whiteRectangle, mColider, Color.Chocolate);
        }
    }
}

