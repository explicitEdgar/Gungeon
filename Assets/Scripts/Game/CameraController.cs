using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class CameraController : MonoBehaviour
    {
        public GameObject Player;
        public static EasyEvent<int, float> Shake = new EasyEvent<int, float>();

        public bool shaking = false;
        public float shakeWave = 0;
        public int shakeFrames = 0;

        public List<Color> Colors = new List<Color>();

        private Camera mCamera; 
        // Start is called before the first frame update
        void Start()
        {   
            
            Shake.Register((i, f) =>
            {
                shaking = true;
                shakeFrames = i;
                shakeWave = f;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            mCamera = gameObject.GetComponent<Camera>();

            Room.OnRoomEnter.Register(room =>
            {
                var currentColor = mCamera.backgroundColor;
                while (room.ColorIndex == -1)
                {
                    room.ColorIndex = UnityEngine.Random.Range(0, Colors.Count);
                    if (Colors[room.ColorIndex] == currentColor) room.ColorIndex = -1;
                }

                var dstColor = Colors[room.ColorIndex];
                ActionKit.Lerp(0, 1, 0.5f, (p) =>
                {
                    mCamera.backgroundColor = Color.Lerp(currentColor, dstColor, p);
                })
                .Start(this);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        // Update is called once per frame
        void Update()
        {   
            //����
            mCamera.orthographicSize = (1.0f - Mathf.Exp(-Time.deltaTime * 5)).Lerp(mCamera.orthographicSize,
                Global.GunAddtionSize + 5);

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

            if(Global.currentRoom)
            {
                var direction = Global.player.Direction2DFrom(Global.currentRoom);
                var halfWidth = Global.currentRoom.Config.Width * 0.5f * 2;
                if(direction.x < 0)
                {
                    var originAngleZ = transform.localEulerAngles.z;
                    var targetAngleZ = (direction.x.Abs() / halfWidth).Lerp(0, 2.5f);
                    if (originAngleZ >= 2.6f) originAngleZ -= 360f;

                    transform.LocalEulerAnglesZ((1.0f - Mathf.Exp(-Time.deltaTime * 5))
                    .Lerp(originAngleZ, targetAngleZ));
                }
                else
                {
                    var originAngleZ = transform.localEulerAngles.z;
                    var targetAngleZ = (direction.x.Abs() / halfWidth).Lerp(0, -2.5f);
                    //2�����»�ȡ���ĽǶ�ת��Ϊ����Χ ��Ȼ����ͻ����Lerp��359��-1�����������
                    if (originAngleZ >= 2.6f) originAngleZ -= 360f;

                    transform.LocalEulerAnglesZ((1.0f - Mathf.Exp(-Time.deltaTime * 5))
                    .Lerp(originAngleZ, targetAngleZ));
                    //1����ת�������ȣ�������������Զ�����Ϊ360����ø����ĺ�

                }
            }
        }
    }
}