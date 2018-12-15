﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2D_Game_Musical_Theme
{
    public class CollisionComparer : IEqualityComparer<Collision>
    {
        public bool Equals(Collision a, Collision b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public int GetHashCode(Collision a)
        {
            return a.GetHashCode();
        }
    }

    public class Collision
    {
        public Collidable A;
        public Collidable B;

        public Collision(Collidable a, Collidable b)
        {
            A = a;
            B = b;
        }

        public bool Equals(Collision other)
        {
            if (other == null) return false;
            if ((this.A.Equals(other.A) && this.B.Equals(other.B))) return true;
            return false;
        }

        public void Resolve()
        {
            this.A.OnCollision(this.B);
        }
    }

}
