using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class Shield : AnimationSprite
    {
        public Shield() : base("sprites/icons/shield.png", 1, 1, 1, false, true)
        {
            SetOrigin(width / 2, height / 2);
        }
    }
}
