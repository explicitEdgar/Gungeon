using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;
using System.Linq;

namespace QFramework.Gungeon
{
    public interface IEnemy
    {
        GameObject GameObject { get; }

        Room Room { get; set; }

        void Hurt(float damage, Vector2 hitDirection);
    }


    public abstract class Enemy : MonoBehaviour,IEnemy
    {
        protected void OnDeath(Vector2 hitDirection,string bodyName,float scale,string soundName = "EnemyDie")
        {   
            if(bodyName.IsNotNullAndEmpty())
            {
                FxFactory.Default.GeneratoEnemyBody(transform.Position2D(), hitDirection, bodyName, scale);
            }
            
            AudioKit.PlaySound("Resources://" + soundName);

            PowerUpFactory.GeneratePowerUp(this);

            Destroy(gameObject);
        }

        public GameObject GameObject => gameObject;

        public Room Room { get; set; }

        public SpriteRenderer SpriteRenderer => gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();

        public enum States
        {
            FollowPlayer,
            PrepareShoot,
            Shoot,
        }

        public FSM<States> State = new FSM<States>();

        public Rigidbody2D Rigidbody2D => gameObject.GetComponent<Rigidbody2D>();

        public abstract void Hurt(float damage, Vector2 hitDirection);

        public List<PathFindingHelper.NodeBase<Vector3Int>> MovementPath = new List<PathFindingHelper.NodeBase<Vector3Int>>();

        public virtual bool isBoss => false;

        //可空变量
        public Vector2? posToMove = null;
        protected Vector2 Move(float velocity = 1)
        {
            if (posToMove == null)
            {
                if (MovementPath.Count > 0)
                {
                    //从路径中拿到下一个位置
                    var pathPos = MovementPath.Last().Coords.Pos;
                    posToMove = new Vector2(pathPos.x + 0.5f, pathPos.y + 0.5f);
                    MovementPath.RemoveAt(MovementPath.Count - 1);
                }
            }

            var directionToPlayer = Player.Default.NormalizedDirectionFrom(transform);

            //有要移动的就进行A*寻路的移动，没有就直线寻路
            if (posToMove == null)
            {
                Rigidbody2D.velocity = directionToPlayer * velocity;
            }
            else
            {
                var direction = posToMove.Value - transform.Position2D();
                Rigidbody2D.velocity = direction.normalized * velocity;

                if (direction.magnitude < 0.2f)
                {
                    posToMove = null;
                }
            }

            return directionToPlayer;
        }

        protected void FollowPlayer(float velocity = 1f)
        {
            if (Global.player)
            {

                if (MovementPath.Count == 0)
                {
                    var grid = LevelController.Default.wallMap.layoutGrid;
                    var myCellPos = grid.WorldToCell(transform.position);
                    var playerCellPos = grid.WorldToCell(Player.Default.Position());
                    PathFindingHelper.FindPath(Room.PathFindingGrid[myCellPos.x, myCellPos.y],
                        Room.PathFindingGrid[playerCellPos.x, playerCellPos.y], MovementPath);
                }

                var direction2Player = Move(velocity);
                AnimationHelper.UpDownAnimation(SpriteRenderer, 0.05f, State.FrameCountOfCurrentState, 10);
                AnimationHelper.RotateAnimation(SpriteRenderer, 5, State.FrameCountOfCurrentState, 30);

                if (direction2Player.x < 0)
                {
                    SpriteRenderer.flipX = true;
                }
                else
                {
                    SpriteRenderer.flipX = false;
                }
            }
        }
    }
}
