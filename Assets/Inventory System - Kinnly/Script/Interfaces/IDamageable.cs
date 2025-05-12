using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinnly
{
    public interface IDamageable
    {
        void Damage(PlayerInventory playerInventory, int damage);
    }
}
