using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class CameraController : Enemy
    {
        public GameObject Player;
        public static EasyEvent<int, float> Shake = new EasyEvent<int, float>();

        public bool shaking = false;
        public float shakeWave = 0;
        public int shakeFrames = 0;
        // Start is called before the first frame update
        void Start()
        {
            Shake.Register((i, f) =>
            {
                shaking = true;
                shakeFrames = i;
                shakeWave = f;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (shaking)
            {
                //Ŀ��λ��
                var targetPosition = new Vector2(Global.player.transform.position.x, Global.player.transform.position.y);
                //���㵱ǰӦ�����λ��
                Vector3 currentPosition = Vector2.Lerp(transform.position, targetPosition, (1.0f - Mathf.Exp(-Time.deltaTime * 5)));
                currentPosition.z = -10;
                
                shakeFrames--;

                var newShakeWave = (shakeFrames / 30.0f).Lerp(shakeWave, 0);

                currentPosition.x = currentPosition.x + UnityEngine.Random.Range(-newShakeWave, newShakeWave);
                currentPosition.y = currentPosition.y + UnityEngine.Random.Range(-newShakeWave, newShakeWave);
                if(shakeFrames <= 0)
                {
                    shaking = false;
                }

                //����λ��
                transform.position = currentPosition;
            }
            else
            {
                //Ŀ��λ��
                var targetPosition = new Vector2(Global.player.transform.position.x, Global.player.transform.position.y);
                //���㵱ǰӦ�����λ��
                Vector3 currentPosition = Vector2.Lerp(transform.position, targetPosition, (1.0f - Mathf.Exp(-Time.deltaTime * 5)));
                currentPosition.z = -10;
                //����λ��
                transform.position = currentPosition;
            }
        }
    }
}