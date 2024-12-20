using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Tilemaps;

namespace QFramework.Gungeon
{
    public partial class LevelController : ViewController
    {
        public static LevelController Default;
        
        public TileBase Wall0;
        public TileBase Wall1;
        public TileBase Wall2;
        public TileBase Wall3;
        public TileBase Floor0;
        public TileBase Floor1;
        public TileBase Floor2;
        public TileBase Floor3;

        public TileBase Wall
        {
            get
            {
                var wallIndex = Random.Range(0, 3 + 1);

                if (wallIndex == 0)
                    return Wall0;
                if (wallIndex == 1)
                    return Wall1;
                if (wallIndex == 2)
                    return Wall2;
                if (wallIndex == 3)
                    return Wall3;

                return Wall0;
            }
        }

        public TileBase Floor
        {
            get
            {
                var floorIndex = Random.Range(0, 3 + 1);

                if (floorIndex == 0)
                    return Floor0;
                if (floorIndex == 1)
                    return Floor1;
                if (floorIndex == 2)
                    return Floor2;
                if (floorIndex == 3)
                    return Floor3;

                return Floor0;
            }
        }

        public Tilemap wallMap;

        public Tilemap floorMap;

        public Player player;

        public Enemy enemy;

        public Final final;

        private int currentX = 0;


        
        private void Awake()
        {
            Default = this;
            player.gameObject.SetActive(false);
            enemy.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            Room.Hide();
            GenerateRoom(currentX, Config.initRoom);
            currentX += Config.initRoom.Codes.First().Length + 2;
            GenerateRoom(currentX, Config.normalRooms.GetRandomItem());
            currentX += Config.initRoom.Codes.First().Length + 2;
            GenerateRoom(currentX, Config.normalRooms.GetRandomItem());
            currentX += Config.initRoom.Codes.First().Length + 2;
            GenerateRoom(currentX, Config.normalRooms.GetRandomItem());
            currentX += Config.initRoom.Codes.First().Length + 2;
            GenerateRoom(currentX, Config.finalRoom);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            Default.DestroySelf();
        }

        void GenerateRoom(int currentX, RoomConfig roomConfig)
        {
            var RoomCode = roomConfig.Codes;
            var roomWidth = RoomCode[0].Length;
            var roomHeight = RoomCode.Count;

            var roomPosX = currentX + roomWidth * 0.5f;
            var roomPosY = 1f + roomHeight * 0.5f;

            var room = Room.InstantiateWithParent(this)
                .WithConfig(roomConfig)
                .Position(roomPosX, roomPosY)
                .Show();

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);
    
            for (int i = 0; i < RoomCode.Count; i++)
            {
                var rowCode = RoomCode[i];
                for (int j = 0; j < rowCode.Length; j++)
                {
                    var code = rowCode[j];

                    int x = j + currentX;
                    int y = RoomCode.Count - i;

                    floorMap.SetTile(new Vector3Int(x, y, 0), Floor);

                    if (code == '1')
                    {
                        wallMap.SetTile(new Vector3Int(x, y, 0), Wall);
                    }
                    else if (code == '@')
                    {
                        var newplayer = Instantiate(player);
                        newplayer.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                        newplayer.gameObject.SetActive(true);
                        Global.player = newplayer;
                    }
                    else if (code == 'e')
                    {   
                        var enemyGeneratePos = new Vector3(x + 0.5f, y + 0.5f, 0);

                        room.AddEnemyGeneratePos(enemyGeneratePos);
                    }
                    else if (code == '#')
                    {
                        var newFinal = Instantiate(final);
                        newFinal.transform.position = new Vector3(x, y, 0);
                        newFinal.gameObject.SetActive(true);
                    }
                    else if (code == 'd')
                    {
                        var door = Door.InstantiateWithParent(room)
                            .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                            .Hide();

                        room.AddDoor(door);
                    }
                }
            }
        }

    }
}
