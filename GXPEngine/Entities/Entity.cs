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
        public float DEFAULT_SCALE = 0.5f;

        protected float move_speed;
        protected float rotation_speed;

        public int health;
        public int max_health;
        public AnimationSprite sprite;
        public bool mirrored = false;
        public float angle_of_movement = 0;

        public Entity(
                string filePath,
                int columns,
                int rows,
                int frames,
                bool addCollider = true) : base(filePath, columns, rows, frames, true, addCollider)
        {
            _animationDelay = 100;
            alpha = 0;
            //scaleX = scaleY = DEFAULT_SCALE;
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
            health = (int)Mathf.Clamp(health + amount, 0, max_health);
        }

        public virtual void Update() 
        {
            AdjustAngleOfMovement();
            AnimateSpriteModel();
            Move();
            WrapAround();
        }

        void AdjustAngleOfMovement()
        {
            angle_of_movement = angle_of_movement % 360;
            if (angle_of_movement < 0)
            {
                angle_of_movement += 360;
            }
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
            if((angle_of_movement >= 0 && angle_of_movement <= 90) || 
               (angle_of_movement >= 270 && angle_of_movement <= 360) )
            {
                mirrored = false;
            }
            else if (angle_of_movement >= 90 && angle_of_movement <= 270)
            {
                mirrored = true;
            }
        }

        public void AnimateSpriteModel()
        {
            sprite.Animate(Time.deltaTime);

            if (mirrored)
            {
                sprite.Mirror(true, false);
            }
            else
            {
                sprite.Mirror(false, false);
            }
        }

        public virtual void OnCollision(GameObject collider)
        {
            
        }

        public virtual void Death() 
        { 
            LateDestroy(); 
        }

    }
}
