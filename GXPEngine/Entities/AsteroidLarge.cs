using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidLarge: Asteroid
    {
        public AsteroidLarge(MyGame world, Player player) : base("sprites/asteroid_large.png")
        {
            move_speed = 0.01f;
            destruction_reward = 20;
            world_reference = world;
            player_reference = player;

            //alpha = 0;
            sprite = new AnimationSprite("sprites/asteroid_large-Sheet.png", 4, 1, 4, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };
            AddChild(sprite);
            sprite.y -= width / 2;
            sprite.x -= height / 2;

            sprite.SetCycle(0, 4, _animationDelay);


            spawn_amount = 2;
            max_health = 3;
            health = max_health;

            world_reference.connectAsteroid(this);
        }
        public override void Update()
        {
            base.Update();
            AnimateSpriteModel();
        }
        public override void Death()
        {
            

            float rotation_divide = 360 / spawn_amount;
            float current_rotation_divide = Utils.Random(0, 360);
            for (int i = 0; i < spawn_amount; i++)
            {
                AsteroidMedium new_asteroid = new AsteroidMedium(world_reference , player_reference);


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
