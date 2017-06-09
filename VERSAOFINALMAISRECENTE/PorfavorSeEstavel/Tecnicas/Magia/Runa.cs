using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Magia
{
    //Se cada speel tem uma runa associada nem preciso de fazer esta class
    public enum tipo { Ice, Fire, Teleport, Nenhuma}
    public class Runa
    {
        public List<Projectile> iceSpikes;
        public List<Projectile> fireBalls;
        public bool NoPlayer = false;
        tipo spellType;

        public Runa()
        {
            iceSpikes = new List<Projectile>();
            fireBalls = new List<Projectile>();
            spellType = tipo.Nenhuma;
        }

        public Runa(tipo t)
        {
            iceSpikes = new List<Projectile>();
            fireBalls = new List<Projectile>();
            spellType = t;
        }


        public void Ativacao(Vector2 position, Vector2 diracao, float rotacao, bool e)
        {
            if (spellType == tipo.Ice)
                DisparaGelo(position, diracao, rotacao, e);
            else
                if (spellType == tipo.Fire)
                    DisparaFogo(position, diracao, rotacao, e);
            else
                if (spellType == tipo.Teleport)
                    UsaTeletransport();
        }

        void DisparaGelo(Vector2 position, Vector2 diracao, float rotacao, bool e)
        {
            bool criarnova = true;
            foreach (Projectile m in iceSpikes)
            {
                if (m.IsVisible == false)
                {
                    criarnova = false;
                    //m.Spell(50, false, 500, new Vector2(100f, 100f), "hadouken",new Vector2(1,0));
                    m.Proj(tipo.Ice, position, diracao, rotacao, e);
                    break;
                }
            }

            if (criarnova == true)
            {
                Projectile S = new Projectile();
                S.LoadContent(tipo.Ice);
                //S.Spell(50, false, 500, new Vector2(100f, 100f), "hadouken",new Vector2(1, 0));
                S.Proj(tipo.Ice, position, diracao, rotacao, e);
                iceSpikes.Add(S);
            }
        }

        void DisparaFogo(Vector2 position, Vector2 diracao, float rotacao, bool e)
        {
            bool criarnova = true;
            foreach (Projectile m in fireBalls)
            {
                if (m.IsVisible == false)
                {
                    criarnova = false;
                    //m.Spell(50, false, 500, new Vector2(100f, 100f), "hadouken",new Vector2(1,0));
                    m.Proj(tipo.Fire, position, diracao, rotacao, e);
                    break;
                }

            }
            if (criarnova == true)
            {
                Projectile S = new Projectile();
                S.LoadContent(tipo.Fire);
                //S.Spell(50, false, 500, new Vector2(100f, 100f), "hadouken",new Vector2(1, 0));
                S.Proj(tipo.Fire, position, diracao, rotacao, e);
                fireBalls.Add(S);
            }
        }

        void UsaTeletransport()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (Projectile p in fireBalls)
            {
                p.Update(gameTime);
            }
            foreach (Projectile p in iceSpikes)
            {
                p.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach(Projectile p in fireBalls)
            {
                p.Draw();
            }
            foreach (Projectile p in iceSpikes)
            {
                p.Draw();
            }
        }

    }


}
