using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine.Entities;
using GXPEngine.Core;

using System.IO.Ports;

namespace GXPEngine.Entities
{
    public class Player : Entity
    {
        public event Action<int> UpdateHealth;
        private Timer shot_delay_timer;
        private Timer gun_upgrade_timer;
        private Timer shield_timer;
        int shield_timer_duration = 8000;
        int gun_upgrade_duration = 8000;
        
        public int gun_upgrade_level = 1;
        public int last_gun_upgrade_level = 1;

        Pivot gun_pivot;
        Sprite gun;

        Shield shield;

        float friction = 0.005f;

        int shot_delay_1 = 300;
        int shot_delay_2 = 100;
        int shot_delay_3 = 50;

        bool immune = false;
        private Timer immunity_timer;
        int immunityDuration = 1000;

        
        Sound health_bonus;
        Sound gun_upgrade;
        Sound player_damage;

        public Player() : base("sprites/player/player_hitbox.png", 1, 1, 1)
        {
            health_bonus = new Sound("sounds/health_bonus.wav");
            gun_upgrade = new Sound("sounds/gun_upgrade.wav");
            player_damage = new Sound("sounds/player_damage.wav");
            move_speed = 0.2f;
            rotation_speed = 4.0f;
            shot_delay_timer = new Timer(shot_delay_1, true, false);

            shield = new Shield();
            
            //AddChild(shield);
            shield.visible = false;
            is_shield_on = false;

            sprite = new AnimationSprite("sprites/player/player1.png", 8, 1, 8, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.scaleX = sprite.scaleY = DEFAULT_SCALE * 0.75f;
            AddChild(sprite);
            
            sprite.SetCycle(0, 4, _animationDelay);

            SetOrigin(width / 2, height / 2);

            gun_pivot = new Pivot();

            gun = new AnimationSprite("sprites/player/gun_1.png", 1, 4, 4, false, false);
            gun.SetOrigin(0, gun.height / 2);

            gun.scaleX = gun.scaleY = DEFAULT_SCALE * 0.75f;
            gun_pivot.AddChild(gun);

            
            AddChild(gun_pivot);
            


            max_health = 3;
            health = max_health;
        }

        public override void Update()
        {
            shield.rotation += rotation_speed;
            base.Update();
            UpdateVelocity();
            ApplyFriction();
            UpdateInformation();
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

        Vector2 direction = new Vector2(1, 0);

        Vector2 facing_direction = new Vector2(1, 0);
        
        void UpdateVelocity() // controls of the player
        {
            //gun_pivot.rotation = angle_of_movement;

            gun_pivot.rotation = angle_of_movement = Mathf.RadiansToDegrees(Mathf.Atan2(facing_direction.y, facing_direction.x));
            //Console.WriteLine(Mathf.RadiansToDegrees(Mathf.Atan2(direction.y, direction.x)));
            if (health <= 0)
            {
                return;
            }
            //Console.WriteLine("rotation: " + rotation);
            if (Input.GetKey(Key.E))
            {
                velocity.x = move_speed * facing_direction.x;
                velocity.y = move_speed * facing_direction.y;

                //velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(angle_of_movement));
                //velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(angle_of_movement));
            }

         

            if (Input.GetKey(Key.W))
            {
                direction.y = -1;
            }
            else if (Input.GetKey(Key.S))
            {
                direction.y = 1;
            }
            else
            {
                direction.y = 0;
            }
            

            if (Input.GetKey(Key.A)) 
            {
                //angle_of_movement -= rotation_speed;
                direction.x = -1;
            }
            else if (Input.GetKey(Key.D))
            {
                //angle_of_movement += rotation_speed;
                direction.x = 1;
            }
            else
            {
                direction.x = 0;
            }

            if(direction.x != 0 || direction.y != 0)
            {
                facing_direction = direction;
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
                Sound gunshot = new Sound("sounds/gunshot/gunshot_" + Utils.Random(1, 4) + ".wav");
                gunshot.Play(false, 15, 0.5f);
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

        bool is_shield_on;
        public void CreateShield()
        {
            if (is_shield_on)
                return;
            LateAddChild(shield);

            shield_timer = new Timer(shield_timer_duration, false, false);
            shield_timer.Timeout += DestroyShield;
            is_shield_on = true;
            shield.visible = true;

        }
        public void DestroyShield()
        {
            RemoveChild(shield);
            is_shield_on = false;
            shield.visible = false;
        }
        public void UpgradeGun()
        {
            gun_upgrade.Play(false, 15, 0.5f);
            gun_upgrade_timer = new Timer(gun_upgrade_duration, false, false);

            gun_upgrade_timer.Timeout += DegradeGun;

            gun_upgrade_level = (int)Mathf.Clamp(last_gun_upgrade_level + 1, 1, 3);
            last_gun_upgrade_level = gun_upgrade_level;

            
        }

        public void DegradeGun()
        {
            gun_upgrade_level = 1;
        }

        void GunLevel1()
        {
            SpawnBullet(new Vector2(gun.width, gun.width), 0);
            shot_delay_timer = new Timer(shot_delay_1, false, false);
        }
        void GunLevel2()
        {
            SpawnBullet(new Vector2(gun.width, gun.width), 5);
            SpawnBullet(new Vector2(gun.width, gun.width), -5);
            shot_delay_timer = new Timer(shot_delay_2, false, false);
        }
        void GunLevel3()
        {
            SpawnBullet(new Vector2(gun.width, gun.width), 0);
            SpawnBullet(new Vector2(gun.width, gun.width), 5);
            SpawnBullet(new Vector2(gun.width, gun.width), -5);
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
            health_bonus.Play(false, 15, 0.5f);
            base.Heal(amount);
            UpdateHealth?.Invoke(health);
        }
        public override void Damage(int amount)
        {
            if (immune || is_shield_on)
                return;
            player_damage.Play(false, 16, 0.5f);
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

