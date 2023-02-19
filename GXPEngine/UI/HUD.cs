using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine.Core;

namespace GXPEngine.UI
{
    public class HUD: GameObject
    {
        private readonly EasyDraw canvas;

        private Vector2 score_position;

        private Vector2 health_position;

        //private HeartHUD[] hearts;
        public int score;
        public int health;

        AnimationSprite awaiting_wave_image;


        public HUD()
        {
            //Initializing variables
            canvas = new EasyDraw(game.width, game.height, false);

            //Monetary Gains
            score = 0;
            score_position = new Vector2(32, 16);

            health_position = new Vector2(32, 48);


            //Canvas properties
            canvas.TextSize(28);
            canvas.Fill(255);
            canvas.TextAlign(CenterMode.Min, CenterMode.Min);

            //Adds canvas to display hierarchy
            AddChild(canvas);

            //Starting text
            UpdateCanvas();
        }

        

        public void UpdateScore(int amount)
        {
            score += amount;
            UpdateCanvas();
        }
        public void UpdateHealth(int amount)
        {
            health = amount;
            UpdateCanvas();
        }

        public void CreateAwaitingWaveImage()
        {
            awaiting_wave_image = new AnimationSprite("sprites/new_wave.png", 1, 1, 1, false, false);
            AddChild(awaiting_wave_image);
        }
        public void DeleteAwaitingWaveImage()
        {
            RemoveChild(awaiting_wave_image);
        }
        /*
        public void SubtractHealth(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                RemoveHeart();
            }
        }
        public void RemoveHeart()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i].IsOn())
                {
                    hearts[i].TurnOff();
                    break;
                }
            }
            UpdateCanvas();
        }

        public void AddHealth(int amount)
        {

            Console.WriteLine("Adding health");
            for (int i = 0; i < amount; i++)
            {
                AddHeart();
            }
        }

        

        private void AddHeart()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (!hearts[i].IsOn())
                {
                    hearts[i].TurnOn();
                    break;
                }
            }
            UpdateCanvas();
        }
        */

        private void UpdateCanvas()
        {
            canvas.ClearTransparent();
            canvas.Text(score.ToString(), score_position.x, score_position.y);
            canvas.Text(health.ToString(), health_position.x, health_position.y);
        }
    }
    /*
    public class HeartHUD : AnimationSprite
    {
        public HeartHUD(string filename, int columns, int rows, TiledObject obj) : base("sprites/collectibles/heart/heart.png", 2, 1, 2)
        {
            _animationDelay = 120;
            SetCycle(0, 2, _animationDelay);
        }

        protected void Update()
        {
            Animate(Time.deltaTime);
        }

        public bool IsOn()
        {
            if (alpha == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TurnOff()
        {
            visible = false;
            alpha = 0;
        }
        public void TurnOn()
        {
            visible = true;
            alpha = 1;
        }
    
    }
    */
}
