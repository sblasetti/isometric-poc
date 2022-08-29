using Assets.Scripts;
using UnityEngine;

public class GridDebug : MonoBehaviour
{
    public GridSystemContainer gridContainer;
    public bool debugOn = false;

    void Start()
    {
    }

    void Update()
    {
        if (debugOn)
        {
            gridContainer.DrawGrid();
        }
    }
}
