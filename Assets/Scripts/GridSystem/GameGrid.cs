using Assets.Scripts.GridSystem;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    private readonly int height;
    private readonly int width;
    private readonly int cellSize;
    private readonly GameGridCell[,] cells;
    private readonly Vector3Int originPosition;
    private readonly List<Vector2Int> occupiedCells;

    public bool debugModeOn;

    public GameGrid(int height, int width, int cellSize, Vector3Int originPosition, List<Vector2Int> occupiedCells)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.cells = new GameGridCell[height, width];
        this.originPosition = originPosition;
        this.occupiedCells = new List<Vector2Int>(occupiedCells);
        Initialize();
    }

    public void Draw()
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                var cell = cells[h, w];
                cell.Draw();
            }
        }
    }

    private void Initialize()
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                var isOccupied = this.occupiedCells.Exists(cell => cell.x == w && cell.y == h);
                cells[h, w] = new GameGridCell(h, w, cellSize, originPosition, isOccupied);
            }
        }
    }
}
