using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Mapa
{
    public class MapGenerator
    {
        public Texture2D zombie;
        public int tamanhosprite;
        public Chao[,] chaoTiles;
        public int tamanhoMapaX;
        public int tamanhoMapaY;





        public void Load()
        {
            zombie = Game1.scontent.Load<Texture2D>("Zombi");
        }
        public void Initializer()
        {
            tamanhosprite = 64;
            tamanhoMapaX = 50;
            tamanhoMapaY = 50;
            Chao[,] chaoTiles = new Chao[tamanhoMapaX, tamanhoMapaY];
            EncherArray(chaoTiles, tamanhoMapaX, tamanhoMapaY);
        }

        public void Update()
        {

        }
        public void Draw()
        {
            for (int a = 0; a<tamanhoMapaX; a++)
            {
                for (int b=0 ;b<tamanhoMapaY; b++)
                {
                    chaoTiles[a, b].Draw();
                }
            }
        }

        public void EncherArray(Chao[,] chaoTiles, int x, int y)
        {
            chaoTiles = new Chao[x, y];
            for(int a = 0; a<x;a++)
            {
                for(int b = 0; b<y; b++)
                {
                    chaoTiles[a, b].texture = zombie;
                    chaoTiles[a, b].posicao = new Vector2(a * 64, b * 64);
                }
            }


        }
    }
}
