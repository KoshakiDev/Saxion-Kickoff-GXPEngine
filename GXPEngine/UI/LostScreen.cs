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
            score_position = new Vector2(game.width / 2, game.height / 2);

            
            //Canvas properties
            canvas.TextSize(25);
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
        private void UpdateCanvas()
        {
            canvas.ClearTransparent();
            canvas.Text("YOUR SCORE: " + score.ToString(), score_position.x, score_position.y);
        }

    }
}
