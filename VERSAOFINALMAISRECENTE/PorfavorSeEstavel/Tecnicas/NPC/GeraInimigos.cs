using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.NPC
{
    public class GeraInimigos
    {
        Random r;
        public List<Inimigo> listaInimigos;

        public GeraInimigos()
        {
            listaInimigos = new List<Inimigo>();
        }

        public void CriaMonstros(int quantidade)
        {
            r = new Random();
            int tipoIni = 0;
            while (quantidade > 0)
            {
                tipoIni = r.Next(0, 2);
                if (tipoIni == 0)
                {
                    Inimigo enemy = new Inimigo();
                    enemy = enemy.Melee(Game1.GerMapas.PoeInimigo());
                    listaInimigos.Add(enemy);
                }
                else
                if (tipoIni == 1)
                {
                    Inimigo enemy = new Inimigo();
                    enemy = enemy.Ranged(Game1.GerMapas.PoeInimigo());
                    listaInimigos.Add(enemy);
                }
                quantidade--;
            }

        }


        
    }
}
