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
        public int Id { get; }

        private readonly Point _startPos;


        public IBullet CurrentBullet { get; set; }

        public Player(int id, float x, float y) : base(x, y, 50)
        {
            Health = MaxHealth;
            Armor = 50;
            Speed = 4.0f;
            Ammunition = 40;
            Score = 0;
            Id = id;

            _startPos = new Point(x, y);

            CurrentBullet = new StandartBullet();
        }

        public void Respawn()
        {
            Position = _startPos;
            Health = MaxHealth;
            Ammunition = 40;
            Score = Math.Max(0, Score - 500);
        }
        
    }
}
