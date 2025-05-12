using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class DropBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        GameObject player;
        PlayerInventory playerInventory;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            playerInventory.IsHoveringDropBox = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            playerInventory.IsHoveringDropBox = false;
        }
    }
}