using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas
{
    class UI
    {
        public static Texture2D _heart;

        static public void LoadContent()
        {
            _heart = Game1.sContent.Load<Texture2D>("BasicHeart");
        }

        static public void UiDraw(SpriteBatch spriteBatch)
        {
            if (Game1.Jogador.vida >= 100)
            {
                spriteBatch.Draw(_heart, Vector2.Zero, Color.White);
                spriteBatch.Draw(_heart, Vector2.Zero + (Vector2.UnitX * _heart.Width), Color.White);
                spriteBatch.Draw(_heart, Vector2.Zero + (Vector2.UnitX * _heart.Width * 2), Color.White);
                spriteBatch.Draw(_heart, Vector2.Zero + (Vector2.UnitX * _heart.Width * 3), Color.White);
            }

            if (Game1.Jogador.vida == 75)
            {
                spriteBatch.Draw(_heart, Vector2.Zero, Color.White);
                spriteBatch.Draw(_heart, Vector2.Zero + (Vector2.UnitX * _heart.Width), Color.White);
                spriteBatch.Draw(_heart, Vector2.Zero + (Vector2.UnitX * _heart.Width * 2), Color.White);
            }

            if (Game1.Jogador.vida == 50)
            {
                spriteBatch.Draw(_heart, Vector2.Zero, Color.White);
                spriteBatch.Draw(_heart, Vector2.Zero + (Vector2.UnitX * _heart.Width), Color.White);

            }
            if (Game1.Jogador.vida == 25)
            {
                spriteBatch.Draw(_heart, Vector2.Zero, Color.White);

            }


        }
    }
}
