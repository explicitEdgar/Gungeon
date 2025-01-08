using System.Collections;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class AnimationHelper
    {
        public static void UpDownAnimation(Component component,float a,long frameCount,int upDownFrames,float upDownOffset = 0)
        {
            var posY = Mathf.Lerp(-a, a,
                (frameCount % upDownFrames - upDownFrames * 0.5f).Abs() / upDownFrames * 0.5f);
            //在动画帧数内，插值变化：1 -> 0 -> 1
            //可以实现摇摆
            //用三角函数也行,比线性插值更灵活，更直观

            component.LocalPositionY(upDownOffset + posY);
        }

        public static void RotateAnimation(Component component,float angle,long frameCount,int rotateFrames)
        {
            float frequency = 1;
            var angleZ = angle * Mathf.Sin(2 * Mathf.PI * frequency * (frameCount % rotateFrames) / rotateFrames);

            component.LocalEulerAnglesZ(angleZ);
        }
    }
}