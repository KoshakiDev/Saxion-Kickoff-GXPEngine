using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    public class Bullet : Entity
    {
        private Timer timer;
    
        public Bullet(float set_rotation, float set_x, float set_y) : base("sprites/icons/bullet_hitbox.png", 1, 1, 1)
        {

            sprite = new AnimationSprite("sprites/icons/bullet.png", 1, 1, 1, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.scaleX = sprite.scaleY = DEFAULT_SCALE * 0.25f;
            AddChild(sprite);

            //sprite.SetCycle(0, 4, _animationDelay);

            SetOrigin(width / 2, height / 2);

            x = set_x;
            y = set_y;

            rotation = set_rotation;

            move_speed = 0.25f;
            rotation_speed = 0.5f;

            timer = new Timer(3000, false, true);
            timer.Timeout += Death;

            velocity.x = move_speed * Mathf.Cos(Mathf.DegreesToRadians(rotation));
            velocity.y = move_speed * Mathf.Sin(Mathf.DegreesToRadians(rotation));
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Asteroid)
            {
                Death();   
            }
        }
        public override void Death()
        {
            base.Death();

        }
        public override void Update()
        {
            DestroyIfOutOfBounds();
            Move();
            //base.Update();
        }
        public virtual void DestroyIfOutOfBounds()
        {
            if (x >= game.width || x <= 0 ||
                y >= game.height || y <= 0)
            {
                Death();
            }
        }
    }
}
