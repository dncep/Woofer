using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Visual.Animation
{
    [PersistentObject]
    class AnimatedSprite
    {
        [PersistentProperty]
        public int SpriteIndex { get; set; }
        [PersistentProperty]
        public Size FrameSize { get; set; }
        [PersistentProperty]
        public Vector2D Origin { get; set; }
        [PersistentProperty]
        public Vector2D Step { get; set; }

        [PersistentProperty]
        public int FrameCount { get; set; }
        [PersistentProperty]
        public bool Loop { get; set; }

        [PersistentProperty]
        public int CurrentFrame { get; set; }
        [PersistentProperty]
        public int FrameProgress { get; set; }

        [PersistentProperty]
        public int[] FrameDurations { get; set; }

        public AnimatedSprite()
        {

        }

        public AnimatedSprite(int spriteIndex, Size frameSize, Vector2D origin, Vector2D step, int frameCount, int[] frameDurations)
        {
            SpriteIndex = spriteIndex;
            FrameSize = frameSize;
            Origin = origin;
            Step = step;
            FrameCount = frameCount;
            FrameDurations = frameDurations;

            CurrentFrame = 0;
            FrameProgress = -1;
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
            FrameProgress = -1;
            Loop = false;
        }
    }
}
