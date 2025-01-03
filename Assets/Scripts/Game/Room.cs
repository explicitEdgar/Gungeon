using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Rendering;

namespace QFramework.Gungeon
{
	public partial class Room : ViewController
	{
        public enum RoomStates
        {
            Close,
            PlayerIn,
            Unlocked,
        }

        public RoomStates State { get; set; } = RoomStates.Close;

		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();
        private List<Door> doors = new List<Door>();
        public List<Door> Doors => doors;
        private HashSet<IEnemy> mEnemies = new HashSet<IEnemy>();
        public HashSet<IEnemy> Enemies => mEnemies;
        private List<EnemyWaveConfig> mWaves = new List<EnemyWaveConfig>();
        public EnemyWaveConfig curWave;
        public HashSet<IPowerUp> PowerUps = new HashSet<IPowerUp>();

        public RoomConfig Config { get; private set; }
        public Room WithConfig(RoomConfig roomConfig)
        {
            Config = roomConfig;
            return this;
        }

        public LevelController.RoomGenerateNode GenerateNode { get; set; }

        public void GenerateEnemy()
        {
            mWaves.RemoveAt(0);

            var enemyCount = UnityEngine.Random.Range(3, 5 + 1);

            var waveEnemyPositions = mEnemyGeneratePoses
                .OrderByDescending(e => (Player.Default.Position2D() - e.ToVector2()).magnitude)
                .Take(enemyCount).ToList();

            foreach (var enemyGeneratePose in waveEnemyPositions)
            {
                var newEnemy = Instantiate(LevelController.Default.Enemy.GameObject);
                var enemy = newEnemy.GetComponent<IEnemy>();

                newEnemy.transform.position = enemyGeneratePose;
                newEnemy.gameObject.SetActive(true);
                enemy.Room = this;

                mEnemies.Add(enemy);
            }
        }

        void Start()
		{
			if(Config.RoomType == RoomTypes.Init)
            {
                foreach(var door in Doors)
                {
                    door.State.ChangeState(Door.States.IdleOpen);
                }
            }
            else if(Config.RoomType == RoomTypes.Normal)
            {
                var waveCount = UnityEngine.Random.Range(1, 3 + 1);

                for(var i = 0;i < waveCount;i++)
                {
                    mWaves.Add(new EnemyWaveConfig());
                }
            }
		}

        private void Update()
        {
            if(Time.frameCount % 30 == 0)
            {

                if(mEnemies.Count == 0)
                {
                    if(State == RoomStates.PlayerIn)
                    {
                        if (mWaves.Count > 0)
                        {
                            GenerateEnemy();
                        }
                        else
                        {   
                            if(Config.RoomType == RoomTypes.Normal)
                            {
                                foreach(var powerUp in PowerUps.Where(p => p.GetType() == typeof(Coin)))
                                {
                                    var cachedPowerUp = powerUp;
                                    ActionKit.OnFixedUpdate.Register(() =>
                                    {
                                        cachedPowerUp.SpriteRenderer.transform.Translate
                                        (
                                            cachedPowerUp.SpriteRenderer.NormalizedDirection2DTo(Player.Default) * Time.fixedDeltaTime * 10
                                        );
                                    }).UnRegisterWhenGameObjectDestroyed(cachedPowerUp.SpriteRenderer.gameObject);
                                }
                            }

                            State = RoomStates.Unlocked;
                            foreach (var door in doors)
                            {   
                                door.State.ChangeState(Door.States.Open);
                            }
                        }

                    }
                }
            }
        }

        public void AddEnemyGeneratePos(Vector3 enemyGeneratePos)
		{
			mEnemyGeneratePoses.Add(enemyGeneratePos);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player") && Config.RoomType == RoomTypes.Normal)
			{
                Global.currentRoom = this;
                if (State == RoomStates.Close)
                {
                    State = RoomStates.PlayerIn;

                    GenerateEnemy();

                    foreach (var door in doors)
                    {
                        door.State.ChangeState(Door.States.BattleClose);
                    }
                }
            }
            else if(collision.CompareTag("Player"))
            {
                Global.currentRoom = this;
                if (State == RoomStates.Close)
                { 
                    State = RoomStates.Unlocked;
                }
            }
        }

        public void AddDoor(Door door)
        {
            doors.Add(door);
        }

        public void AddPowerUp(IPowerUp powerUp)
        {
            PowerUps.Add(powerUp);
            powerUp.Room = this;
        }
    }
}
