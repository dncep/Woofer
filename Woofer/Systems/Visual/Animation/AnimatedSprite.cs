using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Visual.Animation
{
    class AnimatedSprite
    {
        public int SpriteIndex { get; set; }
        public Size FrameSize { get; set; }
        public Vector2D Origin { get; set; }
        public Vector2D Step { get; set; }

        public int FrameCount { get; set; }
        public bool Loop { get; set; }

        public int CurrentFrame { get; set; }
        public int FrameProgress { get; set; }

        public int[] FrameDurations { get; set; }

        public AnimatedSprite(int spriteIndex, Size frameSize, Vector2D origin, Vector2D step, int frameCount, int[] frameDurations)
        {
            SpriteIndex = spriteIndex;
            FrameSize = frameSize;
            Origin = origin;
            Step = step;
            FrameCount = frameCount;
            FrameDurations = frameDurations;

            CurrentFrame = 0;
            FrameProgress = 0;
            Loop = false;
        }

        public AnimatedSprite(int spriteIndex, Size frameSize, Vector2D origin, Vector2D step, int frameCount, int frameDuration)
        {
            SpriteIndex = spriteIndex;
            FrameSize = frameSize;
            Origin = origin;
            Step = step;
            FrameCount = frameCount;
            FrameDurations = new int[frameCount];
            for (int i = 0; i < frameCount; i++) FrameDurations[i] = frameDuration;

            CurrentFrame = 0;
            FrameProgress = 0;
            Loop = false;
        }
    }
}
