using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Mapa
{
    public class Chao : Tiles
    {
        Texture2D texture = Game1.scontent.Load<Texture2D>("Zombi");
        Vector2 posicao = new Vector2( 1, 1);

        public void Draw()
        {
            Rectangle rec = new Rectangle(0,0,1,1);
            Game1.spriteBatch.Draw(texture, rec, Color.White);
        }
    }
}
