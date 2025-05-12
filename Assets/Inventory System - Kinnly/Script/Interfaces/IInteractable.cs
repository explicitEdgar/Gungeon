using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinnly
{
    public interface IInteractable
    {
        void Interact(PlayerInventory playerInventory);
    }
}
