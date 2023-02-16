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
        public event Action<int> UpdateHealth;
        private Timer shot_delay_timer;
        private Timer gun_upgrade_timer;
        int gun_upgrade_duration = 8000;
        
        public int gun_upgrade_level = 1;
        public int last_gun_upgrade_level = 1;

        int shot_delay_1 = 200;
        int shot_delay_2 = 100;
        int shot_delay_3 = 50;

        bool immune = false;
        private Timer immunity_timer;
        int immunityDuration = 1000;



        public Player() : base("sprites/hitbox.png", 1, 1, 1)
        {
            move_speed = 0.1f;
            rotation_speed = 2.0f;
            shot_delay_timer = new Timer(shot_delay_1, true, false);

            
            SetOrigin(width / 2, height / 2);
            health = 3;

        }

        public override void Update()
        {
            base.Update();
            UpdateVelocity();
            UpdateInformation();
        }
        void UpdateInformation()
        {
            if (gun_upgrade_timer == null || gun_upgrade_timer.finished)
            {
                gun_upgrade_level = 1;
            }
            

            if (immunity_timer == null || immunity_timer.finished)
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
            //Console.WriteLine("rotation: " + rotation);
            if (Input.GetKey(Key.W) || Input.GetKey(Key.UP))
            {
                velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(rotation));
                velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(rotation));
            }

            if (Input.GetKey(Key.A) || Input.GetKey(Key.LEFT)) 
            {
                
                rotation += rotation_speed;
                if (rotation < 0)
                {
                    rotation = 360;
                }
                if (rotation > 360)
                {
                    rotation = 0;
                }
            }

            if (Input.GetKey(Key.D) || Input.GetKey(Key.RIGHT))
            {
                
                rotation -= rotation_speed;
                if (rotation < 0)
                {
                    rotation = 360;
                }
                if (rotation > 360)
                {
                    rotation = 0;
                }
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
                switch(gun_upgrade_level)
                {
                    case 1:
                        GunLevel1();
                        break;
                    case 2:
                        GunLevel2();
                        break;
                    case 3:
                        GunLevel3();
                        break;

                }
            }
        }

        public void UpgradeGun()
        {
            gun_upgrade_timer = new Timer(gun_upgrade_duration, false, false);
            gun_upgrade_level = (int)Mathf.Clamp(last_gun_upgrade_level + 1, 1, 3);
            last_gun_upgrade_level = gun_upgrade_level;

        }

        void GunLevel1()
        {
            SpawnBullet(new Vector2(15, 15), 0);
            shot_delay_timer = new Timer(shot_delay_1, false, false);
        }
        void GunLevel2()
        {
            SpawnBullet(new Vector2(15, 15), 90);
            SpawnBullet(new Vector2(15, 15), -90);
            shot_delay_timer = new Timer(shot_delay_2, false, false);
        }
        void GunLevel3()
        {
            SpawnBullet(new Vector2(15, 15), 0);
            SpawnBullet(new Vector2(15, 15), 90);
            SpawnBullet(new Vector2(15, 15), -90);
            shot_delay_timer = new Timer(shot_delay_3, false, false);
        }

        void SpawnBullet(Vector2 position, int rotation_offset)
        {
            Bullet new_bullet = new Bullet(rotation,
                    x + position.x * Mathf.Cos(Mathf.DegreesToRadians(rotation + rotation_offset)),
                    y + position.y * Mathf.Sin(Mathf.DegreesToRadians(rotation + rotation_offset)));
            parent.AddChild(new_bullet);

        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Asteroid)
            {
                Damage(1);
                //Hit();
            }
        }

        public override void Heal(int amount)
        {
            base.Heal(amount);
            UpdateHealth?.Invoke(health);
        }
        public override void Damage(int amount)
        {
            if (immune)
                return;
            health -= amount;
            immunity_timer = new Timer(immunityDuration);
            immune = true;
            if (health <= 0)
            {
                Death();
            }
            UpdateHealth?.Invoke(health);
        }
    }
}

