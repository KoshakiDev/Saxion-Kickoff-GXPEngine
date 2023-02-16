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

        public MyGame world_reference;
        public int spawn_amount;

        public Asteroid(string filePath) : base(filePath, 1, 1, 1)
        {
            SetOrigin(width / 2, height / 2);
            move_speed = 0.01f;
            rotation_speed = 0.5f;
        }

        public override void OnCollision(GameObject collider)
        {
            if(collider is Bullet || collider is Player)
            {
                Damage(1);
            }
        }

        public override void Death()
        {
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
            //Console.WriteLine("dying");
            UpdateScore?.Invoke(destruction_reward);
            world_reference.disconnectAsteroid(this);
            base.Death();
        }

        public override void Update()
        {
            velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(rotation));
            velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(rotation));
            base.Update();
        }

    }
}
