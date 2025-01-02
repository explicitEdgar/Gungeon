using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

namespace QFramework.Gungeon
{
    public interface IEnemy
    {
        GameObject GameObject { get; }

        Room Room { get; set; }

        void Hurt(float damage, Vector2 hitDirection);
    }


    public abstract class Enemy : MonoBehaviour,IEnemy
    {
        protected void OnDeath(Vector2 hitDirection,string bodyName,float scale,string soundName = "EnemyDie")
        {
            FxFactory.Default.GeneratoEnemyBody(transform.Position2D(), hitDirection, bodyName, scale);

            AudioKit.PlaySound("Resources://" + soundName);

            var coin = PowerUpFactory.Default.Coin.Instantiate()
                .Position2D(transform.Position2D())
                .Show();

            Room.AddPowerUp(coin);

            Destroy(gameObject);
        }

        public GameObject GameObject => gameObject;

        public Room Room { get; set; }

        public abstract void Hurt(float damage, Vector2 hitDirection);
    }
}
