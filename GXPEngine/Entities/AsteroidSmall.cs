using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidSmall : Asteroid
    {
        public AsteroidSmall(MyGame world, Player player) : base("sprites/asteroid_small.png")
        {
            move_speed = 0.25f;
            world_reference = world;

            player_reference = player;
            destruction_reward = 100;
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
