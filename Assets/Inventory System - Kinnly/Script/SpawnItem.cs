using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kinnly
{
    public class SpawnItem : MonoBehaviour
    {
        [SerializeField] GameObject itemDrop;
        [SerializeField] Item[] item;

        private Player player;

        private void Start()
        {
            player = Player.Instance;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawn(1);
            }
        }

        public void Spawn(int amount)
        {
            Item item = this.item[Random.Range(0, this.item.Length)];
            GameObject go = Instantiate(itemDrop, player.transform.position + new Vector3(RandomNumber(-3f, 3f, -1f, 1f), RandomNumber(-3f, 3f, -1f, 1f), 0f), transform.rotation);
            go.GetComponent<SpriteRenderer>().sprite = item.image;
            go.GetComponent<ItemDrop>().SetItem(item, amount);
        }

        private float RandomNumber(float minRange, float maxRange, float minExclude, float maxExclude)
        {
            float randomValue;
            do
            {
                randomValue = Random.Range(minRange, maxRange);
            }
            while (randomValue <= minExclude && randomValue >= maxExclude);

            return randomValue;
        }
    }
}
