using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Inimigo
{
    public class Inimigo
    {
        public string Name { get; set; }
        public EnemyType enemyType { get; set; }
        public EnemyFaction enemyFaction { get; set; }
        public Texture2D mSprite { get; set; }
        public Texture2D mSpriteProj { get; set; }
        public Point mPosition { get; set; }
        public int mHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public float Speed { get; set; }

        public Inimigo(string name, Point position, int health,int maxHealth, float speed)
        {
            Name = name;
            mSprite = Game1.scontent.Load<Texture2D>(name);
            mPosition = position;
            mHealth = health;
            Speed = speed;
            MaxHealth = maxHealth;
        }


        public enum EnemyType
        {
            melee,
            ranged,
            boss
        }
        public enum EnemyFaction
        {
            evil,
            holy,
            neutral
        }

        public void Draw()
        {

        }
    }
}
