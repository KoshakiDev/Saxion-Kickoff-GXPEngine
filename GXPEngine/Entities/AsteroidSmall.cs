using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidSmall : Asteroid
    {
        public AsteroidSmall(MyGame world, Player player) : base("sprites/enemy/small_hitbox.png")
        {
            move_speed = 0.25f;
            world_reference = world;

            player_reference = player;
            destruction_reward = 100;

            sprite = new AnimationSprite("sprites/enemy/enemy4.png", 6, 1, 6, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.scaleX = sprite.scaleY = DEFAULT_SCALE;

            AddChild(sprite);

            sprite.SetCycle(0, 8, _animationDelay);

            SetOrigin(width / 2, height / 2);


            spawn_amount = 0;
            max_health = 1;
            health = max_health;

            world_reference.connectAsteroid(this);
        }
        public override void Death()
        {
            base.Death();
        }
    }
}
