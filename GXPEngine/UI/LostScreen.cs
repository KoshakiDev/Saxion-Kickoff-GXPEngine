using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine.Core;

namespace GXPEngine.UI
{
    class LostScreen: GameObject
    {
        private readonly EasyDraw canvas;

        private Vector2 score_position;

        public int score;


        public LostScreen(int set_score)
        {
            //Initializing variables
            canvas = new EasyDraw(game.width, game.height, false);

            //Monetary Gains
            score = set_score;
            score_position = new Vector2(game.width / 2, game.height / 4);

            
            //Canvas properties
            canvas.TextSize(48);
            canvas.Fill(244, 210, 228, 255);
            canvas.TextAlign(CenterMode.Center, CenterMode.Center);

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
        private void UpdateCanvas()
        {
            canvas.ClearTransparent();
            canvas.TextFont(Utils.LoadFont("TEENAGEANGST.ttf", 64f));
            canvas.Text("YOUR SCORE: " + score.ToString(), score_position.x, score_position.y);
        }

    }
}
