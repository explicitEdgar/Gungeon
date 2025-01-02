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

        public IEnemy Enemy => RandomUtility.Choose(EnemyC,EnemyF,EnemyG).GetComponent<IEnemy>();
        
        public Final final;

        //private int currentX = 0;

        public int test = 0;


        
        private void Awake()
        {
            Default = this;
            player.gameObject.SetActive(false);
            Enemy.GameObject.SetActive(false);
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
                .Next(RoomTypes.Chest,n =>
                {
                    n.Next(RoomTypes.Chest)
                    .Next(RoomTypes.Chest)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal);
                })
                .Next(RoomTypes.Chest)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Chest,n =>
                {
                    n.Next(RoomTypes.Chest)
                    .Next(RoomTypes.Chest)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal);
                })
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal,n =>
                {
                    n.Next(RoomTypes.Chest)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal)
                    .Next(RoomTypes.Normal);
                })
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Final);

            var layoutGrid = new DynaGrid<RoomGenerateNode>();

            bool GenerateLayoutBFS(RoomNode roomNode, DynaGrid<RoomGenerateNode> layoutGrid,int predictWeight = 0)
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

                    var availableDirections = LevelGenHelper.GetAvailableDirections(layoutGrid, roomGenerateNode.X, roomGenerateNode.Y);

                    if (availableDirections.Count < roomGenerateNode.Node.Children.Count)
                    {
                        Debug.Log("出现冲突");
                        return false; 
                    }

                    if (layoutGrid[roomGenerateNode.X, roomGenerateNode.Y] == null)
                    {
                        layoutGrid[roomGenerateNode.X, roomGenerateNode.Y] = roomGenerateNode;
                    }
                    else
                    {
                        Debug.Log("出现冲突");
                        return false;
                    }

                    var directionWithCount = LevelGenHelper.Predict(layoutGrid, roomGenerateNode.X, roomGenerateNode.Y);
                    directionWithCount.Sort((a, b) =>
                    {
                        return b.count - a.count;
                    });

                    bool predictGenerate = false;
                    if(Random.Range(0,100) < predictWeight)
                    {
                        predictGenerate = true;
                    }
                    else
                    {
                        predictGenerate = false;
                    }

                    foreach (var roomNodeChild in roomGenerateNode.Node.Children)
                    {
                        var nextRoomDirection = predictGenerate ? directionWithCount.First().direction : directionWithCount.GetRandomItem().direction;

                        if (predictGenerate)
                        {
                            directionWithCount.RemoveAt(0);
                        }

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
                        else
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            var predictWeight = 0;
            print(predictWeight + ": generate");

            while (!GenerateLayoutBFS(layout, layoutGrid,predictWeight))
            {
                predictWeight++;
                print(predictWeight + ": generate");
                layoutGrid.Clear();
            }
            

            Global.RoomGrid = new DynaGrid<Room>();
            layoutGrid.ForEach((x, y, generateNode) =>
            {
                var room = GenerateRoomByNode(x, y, generateNode);
                Global.RoomGrid[x, y] = room;
            });

            Room GenerateRoomByNode(int x,int y,RoomGenerateNode roomNode)
            {
                var roomPosX = x * (Config.initRoom.Codes.First().Length + 2);
                var roomPosY = y * (Config.initRoom.Codes.Count + 2);

                if(roomNode.Node.RoomType == RoomTypes.Init)
                {
                    return GenerateRoom(roomPosX,roomPosY,Config.initRoom, roomNode);
                }
                else if(roomNode.Node.RoomType == RoomTypes.Normal)
                {
                    return GenerateRoom(roomPosX, roomPosY,Config.normalRooms.GetRandomItem(), roomNode);
                }
                else if (roomNode.Node.RoomType == RoomTypes.Chest)
                {
                    return GenerateRoom(roomPosX, roomPosY, Config.chestRoom, roomNode);
                }
                else if(roomNode.Node.RoomType == RoomTypes.Final)
                {
                    return GenerateRoom(roomPosX, roomPosY, Config.finalRoom, roomNode);
                }

                return null;
            }
           
            void GenerateCorridor()
            {
                Global.RoomGrid.ForEach((x, y, room) =>
                {
                    foreach (var door in room.Doors)
                    {
                        if(door.Direction == DoorDirections.Left)
                        {
                            //var dstRoom = Global.RoomGrid[x - 1, y];
                            //var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Right);

                            //for (int i = door.X;i <= dstDoor.X;i++)
                            //{
                            //    floorMap.SetTile(new Vector3Int(i, door.Y, 0), Floor);
                            //    floorMap.SetTile(new Vector3Int(i, door.Y + 1, 0), Floor);
                            //    floorMap.SetTile(new Vector3Int(i, door.Y - 1, 0), Floor);
                            //    wallMap.SetTile(new Vector3Int(i, door.Y + 2, 0), Wall);
                            //    wallMap.SetTile(new Vector3Int(i, door.Y - 2, 0), Wall);
                            //}
                        }
                        else if(door.Direction == DoorDirections.Right)
                        {
                            var dstRoom = Global.RoomGrid[x + 1, y];
                            var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Left);

                            for (int i = door.X; i <= dstDoor.X; i++)
                            {
                                floorMap.SetTile(new Vector3Int(i, door.Y, 0), Floor);
                                floorMap.SetTile(new Vector3Int(i, door.Y + 1, 0), Floor);
                                floorMap.SetTile(new Vector3Int(i, door.Y - 1, 0), Floor);
                                wallMap.SetTile(new Vector3Int(i, door.Y + 2, 0), Wall);
                                wallMap.SetTile(new Vector3Int(i, door.Y - 2, 0), Wall);
                            }
                        }
                        else if (door.Direction == DoorDirections.Up)
                        {
                            var dstRoom = Global.RoomGrid[x, y + 1];
                            var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Down);

                            for (int i = door.Y; i <= dstDoor.Y; i++)
                            {
                                floorMap.SetTile(new Vector3Int(door.X, i, 0), Floor);
                                floorMap.SetTile(new Vector3Int(door.X + 1, i, 0), Floor);
                                floorMap.SetTile(new Vector3Int(door.X - 1, i, 0), Floor);
                                wallMap.SetTile(new Vector3Int(door.X + 2, i, 0), Wall);
                                wallMap.SetTile(new Vector3Int(door.X - 2, i, 0), Wall);
                            }
                        }
                        else if (door.Direction == DoorDirections.Down)
                        {
                            //var dstRoom = Global.RoomGrid[x, y - 1];
                            //var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Up);

                            //for (int i = door.Y; i <= dstDoor.Y; i++)
                            //{
                            //    floorMap.SetTile(new Vector3Int(door.X, i, 0), Floor);
                            //    floorMap.SetTile(new Vector3Int(door.X + 1, i, 0), Floor);
                            //    floorMap.SetTile(new Vector3Int(door.X - 1, i, 0), Floor);
                            //    wallMap.SetTile(new Vector3Int(door.X + 2, i, 0), Wall);
                            //    wallMap.SetTile(new Vector3Int(door.X - 2, i, 0), Wall);
                            //}
                        }
                    }
                });





                //var roomWidth = Config.initRoom.Codes.First().Length;
                //var roomHeight = Config.initRoom.Codes.Count();

                //for (int index = 0; index < roomNumber - 1; index++)
                //{
                //    currentX = index * (roomWidth + 2);
                //    var doorStartX = currentX + roomWidth;
                //    var doorStartY = 0 + roomHeight / 2 + 2;

                //    for (int i = 0; i < 2; i++)
                //    {
                //        floorMap.SetTile(new Vector3Int(doorStartX + i, doorStartY, 0), Floor);
                //        floorMap.SetTile(new Vector3Int(doorStartX + i, doorStartY + 1, 0), Floor);
                //        floorMap.SetTile(new Vector3Int(doorStartX + i, doorStartY - 1, 0), Floor);
                //        wallMap.SetTile(new Vector3Int(doorStartX + i, doorStartY + 2, 0), Wall);
                //        wallMap.SetTile(new Vector3Int(doorStartX + i, doorStartY - 2, 0), Wall);
                //    }
                //}
            }

            GenerateCorridor();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            Default.DestroySelf();
        }

        Room GenerateRoom(int currentX,int currentY, RoomConfig roomConfig,RoomGenerateNode node)
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

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 4, roomHeight - 4);
    
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
                        int cashedx = x;
                        int cashedy = y;
                        var doorDistance = new Vector2(x + 0.5f, y + 0.5f) - new Vector2(roomPosX, roomPosY);
                        
                        if(doorDistance.x.Abs() > doorDistance.y.Abs())
                        {
                            if(doorDistance.x > 0)
                            {
                                if(node.Directions.Contains(DoorDirections.Right))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.LocalRotation(Quaternion.Euler(0, 0, 90));
                                    door.Direction = DoorDirections.Right;

                                    ActionKit.NextFrame(() =>
                                    {
                                        wallMap.SetTile(new Vector3Int(cashedx, cashedy + 1, 0), null);
                                        wallMap.SetTile(new Vector3Int(cashedx, cashedy - 1, 0), null);
                                    }).Start(this);

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
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.LocalRotation(Quaternion.Euler(0, 0, 90));
                                    door.Direction = DoorDirections.Left;

                                    ActionKit.NextFrame(() =>
                                    {
                                        wallMap.SetTile(new Vector3Int(cashedx, cashedy + 1, 0), null);
                                        wallMap.SetTile(new Vector3Int(cashedx, cashedy - 1, 0), null);
                                    }).Start(this);

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
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.Direction = DoorDirections.Up;

                                    ActionKit.NextFrame(() =>
                                    {
                                        wallMap.SetTile(new Vector3Int(cashedx + 1, cashedy, 0), null);
                                        wallMap.SetTile(new Vector3Int(cashedx - 1, cashedy, 0), null);
                                    }).Start(this);

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
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x + 0.5f, y + 0.5f, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.Direction = DoorDirections.Down;

                                    ActionKit.NextFrame(() =>
                                    {
                                        wallMap.SetTile(new Vector3Int(cashedx + 1, cashedy, 0), null);
                                        wallMap.SetTile(new Vector3Int(cashedx - 1, cashedy, 0), null);
                                    }).Start(this);

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
                        room.AddPowerUp(newChest);
                    }
                }
            }

            return room;
        }

    }
}
