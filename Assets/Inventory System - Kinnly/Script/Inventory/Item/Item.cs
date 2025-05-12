using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kinnly
{
    [CreateAssetMenu(fileName = "item", menuName = "ScriptableObjects/item")]
    public class Item : ScriptableObject
    {
        [Header("Details")]
        public int id;
        public new string name;
        public string description;
        public int price;

        [Header("Assets")]
        public Sprite image;

        [Header("Toggles")]
        public bool isStackable;

        [Space(10)]
        [Header("Consumable")]
        public bool isConsumable;

        [Header("Buildings")]
        public bool isBuildable;
        public GameObject building;

        [Header("Tools")]
        public bool isTools;
        public bool isAxe;
        public bool isPickaxe;
    }
}