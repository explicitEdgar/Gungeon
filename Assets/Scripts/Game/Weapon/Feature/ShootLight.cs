using UnityEngine;

namespace QFramework.Gungeon
{
    public class ShootLight
    {
        public void ShowLight(Vector2 pos,Vector2 direction)
        {
            Player.Default.GunShootLight.Position2D(pos);
            Player.Default.GunShootLight.transform.right = direction;
            Player.Default.GunShootLight.Show();

            ActionKit.DelayFrame(3, () =>
            {
                Player.Default.GunShootLight.Hide();
            }).StartCurrentScene();
        }
    }
}