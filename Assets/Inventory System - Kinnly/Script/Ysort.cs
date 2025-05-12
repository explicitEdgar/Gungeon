using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Costum Script for Rendering Objects Based on Sorting Order Value of Sprite Renderer Component.

//Ysort Script - All objects with SpriteRenderer must have the Ysort to make sure objects are drawing correctly on the scene.
//it will change Order in layer of an object's Sprite Renderer based on Y Position with a scale of 10.

namespace Kinnly
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ysort : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        [Header("Core")]
        [SerializeField] float Offset;
        int scale;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            scale = -10;
        }

        // Update is called once per frame
        void Update()
        {
            //Every point of offset will move the pivot of the SortingOrder of the game object by 1 in the axis of y
            //Higher offset will render objects at the front.
            //Lower offset (minus) will render objects behind.
            spriteRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + -Offset) * scale);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(transform.position.x, Mathf.RoundToInt(transform.position.y + -Offset), transform.position.z), 0.1f);
        }
    }
}
