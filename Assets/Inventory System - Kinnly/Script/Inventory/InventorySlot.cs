using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] PlayerInventory playerInventory;

        public void OnPointerEnter(PointerEventData eventData)
        {
            playerInventory.CurrentlyHoveredInventorySlot = this.gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            playerInventory.CurrentlyHoveredInventorySlot = null;
        }
    }
}