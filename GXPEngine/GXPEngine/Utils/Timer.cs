using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Timer : GameObject
    {
        public event Action Timeout;

        public bool finished;
        private int length;
        private int startTime;
        private bool paused;
        private bool destroyOnFinished;

        public Timer(int length, bool paused = false, bool destroyOnFinished = true)
        {
            finished = false;
            this.length = length;
            this.paused = paused;
            this.destroyOnFinished = destroyOnFinished;
            startTime = Time.now;
            game.AddChild(this);
        }

        void Update()
        {
            if (!paused)
            {
                if (startTime + length < Time.now)
                {
                    Finish();
                }
            }
        }

        public bool IsPaused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
            }
        }

        public void UnPause()
        {
            paused = false;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Reset()
        {
            startTime = Time.now;
            finished = false;
        }


        private void Finish()
        {
            finished = true;
            Timeout?.Invoke();
            if (destroyOnFinished)
            {
                this.Destroy();
            }
            else
            {
                paused = true;
                startTime = Time.now;
            }

        }
    }
}