using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D_Game_Musical_Theme
{
    public class CollisionManager
    {

        //List of classes inheriting from Collidables
        private List<Collidable> m_Collidables = new List<Collidable>(); 

        //Hashset to collect all collisions in a single update
        private HashSet<Collision> m_Collisions = new HashSet<Collision>(new CollisionComparer()); 

        public void AddCollidable(Collidable c)
        {
            m_Collidables.Add(c);
        }

        public void RemoveCollidable(Collidable c)
        {
            m_Collidables.Remove(c);
        }

        public void Update()
        {
            UpdateCollisions();
            ResolveCollisions();
        }

        //Check Collisions hashset
     
        private void UpdateCollisions()
        {
            if (m_Collisions.Count > 0) m_Collisions.Clear();

            //Iterate thgouh collidable objects to check for collisions
            for(int i =0; i<m_Collidables.Count; i++)
            {
                for (int j = 0; j < m_Collidables.Count; j++)
                {
                    Collidable collidable1 = m_Collidables[i];
                    Collidable collidable2 = m_Collidables[j];

                    //Ensure we are not checking object with itself
                    if (!collidable1.Equals(collidable2))
                    {
                        //if two objects colliding then add them to the set
                        if (collidable1.CollisionTest(collidable2))
                        {
                            m_Collisions.Add(new Collision(collidable1, collidable2));
                        }
                    }
                }
            }
        }

        private void ResolveCollisions()
        {
            foreach (Collision c in m_Collisions)
            {
                c.Resolve();
            }
        }

    }
}
