using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.Entities
{
    class GunPickup: ItemPickup
    {
        public GunPickup() : base("sprites/icons/pickup_hitbox.png", 1, 1, 1)
        {
            sprite = new AnimationSprite("sprites/icons/gun_pickup.png", 1, 1, 1, true, false)
            {
                alpha = 1
                //width = 16,
                //height = 16
            };
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.scaleX = sprite.scaleY = DEFAULT_SCALE * 0.5f;
            //sprite.scaleX = sprite.scaleY = DEFAULT_SCALE * 0.75f;
            AddChild(sprite);


            SetOrigin(width / 2, height / 2);
            _animationDelay = 100;
        }
        public override void Update()
        {
            base.Update();
        }
        public override void AddItem(Player player)
        {
            player.UpgradeGun();
            Death();
        }
    }
}
