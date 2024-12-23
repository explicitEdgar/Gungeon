using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QFramework.Gungeon.LevelController;

namespace QFramework.Gungeon
{
    public class LevelGenHelper
    {
        public static List<DoorDirections> GetAvailableDirections(DynaGrid<RoomGenerateNode> layoutGrid, int x, int y)
        {
            var availableDirections = new List<DoorDirections>();
            if (layoutGrid[x + 1, y] == null)
            {
                availableDirections.Add(DoorDirections.Right);
            }
            if (layoutGrid[x - 1, y] == null)
            {
                availableDirections.Add(DoorDirections.Left);
            }
            if (layoutGrid[x, y + 1] == null)
            {
                availableDirections.Add(DoorDirections.Up);
            }
            if (layoutGrid[x, y - 1] == null)
            {
                availableDirections.Add(DoorDirections.Down);
            }

            return availableDirections;
        }
        
        public class DirectionWithCount
        {
            public int count;
            public DoorDirections direction;
        }

        public static List<DirectionWithCount> Predict(DynaGrid<RoomGenerateNode> layoutGrid, int x, int y)
        {
            var availableDirections = GetAvailableDirections(layoutGrid, x, y);

            var directionWithCount = new List<DirectionWithCount>();

            foreach(var availableDirection in availableDirections)
            {
                if(availableDirection == DoorDirections.Right)
                {
                    var rightNodeDirections = GetAvailableDirections(layoutGrid, x + 1, y);
                    directionWithCount.Add(new DirectionWithCount()
                    {
                        count = rightNodeDirections.Count,
                        direction = availableDirection
                    });
                }
                if (availableDirection == DoorDirections.Left)
                {
                    var leftNodeDirections = GetAvailableDirections(layoutGrid, x - 1, y);
                    directionWithCount.Add(new DirectionWithCount()
                    {
                        count = leftNodeDirections.Count,
                        direction = availableDirection
                    });
                }
                if (availableDirection == DoorDirections.Up)
                {
                    var upNodeDirections = GetAvailableDirections(layoutGrid, x, y + 1);
                    directionWithCount.Add(new DirectionWithCount()
                    {
                        count = upNodeDirections.Count,
                        direction = availableDirection
                    });
                }
                if (availableDirection == DoorDirections.Down)
                {
                    var downNodeDirections = GetAvailableDirections(layoutGrid, x, y - 1);
                    directionWithCount.Add(new DirectionWithCount()
                    {
                        count = downNodeDirections.Count,
                        direction = availableDirection
                    });
                }
            }

            return directionWithCount;
        }
    }
}