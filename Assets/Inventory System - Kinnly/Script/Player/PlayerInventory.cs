using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Kinnly
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] GameObject inventoryUI;
        [SerializeField] GameObject toolbarUI;
        [SerializeField] List<GameObject> inventorySlot = new List<GameObject>();
        [SerializeField] List<GameObject> toolbarSlot = new List<GameObject>();

        [Header("Prefabs")]
        [SerializeField] GameObject inventoryItem;
        [SerializeField] GameObject toolbarItem;
        [SerializeField] GameObject itemDrop;

        [Header("Config")]
        public int MaxAmount;

        [HideInInspector] public GameObject CurrentlyHoveredInventorySlot;
        [HideInInspector] public GameObject CurrentlyHoveredToolbarSlot;
        [HideInInspector] public InventoryItem CurrentlySelectedInventoryItem;
        [HideInInspector] public int CurrentlySelectedToolBar;
        [HideInInspector] public bool IsHoveringDropBox;
        [HideInInspector] public bool IsHoveringTrashcan;
        [HideInInspector] public bool IsDragging;
        [HideInInspector] public bool IsClicking;

        private DialogBox dialogBox;

        // Start is called before the first frame update
        void Start()
        {
            MaxAmount = 999;

            CurrentlySelectedToolBar = 0;
            dialogBox = DialogBox.instance;
        }

        // Update is called once per frame
        void Update()
        {
            //Num Key to switch selected Toolbar
            int keyNumber = GetKeyNumber();
            if (keyNumber != -1)
            {
                CurrentlySelectedToolBar = keyNumber;
            }

            //Mouse Scroll to switch selected Toolbar
            if (Input.mouseScrollDelta.y < 0)
            {
                CurrentlySelectedToolBar += 1;
                if (CurrentlySelectedToolBar > 11)
                {
                    CurrentlySelectedToolBar -= 12;
                }
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                CurrentlySelectedToolBar -= 1;
                if (CurrentlySelectedToolBar < 0)
                {
                    CurrentlySelectedToolBar += 12;
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventory();
            }

            if (Input.GetMouseButtonUp(0))
            {
                IsClicking = false;
            }

            UpdateCurrentlySelectedItem();
        }

        public void ToggleInventory()
        {
            if (IsDragging)
            {
                return;
            }

            if (inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(false);
                toolbarUI.SetActive(true);
                UpdateToolbar();
            }
            else
            {
                inventoryUI.SetActive(true);
                toolbarUI.SetActive(false);
            }
        }

        public void AddItem(Item item, int amount)
        {
            foreach (GameObject slot in inventorySlot)
            {
                if (slot.gameObject.transform.childCount >= 1)
                {
                    InventoryItem inventoryItem = slot.gameObject.GetComponentInChildren<InventoryItem>();
                    if (inventoryItem.Item.name == item.name && inventoryItem.Item.isStackable && inventoryItem.Amount < MaxAmount)
                    {
                        int total = inventoryItem.Amount + amount;
                        if (total <= MaxAmount)
                        {
                            inventoryItem.AddAmount(amount);
                            UpdateToolbar();
                            return;
                        }
                        else
                        {
                            inventoryItem.AddAmount(amount - (total - MaxAmount));
                            SpawnItemDrop(item, total - MaxAmount);
                            UpdateToolbar();
                            return;
                        }
                    }
                }
            }

            foreach (GameObject slot in inventorySlot)
            {
                if (slot.gameObject.transform.childCount <= 0)
                {
                    GameObject go = Instantiate(inventoryItem, slot.transform);
                    go.GetComponent<InventoryItem>().SetItem(item, amount);
                    UpdateToolbar();
                    return;
                }
            }

            SpawnItemDrop(item, amount);
        }

        public void RemoveItem(InventoryItem inventoryItem, int amount)
        {
            inventoryItem.RemoveAmount(amount);
            UpdateToolbar();
        }

        public bool IsSlotAvailable(Item item, int amount)
        {
            foreach (GameObject slot in inventorySlot)
            {
                if (slot.gameObject.transform.childCount >= 1)
                {
                    InventoryItem inventoryItem = slot.gameObject.GetComponentInChildren<InventoryItem>();
                    if (inventoryItem.Item.name == item.name && inventoryItem.Item.isStackable && inventoryItem.Amount < MaxAmount)
                    {
                        return true;
                    }
                }
            }

            foreach (GameObject slot in inventorySlot)
            {
                if (slot.gameObject.transform.childCount <= 0)
                {
                    return true;
                }
            }

            dialogBox.Show("Inventory Full", 1f);
            return false;
        }

        public void SpawnItemDrop(Item item, int amount, float minRange = -3f, float maxRange = 3f, float minExclude = -1f, float maxExclude = 1f)
        {
            GameObject go = Instantiate(itemDrop, transform.position + new Vector3(RandomNumber(minRange, maxRange, minExclude, maxExclude), RandomNumber(minRange, maxRange, minExclude, maxExclude), 0f), transform.rotation);
            go.GetComponent<SpriteRenderer>().sprite = item.image;
            go.GetComponent<ItemDrop>().SetItem(item, amount);
        }

        public void SpawnInventoryItem(Item item, int amount)
        {
            GameObject go = Instantiate(inventoryItem, inventoryUI.transform.root);
            InventoryItem inventory = go.GetComponent<InventoryItem>();
            inventory.SetItem(item, amount);
            inventory.IsDragging = true;
            inventory.Amount = amount;
            go.GetComponent<Image>().raycastTarget = false;
            this.IsDragging = true;
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

        private int GetKeyNumber()
        {
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
                {
                    return i;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                return 9;
            }
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                return 10;
            }
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                return 11;
            }
            return -1;
        }

        private void UpdateToolbar()
        {
            for (int i = 0; i < toolbarSlot.Count; i++)
            {
                ToolbarItem[] components = toolbarSlot[i].GetComponentsInChildren<ToolbarItem>();
                foreach (var component in components)
                {
                    Destroy(component.gameObject);
                }

                if (inventorySlot[i].gameObject.transform.childCount >= 1)
                {
                    GameObject go = Instantiate(toolbarItem, toolbarSlot[i].transform);
                    InventoryItem inventoryItem = inventorySlot[i].GetComponentInChildren<InventoryItem>();
                    go.GetComponent<ToolbarItem>().SetItem(inventoryItem.Item, inventoryItem.Amount);
                }
            }
            UpdateCurrentlySelectedItem();
        }

        private void UpdateCurrentlySelectedItem()
        {
            try
            {
                CurrentlySelectedInventoryItem = inventorySlot[CurrentlySelectedToolBar].GetComponentInChildren<InventoryItem>();
            }
            catch
            {
                CurrentlySelectedInventoryItem = null;
            }
        }
    }
}