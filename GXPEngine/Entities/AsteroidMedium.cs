using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidMedium : Asteroid
    {
        public AsteroidMedium(MyGame world, Player player) : base("sprites/enemy/medium_hitbox.png")
        {
            move_speed = 0.1f;

            world_reference = world;

            player_reference = player;
            destruction_reward = 50;

            sprite = new AnimationSprite("sprites/enemy/enemy3.png", 10, 1, 10, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };

            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.scaleX = sprite.scaleY = DEFAULT_SCALE * 1.2f;

            AddChild(sprite);


            sprite.SetCycle(0, 10, _animationDelay);


            spawn_amount = 2;
            max_health = 2;
            health = max_health;

            world_reference.connectAsteroid(this);
        }
        public override void Death()
        {
            
            float rotation_divide = 360 / spawn_amount;

            float current_rotation_divide = Utils.Random(0, 360);
            for (int i = 0; i < spawn_amount; i++)
            {
                AsteroidSmall new_asteroid = new AsteroidSmall(world_reference , player_reference);


                new_asteroid.x = x;
                new_asteroid.y = y;


                new_asteroid.angle_of_movement = current_rotation_divide;
                current_rotation_divide += rotation_divide;
                parent.LateAddChild(new_asteroid);
            }
            base.Death();
        }
    }
}
