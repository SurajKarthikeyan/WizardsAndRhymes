using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    #region Variables
    public enum OccupationStatus
    {
        None,
        Player,
        NormalBox,
        IceBox
    }

    public enum FloorStatus
    {
        Normal,
        Walker,
        Ice
    }
    [Tooltip("Floor status of this tile")]
    [SerializeField] public FloorStatus floorStatus;

    [Tooltip("Occupation status of this tile")]
    [SerializeField] public OccupationStatus occupationStatus;


    [SerializeField] private PuzzleGrid puzzleGrid;

    [Tooltip("Object currently occupying this tile")]
    public GameObject occupyingObject;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GroundCheck"))
        {
            puzzleGrid.EvaluateObjectMovement(gameObject.name);
        }
    }

}
