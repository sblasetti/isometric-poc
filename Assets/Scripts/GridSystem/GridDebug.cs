using Assets.Scripts;
using UnityEngine;

public class GridDebug : MonoBehaviour
{
    public GridSystemContainer gridContainer;
    public bool drawGrid = false;

    void Start()
    {
    }

    void Update()
    {
        if (drawGrid)
        {
            gridContainer.DrawGrid();
        }
    }
}
