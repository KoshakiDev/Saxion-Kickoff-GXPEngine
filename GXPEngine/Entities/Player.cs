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
        public int health;
        public event Action<int> UpdateHealth;
        private Timer shot_delay_timer;
        //private Pivot bullet_spawn_position = new Pivot();

        int basic_shot_delay = 200;
        int upgraded_shot_delay = 100;

        bool immune = false;
        private Timer immunityTimer;
        int immunityDuration = 1000;
        public bool IsGunUpgraded = false;

        public Player() : base("sprites/hitbox.png", 1, 1, 1)
        {
            move_speed = 0.1f;
            rotation_speed = 1.5f;
            shot_delay_timer = new Timer(basic_shot_delay, true, false);

            IsGunUpgraded = true;
            
            SetOrigin(width / 2, height / 2);
            health = 1;

        }

        public override void Update()
        {
            base.Update();
            UpdateVelocity();
            UpdateInformation();
        }
        void UpdateInformation()
        {
            if (immunityTimer == null || immunityTimer.finished)
            {
                immune = false;
                alpha = 1;

            }
            else
            {
                immune = true;
                alpha = Utils.Random(0.4f, 1);
            }
        }
        void UpdateVelocity() // controls of the player
        {
            if(health <= 0)
            {
                return;
            }

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
                if (!IsGunUpgraded)
                    BasicGun();
                else
                    UpgradedGun();
            }
        }

        void BasicGun()
        {
            Bullet new_bullet_1 = new Bullet(rotation,
                    x + 10 * Mathf.Cos(Mathf.DegreesToRadians(rotation)),
                    y + 10 * Mathf.Sin(Mathf.DegreesToRadians(rotation)));
            parent.AddChild(new_bullet_1);

            shot_delay_timer = new Timer(basic_shot_delay, false, false);

        }
        void UpgradedGun()
        {
            Bullet new_bullet_1 = new Bullet(rotation,
                    x + 15 * Mathf.Cos(Mathf.DegreesToRadians(rotation + 90)),
                    y + 15 * Mathf.Sin(Mathf.DegreesToRadians(rotation + 90)));
            parent.AddChild(new_bullet_1);

            Bullet new_bullet_2 = new Bullet(rotation,
                    x + 15 * Mathf.Cos(Mathf.DegreesToRadians(rotation - 90)),
                    y + 15 * Mathf.Sin(Mathf.DegreesToRadians(rotation - 90)));
            parent.AddChild(new_bullet_2);

            shot_delay_timer = new Timer(upgraded_shot_delay, false, false);
        }


        public override void OnCollision(GameObject collider)
        {
            if (collider is Asteroid)
            {
                Damage(1);
                //Hit();
            }
        }

        public void Heal(int amount)
        {
            health += amount;
            UpdateHealth?.Invoke(health);
        }
        public void Damage(int amount)
        {
            if (immune)
                return;

            health -= amount;
            //PlayPassedSound(HurtSound);

            //ApplyExternalForce(CollisionInfo.normal, 0.5f);

            immunityTimer = new Timer(immunityDuration);
            immune = true;
            if (health == 0)
            {
                Hit();
            }
            UpdateHealth?.Invoke(health);
        }
    }
}

