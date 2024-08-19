using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int cellSize = 10;
    Dictionary<Vector2Int, List<Collider>> grid = new Dictionary<Vector2Int, List<Collider>>();
    public static GridManager GridManagerInstance { get; private set; }

    private void Awake()
    {
        if (GridManagerInstance == null)
        {
            GridManagerInstance = this;
        }
    }

    private Vector2Int WorldToCell(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x/cellSize);
        int y = Mathf.RoundToInt(position.z/cellSize);
        return new Vector2Int(x, y);
    }

    public void AddEnemy(Collider collider)
    {
        Vector2Int cell = WorldToCell(collider.transform.position);

        if (!grid.ContainsKey(cell))
        {
            grid[cell] = new List<Collider>();
        }

        grid[cell].Add(collider);
    }

    public void RemoveEnemy(Collider collider)
    {
        Vector2Int cell = WorldToCell(collider.transform.position);

        if (grid.ContainsKey(cell))
        {
            grid[cell].Remove(collider);
            if(grid[cell].Count == 0)
            {
                grid.Remove(cell);
            }
        }   
    }

    public void UpdatePosition(Collider collider)
    {
        RemoveEnemy(collider);
        AddEnemy(collider);
    }

    public Collider GetNearestEnemy(Vector3 playerPosition, float searchRadius)
    {
        // Find the cell the player is currently in
        Vector2Int playerCell = WorldToCell(playerPosition);
        Collider nearestEnemy = null;

        // Determine how many cells around player cell to check.  Will result in partial cells due to fractional result.  Round up to include entirety of cells.
        int range = Mathf.CeilToInt(searchRadius / cellSize);
        
        // Identify the coordinates of cells within the range and if there are colliders present, find the nearest one and return it.
        for (int x = playerCell.x - range; x <= playerCell.x + range; x++)
        {
            for (int y = playerCell.y - range; y <= playerCell.y + range; y++)
            {
                Vector2Int cellToSearch = new Vector2Int(x, y);
                
                if (grid.ContainsKey(cellToSearch))
                {
                    float nearestColliderDistance = Mathf.Infinity;

                    foreach (Collider collider in grid[cellToSearch])
                    {
                        float colliderDistance = Vector3.Distance(playerPosition, collider.transform.position);

                        if (colliderDistance <= searchRadius)
                        {
                            if (colliderDistance < nearestColliderDistance)
                            {
                                nearestColliderDistance = colliderDistance;
                                nearestEnemy = collider;
                            }
                        }
                    }
                }
            }
        }

        return nearestEnemy;
    }
}
