using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine.UI
{
    public class Menu : GameObject
    {
        private readonly EasyDraw canvas;

        private Vector2 highscore_position;

        
        //private HeartHUD[] hearts;
        public int highscore;
        

        public Menu(int set_highscore)
        {
            //Initializing variables
            canvas = new EasyDraw(game.width, game.height, false);

            //Monetary Gains
            highscore = set_highscore;
            highscore_position = new Vector2(game.width / 2, game.height / 4);


            //Canvas properties
            canvas.TextSize(48);

            canvas.Fill(244, 210, 228, 255);
            canvas.TextAlign(CenterMode.Center, CenterMode.Center);

            //Adds canvas to display hierarchy
            AddChild(canvas);

            //Starting text
            UpdateCanvas();
        }
       

        private void UpdateCanvas()
        {
            canvas.ClearTransparent();
            canvas.TextFont(Utils.LoadFont("TEENAGEANGST.ttf", 72f));
            canvas.Text("HIGH SCORE: " + highscore.ToString(), highscore_position.x, highscore_position.y);
        }
    }
}
