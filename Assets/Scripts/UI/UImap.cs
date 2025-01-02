using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class UImap : ViewController
	{
        private void OnEnable()
        {
            Global.UIOpened = true;
            Time.timeScale = 0;
            MapItemRoot.DestroyChildren();

            Global.RoomGrid.ForEach((x, y, room) =>
            {
                if (room.State == Room.RoomStates.Unlocked)
                {
                    MapItem
                    .InstantiateWithParent(MapItemRoot)
                    .WithData(room)
                    .LocalPosition(x * 60, y * 60)
                    .Show();
                }

                if(room == Global.currentRoom)
                {
                    MapItemRoot.LocalPosition(-x * 60, -y * 60);
                }
            });
        }

        private void OnDisable()
        {
            Global.UIOpened = false;
            Time.timeScale = 1;
        }
    }
}
