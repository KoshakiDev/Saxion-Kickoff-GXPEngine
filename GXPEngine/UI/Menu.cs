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

        private Vector2 title_position;

        //private HeartHUD[] hearts;
        public int highscore;
        

        public Menu(int set_highscore)
        {
            //Initializing variables
            canvas = new EasyDraw(game.width, game.height, false);

            //Monetary Gains
            highscore = set_highscore;
            highscore_position = new Vector2(game.width / 2, game.height / 3 + 32);

            title_position = new Vector2(game.width / 2, game.height / 3);


            //Canvas properties
            canvas.TextSize(28);
            canvas.Fill(255);
            canvas.TextAlign(CenterMode.Center, CenterMode.Center);

            //Adds canvas to display hierarchy
            AddChild(canvas);

            //Starting text
            UpdateCanvas();
        }
       

        private void UpdateCanvas()
        {
            canvas.ClearTransparent();
            canvas.Text("HIGH SCORE: " + highscore.ToString(), highscore_position.x, highscore_position.y);
            canvas.Text("Wrath of the Plush Demo", title_position.x, title_position.y);
        }
    }
}
