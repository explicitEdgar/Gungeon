using System.Collections.Generic;

namespace QFramework.Gungeon
{
    public class ShopSystem
    {
        public static List<Tuple<IPowerUp,int>> CalculateNormalShopItems()
        {
            var normalShopItem = new List<Tuple<IPowerUp, int>>()
            {
                new (PowerUpFactory.Default.ArmorDroped,UnityEngine.Random.Range(3,6 + 1)),
                new (PowerUpFactory.Default.Hp1,UnityEngine.Random.Range(3,6 + 1)),
                new (PowerUpFactory.Default.AllBulletHalf,UnityEngine.Random.Range(15,20 + 1)),
                new (PowerUpFactory.Default.SingleFullBullet,UnityEngine.Random.Range(5,10 + 1)),
                new (PowerUpFactory.Default.Key,UnityEngine.Random.Range(3,6 + 1)),
            };

            return normalShopItem;
        }
    }
}