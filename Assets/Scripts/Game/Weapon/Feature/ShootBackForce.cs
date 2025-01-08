using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace QFramework.Gungeon
{
    public class ShootBackForce
    {
        private SpriteRenderer spriteRenderer;
        private Vector2 originPos;
        private Vector2 backPos;
        private int spriteBackwardFrames = 0;
        private int spriteBackwardTotalFrames = 0;

        public void Setup(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
            originPos = spriteRenderer.LocalPosition2D();
        }

        public void Update()
        {
            if(spriteBackwardFrames > 0)
            {
                spriteRenderer.LocalPosition2D(Vector2.Lerp(backPos, originPos, 1 - spriteBackwardFrames / (float)spriteBackwardTotalFrames));
                spriteBackwardFrames--;
            }
            else
            {
                spriteRenderer.LocalPosition2D(originPos);
            }
        }

        public void Shoot(float a,int frames)
        {
            backPos = originPos + Vector2.left * a * 2;
            spriteBackwardFrames = frames * 2;
            spriteBackwardFrames = frames * 2;
        }
    }

}