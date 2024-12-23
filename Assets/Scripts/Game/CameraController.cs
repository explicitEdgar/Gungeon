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
            //目标位置
            var targetPosition = new Vector2(Global.player.transform.position.x, Global.player.transform.position.y);
            //计算当前应到达的位置
            Vector3 currentPosition = Vector2.Lerp(transform.position, targetPosition, (1.0f - Mathf.Exp(-Time.deltaTime * 5)));
            currentPosition.z = -10;
            //设置位置
            transform.position = currentPosition;
        }
    }
}