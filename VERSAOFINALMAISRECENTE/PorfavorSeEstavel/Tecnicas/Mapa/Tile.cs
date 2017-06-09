using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Mapa
{
    public class Tile
    {
        public Vector2 position;
        public Texture2D sprite;
        public Rectangle Colider;
        public Rectangle Ori;
        Random random = new Random();
        public bool existe = true;
        public bool door = false;
        public int tipo;
        private SpritePrimitive tile;

        Texture2D whiteRectangle;


        public Tile(Vector2 position)
        {
            tile = new SpritePrimitive("tilesetComColunas", position, new Vector2(320, 320), 5, 5, 0);

            this.position = position;
        }

        public bool ChecaColisao(Rectangle r1, Rectangle r2)
        {
            if (r1.Intersects(r2))
            {
                return true;
            }

            return false;
        }

        public void Update()
        {
            if (tipo == 0) //canto superior esquerdo
            {
                tile.SetSpriteAnimation(0, 0, 0, 0, 0);
            }
            if (tipo == 1) //canto inferior direito
            {
                tile.SetSpriteAnimation(2,2,2,2,0);

            }
            if (tipo == 2) //canto superior direito
            {
                tile.SetSpriteAnimation(0,2,0,2,0);

            }
            if (tipo == 3) //canto inferior esquerdo
            {
                tile.SetSpriteAnimation(2,0,2,0,0);

            }
            if (tipo == 4) //vertical esqueda
            {
                tile.SetSpriteAnimation(1,0,1,0,0);

            }
            if (tipo == 5) //vertical direita
            {
                tile.SetSpriteAnimation(1,2,1,2,0);

            }
            if (tipo == 6) //horizontal cima
            {
                tile.SetSpriteAnimation(0,1,0,1,0);

            }
            if (tipo == 7) //horizontal baixo
            {
                tile.SetSpriteAnimation(2,1,2,1,0);

            }
        }

        public void LoadImage()
        {
            sprite = Game1.sContent.Load<Texture2D>("bloco");
            whiteRectangle = new Texture2D(Game1.spriteBatch.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }


        virtual public void Draw()
        {
            Vector2 centro = new Vector2(sprite.Width / 2, sprite.Height / 2);
            //Retangulo da sprite
            Ori = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Colider = Ori;
            Colider.Y = (int)position.Y - (int)centro.Y;
            Colider.Height = sprite.Height;
            Colider.X = (int)position.X - (int)centro.X;
            Colider.Width = sprite.Width;
            tile.Draw(Ori);
            if (existe == true)
                Game1.spriteBatch.Draw(sprite, position, Ori, Color.White, 0, centro, 1.0f, SpriteEffects.None, 1);
            //Game1.spriteBatch.Draw(sprite, Colider, Color.Chocolate);
        }
    }
}
