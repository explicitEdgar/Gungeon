using UnityEngine;
using QFramework;
using static QFramework.Gungeon.LevelController;

namespace QFramework.Gungeon
{
	public partial class Door : ViewController
	{	
		public enum States
		{   
            IdleOpen,
			IdleClose,
			Open,
            BattleClose,
		}
		public int X { get; set; }
		public int Y { get; set; }
		public DoorDirections Direction { get; set; }

		public FSM<States> State = new FSM<States>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && State.CurrentStateId == States.IdleClose)
            {
                State.ChangeState(States.Open);
            }
        }

        private void Awake()
        {
            State.State(States.IdleOpen)
               .OnEnter(() =>
               {
                   SelfBoxCollider2DWithBullet.Disable();
                   SelfBoxCollider2D.Disable();
                   SelfSpriteRenderer.sprite = DoorOpen;
               });
            State.State(States.IdleClose)
                .OnEnter(() =>
                {
                    SelfBoxCollider2DWithBullet.Enable();
                    SelfBoxCollider2D.isTrigger = true;
                    SelfSpriteRenderer.sprite = DoorClose;
                });
            State.State(States.Open)
				.OnEnter(() =>
				{
					AudioKit.PlaySound("Resources://DoorOpen");
                    SelfBoxCollider2D.Enable(false);
                    SelfBoxCollider2DWithBullet.Disable();
                    SelfSpriteRenderer.sprite = DoorOpen;
				});
            State.State(States.BattleClose)
                .OnEnter(() =>
                {
                    AudioKit.PlaySound("Resources://DoorOpen");
                    SelfBoxCollider2D.isTrigger = false;
                    SelfBoxCollider2D.Enable(true);
                    SelfSpriteRenderer.sprite = DoorClose;
                });

            State.StartState(States.IdleClose);
        }
    }
}
