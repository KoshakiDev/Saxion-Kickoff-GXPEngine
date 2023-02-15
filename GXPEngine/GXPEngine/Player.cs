using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Player : AnimationSprite //Playerclass with very basic movement and basic collisions
    {
        int speed = 3;

        float fallingvelocity;
        bool Grounded;

        public Player(string Sprite, int columns, int rows) : base(Sprite, columns, rows)
        {
            scale = 0.3f;
            _animationDelay = 5;
        }

        public void Update()
        {
            Playermove();
            Animate();
            if (!Grounded)
            { MoveDown(); }
            Grounded = false;
        }


        void Playermove() // controls of the player
        {
            if (Input.GetKey(Key.W)) // w
            {
                y += -speed;

            }
            if (Input.GetKey(Key.A)) // a
            {
                x += -speed;
                Mirror(true, false);
            }

            if (Input.GetKey(Key.D)) // d
            {
                x += +speed;
                Mirror(false, false);

            }
        }
        void MoveDown()
        {
            // This is the gravity of the player wich increases over time and has a maximum increase
            y += fallingvelocity;

            if (fallingvelocity < 5)
            {
                fallingvelocity += 0.1f;
            }
        }

        void OnCollision(GameObject collider)
        {
            // this checks if the collided object is am object from the background layer
            if (collider.name == "Background")
            {
                return;
            }
            // this checks for collisions with tiles on the foreground
            if (collider is Tiles)
            {
                Grounded = true;
                fallingvelocity = 0;
            }
            

        }

    }
}

