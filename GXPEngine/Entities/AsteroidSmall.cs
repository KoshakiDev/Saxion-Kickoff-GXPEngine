using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidSmall : Asteroid
    {
        public AsteroidSmall() : base("sprites/asteroid_small.png")
        {
            move_speed = 1.0f;
        }
        public override void Hit()
        {
            base.Hit();
        }
    }
}
