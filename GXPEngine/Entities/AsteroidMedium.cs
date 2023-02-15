using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class AsteroidMedium : Asteroid
    {
        public AsteroidMedium() : base("sprites/asteroid_medium.png")
        {
            move_speed = 0.1f;
        }
        public override void Hit()
        {
            float rotation_divide = 360 / 3;
            float current_rotation_divide = 0;
            for(int i = 0; i < 3; i++)
            {
                AsteroidSmall new_asteroid = new AsteroidSmall();


                new_asteroid.x = x;
                new_asteroid.y = y;


                new_asteroid.rotation = current_rotation_divide;
                current_rotation_divide += rotation_divide;
                parent.LateAddChild(new_asteroid);
            }
            base.Hit();
        }
    }
}
