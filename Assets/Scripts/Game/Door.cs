using UnityEngine;
using QFramework;
using static QFramework.Gungeon.LevelController;

namespace QFramework.Gungeon
{
	public partial class Door : ViewController
	{	
		public enum States
		{
			Open,
			Close,
		}
		public int X { get; set; }
		public int Y { get; set; }
		public DoorDirections Direction { get; set; }

		public FSM<States> State = new FSM<States>();

        private void Awake()
        {
			State.State(States.Open)
				.OnEnter(() =>
				{
					GetComponent<BoxCollider2D>().isTrigger = true;
					GetComponent<SpriteRenderer>().sprite = DoorOpen;
				})
                .OnExit(() =>
                {
                    AudioKit.PlaySound("Resources://DoorOpen");
                });

            State.State(States.Close)
                .OnEnter(() =>
                {
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    GetComponent<SpriteRenderer>().sprite = DoorClose;
                })
				.OnExit(() =>
				{
					AudioKit.PlaySound("Resources://DoorOpen");
				});

			State.StartState(States.Open);
        }
    }
}
