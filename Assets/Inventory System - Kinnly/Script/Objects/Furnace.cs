using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using UnityEngine;


namespace Kinnly
{
    public class Furnace : MonoBehaviour, IInteractable, IDamageable
    {
        [Header("Core")]
        [SerializeField] Animator animator;

        [Header("Assets")]
        [SerializeField] GameObject itemDrop;
        [SerializeField] Item furnace;

        [Header("Recipe")]
        [SerializeField] List<Item> input;
        [SerializeField] List<Item> output;
        [SerializeField] List<int> processingTime;

        //Health
        int health;

        //Smelting Logic
        bool isSmelting;
        int index;

        // Start is called before the first frame update
        void Start()
        {
            health = 1;
            index = -1;
            isSmelting = false;
        }

        // Update is called once per frame
        void Update()
        {
            CheckHealth();
        }

        public void Interact(PlayerInventory playerInventory)
        {
            if (isSmelting)
            {
                DialogBox.instance.Show("Currently Working", 1f);
                return;
            }

            if (playerInventory.CurrentlySelectedInventoryItem.Item != null)
            {
                Smelting(playerInventory);
            }
        }

        public void Damage(PlayerInventory playerInventory, int damage)
        {
            if (playerInventory.CurrentlySelectedInventoryItem.Item.isPickaxe)
            {
                health -= damage;
            }
        }

        private void Smelting(PlayerInventory playerInventory)
        {
            Item item = playerInventory.CurrentlySelectedInventoryItem.Item;
            for (int i = 0; i < input.Count; i++)
            {
                if (item != null)
                {
                    if (item == input[i])
                    {
                        index = i;
                        isSmelting = true;
                        playerInventory.RemoveItem(playerInventory.CurrentlySelectedInventoryItem, 1);
                        animator.SetBool("State", true);
                        InvokeSmelted();
                    }
                }
            }

            if (isSmelting == false)
            {
                DialogBox.instance.Show("This Item Cannot be Smelted", 1f);
            }
        }

        private void InvokeSmelted()
        {
            Invoke("Smelted", processingTime[index]);
        }

        private void Smelted()
        {
            animator.SetBool("State", false);
            isSmelting = false;
            SpawnItem(output[index], 1);
            index = -1;
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
                if (index != -1)
                {
                    SpawnItem(input[index], 1);
                }
                SpawnItem(furnace, 1);
                Destroy(this.gameObject);
            }
        }
    }
}
