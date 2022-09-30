using Assets.Scripts.GridSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IsometricAStarPathfinding3D : MonoBehaviour
{
    public Vector3 startCell;
    public Vector3 endCell;
    public bool targetPlayer;
    public bool drawPath = false;
    
    public GridSystemContainer gridContainer;
    public Transform playerTransform;

    private int gridWidth;
    private int gridHeight;

    private AStarPathfindingAlgorithm algo;
    private Dictionary<AStarNode, List<AStartNodeAndVisitCost>> graph;

    private AStarNode lastStartNode;
    private AStarNode lastEndNode;

    private void Start()
    {
        gridWidth = gridContainer.width;
        gridHeight = gridContainer.height;
        algo = new AStarPathfindingAlgorithm();

        graph = InitializeGraph();
    }

    private void Update()
    {
        var startNode = new AStarNode(Convert.ToInt32(startCell.x), Convert.ToInt32(startCell.z));
        var endNode = GetEndNode();

        bool sameStart = false;
        if (lastStartNode != null && lastStartNode == startNode)
        {
            sameStart = true;
        }
        bool sameEnd = false;
        if (lastEndNode != null && lastEndNode == endNode)
        {
            sameEnd = true;
        }

        if (sameStart && sameEnd) return;

        var heuristicsTable = new Dictionary<AStarNode, float>();
        foreach (var node in graph.Keys)
        {
            heuristicsTable.Add(node, algo.calculateHeuristics(node, endNode));
        }

        var nav = algo.findNodeNavigation(graph, startNode, endNode, heuristicsTable);
        var path = algo.reconstructPath(nav, endNode);

        if (drawPath)
        {
            DrawPath(path);
        }
    }

    private void DrawPath(List<AStarNode> path)
    {
        var size = gridContainer.cellSize;
        var mid = new Vector3(size / 2, 0, size / 2);
        for (int i = 1; i < path.Count; i++)
        {
            var aN = path[i - 1];
            var a = GameGridUtils.GetWorldPosition(aN.x, aN.y, size, gridContainer.originPosition) + mid;
            var bN = path[i];
            var b = GameGridUtils.GetWorldPosition(bN.x, bN.y, size, gridContainer.originPosition) + mid;
            Debug.DrawLine(a, b, Color.red);
        }
    }

    private AStarNode GetEndNode()
    {
        if (targetPlayer)
        {
            var position = GameGridUtils.GetCellPosition(playerTransform.position, gridContainer.cellSize, gridContainer.originPosition);
            return new AStarNode(position.x, position.y);
        }

        return new AStarNode(Convert.ToInt32(endCell.x), Convert.ToInt32(endCell.z));
    }

    private Dictionary<AStarNode, List<AStartNodeAndVisitCost>> InitializeGraph()
    {
        var graph = new Dictionary<AStarNode, List<AStartNodeAndVisitCost>>();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // TODO: improve this search
                var isOccupied = gridContainer.occupiedCells.Exists(cell => cell.x == x && cell.y == y);
                if (!isOccupied)
                {
                    var node = new AStarNode(x, y);
                    graph.Add(node, new List<AStartNodeAndVisitCost>());
                }
            }
        }
        foreach (var node in graph.Keys)
        {
            for (int cellX = -1; cellX < 2; cellX++)
            {
                var x = node.x + cellX;
                if (x < 0 || x >= gridWidth) continue;

                for (int cellY = -1; cellY < 2; cellY++)
                {
                    var y = node.y + cellY;
                    if (y < 0 || y >= gridHeight) continue;
                    if (x == node.x && y == node.y) continue;
                    // TODO: improve this search
                    var isOccupied = gridContainer.occupiedCells.Exists(cell => cell.x == x && cell.y == y);
                    if (isOccupied) continue;

                    graph[node].Add(new AStartNodeAndVisitCost(new AStarNode(x, y), 1));
                }
            }
        }

        return graph;
    }
}

public class AStarPathfindingAlgorithm
{
    // http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#a-stars-use-of-the-heuristic
    public float calculateHeuristics(AStarNode aNode, AStarNode targetNode)
    {
        var D = 1;
        var D2 = (float)Math.Sqrt(2);

        var dx = Math.Abs(aNode.x - targetNode.x);
        var dy = Math.Abs(aNode.y - targetNode.y);
        return D * (dx + dy) + (D2 - 2 * D) * Math.Min(dx, dy);
    }

    // https://www.thecriticalcoder.com/a-a-star-algorithm-a-step-by-step-illustrated-explanation/
    public Dictionary<AStarNode, AStarNode> findNodeNavigation(Dictionary<AStarNode, List<AStartNodeAndVisitCost>> graph, AStarNode startNode, AStarNode endNode, Dictionary<AStarNode, float> heuristicsTable)
    {
        var nodeNavigation = new Dictionary<AStarNode, AStarNode>();
        try
        {
            var distances = new Dictionary<AStarNode, float>();
            distances.Add(startNode, 0);
            var visitedNodes = new HashSet<AStarNode>();
            nodeNavigation.Add(startNode, null);

            startNode.cost = 0 + heuristicsTable[startNode];
            var priorityQueue = new List<AStarNode>();
            priorityQueue.Add(startNode);

            while (priorityQueue.Count > 0)
            {
                var node = priorityQueue[0];
                priorityQueue.RemoveAt(0);

                if (node.Equals(endNode))
                {
                    break;
                }

                foreach (var neighbourWithCost in graph[node])
                {
                    if (visitedNodes.Contains(neighbourWithCost.node)) continue;

                    var previousDistance = distances.ContainsKey(neighbourWithCost.node) ? distances[neighbourWithCost.node] : float.PositiveInfinity;
                    var updatedDistance = distances[node] + neighbourWithCost.visitCost;
                    if (updatedDistance >= previousDistance) continue;

                    distances[neighbourWithCost.node] = updatedDistance;
                    var priority = updatedDistance + heuristicsTable[neighbourWithCost.node];
                    neighbourWithCost.node.cost = priority;

                    // Insert in the right position
                    var position = priorityQueue.FindIndex(n => n.cost > priority);
                    var insertIndex = position >= 0 ? position : priorityQueue.Count;
                    priorityQueue.Insert(insertIndex, neighbourWithCost.node);

                    nodeNavigation.Add(neighbourWithCost.node, node);
                }

                visitedNodes.Add(node);
            }

            return nodeNavigation;

        }
        catch (Exception ex)
        {
            Debug.LogWarning(nodeNavigation.Keys);
            return nodeNavigation;
        }
    }

    public List<AStarNode> reconstructPath(Dictionary<AStarNode, AStarNode> nodeNavigation, AStarNode toNode)
    {
        var currentNode = toNode;
        var path = new List<AStarNode>(new[] { toNode });

        while (nodeNavigation.ContainsKey(currentNode))
        {
            currentNode = nodeNavigation[currentNode];
            if (currentNode == null) break;

            path.Insert(0, currentNode);
        }

        return path;
    }
}

public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
{
    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        // Handle equality as being greater. Note: this will break Remove(key) or
        // IndexOfKey(key) since the comparer never returns 0 to signal key equality
        return result == 0 ? 1 : result;
    }
}

public class AStarNode
{
    public int x { get; private set; }
    public int y { get; private set; }
    public string referenceKey => $"{x};{y}";
    public float cost { get; set; }

    public AStarNode(int x, int y, float cost = float.PositiveInfinity)
    {
        this.x = x;
        this.y = y;
        this.cost = cost;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as AStarNode);
    }

    public bool Equals(AStarNode node)
    {
        if (node == null) return false;

        return this.x == node.x && this.y == node.y;
    }

    public override int GetHashCode()
    {
        // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        return (this.x + 2) ^ (this.y + 1);
    }

    public override string ToString()
    {
        return $"({this.x};{this.y})";
    }
}

public class AStartNodeAndVisitCost
{
    public AStarNode node { get; private set; }
    public float visitCost { get; private set; }

    public AStartNodeAndVisitCost(AStarNode node, float visitCost)
    {
        this.node = node;
        this.visitCost = visitCost;
    }
}