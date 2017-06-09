using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Animations
{
    class AfterImages
    {
        Texture2D mSprite;
        Rectangle Colider;
        Vector2 position;
        bool Ativa;
        float alpha;
        int Lado;

        public AfterImages()
        {

        }

        public AfterImages(Vector2 posicao, int l)
        {
            position = posicao;
            alpha = 1.00f;
            Ativa = true;
            Lado = l;
            if (Lado == 2)
            {
                mSprite = Game1.sContent.Load<Texture2D>("Dash/TopPersonagem");
            }
            if (Lado == 1)
            {
                mSprite = Game1.sContent.Load<Texture2D>("Dash/LeftPersonagem");
            }
            if (Lado == 0)
            {
                mSprite = Game1.sContent.Load<Texture2D>("Dash/BotPersonageml");
            }
            if (Lado == 3)
            {
                mSprite = Game1.sContent.Load<Texture2D>("Dash/RightPersonagem");
            }

        }

        public void LoadImage()
        {
            if (Lado == 0)
            {
                mSprite = Game1.sContent.Load<Texture2D>("TopPersonageml");
            }
            if (Lado == 1)
            {
                mSprite = Game1.sContent.Load<Texture2D>("LeftPersonagem");
            }
            if (Lado == 2)
            {
                mSprite = Game1.sContent.Load<Texture2D>("BotPersonagem");
            }
            if (Lado == 3)
            {
                mSprite = Game1.sContent.Load<Texture2D>("RightPersonagem");
            }
        }

        public void FadeAway()
        {
            if (Ativa == true)
            {
                alpha -= 0.1f;


                //if (alpha <= 0)
                //{
                //    alpha = 1;
                //    Ativa = false;
                //}
            }
        }


        public void Draw()
        {
            Game1.spriteBatch.Draw(mSprite, position, Color.White * alpha);
        }
    }
}
