using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour
{
    #region Variables
    [Header("Grid Lists")]

    [Tooltip("Grid List A")]
    [SerializeField] private List<GameObject> AGrid;
    [Tooltip("Grid List B")]
    [SerializeField] private List<GameObject> BGrid;
    [Tooltip("Grid List C")]
    [SerializeField] private List<GameObject> CGrid;
    [Tooltip("Grid List D")]
    [SerializeField] private List<GameObject> DGrid;
    [Tooltip("Grid List E")]
    [SerializeField] private List<GameObject> EGrid;

    [Tooltip("MoveDireciton instance")]
    [SerializeField] private PlayerController.MoveDirection moveDirection;

    [Tooltip("2D Array representing the entier grid array")]
    public List<List<GameObject>> GridArray;

    public GameObject moveObject;

    public bool isRunning;
    #endregion

    #region UnityMethods

    private void Start()
    {
        GridArray = new List<List<GameObject>>();
        GridArray.Add(AGrid);
        GridArray.Add(BGrid);
        GridArray.Add(CGrid);
        GridArray.Add(DGrid);
        GridArray.Add(EGrid);


        // temp code;
        moveObject = PlayerController.instance.gameObject;
    }
    #endregion

    #region CustomMethods

    public void EvaluateObjectMovement(string name)
    {
        moveDirection = PlayerController.instance.moveDirection;
        (int, int) startCoord = StringToGridCoordinate(name);

        (int, int,int) endCoord = CalculateEndCoordinate(startCoord, moveDirection);

        MoveObject(startCoord, (endCoord.Item1, endCoord.Item2), endCoord.Item3, moveObject);
    }

    (int, int) StringToGridCoordinate(string name)
    {
        char x = name[0];
        char y = name[1];

        int yCoord = y- '0' - 1;

        int xCoord = 0;

        switch (x)
        {
            case 'A':
                xCoord = 0;
                break;
            case 'B':
                xCoord = 1;
                break;
            case 'C':
                xCoord = 2;
                break;
            case 'D':
                xCoord = 3;
                break;
            case 'E':
                xCoord = 4;
                break;
        }

        (int,int) returnTuple = (xCoord, yCoord);

        return returnTuple;
    }

    (int,int,int) CalculateEndCoordinate((int,int) startCoord, PlayerController.MoveDirection direction)
    {
        int tileCount = 0;
        switch (direction)
        {
            case PlayerController.MoveDirection.Up:
                //Iterate through i in reverse
                if(startCoord.Item1 == 0)
                {
                    return (-1, -1, -1);
                }
                for (int i = startCoord.Item1-1; i >= 0; i--)
                {
                    GridTile currentTile = GridArray[i][startCoord.Item2].GetComponent<GridTile>();
                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice && 
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        if (i == 0)
                        {
                            return (0, startCoord.Item2, tileCount);
                        }
                        continue;
                    }
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Normal &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        return (i, startCoord.Item2, tileCount);
                    }
                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.NormalBox)
                    {
                        return (i + 1, startCoord.Item2, tileCount);
                    }
                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.IceBox)
                    {
                        //Move the box that you are collding with 
                        return (i + 1, startCoord.Item2, tileCount);
                    }
                    else
                    {
                        Debug.LogError("in the up case but still broke");
                        return (-1, -1, -1);
                    }
                }
                Debug.LogError("full iteration");
                return (-1, -1, -1);
            case PlayerController.MoveDirection.Down:
                //iterate through i 
                if (startCoord.Item1 == 4)
                {
                    return (-1, -1, -1);
                }
                for (int i = startCoord.Item1+1; i<5; i++)
                {
                    GridTile currentTile = GridArray[i][startCoord.Item2].GetComponent<GridTile>();

                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        if (i == 4)
                        {
                            return (4, startCoord.Item2, tileCount);
                        }
                        continue;
                    }
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Normal &&
                         currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        return (i, startCoord.Item2, tileCount);
                    }

                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.NormalBox)
                    {
                        return (i - 1, startCoord.Item2, tileCount);
                    }

                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.IceBox)
                    {
                        //Move the box that you are collding with 
                        return (i - 1, startCoord.Item2, tileCount);
                    }

                    else
                    {
                        Debug.LogError("in the down case but still broke");
                        return (-1, -1, -1);
                    }
                }
                Debug.LogError("full iteration");
                return (-1, -1, -1);
            case PlayerController.MoveDirection.Right:
                //iterate through j
                if (startCoord.Item2 == 7)
                {
                    return (-1, -1, -1);
                }

                for (int j = startCoord.Item2 + 1; j < 8; j++)
                {
                    GridTile currentTile = GridArray[startCoord.Item1][j].GetComponent<GridTile>();
                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        if (j == 7)
                        {
                            return (startCoord.Item1,7, tileCount);
                        }
                        continue;
                    }

                    else if (currentTile.floorStatus == GridTile.FloorStatus.Normal &&
                currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        return (startCoord.Item1, j, tileCount);
                    }

                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.NormalBox)
                    {
                        return (startCoord.Item1, j - 1, tileCount);
                    }

                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.IceBox)
                    {
                        //Move box
                        return (startCoord.Item1, j - 1, tileCount);
                    }

                    else
                    {
                        Debug.LogError("in the right case but still broke");
                        return (-1, -1, -1);
                    }

                }
                Debug.LogError("full iteration");
                return (-1, -1, -1);
            case PlayerController.MoveDirection.Left:
                if (startCoord.Item2 == 0)
                {
                    return (-1, -1, -1);
                } 

                for (int j = startCoord.Item2-1; j >= 0; j--)
                {
                    GridTile currentTile = GridArray[startCoord.Item1][j].GetComponent<GridTile>();
                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        if (j == 0)
                        {
                            return (startCoord.Item1, 0, tileCount);
                        }
                        continue;
                    }

                    else if (currentTile.floorStatus == GridTile.FloorStatus.Normal &&
                currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        return (startCoord.Item1, j, tileCount);
                    }

                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.NormalBox)
                    {
                        return (startCoord.Item1, j + 1, tileCount);
                    }

                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.IceBox)
                    {
                        //Move box
                        return (startCoord.Item1, j + 1, tileCount);
                    }

                    else
                    {
                        Debug.LogError("in the left case but still broke");
                        return (-1, -1, -1);
                    }

                }
                Debug.LogError("full iteration");
                return (-1, -1, -1);
            default:
                Debug.LogError("default case");
                return (-1, -1, -1);

        }
    }

    public void MoveObject((int, int) startCoord, (int, int) endCoord, int tileCount, GameObject go)
    {
        Debug.Log("Start coord: " + startCoord.Item1 + "," + startCoord.Item2);
        Debug.Log("End coord: " + endCoord.Item1 + "," + endCoord.Item2);
        Transform start = GridArray[startCoord.Item1][startCoord.Item2].transform;
        Transform end;
        if (endCoord.Item1 < 0 || endCoord.Item2 < 0)
        {
            end = start;
        }
        else
        {
            end = GridArray[endCoord.Item1][endCoord.Item2].transform;
        }
        float time = 1 * tileCount;
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(MoveObjectCoroutine(time, start, end, go));
        }
    }

    private IEnumerator MoveObjectCoroutine(float time, Transform start, Transform end, GameObject go)
    {
        Vector3 startingPos = new Vector3(start.position.x, 2.3f, start.position.z);
        Vector3 endingPos = new Vector3(end.position.x, 2.3f, end.position.z);

        float elapsedTime = 0;
        go.transform.position = startingPos;
        while(elapsedTime < time)
        {
            go.transform.position = Vector3.Lerp(startingPos, endingPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRunning = false;
    }
    #endregion
}