using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace QFramework.Gungeon
{
    public class BulletHelper
    {
        public static void ShootSpread(int count, Vector2 origin,Vector2 mainDirection ,float radius, EnemyBullet enemyBullet,float durationAngle)
        {

            var mainAngle = mainDirection.ToAngle();
            for (int i = 1; i <= count; i++)
            {
                var angle = mainAngle + i * durationAngle - (count / 2 + 1) * durationAngle;
                var direction = angle.AngleToDirection2D();
                var pos = origin + radius * direction.normalized;

                var bullet = Object.Instantiate(enemyBullet);
                bullet.transform.position = pos;
                bullet.velocity = direction * 5;
                bullet.gameObject.SetActive(true);
            }
        }
        public static void ShootAround(int count,Vector2 origin,float radius,EnemyBullet enemyBullet)
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
                bullet.velocity = direction * 5;
                bullet.gameObject.SetActive(true);
            }
        }
    }
}