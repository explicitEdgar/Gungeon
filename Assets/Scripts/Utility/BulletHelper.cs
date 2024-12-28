using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace QFramework.Gungeon
{
    public class BulletHelper
    {   
        public static void Shoot(Vector2 pos,Vector2 direction,float speed,float damage)
        {
            var bullet = Object.Instantiate(BulletFactory.Default.PistolBullet);
            bullet.transform.position = pos;
            bullet.velocity = direction.normalized * speed;
            bullet.Damage = damage;
            bullet.gameObject.SetActive(true);
        }

        public static void ShootSpread(int count, Vector2 origin,Vector2 mainDirection ,float radius, EnemyBullet enemyBullet,float durationAngle,float speed = 5)
        {

            var mainAngle = mainDirection.ToAngle();
            for (int i = 1; i <= count; i++)
            {
                var angle = mainAngle + i * durationAngle - (count / 2 + 1) * durationAngle;
                var direction = angle.AngleToDirection2D();
                var pos = origin + radius * direction.normalized;

                var bullet = Object.Instantiate(enemyBullet);
                bullet.transform.position = pos;
                bullet.velocity = direction * speed;
                bullet.gameObject.SetActive(true);
            }
        }
        public static void ShootAround(int count,Vector2 origin,float radius,EnemyBullet enemyBullet,float speed = 5)
        {
            var durationAngle = 360 / count;

            var angleOffset = Random.Range(0, 360);
            for (int i = 0; i < count; i++)
            {
                var angle = angleOffset + i * durationAngle;
                var direction = angle.AngleToDirection2D();
                var pos = origin + radius * direction.normalized;

                var bullet = Object.Instantiate(enemyBullet);
                bullet.transform.position = pos;
                bullet.velocity = direction * speed;
                bullet.gameObject.SetActive(true);
            }
        }
    }
}