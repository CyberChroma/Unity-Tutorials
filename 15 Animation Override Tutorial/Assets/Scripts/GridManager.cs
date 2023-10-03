using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static Dictionary<Vector2Int, List<GameObject>> gridMap = new Dictionary<Vector2Int, List<GameObject>>();
    private static Vector2Int playerPosition;

    public static void AddToGrid(GameObject thingToAdd, bool isPlayer = false) {
        Vector2Int gridPosition = GetGridPosition(thingToAdd);
        AddToGridCommon(thingToAdd, gridPosition);
        if (isPlayer) {
            playerPosition = gridPosition;
        }
    }

    public static void AddToGrid(GameObject thingToAdd, Vector2Int newPosition) {
        AddToGridCommon(thingToAdd, newPosition);
    }

    public static void AddToGridCommon(GameObject thingToAdd, Vector2Int newPosition) {
        Vector2Int gridPosition = newPosition;

        if (gridMap.ContainsKey(gridPosition)) {
            // Something in that spot, add to list in dictionary
            List<GameObject> thingsAtPoint = gridMap[gridPosition];
            thingsAtPoint.Add(thingToAdd);
            gridMap[gridPosition] = thingsAtPoint;
        } else {
            // Nothing in that spot, add new element to dictionary
            List<GameObject> thingsAtPoint = new List<GameObject> {
                thingToAdd
            };
            gridMap.Add(gridPosition, thingsAtPoint);
        }
    }

    public static List<GameObject> LookInGrid(Vector2Int position) {
        if (gridMap.ContainsKey(position)) {
            return gridMap[position];
        }
        else
        {
            return null;
        }
    }

    public static void DeleteFromGrid(GameObject thingToDelete, Vector2Int oldPosition) {
        if (gridMap.ContainsKey(oldPosition)) {
            List<GameObject> thingsAtPoint = gridMap[oldPosition];
            if (thingsAtPoint.Contains(thingToDelete)) {
                thingsAtPoint.Remove(thingToDelete);
                if (thingsAtPoint.Count == 0) {
                    gridMap.Remove(oldPosition);
                }
            } else {
                print("ERROR COULDN\'T FIND OBJECT IN GRID POSITION");
            }
        } else {
            print("ERROR COULDN\'T FIND OBJECT IN GRID POSITION");
        }
    }

    public static void MoveObjectInGrid(GameObject thingToDelete, Vector2Int oldPosition, Vector2Int newPosition, bool isPlayer = false) {
        DeleteFromGrid(thingToDelete, oldPosition);
        AddToGrid(thingToDelete, newPosition);
        if (isPlayer) {
            playerPosition = newPosition;
        }
    }

    public static Vector2Int GetGridPosition(GameObject thingToFind) {
        Vector2Int gridPosition = Vector2Int.zero;
        gridPosition.x = Mathf.RoundToInt(thingToFind.transform.position.x);
        gridPosition.y = Mathf.RoundToInt(thingToFind.transform.position.z);
        return gridPosition;
    }

    public static Vector2Int GetGridPosition(Vector3 position) {
        Vector2Int gridPosition = Vector2Int.zero;
        gridPosition.x = (int)position.x;
        gridPosition.y = (int)position.z;
        return gridPosition;
    }

    public static void ResetGrid() {
        gridMap.Clear();
    }

    public static Vector2Int GetPlayerPosition() {
        return playerPosition;
    }
}
