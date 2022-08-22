using Assets.Scripts;
using UnityEngine;

public class GridDebug : MonoBehaviour
{
    public GridSystemContainer container;
    public bool debugOn = false;

    void Start()
    {
    }

    void Update()
    {
        if (debugOn)
        {
            container.DrawGrid();
        }
    }
}
