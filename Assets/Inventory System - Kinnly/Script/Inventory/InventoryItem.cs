using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kinnly
{
    public class InventoryItem : MonoBehaviour, IPointerClickHandler
    {
        [Header("Core")]
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Image image;
        [SerializeField] TMP_Text amountText;

        [Header("Item")]
        public Item Item;
        public int Amount;

        //Local isDragging to Check which Inventory Item Currently Dragged.
        //While PlayerInvetory.isDragging is Global to check if player currently dragging something.
        [HideInInspector] public bool IsDragging;

        GameObject player;
        PlayerInventory playerInventory;

        private int maxAmount;

        // Start is called before the first frame update
        void Start()
        {
            maxAmount = playerInventory.MaxAmount;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUI();

            if (IsDragging)
            {
                // Follow the mouse position
                transform.position = Input.mousePosition;

                // Input to Cancel Dragging
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelDragging();
                }

                //Left Click to Put Dragged Item on the Slot
                if (Input.GetMouseButtonDown(0))
                {
                    HandleItemDrop();
                }

                //Right Click to Drag more Items from the Slot
                if (Input.GetMouseButtonDown(1))
                {
                    AddSplitAmount(1);
                }
            }
        }

        public void SetItem(Item item, int amount)
        {
            this.Item = item;
            this.Amount = amount;
            this.image.sprite = Item.image;
            this.IsDragging = false;

            player = Player.Instance.gameObject;
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (playerInventory.IsDragging)
            {
                return;
            }

            if (playerInventory.IsClicking)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left && IsDragging == false)
            {
                StartDragging();
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                SplitAmount();
            }
        }

        public void AddAmount(int amount)
        {
            this.Amount += amount;
            UpdateUI();
        }

        public void RemoveAmount(int amount)
        {
            this.Amount -= amount;
            UpdateUI();
        }

        public void SplitAmount()
        {
            if (playerInventory.IsDragging)
            {
                return;
            }

            playerInventory.SpawnInventoryItem(Item, 1);
            RemoveAmount(1);
            UpdateUI();
        }

        public void AddSplitAmount(int amount)
        {
            if (playerInventory.CurrentlyHoveredInventorySlot != null)
            {
                InventoryItem hoveredItem = playerInventory.CurrentlyHoveredInventorySlot.GetComponentInChildren<InventoryItem>();
                if (hoveredItem != null)
                {
                    if (hoveredItem.Item.name == this.Item.name && this.Amount <= maxAmount)
                    {
                        hoveredItem.RemoveAmount(amount);
                        AddAmount(amount);
                    }
                }
            }
        }

        private void UpdateUI()
        {
            if (Amount <= 0)
            {
                Destroy(this.gameObject);
            }
            else if (Amount == 1)
            {
                amountText.gameObject.SetActive(false);
            }
            else
            {
                amountText.gameObject.SetActive(true);
                amountText.GetComponent<TMP_Text>().text = Amount.ToString();
            }
        }

        private void StartDragging()
        {
            IsDragging = true;
            playerInventory.IsDragging = true;
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }

        private void CancelDragging()
        {
            playerInventory.SpawnItemDrop(Item, Amount);
            playerInventory.IsDragging = false;
            Destroy(this.gameObject);
        }

        private void HandleItemDrop()
        {
            if (playerInventory.IsClicking)
            {
                return;
            }

            if (playerInventory.CurrentlyHoveredInventorySlot != null)
            {
                InventoryItem hoveredItem = playerInventory.CurrentlyHoveredInventorySlot.GetComponentInChildren<InventoryItem>();

                if (hoveredItem != null)
                {
                    if (hoveredItem.Item.name == this.Item.name && Item.isStackable && hoveredItem.Amount + this.Amount <= maxAmount)
                    {
                        hoveredItem.AddAmount(this.Amount);
                        Destroy(this.gameObject);
                        playerInventory.IsDragging = false;
                    }
                    else if (hoveredItem.Item.name == this.Item.name && Item.isStackable && hoveredItem.Amount + this.Amount > maxAmount)
                    {
                        int _excess = maxAmount - hoveredItem.Amount;
                        hoveredItem.AddAmount(_excess);
                        this.Amount = this.Amount - _excess;
                    }
                    else
                    {
                        SwapItems(hoveredItem);
                    }
                }
                else
                {
                    PlaceInEmptySlot();
                }
            }

            if (playerInventory.IsHoveringDropBox == true)
            {
                playerInventory.SpawnItemDrop(Item, Amount);
                playerInventory.IsDragging = false;
                Destroy(this.gameObject);
            }

            if (playerInventory.IsHoveringTrashcan == true)
            {
                Destroy(this.gameObject);
                playerInventory.IsDragging = false;
            }

            playerInventory.IsClicking = true;
        }

        private void SwapItems(InventoryItem hoveredItem)
        {
            Debug.Log(Item.name + " : Switching");

            // Place the current item in the hovered slot
            transform.SetParent(playerInventory.CurrentlyHoveredInventorySlot.transform);
            rectTransform.localPosition = Vector3.zero;
            image.raycastTarget = true;
            IsDragging = false;

            // Set the hovered item to be dragged
            hoveredItem.IsDragging = true;
            hoveredItem.transform.SetParent(transform.root);
            hoveredItem.image.raycastTarget = false;

            playerInventory.IsDragging = true;
        }

        private void PlaceInEmptySlot()
        {
            transform.SetParent(playerInventory.CurrentlyHoveredInventorySlot.transform);
            rectTransform.localPosition = Vector3.zero;
            image.raycastTarget = true;
            IsDragging = false;
            playerInventory.IsDragging = false;
        }
    }
}