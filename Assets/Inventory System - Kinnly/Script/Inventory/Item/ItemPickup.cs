using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        public Item Item;
        public int Amount;

        public void Interact(PlayerInventory playerInventory)
        {
            playerInventory.AddItem(Item, Amount);
            Destroy(this.gameObject);
        }
    }
}