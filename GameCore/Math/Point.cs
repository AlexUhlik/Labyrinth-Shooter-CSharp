using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public struct Point
    {
        public float X {  get; set; } 
        public float Y { get; set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator +(Point a, float number)
        {
            return new Point(a.X + number, a.Y + number);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator -(Point a, float number)
        {
            return new Point(a.X - number, a.Y - number);
        }
    }
}
