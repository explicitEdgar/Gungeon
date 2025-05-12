using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinnly
{
    public class PlayerEvents : MonoBehaviour
    {
        PlayerInventory playerInventory;
        PlayerMovement playerMovement;
        PlayerTools playerTools;

        // Start is called before the first frame update
        private void Start()
        {
            playerInventory = Player.Instance.gameObject.GetComponent<PlayerInventory>();
            playerMovement = Player.Instance.gameObject.GetComponent<PlayerMovement>();
            playerTools = Player.Instance.gameObject.GetComponentInChildren<PlayerTools>();
        }

        public void UseTools()
        {
            playerTools.DamageInsideTrigger();
        }

        public void SetUsingToolsTrue()
        {
            playerMovement.isUsingTools = true;
        }

        public void SetUsingToolsFalse()
        {
            playerMovement.isUsingTools = false;
        }

        public void ToggleInventoryEvent()
        {
            playerInventory.ToggleInventory();
        }
    }
}
