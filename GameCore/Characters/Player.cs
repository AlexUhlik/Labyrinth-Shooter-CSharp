using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameCore.Bullets;

namespace GameCore.Characters
{   
    public class Player : Unit
    {
        public const int MaxHealth = 100;
        public int Ammunition {  get; set; }
        public int Score { get; set; }

        public IBullet CurrentBullet { get; set; }

        public Player(float x, float y) : base(x, y, 50)
        {
            Health = MaxHealth;
            Armor = 50;
            Speed = 4.0f;
            Ammunition = 40;
            Score = 0;

            CurrentBullet = new StandartBullet();
        }
        // test
        public override void Draw()
        {
            
        }
    }
}
