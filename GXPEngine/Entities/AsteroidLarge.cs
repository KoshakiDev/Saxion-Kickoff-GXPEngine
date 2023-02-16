using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidLarge: Asteroid
    {
        public AsteroidLarge(MyGame world) : base("sprites/asteroid_large.png")
        {
            move_speed = 0.01f;
            destruction_reward = 500;
            world_reference = world;

            spawn_amount = 2;
            health = 3;

            world_reference.connectAsteroid(this);
        }
        public override void Death()
        {
            

            float rotation_divide = 360 / spawn_amount;
            float current_rotation_divide = Utils.Random(0, 360);
            for (int i = 0; i < spawn_amount; i++)
            {
                AsteroidMedium new_asteroid = new AsteroidMedium(world_reference);


                new_asteroid.x = x;
                new_asteroid.y = y;

                new_asteroid.rotation = current_rotation_divide;
                current_rotation_divide += rotation_divide;
                parent.LateAddChild(new_asteroid);
            }
            base.Death();
        }

    }
}
