using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class PlayerBuild : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] Camera mainCamera;

        [Header("Direction")]
        [SerializeField] Vector2 offsetUp;
        [SerializeField] Vector2 offsetDown;
        [SerializeField] Vector2 offsetLeft;
        [SerializeField] Vector2 offsetRight;

        //If isMouseControl Active, collider will be based on MousePosition if it's near the player.
        [Header("Mouse Control")]
        [SerializeField] bool isMouseControl;
        [SerializeField] Vector2 mouseControlSize;
        [SerializeField] Vector2 mouseControlOffset;

        [Header("Outline")]
        [SerializeField] GameObject outline;

        GameObject player;
        PlayerMovement playerMovement;
        PlayerInventory playerInventory;

        BoxCollider2D boxCollider;
        SpriteRenderer spriteRenderer;

        Item item;
        bool isBuildable;
        bool isMouseNear;

        int xSize;
        int ySize;

        // Start is called before the first frame update
        void Start()
        {
            player = Player.Instance.gameObject;
            playerMovement = Player.Instance.gameObject.GetComponent<PlayerMovement>();
            playerInventory = Player.Instance.gameObject.GetComponent<PlayerInventory>();

            boxCollider = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            xSize = 1;
            ySize = 1;
        }

        // Update is called once per frame
        void Update()
        {
            Direction();
            MouseControl();
            OnDrawOutline();

            InventoryItem inventoryItem = playerInventory.CurrentlySelectedInventoryItem;
            if (inventoryItem == null || inventoryItem.Item.isBuildable == false)
            {
                isBuildable = true;
                ResetItem();
                return;
            }
            else
            {
                SetItem(inventoryItem.Item);
                if (Input.GetMouseButtonDown(0))
                {
                    Build(inventoryItem);
                }
            }

            if (isBuildable)
            {
                spriteRenderer.color = new Color(0, 255, 0, 75);
            }
            else
            {
                spriteRenderer.color = new Color(255, 0, 0, 75);
            }
        }

        void Direction()
        {
            if (isMouseNear)
            {
                return;
            }

            Vector3 playerPosition = player.transform.position;
            Vector2 direction = playerMovement.direction;

            Vector2 _position;
            if (direction == Vector2.up)
            {
                _position = new Vector3(offsetUp.x, direction.y * offsetUp.y) + playerPosition;
            }
            else if (direction == Vector2.down)
            {
                _position = new Vector3(offsetDown.x, direction.y * offsetDown.y) + playerPosition;
            }
            else if (direction == Vector2.left)
            {
                _position = new Vector3(direction.x * offsetLeft.x, offsetLeft.y) + playerPosition;
            }
            else if (direction == Vector2.right)
            {
                _position = new Vector3(direction.x * offsetRight.x, offsetRight.y) + playerPosition;
            }
            else
            {
                _position = new Vector3(direction.x, direction.y) + playerPosition;
            }
            transform.position = new Vector2(Mathf.Round(_position.x), Mathf.Round(_position.y));
        }

        void MouseControl()
        {
            if (isMouseControl == false)
            {
                return;
            }

            Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 _playerPosition = player.transform.position + new Vector3(mouseControlOffset.x, mouseControlOffset.y, 0f);
            _mousePosition.z = 0;

            float _distanceX = _mousePosition.x - _playerPosition.x;
            float _distanceY = _mousePosition.y - _playerPosition.y;
            Vector2 _distance = new Vector2(_distanceX, _distanceY);

            if (Mathf.Abs(_distance.x) < mouseControlSize.x + mouseControlOffset.x && Mathf.Abs(_distance.y) < mouseControlSize.y + mouseControlOffset.y)
            {
                isMouseNear = true;
                float xPos = _mousePosition.x;
                float yPos = _mousePosition.y;
                transform.position = new Vector2(Mathf.Round(xPos), Mathf.Round(yPos));
            }
            else
            {
                isMouseNear = false;
            }
        }

        void SetItem(Item item)
        {
            this.item = item;
            spriteRenderer.sprite = item.image;
            boxCollider.size = new Vector2(xSize, ySize);
        }

        void ResetItem()
        {
            this.item = null;
            spriteRenderer.sprite = null;
            boxCollider.size = Vector2.one;
            transform.position = Vector2.zero;
        }

        void Build(InventoryItem inventoryItem)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (isBuildable == false)
            {
                return;
            }

            playerInventory.RemoveItem(inventoryItem, 1);
            GameObject go = Instantiate(inventoryItem.Item.building, transform.position, Quaternion.identity);
            ResetItem();
        }

        private void OnDrawOutline()
        {
            if (item != null)
            {
                outline.SetActive(true);
                outline.transform.position = transform.position;
            }
            else
            {
                outline.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            isBuildable = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            isBuildable = false;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            isBuildable = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (isMouseControl == false)
            {
                return;
            }

            player = GetComponentInParent<Player>().gameObject;

            Vector3 _playerPosition = player.transform.position + new Vector3(mouseControlOffset.x, mouseControlOffset.y, 0f);
            Vector3 _size = new Vector3(mouseControlSize.x * 2, mouseControlSize.y * 2, 1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_playerPosition, _size);
        }
    }
}
