using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.UI
{
    public class Menu : GameObject
    {
        private readonly EasyDraw canvas;

        private Vector2 score_position;

        private Vector2 health_position;

        //private HeartHUD[] hearts;
        public int score;
        public int health;


        public Menu()
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
}
