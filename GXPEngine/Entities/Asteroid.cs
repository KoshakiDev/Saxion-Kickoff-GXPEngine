using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GXPEngine.Entities
{
    public class Asteroid: Entity
    {
        public int destruction_reward = 0;
        public event Action<int> UpdateScore;
        public float flying_direction_rotation = 0;
        int follow_distance = 250;

        private Sound hit = new Sound("sounds/hit.wav");
        private Sound death = new Sound("sounds/death.wav");


        public MyGame world_reference;
        public Player player_reference;
        public int spawn_amount;
        Timer follow_timer;

        public Asteroid(string filePath) : base(filePath, 1, 1, 1)
        {
            SetOrigin(width / 2, height / 2);
            move_speed = 0.01f;
            rotation_speed = 0.5f;
            follow_timer = new Timer(5000, false, false);
        }

        public override void OnCollision(GameObject collider)
        {
            if(collider is Bullet || collider is Player)
            {
                hit.Play();
                Damage(1);
            }
        }

        public override void Death()
        {
            death.Play();
            for (int i = 0; i < 10; i++)
            {
                float speed = 5.0f;
                string filepath = "sprites/particle_" + Utils.Random(1, 3) + ".png";
                Particle newParticle = new Particle(filepath, BlendMode.NORMAL, 2000);
                // An example of chaining:
                newParticle.SetColor(Color.White, Color.White).
                    SetScale(1.0f, 0.0f).
                    SetVelocity(Utils.Random(-speed, speed), Utils.Random(-speed, speed));
                //Console.WriteLine("Spawning particles");
                newParticle.SetXY(Utils.Random(x, x + width), Utils.Random(y, y + height));
                parent.LateAddChild(newParticle);
            }
            UpdateScore?.Invoke(destruction_reward);
            world_reference.disconnectAsteroid(this);
            base.Death();
        }

        public override void Update()
        {
            velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(flying_direction_rotation));
            velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(flying_direction_rotation));
            base.Update();
            if (DistanceTo(player_reference) <= follow_distance && (follow_timer == null || follow_timer.finished))
            {
                //Console.WriteLine("I am following the player");
                float new_x = Mathf.Normalize(player_reference.x - x, 0, 1);
                float new_y = Mathf.Normalize(player_reference.y - y, 0, 1);
                float angle_in_radians = Mathf.Atan2(new_y, new_x);
                flying_direction_rotation = (angle_in_radians / Mathf.PI) * 180.0f;

                follow_timer = new Timer(5000, false, false);
            }
        }

    }
}
