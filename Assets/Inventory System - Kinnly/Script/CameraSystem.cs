using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinnly
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] GameObject player;

        Vector2 Offset;
        // Start is called before the first frame update
        void Start()
        {
            Offset.x = 0.1f;
            Offset.y = 0.25f;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(player.transform.position.x + Offset.x, player.transform.position.y + Offset.y, -1);
        }
    }
}

