using UnityEngine;
using QFramework;
using TMPro;
using UnityEngine.UI;

namespace QFramework.Gungeon
{
	public partial class MapItem : ViewController
	{
		private Room mRoom;

		void Start()
		{
			Button.onClick.AddListener(() =>
			{
				FindAnyObjectByType<UImap>().Hide();
				Global.player.Position2D(mRoom.Position2D() + Vector2.one);
			});
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
                    break;
					
				case RoomTypes.Chest:
					if (mRoom.PowerUps.Count > 0)
					{
						foreach (var item in mRoom.PowerUps)
						{
                            bool has = false;
                            for (int i = 0; i < IconGroup.childCount; i++)
                            {
                                if (IconGroup.GetChild(i).GetComponent<Image>().sprite.name == item.SpriteRenderer.sprite.name)
                                {
                                    has = true;
                                    break;
                                }
                            }
                            if (has) continue;
                            Icon.InstantiateWithParent(IconGroup)
								.Self(self =>
								{
									self.sprite = item.SpriteRenderer.sprite;
								})
								.Show();
						}
						IconGroup.Show();
                    }
					else
					{
						IconGroup.Hide();
					}
                    break;

                case RoomTypes.Normal:
                    if (mRoom.PowerUps.Count > 0)
                    {
                        foreach (var item in mRoom.PowerUps)
                        {
                            bool has = false;
							for(int i = 0;i < IconGroup.childCount;i++)
							{
								if (IconGroup.GetChild(i).GetComponent<Image>().sprite.name == item.SpriteRenderer.sprite.name)
								{
									has = true;
									break;
								}
                            }
							if (has) continue;
                            Icon.InstantiateWithParent(IconGroup)
                                .Self(self =>
                                {
                                    self.sprite = item.SpriteRenderer.sprite;
                                })
                                .Show();
                        }
                        IconGroup.Show();
                    }
                    else
                    {
                        IconGroup.Hide();
                    }
                    break;

                default:

					TypeText.Hide();
					break;

			}
			
			if(Global.currentRoom == mRoom)
			{
				TypeText.text = "我在这";
				TypeText.Show();
			}
		}
	}
}
