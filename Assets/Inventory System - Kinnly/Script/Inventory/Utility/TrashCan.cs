using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class TrashCan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Animator animator;
        [SerializeField] PlayerInventory playerInventory;

        public void OnPointerEnter(PointerEventData eventData)
        {
            playerInventory.IsHoveringTrashcan = true;
            animator.SetBool("State", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            playerInventory.IsHoveringTrashcan = false;
            animator.SetBool("State", false);
        }
    }
}
