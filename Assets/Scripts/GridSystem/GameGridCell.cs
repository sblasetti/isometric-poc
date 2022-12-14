using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GridSystem
{
    class GameGridCell
    {
        private int x;
        private int y;
        private int size;
        private Vector3Int originPosition;
        private bool isOccupied = false;

        public GameGridCell(int y, int x, int size, Vector3Int originPosition, bool isOccupied)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.originPosition = originPosition;
            this.isOccupied = isOccupied;
        }

        public void Draw()
        {
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
            Debug.DrawLine(GetWorldPosition(x, y + 1), GetWorldPosition(x + 1, y + 1));
            Debug.DrawLine(GetWorldPosition(x + 1, y + 1), GetWorldPosition(x + 1, y));
            Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x, y));

            if (isOccupied)
            {
                Debug.DrawLine(GetWorldPosition(x + 1, y + 1), GetWorldPosition(x, y));
                Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x, y + 1));
            }
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return GameGridUtils.GetWorldPosition(x, y, size, originPosition);
        }
    }
}
