using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas
{
    public class PlayerCutScene
    {
        static private Vector2 ponto1 = new Vector2(0, 100);
        static private bool check1;

        static private Vector2 ponto2 = new Vector2(200, 100);
        static private bool check2;

        static private Vector2 ponto3 = new Vector2(300, 200);
        static private bool check3;
        static public void introIn()
        {
            Game1.Jogador.Position = Vector2.Zero;
            check1 = false;
            check2 = false;
            check3 = false;
        }
        static public void introUpdate(GameTime gameTime)
        {
            if (Game1.Jogador.Position == Vector2.Zero && check3 == false)
            {

                FontSupport.Mensagem("Onde estou?");
            }

            if (Game1.Jogador.Position != ponto1 && check1 == false)
            {
                Game1.Jogador.Position = Game1.Jogador.Position + Vector2.UnitY;
                if (Game1.Jogador.Position == ponto1 && check1 == false)
                {
                    check1 = true;

                    FontSupport.Mensagem("Onde o frio encontra o medo.");
                }
            }

            if (Game1.Jogador.Position != ponto2 && check1 == true && check2 == false)
            {
                Game1.Jogador.Position = Game1.Jogador.Position + Vector2.UnitX;
                if (Game1.Jogador.Position == ponto2 && check2 == false)
                {
                    check2 = true;
                    FontSupport.Mensagem("Onde ninguem se atreveu a ir...");
                }
            }


            if (Game1.Jogador.Position != ponto3 && check1 == true && check2 == true && check3 == false)
            {
                Game1.Jogador.Position = Game1.Jogador.Position + Vector2.UnitX;
                Game1.Jogador.Position = Game1.Jogador.Position + Vector2.UnitY;
                if (Game1.Jogador.Position == ponto3 && check3 == false)
                {
                    check3 = true;
                    //FontSupport.Mensagem("Uma faca, uma escolha, uma aventura!");
                    Game1.Jogador.inCutscene = false;
                    FontSupport.Mensagem("");
                }
            }

            /*if RoomBoos{ Game1.Jogador.inCutscene = false,  Game1.Jogador.Position corre e grita }*/
        }

     
    }
}
