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

        //private HeartHUD[] hearts = new HeartHUD()[3];
        public int score;
        public int health;

        bool show_new_wave_text = false;
        //int wave_counter;

        public HUD()
        {
            //Initializing variables
            canvas = new EasyDraw(game.width, game.height, false);

            //wave_counter = 0;
            //Monetary Gains
            score = 0;
            score_position = new Vector2(32, 16);

            health_position = new Vector2(32, 48);


            //Canvas properties

            canvas.TextSize(28);
            canvas.Fill(244, 210, 228, 255);
            canvas.TextAlign(CenterMode.Min, CenterMode.Min);

            //Adds canvas to display hierarchy
            AddChild(canvas);
;

            //AddChild(hearts);

            //Starting text
            UpdateCanvas();
        }

        

        public void UpdateScore(int amount)
        {
            score += amount;
            UpdateCanvas();
        }
        int prev_health = 3;
        public void UpdateHealth(int amount)
        {
            health = amount;
            /*
            if(prev_health > health)
            {
                SubtractHealth(prev_health - health);
            }
            else if (prev_health < health)
            {
                AddHealth(health - prev_health);
            }
            */
            UpdateCanvas();
        }

        public void CreateAwaitingWaveImage()
        {
            show_new_wave_text = true;
            UpdateCanvas();
            //AddChild(awaiting_wave_image);
        }
        public void DeleteAwaitingWaveImage()
        {
            show_new_wave_text = false;
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
            if (show_new_wave_text)
            {
                
                canvas.TextSize(64);
                canvas.TextFont(Utils.LoadFont("TEENAGEANGST.ttf", 128f, System.Drawing.FontStyle.Bold));
                canvas.TextAlign(CenterMode.Center, CenterMode.Center);

                canvas.Text("New Wave Incoming!", game.width / 2, game.height / 2);

                canvas.TextAlign(CenterMode.Min, CenterMode.Min);
            }
            canvas.TextFont(Utils.LoadFont("TEENAGEANGST.ttf", 48f));
            canvas.Text(score.ToString(), score_position.x, score_position.y);
            canvas.Text(health.ToString(), health_position.x, health_position.y);
        }
    }
    
    public class HeartHUD : AnimationSprite
    {
        string off_path = "sprites/icons/brokenHeart.png";
        string on_path = "sprites/icons/Heart.png";
        bool is_on;
        AnimationSprite off_heart;

        AnimationSprite on_heart;
        public HeartHUD() : base("sprites/icons/heart_pickup.png", 1, 1, 1)
        {
            on_heart = new AnimationSprite(on_path, 1, 1, 1);
            off_heart = new AnimationSprite(off_path, 1, 1, 1);
            off_heart.visible = false;
            is_on = true;
        }

        protected void Update()
        {
        }

        public bool IsOn()
        {
            return is_on;
        }

        public void TurnOff()
        {
            off_heart.visible = true;
            on_heart.visible = false;
            is_on = false;
        }
        public void TurnOn()
        {
            off_heart.visible = false;
            on_heart.visible = true;
            is_on = true;
        }
    
    }
    
}
