using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Tilemaps;
using static QFramework.Gungeon.RoomConfig;

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

        public class RoomGenerateNode
        {
            public RoomNode Node { get; set; }

            public HashSet<DoorDirections> Directions { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public enum DoorDirections
        { 
            Up,
            Down,
            Left,
            Right
        }

        // Start is called before the first frame update
        void Start()
        {
            Room.Hide();

            

            var layout = new RoomNode(RoomTypes.Init);
            layout.Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Chest)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Final);

            var layoutGrid = new DynaGrid<RoomGenerateNode>();

            void GenerateLayoutBFS(RoomNode roomNode, DynaGrid<RoomGenerateNode> layoutGrid)
            {
                var queue = new Queue<RoomGenerateNode>();
                queue.Enqueue(new RoomGenerateNode()
                {
                    X = 0,
                    Y = 0,
                    Node = roomNode,
                    Directions = new HashSet<DoorDirections>(),
                });

                while (queue.Count > 0)
                {
                    var roomGenerateNode = queue.Dequeue();

                    layoutGrid[roomGenerateNode.X, roomGenerateNode.Y] = roomGenerateNode;

                    var availableDirections = new List<DoorDirections>();
                    if (layoutGrid[roomGenerateNode.X + 1, roomGenerateNode.Y] == null)
                    {
                        availableDirections.Add(DoorDirections.Right);
                    }
                    if (layoutGrid[roomGenerateNode.X - 1, roomGenerateNode.Y] == null)
                    {
                        availableDirections.Add(DoorDirections.Left);
                    }
                    if (layoutGrid[roomGenerateNode.X, roomGenerateNode.Y + 1] == null)
                    {
                        availableDirections.Add(DoorDirections.Up);
                    }
                    if (layoutGrid[roomGenerateNode.X, roomGenerateNode.Y - 1] == null)
                    {
                        availableDirections.Add(DoorDirections.Down);
                    }


                    foreach(var roomNodeChild in roomGenerateNode.Node.Children)
                    {
                        var nextRoomDirection = availableDirections.GetRandomItem();

                        if (nextRoomDirection == DoorDirections.Right)
                        {
                            roomGenerateNode.Directions.Add(DoorDirections.Right);
                            queue.Enqueue(new RoomGenerateNode
                            {
                                X = roomGenerateNode.X + 1,
                                Y = roomGenerateNode.Y,
                                Node = roomNodeChild,
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Left
                                },
                            });
                        }
                        else if (nextRoomDirection == DoorDirections.Left)
                        {
                            roomGenerateNode.Directions.Add(DoorDirections.Left);
                            queue.Enqueue(new RoomGenerateNode
                            {
                                X = roomGenerateNode.X - 1,
                                Y = roomGenerateNode.Y,
                                Node = roomNodeChild,
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Right
                                },
                            });
                        }
                        else if (nextRoomDirection == DoorDirections.Up)
                        {
                            roomGenerateNode.Directions.Add(DoorDirections.Up);
                            queue.Enqueue(new RoomGenerateNode
                            {
                                X = roomGenerateNode.X,
                                Y = roomGenerateNode.Y + 1,
                                Node = roomNodeChild,
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Down
                                },
                            });
                        }
                        else if (nextRoomDirection == DoorDirections.Down)
                        {
                            roomGenerateNode.Directions.Add(DoorDirections.Down);
                            queue.Enqueue(new RoomGenerateNode
                            {
                                X = roomGenerateNode.X,
                                Y = roomGenerateNode.Y - 1,
                                Node = roomNodeChild,
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Up
                                },
                            });
                        }
                    }
                }
            }

            GenerateLayoutBFS(layout, layoutGrid);

            layoutGrid.ForEach((x, y, generateNode) =>
            {
                GenerateRoomByNode(x, y, generateNode);
            });

            currentX = 0;
            void GenerateRoomByNode(int x,int y,RoomGenerateNode roomNode)
            {
                var roomPosX = x * (Config.initRoom.Codes.First().Length + 2);
                var roomPosY = y * (Config.initRoom.Codes.Count + 2);

                if(roomNode.Node.RoomType == RoomTypes.Init)
                {
                    GenerateRoom(roomPosX,roomPosY,Config.initRoom, roomNode);
                }
                else if(roomNode.Node.RoomType == RoomTypes.Normal)
                {
                    GenerateRoom(roomPosX, roomPosY,Config.normalRooms.GetRandomItem(), roomNode);
                }
                else if (roomNode.Node.RoomType == RoomTypes.Chest)
                {
                    GenerateRoom(roomPosX, roomPosY, Config.chestRoom, roomNode);
                }
                else if(roomNode.Node.RoomType == RoomTypes.Final)
                {
                    GenerateRoom(roomPosX, roomPosY, Config.finalRoom, roomNode);
                }
            }
           
            void GenerateCorridor(int roomNumber)
            {
                var roomWidth = Config.initRoom.Codes.First().Length;
                var roomHeight = Config.initRoom.Codes.Count();

                for (int index = 0; index < roomNumber - 1; index++)
                {
                    currentX = index * (roomWidth + 2);
                    var doorStartX = currentX + roomWidth;
                    var doorStartY = 0 + roomHeight / 2 + 2;

                    for (int i = 0; i < 2; i++)
                    {
                        floorMap.SetTile(new Vector3Int(doorStartX + i, doorStartY, 0), Floor);
                        floorMap.SetTile(new Vector3Int(doorStartX + i, doorStartY + 1, 0), Floor);
                        floorMap.SetTile(new Vector3Int(doorStartX + i, doorStartY - 1, 0), Floor);
                        wallMap.SetTile(new Vector3Int(doorStartX + i, doorStartY + 2, 0), Wall);
                        wallMap.SetTile(new Vector3Int(doorStartX + i, doorStartY - 2, 0), Wall);
                    }
                }
            }

            //GenerateRoomByNode(layout);
            //GenerateCorridor(8);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            Default.DestroySelf();
        }

        void GenerateRoom(int currentX,int currentY, RoomConfig roomConfig,RoomGenerateNode node)
        {
            var RoomCode = roomConfig.Codes;
            var roomWidth = RoomCode[0].Length;
            var roomHeight = RoomCode.Count;

            var roomPosX = currentX + roomWidth * 0.5f;
            var roomPosY = currentY + 1f + roomHeight * 0.5f;

            var room = Room.InstantiateWithParent(this)
                .WithConfig(roomConfig)
                .Position(roomPosX, roomPosY)
                .Show();

            room.GenerateNode = node;

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);
    
            for (int i = 0; i < RoomCode.Count; i++)
            {
                var rowCode = RoomCode[i];
                for (int j = 0; j < rowCode.Length; j++)
                {
                    var code = rowCode[j];

                    int x = j + currentX;
                    int y = currentY + RoomCode.Count - i;

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
                        var doorDistance = new Vector2(x + 0.5f, y + 0.5f) - new Vector2(roomPosX, roomPosY);
                        
                        if(doorDistance.x.Abs() > doorDistance.y.Abs())
                        {
                            if(doorDistance.x > 0)
                            {
                                if(node.Directions.Contains(DoorDirections.Right))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                      .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                      .Hide();  

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    wallMap.SetTile(new Vector3Int(x, y, 0), Wall);
                                }
                            }
                            else
                            {
                                if (node.Directions.Contains(DoorDirections.Left))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                      .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                      .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    wallMap.SetTile(new Vector3Int(x, y, 0), Wall);
                                }
                            }
                        }
                        else
                        {
                            if (doorDistance.y > 0)
                            {
                                if (node.Directions.Contains(DoorDirections.Up))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                      .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                      .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    wallMap.SetTile(new Vector3Int(x, y, 0), Wall);
                                }
                            }
                            else
                            {
                                if (node.Directions.Contains(DoorDirections.Down))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                      .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                      .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    wallMap.SetTile(new Vector3Int(x, y, 0), Wall);
                                }
                            }
                        }




                      
                    }
                    else if (code == 'c')
                    {
                        var newChest = Instantiate(Chest);
                        newChest.transform.position = new Vector3(x, y, 0);
                        newChest.gameObject.SetActive(true);
                    }
                }
            }
        }

    }
}
