using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Mapa
{
    public class Tiles
    {
        public Texture2D texture { get; set; }
        public Vector2 posicao { get; set; }

        //public enum Walkable walkable {get;set;}
        //public enum TypeOfTile tipe {get;set;}




        enum Walkable
        {
            yes,
            no
        }
        enum TypeOfTile
        {
            start,
            end,
            room,
            corridor
        }
    }
}
