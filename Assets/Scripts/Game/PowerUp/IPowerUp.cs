using UnityEngine;

namespace QFramework.Gungeon
{
    public interface IPowerUp
    {   
        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer { get; }
    }
}