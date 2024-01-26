using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramsformMaterialProperty : MonoBehaviour
{
    [Tooltip("The transform to pull data from")]
    [SerializeField] Transform source;
    [Tooltip("The Vector3 property on the material to set as the world position of the transform")]
    [SerializeField] string worldSpaceProperty;
    [Tooltip("The Vector3 property on the material to set as the local position of the transform")]
    [SerializeField] string localSpaceProperty;
    [Tooltip("The Vector3 property on the material to set as the clip space position of the transform")]
    [SerializeField] string clipSpaceProperty;
    [Tooltip("Whether to apply this to all instances of the material")]
    [SerializeField] bool applyGlobal;

    //Condition tag relies on Feel. Can be safely removed if needed
    [MMCondition("applyGlobal", true, true)] [Tooltip("The renderer whose material to control")]
    [SerializeField] Renderer render;
    [MMCondition("applyGlobal", true, true)] [Tooltip("The index of the material to control")]
    [SerializeField] int materialIndex = 0;

    [MMCondition("applyGlobal", true, false)] [Tooltip("The material to control all instances of")]
    [SerializeField] Material globalMaterial;

    Material material; //The controlled material
    Material materialBackup; //A backup of the material used to restore values when the game ends

    private void Awake()
    {
        //Get the material to control
        if (applyGlobal)
        {
            material = globalMaterial;
            materialBackup = new Material(material);
        }
        else
            material = render.materials[materialIndex];
    }

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

    private void OnDestroy()
    {
        //Restore original material values to prevent extraneous version control changes
        if (applyGlobal)
            material.CopyMatchingPropertiesFromMaterial(materialBackup);
    }
}
