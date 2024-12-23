using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class Aim : ViewController
	{
		public List<Sprite> mFrames = new List<Sprite>();
		private int mFrameIndex = 0;
        private int frameCount = 0;
        private void Awake()
        {
			mFrames.Add(Aim1);
            mFrames.Add(Aim2);
            mFrames.Add(Aim3);

        }
        private void Update()
        {   
            if(frameCount % 6 == 0)
            {
                mFrameIndex++;
                if(mFrameIndex > 2)
                {
                    mFrameIndex = 0;
                }

                SelfSpriteRenderer.sprite = mFrames[mFrameIndex];
            }

            frameCount++;
        }
    }
}
