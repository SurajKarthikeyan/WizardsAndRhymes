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
    #endregion

    #region UnityMethods

    private void Start()
    {
        GridArray = new List<List<GameObject>>();
        GridArray[0] = AGrid;
        GridArray[1] = BGrid;
        GridArray[2] = CGrid;
        GridArray[3] = DGrid;
        GridArray[4] = EGrid;
        Debug.Log(GridArray);
    }
    #endregion

    #region CustomMethods

    public void EvaluateObjectMovement(string name)
    {
        moveDirection = PlayerController.instance.moveDirection;
        (int, int) startCoord = StringToGridCoordinate(name);

        (int, int,int) endCoord = CalculateEndCoordinate(startCoord, moveDirection);

        Debug.Log(endCoord);
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
                Debug.Log(startCoord);
                for (int i = startCoord.Item1; i >= 0; i--)
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
            //case PlayerController.MoveDirection.Down:
            //    //Iterate through i 
            //    break;
            //case PlayerController.MoveDirection.Right:
            //    //Iterate through j
            //    break;
            //case PlayerController.MoveDirection.Left:
            //    //Iterate through j in reverse
            //    break;
            default:
                Debug.LogError("default case");
                return (-1, -1, -1);

        }
    }
    #endregion
}
