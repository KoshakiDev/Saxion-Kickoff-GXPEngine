using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine.Entities;
using GXPEngine.Core;

namespace GXPEngine.Entities
{
    public class Player : Entity
    {
        private Timer shot_delay_timer;
        public Player() : base("sprites/hitbox.png", 1, 1, 1)
        {
            move_speed = 0.1f;
            rotation_speed = 1.5f;
            shot_delay_timer = new Timer(300, true, false);

            SetOrigin(width / 2, height / 2);

        }

        public override void Update()
        {
            base.Update();
            UpdateVelocity();   
        }
        void UpdateVelocity() // controls of the player
        {

            if (Input.GetKey(Key.W) || Input.GetKey(Key.UP))
            {
                velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(rotation));
                velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(rotation));
            }

            if (Input.GetKey(Key.A) || Input.GetKey(Key.LEFT)) 
            {
                rotation += rotation_speed;
            }

            if (Input.GetKey(Key.D) || Input.GetKey(Key.RIGHT))
            {
                rotation -= rotation_speed;
            }
            if (Input.GetKey(Key.SPACE)) // d
            {
                Shoot();
            }

        }

        public void Shoot()
        {
            if(shot_delay_timer.IsPaused)
            {
                Bullet new_bullet = new Bullet(rotation, 
                        x + 10 * Mathf.Cos(Mathf.DegreesToRadians(rotation)), 
                        y + 10 * Mathf.Sin(Mathf.DegreesToRadians(rotation)));
                parent.AddChild(new_bullet);
                shot_delay_timer = new Timer(200, false, false);
            }
        }


        public override void OnCollision(GameObject collider)
        {
            if (collider is Asteroid)
            {
                Hit();
            }
        }
    }
}

