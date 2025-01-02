using UnityEngine;
using QFramework;
using TMPro;

namespace QFramework.Gungeon
{
	public partial class MapItem : ViewController
	{
		private Room mRoom;

		void Start()
		{
			// Code Here
		}

		public MapItem WithData(Room room)
		{
			mRoom = room;
			UpdateView();
			return this;
		}

		private void UpdateView()
		{
			LeftDoor.Hide();
			RightDoor.Hide();
			UpDoor.Hide();
			DownDoor.Hide();

			foreach(var direction in mRoom.GenerateNode.Directions)
			{
				if(direction == LevelController.DoorDirections.Up)
				{
					UpDoor.Show();
				}
                if (direction == LevelController.DoorDirections.Down)
                {
                    DownDoor.Show();
                }
                if (direction == LevelController.DoorDirections.Left)
                {
                    LeftDoor.Show();
                }
                if (direction == LevelController.DoorDirections.Right)
                {
                    RightDoor.Show();
                }
            }

			switch (mRoom.GenerateNode.Node.RoomType)
			{
				case RoomTypes.Init:
					
					TypeText.text = "初始房";
                    TypeText.Show();
					Debug.Log(1);
                    break;
					
				case RoomTypes.Chest:

					TypeText.text = "宝箱房";
                    TypeText.Show();
                    break;

				default:

					TypeText.Hide();
					break;

			}

			IconGroup.Hide();
			
			if(Global.currentRoom == mRoom)
			{
				TypeText.text = "我在这";
				TypeText.Show();
				Debug.Log(2);
			}
		}
	}
}
