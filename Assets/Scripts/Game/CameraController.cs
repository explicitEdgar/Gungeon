using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class CameraController : MonoBehaviour
    {
        public GameObject Player;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
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