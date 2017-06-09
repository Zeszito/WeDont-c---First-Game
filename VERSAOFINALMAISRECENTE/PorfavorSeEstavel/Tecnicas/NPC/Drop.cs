using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnicas.Magia;

namespace Tecnicas.NPC
{
    public class Drop
    {
        tipo t;
        Vector2 mPosicao;
        bool IsVisible;
        public Rectangle mColider;
        Texture2D mSprite;

        public Drop(tipo tipe, Vector2 pos)
        {
            t = tipe;
            mPosicao = pos;
            IsVisible = true;
        }

        public void LoadContent()
        {
            if (t == tipo.Fire)
            {
                mSprite = Game1.sContent.Load<Texture2D>("fire");
            }
            if (t == tipo.Ice)
            {
                mSprite = Game1.sContent.Load<Texture2D>("ice");
            }
        }

        public void Update()
        {
            LoadContent();
            bool colidiu = mColider.Intersects(Game1.Jogador.ColiderPlayer);
            if (colidiu == true)
            {
                Runa r = new Runa(t);
                Game1.Jogador.PlayerRuna = r;
                IsVisible = false;
              
            }
        }

        public void Draw()
        {
            if (IsVisible == true)
            {
                mColider = new Rectangle(-35, 30, 70, 50);
                mColider.X += (int)mPosicao.X;
                mColider.Y += (int)mPosicao.Y;
                Rectangle loc = new Rectangle(0, 0, mSprite.Width, mSprite.Height);
                Vector2 centro = new Vector2(mSprite.Width / 2, mSprite.Height / 2);
                Game1.spriteBatch.Draw(mSprite, mPosicao, loc, Color.White, 0f, centro, 1.0f, SpriteEffects.None, 1);
            }
        }
    }
}
