using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System;

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
        private HashSet<Enemy> mEnemies = new HashSet<Enemy>();
        private List<EnemyWaveConfig> mWaves = new List<EnemyWaveConfig>()
        {
            new EnemyWaveConfig(),
            new EnemyWaveConfig(),
            new EnemyWaveConfig(),
        };
        public EnemyWaveConfig curWave;


        public RoomConfig Config { get; private set; }
        public Room WithConfig(RoomConfig roomConfig)
        {
            Config = roomConfig;
            return this;
        }

        public void GenerateEnemy()
        {
            mWaves.RemoveAt(0);
            foreach (var enemyGeneratePose in mEnemyGeneratePoses)
            {
                var newEnemy = Instantiate(LevelController.Default.enemy);
                newEnemy.transform.position = enemyGeneratePose;
                newEnemy.gameObject.SetActive(true);

                mEnemies.Add(newEnemy);
            }
        }

        void Start()
		{
			// Code 
		}

        private void Update()
        {
            if(Time.frameCount % 30 == 0)
            {
                mEnemies.RemoveWhere(e => !e);

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
                            State = RoomStates.Unlocked;
                            foreach (var door in doors)
                            {
                                door.Hide();
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
                if (State == RoomStates.Close)
                {
                    State = RoomStates.PlayerIn;

                    GenerateEnemy();

                    foreach (var door in doors)
                    {
                        door.Show();
                    }
                }
            }
        }

        public void AddDoor(Door door)
        {
            doors.Add(door);
        }
    }
}
