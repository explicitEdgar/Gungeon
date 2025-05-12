using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class MiningNode : MonoBehaviour, IDamageable
    {
        [Header("Core")]
        [SerializeField] int maxHealth;
        [SerializeField] GameObject itemDrop;
        [SerializeField] Item item;

        int health;

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

        public void Damage(PlayerInventory playerInventory, int damage)
        {
            if (playerInventory.CurrentlySelectedInventoryItem.Item.isPickaxe)
            {
                health -= damage;
                Debug.Log(gameObject.name + health);
            }
        }
    }
}
