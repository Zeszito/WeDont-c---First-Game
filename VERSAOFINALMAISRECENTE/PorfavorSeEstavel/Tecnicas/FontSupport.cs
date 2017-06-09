using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas
{
    class FontSupport
    { 

    static private SpriteFont sTheFont = null;
    static private Color sDefaultDrawColor = Color.Black;
    static public Vector2 sStatusLocation = new Vector2(200, 200);
    static public Vector2 CoinsDStatusLocation = new Vector2(300,0);
    static private string msg = "";

    /// <summary>
    /// Loads the font if not already loaded
    /// </summary>
    static private void LoadFont()
    {
        // For demo purposes, loads Arial.spritefont
        if (null == sTheFont)
            sTheFont = Game1.sContent.Load<SpriteFont>("FontUs");
    }

    /// <summary>
    /// Determines if use default color (black) or user specified color
    /// </summary>
    /// <param name="c">user specified color (may be null)</param>
    /// <returns>Color to used for drawing font</returns>
    static private Color ColorToUse(Nullable<Color> c)
    {
        return (null == c) ? sDefaultDrawColor : (Color)c;
    }

    /// <summary>
    /// Draws font at specified location
    /// </summary>
    /// <param name="pos">location to start drawing the font</param>
    /// <param name="msg">message to draw</param>
    /// <param name="drawColor">color to draw in (if null, use black)</param>
    static public void PrintStatusAt(Vector2 pos, String msg, Nullable<Color> drawColor)
    {
        LoadFont();

        Color useColor = ColorToUse(drawColor);
        Game1.spriteBatch.DrawString(sTheFont, msg, Vector2.Zero, Color.Black);
    }

    /*Metodos para alterar*/
    static public void Mensagem(string mensagem) //Receber alguma indicaçao do que deve ser printado, talvez o player deva passar as coisas
    {
        msg = mensagem; //msg = mensagem                   
    }


    /// <summary>
    /// Draws font from upper left corner.
    /// </summary>
    /// <param name="msg">message to draw</param>
    /// <param name="drawColor">color to draw in (if null, use black)</param>
    static public void PrintStatus(Nullable<Color> drawColor)
    {
        LoadFont();
        //Mensagem(msg);
        Color useColor = ColorToUse(drawColor);
        Game1.spriteBatch.DrawString(sTheFont, msg, sStatusLocation, useColor, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
    }

    static public void PrintCoins(Nullable<Color> drawColor)
    {
        LoadFont();

        Color useColor = ColorToUse(drawColor);
        Game1.spriteBatch.DrawString(sTheFont, "ENEMIES SLAYIN:" + Game1.Ninidead.ToString(), CoinsDStatusLocation, useColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}
}
