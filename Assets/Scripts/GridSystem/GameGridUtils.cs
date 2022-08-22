using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GridSystem
{
    public static class GameGridUtils
    {
        public static Vector3 GetWorldPosition(int x, int y, int cellSize, Vector3Int worldOffset)
        {
            return new Vector3(x, 0, y) * cellSize + worldOffset;
        }

        public static Point GetCellPosition(Vector3 worldPosition, int cellSize, Vector3Int worldOffset)
        {
            var x = Mathf.FloorToInt((worldPosition - worldOffset).x / cellSize);
            var y = Mathf.FloorToInt((worldPosition - worldOffset).z / cellSize);
            return new Point(x, y);
        }
    }
}
