using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidMedium : Asteroid
    {
        public AsteroidMedium(MyGame world, Player player) : base("sprites/asteroid_medium.png")
        {
            move_speed = 0.1f;

            world_reference = world;

            player_reference = player;
            destruction_reward = 50;
            spawn_amount = 3;
            max_health = 2;
            health = max_health;

            world_reference.connectAsteroid(this);
        }
        public override void Death()
        {
            
            float rotation_divide = 360 / spawn_amount;
            float current_rotation_divide = 0;
            for(int i = 0; i < spawn_amount; i++)
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
