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
        public static EasyEvent<Room> OnRoomEnter = new EasyEvent<Room>();
		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();
        private List<Door> doors = new List<Door>();
        public List<Door> Doors => doors;
        private HashSet<IEnemy> mEnemies = new HashSet<IEnemy>();
        public HashSet<IEnemy> Enemies => mEnemies;
        private List<EnemyWaveConfig> mWaves = new List<EnemyWaveConfig>();
        public EnemyWaveConfig curWave;
        public HashSet<IPowerUp> PowerUps = new HashSet<IPowerUp>();
        public int ColorIndex { get; set; } = -1;

        public RoomConfig Config { get; private set; }
        public Room WithConfig(RoomConfig roomConfig)
        {
            Config = roomConfig;
            return this;
        }

        public LevelController.RoomGenerateNode GenerateNode { get; set; }

        public void GenerateEnemy(EnemyWaveConfig waveConfig)
        {   
            mWaves.RemoveAt(0);

            var enemyCount = waveConfig.EnemyNames.Count;

            var waveEnemyPositions = mEnemyGeneratePoses
                .OrderByDescending(e => (Player.Default.Position2D() - e.ToVector2()).magnitude)
                .Take(enemyCount).ToList();

            foreach (var enemyName in waveConfig.EnemyNames)
            {
                //根据名字生成敌人
                var enemy = EnemyFactory.EnemyByName(enemyName)
                    .GameObject.Instantiate()
                    .Position2D(waveEnemyPositions.GetAndRemoveRandomItem())
                    .Show()
                    .GetComponent<IEnemy>();

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
                        //还有波次
                        if (mWaves.Count > 0)
                        {
                            var wave = mWaves.First();
                            GenerateEnemy(wave);
                        }
                        else
                        {   
                            //打完结束吃金币
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

                            //改房间状态，开门
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
            OnRoomEnter.Trigger(this);
            if(collision.CompareTag("Player") && Config.RoomType == RoomTypes.Normal)
			{
                Global.currentRoom = this;
                if (State == RoomStates.Close)
                {
                    State = RoomStates.PlayerIn;

                    //根据难度节奏动态配置波次
                    var difficultyLevel = Global.CurrentPacing.Dequeue();
                    var difficultyScore = 10 + difficultyLevel * 3;

                    //波次数量
                    int waveCount;
                    if (difficultyLevel <= 3)
                    {
                        waveCount = UnityEngine.Random.Range(1, difficultyLevel + 1);
                    }
                    else
                    {
                        waveCount = UnityEngine.Random.Range(difficultyLevel / 3, difficultyLevel / 2);
                    }

                    //每波分别配置
                    for(int i = 0;i < waveCount;i++)
                    {   
                        //当前波次目标配置分数
                        var targetScore = difficultyScore / waveCount + UnityEngine.Random.Range(-difficultyScore / 10 * 2 + 1, 
                            difficultyScore / 20 * 2 + 1 + 1);
                        var waveConfig = new EnemyWaveConfig();

                        //添加敌人
                        while(targetScore > 0 && waveConfig.EnemyNames.Count < mEnemyGeneratePoses.Count)
                        {
                            var enemyScore = EnemyFactory.GenTargetEnemyScore();
                            targetScore -= enemyScore;
                            waveConfig.EnemyNames.Add(EnemyFactory.EnemyNameByScore(enemyScore));
                        }

                        mWaves.Add(waveConfig);
                    }

                    var wave = mWaves.First();
                    GenerateEnemy(wave);

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
