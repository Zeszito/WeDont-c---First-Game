using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Inimigo
{
    public class MeleeEnemy : Inimigo
    {
        
        public MeleeEnemy(string name,
          Point position, int health,int maxHealth, float speed, EnemyType type, EnemyFaction faction) 
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
        public void NeutralMelee(Point position)
        {
            string name = "NeutralMelee";
            Point location = position;
            Texture2D nmelee = Game1.scontent.Load<Texture2D>(name);
            int health = 10;
            int maxHealth = 10;
            float speed = 50;
            MeleeEnemy.EnemyType type = MeleeEnemy.EnemyType.melee;
            MeleeEnemy.EnemyFaction faction = MeleeEnemy.EnemyFaction.neutral;

            MeleeEnemy meleeNeutral = new MeleeEnemy(name, position, health,
               maxHealth, speed, type, faction);
        }
        /*HOLY ENEMY*/
        public void HolyMelee(Point position)
        {
            string name = "HolyMelee";
            Point location = position;
            Texture2D hmelee = Game1.scontent.Load<Texture2D>(name);
            int health = 10;
            int maxHealth = 10;
            float speed = 50;
            MeleeEnemy.EnemyType type = MeleeEnemy.EnemyType.melee;
            MeleeEnemy.EnemyFaction faction = MeleeEnemy.EnemyFaction.holy;

            MeleeEnemy meleeHoly = new MeleeEnemy(name, position, health,
               maxHealth, speed, type, faction);
        }
        /*EVIL ENEMY*/
        public void EvilMelee(Point position)
        {
            string name = "EvilMelee";
            Point location = position;
            Texture2D emelee = Game1.scontent.Load<Texture2D>(name);
            int health = 10;
            int maxHealth = 10;
            float speed = 50;
            MeleeEnemy.EnemyType type = MeleeEnemy.EnemyType.melee;
            MeleeEnemy.EnemyFaction faction = MeleeEnemy.EnemyFaction.evil;

            MeleeEnemy meleeHoly = new MeleeEnemy(name, position, health,
               maxHealth, speed, type, faction);
        }












        public void Update()
        {

        }
        public void Draw()
        {

        }
    }
}
