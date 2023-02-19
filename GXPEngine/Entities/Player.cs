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

        Pivot gun_pivot;
        Sprite gun;

        float friction = 0.0005f;

        int shot_delay_1 = 300;
        int shot_delay_2 = 100;
        int shot_delay_3 = 50;

        bool immune = false;
        private Timer immunity_timer;
        int immunityDuration = 1000;

        Sound gunshot = new Sound("sounds/gunshot.wav");
        Sound health_bonus = new Sound("sounds/health_bonus.wav");
        Sound gun_upgrade = new Sound("sounds/gun_upgrade.wav");
        Sound player_damage = new Sound("sounds/player_damage.wav");

        public Player() : base("sprites/hitbox.png", 1, 1, 1)
        {
            

            move_speed = 0.1f;
            rotation_speed = 2.0f;
            shot_delay_timer = new Timer(shot_delay_1, true, false);

            sprite = new AnimationSprite("sprites/Player-SpriteSheet-Idle.png", 2, 2, 4, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.scaleX = sprite.scaleY = 0.1f;
            AddChild(sprite);
            
            sprite.SetCycle(0, 4, _animationDelay);

            SetOrigin(width / 2, height / 2);

            gun_pivot = new Pivot();
            gun = new AnimationSprite("sprites/gun.png", 1, 1, 1, false, false);
            gun.SetOrigin(0, gun.height / 2);
            gun.scaleX = gun.scaleY = 0.05f;
            gun_pivot.AddChild(gun);


            AddChild(gun_pivot);

            max_health = 3;
            health = max_health;
        }

        public override void Update()
        {
            base.Update();
            UpdateVelocity();
            ApplyFriction();
            UpdateInformation();
            AnimateSpriteModel();
        }
        void ApplyFriction()
        {
            //Console.WriteLine(velocity);
            if(velocity.x > 0)
            {
                velocity.x -= friction;
            }
            else if (velocity.x < 0)
            {
                velocity.x += friction;
            }
            if (velocity.y > 0)
            {
                velocity.y -= friction;
            }
            else if (velocity.y < 0)
            {
                velocity.y += friction;
            }
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
                sprite.alpha = 1;

            }
            else
            {
                immune = true;
                sprite.alpha = Utils.Random(0.4f, 1);
            }
        }
        void UpdateVelocity() // controls of the player
        {
            gun_pivot.rotation = angle_of_movement;
            if(health <= 0)
            {
                return;
            }
            //Console.WriteLine("rotation: " + rotation);
            if (Input.GetKey(Key.W) || Input.GetKey(Key.UP))
            {
                velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(angle_of_movement));
                velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(angle_of_movement));
            }

            if (Input.GetKey(Key.A) || Input.GetKey(Key.LEFT)) 
            {
                angle_of_movement += rotation_speed;
            }

            if (Input.GetKey(Key.D) || Input.GetKey(Key.RIGHT))
            {

                angle_of_movement -= rotation_speed;
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
                gunshot.Play();
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
            gun_upgrade.Play();
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
            Bullet new_bullet = new Bullet(angle_of_movement,
                    x + position.x * Mathf.Cos(Mathf.DegreesToRadians(angle_of_movement + rotation_offset)),
                    y + position.y * Mathf.Sin(Mathf.DegreesToRadians(angle_of_movement + rotation_offset)));
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
            health_bonus.Play();
            base.Heal(amount);
            UpdateHealth?.Invoke(health);
        }
        public override void Damage(int amount)
        {
            if (immune)
                return;
            player_damage.Play();
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

