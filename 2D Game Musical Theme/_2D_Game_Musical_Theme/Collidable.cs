using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2D_Game_Musical_Theme
{
    public class Collidable
    {
        protected Rectangle boundingRectangle = new Rectangle();
        public bool FlaggedForRemoval { get; protected set; }
      
        //Constructor
        public Collidable()
        {
            FlaggedForRemoval = false;
        }

        public Rectangle BoundingRectangle
        {
            get { return boundingRectangle; }
        }

       
        public virtual bool CollisionTest(Collidable obj)
        {
            return false;
        }

        public virtual void OnCollision(Collidable obj)
        {
        }
    }
}
