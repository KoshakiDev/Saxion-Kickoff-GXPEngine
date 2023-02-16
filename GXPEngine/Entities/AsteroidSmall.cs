using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidSmall : Asteroid
    {
        public AsteroidSmall(MyGame world) : base("sprites/asteroid_small.png")
        {
            move_speed = 0.25f;
            world_reference = world;
            destruction_reward = 125;
            spawn_amount = 0;
            health = 1;

            world_reference.connectAsteroid(this);
        }
        public override void Death()
        {
            base.Death();
        }
    }
}
