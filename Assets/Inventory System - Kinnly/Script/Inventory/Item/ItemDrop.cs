using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Kinnly
{
    public class ItemDrop : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Item item;
        [SerializeField] int amount;

        bool isDelay;
        bool isNear;
        bool isSlotAvailable;

        private Player player;
        float speed;
        float delay;

        // Start is called before the first frame update
        void Start()
        {
            player = Player.Instance;

            if (item == null)
            {
                return;
            }

            speed = 10f;
            delay = 0.25f;
            isDelay = true;
            spriteRenderer.sprite = item.image;
        }

        // Update is called once per frame
        void Update()
        {
            if (isDelay)
            {
                TimeCountDown();
            }
            else
            {
                CheckDistance();
                if (isNear)
                {
                    CheckSlotAvailability();
                    if (isSlotAvailable)
                    {
                        MovingtoTarget();
                        AddingItem();
                    }
                }
            }
        }

        public void SetItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
            this.gameObject.name = item.name;
            Start();
        }

        private void TimeCountDown()
        {
            delay -= 1f * Time.deltaTime;
            if (delay <= 0f)
            {
                isDelay = false;
            }
        }

        private void CheckDistance()
        {
            if (Vector2.Distance(this.transform.position, player.transform.position) <= 1f)
            {
                isNear = true;
            }
        }

        private void CheckSlotAvailability()
        {
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory.IsSlotAvailable(item, amount))
            {
                isSlotAvailable = true;
            }
            else
            {
                isDelay = false;
                isNear = false;
                delay = 1f;
            }
        }

        private void MovingtoTarget()
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
            speed += 20f * Time.deltaTime;
        }

        private void AddingItem()
        {
            if (Vector2.Distance(this.transform.position, player.transform.position) <= 0.5f)
            {
                PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
                if (playerInventory != null)
                {
                    playerInventory.AddItem(item, amount);
                    Destroy(gameObject);
                }
            }
        }
    }
}