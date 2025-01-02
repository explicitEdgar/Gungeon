using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class UImap : ViewController
	{
        private void OnEnable()
        {
            Time.timeScale = 0;
            MapItemRoot.DestroyChildren();

            Global.RoomGrid.ForEach((x, y, room) =>
            {
                if (room.State == Room.RoomStates.Unlocked)
                {
                    MapItem.InstantiateWithParent(MapItemRoot)
                    .WithData(room)
                    .LocalPosition(x * 60, y * 60)
                    .Show();
                }
            });
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }
    }
}
