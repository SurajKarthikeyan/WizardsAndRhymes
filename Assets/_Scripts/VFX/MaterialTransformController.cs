using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets Vector3 properties on a material to the position of a Transform
/// </summary>
public class MaterialTransformController : MaterialPropertyController
{
    #region Variables
    [Header("Transform Material Property Settings")]
    [Tooltip("The transform to pull data from")]
    [SerializeField] Transform source;
    [Tooltip("The Vector3 property on the material to set as the world position of the transform")]
    [SerializeField] string worldSpaceProperty;
    [Tooltip("The Vector3 property on the material to set as the local position of the transform")]
    [SerializeField] string localSpaceProperty;
    [Tooltip("The Vector3 property on the material to set as the clip space position of the transform")]
    [SerializeField] string clipSpaceProperty;
    #endregion

    #region Unity Methods
    private void Update()
    {
        //Set the material properties
        if (worldSpaceProperty != null) 
            material.SetVector(worldSpaceProperty, source.position);

        if (localSpaceProperty != null)
            material.SetVector(localSpaceProperty, source.localPosition);
                
        if (clipSpaceProperty != null)
            material.SetVector(clipSpaceProperty, Camera.main.WorldToViewportPoint(source.position));
    }
    #endregion
}
