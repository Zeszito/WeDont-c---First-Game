using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Inimigo
{
    public class RangedEnemy : Inimigo
    {
        public RangedEnemy(string name,
          Point position, int health, int maxHealth, float speed, EnemyType type, EnemyFaction faction) 
            : base(name,position, health,maxHealth, speed)
        {
            name = "";
            mSprite = Game1.scontent.Load<Texture2D>(name);
            enemyType = type;
            enemyFaction = faction;
            mSprite = Game1.scontent.Load<Texture2D>(name);
            mPosition = position;
            mHealth = health;
            MaxHealth = maxHealth;
            speed = 50;
        }
        /*NEUTRAL ENEMY*/
        public void NeutralRanged(Point position)
        {
            string name = "NeutralRanged";
            Point location = position;
            Texture2D ranged = Game1.scontent.Load<Texture2D>(name);
            Texture2D nproj = Game1.scontent.Load<Texture2D>("nproj");
            int health = 10;
            int maxHealth = 100;
            float speed = 50;
            RangedEnemy.EnemyType type = RangedEnemy.EnemyType.ranged;
            RangedEnemy.EnemyFaction faction = RangedEnemy.EnemyFaction.neutral;
        }


    }
}
