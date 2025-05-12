using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class LoggingNode : MonoBehaviour, IInteractable, IDamageable
    {
        [Header("Core")]
        [SerializeField] float maxHealth;
        [SerializeField] GameObject itemDrop;
        [SerializeField] Item item;

        Item currentlySelectedItem;
        float health;

        private void Start()
        {
            health = maxHealth;
        }

        void Update()
        {
            if (health <= 0)
            {
                for (int i = 0; i < Random.Range(1, 4); i++)
                {
                    Vector3 direction = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    GameObject go = Instantiate(itemDrop, transform.position + direction, Quaternion.identity);
                    go.GetComponent<ItemDrop>().SetItem(item, 1);
                }

                Destroy(gameObject);
            }
        }

        public void Interact(PlayerInventory playerInventory)
        {

        }

        public void Damage(PlayerInventory playerInventory, int damage)
        {
            if (playerInventory.CurrentlySelectedInventoryItem.Item.isAxe)
            {
                health -= damage;
            }
        }
    }
}