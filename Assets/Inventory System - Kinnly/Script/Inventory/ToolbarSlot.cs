using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kinnly
{
    public class ToolbarSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public int slotNumber;

        [SerializeField] PlayerInventory playerInventory;
        [SerializeField] Image image;

        Color32 defaultColor = new Color32(255, 255, 255, 255);
        Color32 selectedColor = new Color32(255, 161, 161, 128);

        void Update()
        {
            if (playerInventory.CurrentlySelectedToolBar == slotNumber)
            {
                image.color = selectedColor;
            }
            else
            {
                image.color = defaultColor;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            playerInventory.CurrentlyHoveredToolbarSlot = this.gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            playerInventory.CurrentlyHoveredToolbarSlot = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            playerInventory.CurrentlySelectedToolBar = slotNumber;
        }
    }
}