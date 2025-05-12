using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class DisplayTable : MonoBehaviour, IInteractable, IDamageable
    {
        [Header("Core")]
        [SerializeField] private Item item;
        [SerializeField] private int amount;
        [SerializeField] SpriteRenderer spriteRenderer;

        [Header("Assets")]
        [SerializeField] GameObject itemDrop;
        [SerializeField] Item displayTable;

        private int health;
        // Start is called before the first frame update
        void Start()
        {
            health = 1;

            if (this.item != null)
            {
                SetItem(this.item, amount);
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckHealth();
        }

        public void SetItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
            spriteRenderer.sprite = item.image;
        }

        public void Interact(PlayerInventory playerInventory)
        {
            if (item == null)
            {
                AddItem(playerInventory);
            }
            else
            {
                RemoveItem(playerInventory);
            }
        }

        public void Damage(PlayerInventory playerInventory, int damage)
        {
            if (playerInventory.CurrentlySelectedInventoryItem.Item.isPickaxe)
            {
                health -= damage;
            }
        }

        public void AddItem(PlayerInventory playerInventory)
        {
            if (playerInventory != null)
            {
                InventoryItem _inventoryItem = playerInventory.CurrentlySelectedInventoryItem;
                SetItem(_inventoryItem.Item, 1);
                playerInventory.RemoveItem(_inventoryItem, 1);
            }
        }

        public void RemoveItem(PlayerInventory playerInventory)
        {
            playerInventory.AddItem(item, amount);
            this.item = null;
            spriteRenderer.sprite = null;
        }

        private void SpawnItem(Item item, int amount)
        {
            Vector3 direction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            GameObject go = Instantiate(itemDrop, transform.position + direction, Quaternion.identity);
            go.GetComponent<ItemDrop>().SetItem(item, amount);
        }

        private void CheckHealth()
        {
            if (health <= 0)
            {
                if (item != null)
                {
                    SpawnItem(item, this.amount);
                }
                SpawnItem(displayTable, 1);
                Destroy(this.gameObject);
            }
        }
    }
}