using System;

namespace GXPEngine
{
    [Serializable]
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Sprite = new Vector2(64, 64);



        override public string ToString()
        {
            return "[Vector2 " + x + ", " + y + "]";
        }

        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x * x + y * y);
            }
        }
        public Vector2 normalized
        {
            get
            {
                return new Vector2(
                    x != 0 ? x / magnitude : 0,
                    y != 0 ? y / magnitude : 0);
            }
        }

        public Vector2 SetLength(float length) //set length of vector
        {
            return this = normalized * length;
        }

        /// <summary>
        /// Rotates vector towards angle
        /// </summary>
        /// <param name="rotation">Angle of rotation in radians</param>
        /// <returns></returns>
        public Vector2 RotateTowards(float rotation)
        {
            float cosAngle = Mathf.Cos(rotation);
            float sinAngle = Mathf.Sin(rotation);

            return new Vector2(x * cosAngle - y * sinAngle, x * sinAngle + y * cosAngle);
        }
        static Random random = new Random();
        public static Vector2 Random(int range)
        {
            return new Vector2(random.Next(-range, range), random.Next(-range, range));
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2));
        }

        //thx Sep
        public static float Lerp(float current, float target, float speed)
        {
            return current * (1 - speed) * target * speed;
        }

        public static Vector2 DirectionBetween(Vector2 a, Vector2 b)
        {
            return b - a;
        }

        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            Vector2 dirBetween = DirectionBetween(a, b);
            return Mathf.Atan2(dirBetween.x, dirBetween.y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
        public static Vector2 operator *(Vector2 a, float b) => new Vector2(a.x * b, a.y * b);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator -(Vector2 a) => new Vector2(-a.x, -a.y);
        public static Vector2 operator /(Vector2 a, float b) => new Vector2(a.x / b, a.y / b);

        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);

    }

}

