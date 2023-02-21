using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    /// <summary>
    /// Simple class to make backgrounds with animation or without whatever you want.
    /// </summary>
    public class Background : AnimationSprite
    {
        public Background(string filePath, int cols = 1, int rows = 1, int frames = 1) : base(filePath, cols, rows, frames, false, false)
        {

        }
    }
}