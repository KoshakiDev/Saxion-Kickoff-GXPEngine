using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    public class ItemPickup: Entity
    {
        Timer life_timer;


        public ItemPickup(
               string filePath,
               int columns,
               int rows,
               int frames) : base(filePath, columns, rows, frames, true)
        {
            _animationDelay = 100;

            life_timer = new Timer(10000, false, false);
            life_timer.Timeout += Death;
        }
        public override void OnCollision(GameObject collider)
        {
            if (collider is Player)
            {
                Player player = (Player)collider;
                AddItem(player);
            }
        }
        public virtual void AddItem(Player player)
        {

        }
    }
}
