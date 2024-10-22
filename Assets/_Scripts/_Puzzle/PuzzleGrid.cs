using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that represents a grid for an ice puzzle
/// </summary>
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
    [Tooltip("Grid List F")]
    [SerializeField] private List<GameObject> FGrid;
    [Tooltip("Grid List Z")]
    [SerializeField] private List<GameObject> ZGrid;
    

    
    
    [Tooltip("MoveDireciton instance")]
    [SerializeField] private PlayerController.MoveDirection moveDirection;

    [Tooltip("2D Array representing the entier grid array")]
    public List<List<GameObject>> gridArray;

    public GameObject moveObject;

    public bool isMovingObject;

    public int letterCoord;

    public int numberCoord;

    public bool hasTouchedIceBox;

    [SerializeField] private AK.Wwise.Event icePushSoundEffect;

    [SerializeField] private AK.Wwise.Event playerIceSlideEffect;

    public bool touchedTile;


    public float boxPushYOffset = 0;
    #endregion

    #region UnityMethods

    private void Start()
    {
        touchedTile = false;
        gridArray = new List<List<GameObject>>();
        gridArray.Add(ZGrid);
        gridArray.Add(AGrid);
        gridArray.Add(BGrid);
        gridArray.Add(CGrid);
        gridArray.Add(DGrid);
        gridArray.Add(EGrid);
        gridArray.Add(FGrid);

        // temp code;
        moveObject = PlayerController.instance.gameObject;
    }
    #endregion

    #region CustomMethods

    public void EvaluateObjectMovement(string name)
    {
        moveDirection = PlayerController.instance.moveDirection;
        (int, int) startCoord = StringToGridCoordinate(name);

        GridTile startTile = gridArray[startCoord.Item1][startCoord.Item2].GetComponent<GridTile>();

        if (startTile.occupationStatus == GridTile.OccupationStatus.IceBox)
        {
            moveObject = startTile.occupyingObject;
            (int, int, int) endCoord = CalculateEndCoordinate(startCoord, moveDirection);
            GridTile endtile = gridArray[endCoord.Item1][endCoord.Item2].GetComponent<GridTile>();

            MoveObject(startCoord, (endCoord.Item1, endCoord.Item2), endCoord.Item3, moveObject);
            endtile.occupyingObject = startTile.occupyingObject;
            endtile.occupationStatus = GridTile.OccupationStatus.IceBox;
            startTile.occupationStatus = GridTile.OccupationStatus.None;
            startTile.occupyingObject = null;
        }

        else
        {
            moveObject = PlayerController.instance.gameObject;

            (int, int,int) endCoord = CalculateEndCoordinate(startCoord, moveDirection);

            MoveObject(startCoord, (endCoord.Item1, endCoord.Item2), endCoord.Item3, moveObject);

        }
    }

    (int, int) StringToGridCoordinate(string name)
    {
        char x = name[0];
        char y = name[1];

        int yCoord = y- '0' - 1;

        int xCoord = 0;

        switch (x)
        {
            case 'Z':
                xCoord = 0;
                break;
            case 'A':
                xCoord = 1;
                break;
            case 'B':
                xCoord = 2;
                break;
            case 'C':
                xCoord = 3;
                break;
            case 'D':
                xCoord = 4;
                break;
            case 'E':
                xCoord = 5;
                break;
            case 'F':
                xCoord = 6;
                break;
        }

        (int,int) returnTuple = (xCoord, yCoord);

        return returnTuple;
    }

    (int,int,int) CalculateEndCoordinate((int,int) startCoord, PlayerController.MoveDirection direction)
    {
        int tileCount = 1;
        switch (direction)
        {
            case PlayerController.MoveDirection.Up:
                //Iterate through i in reverse
                if(startCoord.Item1 == 1)
                {
                    return (-1, -1, -1);
                }
                for (int i = startCoord.Item1-1; i >= 0; i--)
                {
                    GridTile currentTile = gridArray[i][startCoord.Item2].GetComponent<GridTile>();
                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice && 
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        if (i == 1)
                        {
                            return (1, startCoord.Item2, tileCount);
                        }
                        continue;
                    }
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Normal &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        return (i, startCoord.Item2, tileCount);
                    }
                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.NormalBox )
                    {
                        return (i + 1, startCoord.Item2, tileCount);
                    }
                    else if (currentTile.occupationStatus == GridTile.OccupationStatus.IceBox)
                    {
                        //Move the box that you are collding with 
                        hasTouchedIceBox = true;
                        letterCoord = i;
                        numberCoord = startCoord.Item2;
                        return (i + 1, startCoord.Item2, tileCount);
                    }
                    
                    
                    
                    /// NEW CODE HERE ///
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject != PlayerController.instance.gameObject)
                    {
                        return (i + 1, startCoord.Item2, tileCount);
                    }
                    
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject == PlayerController.instance.gameObject)
                    {
                        tileCount++;
                        return (i, startCoord.Item2, tileCount);
                    }
                    /// NEW CODE HERE ///
                    else
                    {
                        return (-1, -1, -1);
                    }
                }
                return (-1, -1, -1);
            case PlayerController.MoveDirection.Down:
                //iterate through i 
                if (startCoord.Item1 == 6)
                {
                    return (-1, -1, -1);
                }
                for (int i = startCoord.Item1+1; i<gridArray.Count; i++)
                {
                    GridTile currentTile = gridArray[i][startCoord.Item2].GetComponent<GridTile>();

                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        Debug.Log("Yo mama 1");
                        tileCount++;
                        if (i == gridArray.Count - 1)
                        {
                            return (gridArray.Count - 1, startCoord.Item2, tileCount);
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
                        Debug.Log("Yo mama 4");
                        //Move the box that you are collding with 
                        hasTouchedIceBox = true;
                        letterCoord = i;
                        numberCoord = startCoord.Item2;
                        return (i - 1, startCoord.Item2, tileCount);
                    }
                    
                    /// NEW CODE HERE ///
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject != PlayerController.instance.gameObject)
                    {
                        return (i - 1, startCoord.Item2, tileCount);
                    }
                    
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject == PlayerController.instance.gameObject)
                    {
                        tileCount++;
                        return (i, startCoord.Item2, tileCount);
                    }
                    /// NEW CODE HERE ///

                    else
                    {
                        return (-1, -1, -1);
                    }
                }
                return (-1, -1, -1);
            case PlayerController.MoveDirection.Right:
                //iterate through j
                if (startCoord.Item2 == gridArray[0].Count - 1)
                {
                    return (-1, -1, -1);
                }

                for (int j = startCoord.Item2 + 1; j < gridArray[0].Count; j++)
                {
                    GridTile currentTile = gridArray[startCoord.Item1][j].GetComponent<GridTile>();
                    if (currentTile.floorStatus == GridTile.FloorStatus.Ice &&
                        currentTile.occupationStatus == GridTile.OccupationStatus.None)
                    {
                        tileCount++;
                        if (j == gridArray[0].Count - 1)
                        {
                            return (startCoord.Item1, gridArray[0].Count - 1, tileCount);
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
                        hasTouchedIceBox = true;
                        letterCoord = startCoord.Item1;
                        numberCoord = j;
                        return (startCoord.Item1, j - 1, tileCount);
                    }
                    
                    /// NEW CODE HERE ///
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject != PlayerController.instance.gameObject)
                    {
                        return (startCoord.Item1, j - 1, tileCount);
                    }
                    
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject == PlayerController.instance.gameObject)
                    {
                        tileCount++;
                        return (startCoord.Item1, j , tileCount);
                    }
                    /// NEW CODE HERE ///

                    else
                    {
                        return (-1, -1, -1);
                    }

                }
                return (-1, -1, -1);
            case PlayerController.MoveDirection.Left:
                if (startCoord.Item2 == 0)
                {
                    return (-1, -1, -1);
                } 

                for (int j = startCoord.Item2-1; j >= 0; j--)
                {
                    GridTile currentTile = gridArray[startCoord.Item1][j].GetComponent<GridTile>();
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
                        hasTouchedIceBox = true;
                        letterCoord = startCoord.Item1;
                        numberCoord = j;
                        return (startCoord.Item1, j + 1, tileCount);
                    }
                    
                    /// NEW CODE HERE ///
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject != PlayerController.instance.gameObject)
                    {
                        return (startCoord.Item1, j + 1, tileCount);
                    }
                    
                    else if (currentTile.floorStatus == GridTile.FloorStatus.Walker &&
                             moveObject == PlayerController.instance.gameObject)
                    {
                        tileCount++;
                        return (startCoord.Item1, j , tileCount);
                    }
                    /// NEW CODE HERE ///

                    else
                    {
                        return (-1, -1, -1);
                    }

                }
                return (-1, -1, -1);
            default:
                return (-1, -1, -1);

        }
    }

    void MoveSecondObject((int,int) startCoord, PlayerController.MoveDirection direction)
    {
        (int, int, int) iceBoxEndCoord = CalculateEndCoordinate(startCoord, direction);
        GridTile startTile = gridArray[startCoord.Item1][startCoord.Item2].GetComponent<GridTile>();
        GridTile endTile;
        if (iceBoxEndCoord == (-1, -1, -1))
        {
            endTile = startTile;
            isMovingObject = false;
            moveObject = null;
        }
        else
        {
            endTile = gridArray[iceBoxEndCoord.Item1][iceBoxEndCoord.Item2].GetComponent<GridTile>();
            if (endTile.gameObject != startTile.gameObject)
            {
                
                endTile.occupyingObject = startTile.occupyingObject;
                endTile.occupationStatus = GridTile.OccupationStatus.IceBox;
                startTile.occupyingObject = null;
                startTile.occupationStatus = GridTile.OccupationStatus.None;
                
            }
            else
            {
                Debug.Log("Same Tile");
            }
            moveObject = endTile.occupyingObject;
            MoveObject(startCoord, (iceBoxEndCoord.Item1, iceBoxEndCoord.Item2),
                iceBoxEndCoord.Item3, moveObject);
        }
    }

    public void MoveObject((int, int) startCoord, (int, int) endCoord, int tileCount, GameObject go)
    {
        Transform start = gridArray[startCoord.Item1][startCoord.Item2].transform;
        Transform end;
        if (endCoord.Item1 < 0 || endCoord.Item2 < 0)
        {
            end = start;
            isMovingObject = false;
            moveObject = null;
        }
        else
        {
            end = gridArray[endCoord.Item1][endCoord.Item2].transform;
        }
        float time = 0.33f * tileCount;
        if (!isMovingObject)
        {
            isMovingObject = true;
            if (go != null)
            {
                StartCoroutine(MoveObjectCoroutine(time, start, end, go));
            }
        }
    }

    private IEnumerator MoveObjectCoroutine(float time, Transform start, Transform end, GameObject go)
    {
        bool hasEmission = false;
        isMovingObject = true;
        PlayerController.instance.DisablePlayerControls();

        Vector3 startingPos = new Vector3(start.position.x, go.transform.position.y, start.position.z);
        Vector3 endingPos = new Vector3(end.position.x, go.transform.position.y, end.position.z);

        float elapsedTime = 0;
        go.transform.position = startingPos;
        
        if(go.TryGetComponent(out IndividualEmissionChange var))
        {
            icePushSoundEffect.Post(this.gameObject);
            hasEmission = true;
            StartCoroutine(var.AlterEmissionOverTime(true));
        }
        
        else if (go.TryGetComponent(out PlayerController instance))
        {
            playerIceSlideEffect.Post(this.gameObject);
            AkSoundEngine.SetState("PlayerOnIce", "IsSliding");
        }
        while(elapsedTime < time)
        {
            isMovingObject = true;
            PlayerController.instance.DisablePlayerControls();
            go.transform.position = Vector3.Lerp(startingPos, endingPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        AkSoundEngine.SetState("PlayerOnIce", "NotSliding");

        if (hasEmission)
        {
            StartCoroutine(var.AlterEmissionOverTime(false));

        }
        isMovingObject = false;
        moveObject = null;
        if (hasTouchedIceBox)
        {
            hasTouchedIceBox = false;
            (int, int) iceBoxStartCoord = (letterCoord, numberCoord);
            MoveSecondObject(iceBoxStartCoord, moveDirection);
            isMovingObject = false;
        }

        if (!isMovingObject)
        {
            PlayerController.instance.EnablePlayerControls();
        }
    }
    #endregion
}
