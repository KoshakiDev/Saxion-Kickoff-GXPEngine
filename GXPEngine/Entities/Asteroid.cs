using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class Asteroid: Entity
    {
        public Asteroid(string filePath) : base(filePath, 1, 1, 1)
        {
            SetOrigin(width / 2, height / 2);
            move_speed = 0.01f;
            rotation_speed = 0.5f;
        }

        public override void OnCollision(GameObject collider)
        {
            if(collider is Bullet)
            {
                Hit();
            }
        }

        public override void Hit()
        {
            base.Hit();
        }

        public override void Update()
        {
            velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(rotation));
            velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(rotation));
            base.Update();
        }

    }
}
