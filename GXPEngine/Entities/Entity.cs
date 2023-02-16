using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine.Core;

namespace GXPEngine.Entities
{
    public class Entity : AnimationSprite
    {

        /*
        NOTE TO SELF: Rotation uses degrees, Mathf only accepts radians. Convert rotation to radians.
        */
        protected Vector2 velocity;

        protected float move_speed;
        protected float rotation_speed;

        public int health;
        protected Entity(
                string filePath,
                int columns,
                int rows,
                int frames,
                bool addCollider = true) : base(filePath, columns, rows, frames, true, addCollider)
        {
            

        }
        public virtual void Damage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Death();
            }
        }
        public virtual void Heal(int amount)
        {
            health += amount;
        }

        public virtual void Update() 
        {
            Move();
            WrapAround();
        }

        public virtual void WrapAround()
        {
            if(x > game.width)
            {
                x = 0;
            }

            if (x < 0)
            {
                x = game.width;
            }


            if (y > game.height)
            {
                y = 0;
            }
            if (y < 0)
            {
                y = game.height;
            }

        }

        public virtual void Move()
        {
            x += velocity.x * Time.deltaTime;
            y += velocity.y * Time.deltaTime;
            //MoveUntilCollision(0, velocity.y * Time.deltaTime);
            //MoveUntilCollision(velocity.x * Time.deltaTime, 0);
        }

        public virtual void OnCollision(GameObject collider)
        {
            // this checks if the collided object is am object from the background layer
            if (collider.name == "Background")
            {
                return;
            }
            // this checks for collisions with tiles on the foreground
            if (collider is Tiles)
            {
                //    Grounded = true;
                //    fallingvelocity = 0;
            }
        }

        public virtual void Death() 
        { 
            LateDestroy(); 
        }

    }
}
