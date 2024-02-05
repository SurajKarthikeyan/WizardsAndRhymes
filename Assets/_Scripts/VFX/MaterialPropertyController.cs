using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Base class for controlling properties on materials
/// </summary>
public class MaterialPropertyController : MonoBehaviour
{
    #region Variables
    [Tooltip("Possible component types to control the materials of")]
    enum TargetType { None, Renderer, Decal}


    [Header("Base Material Property Controller Settings")]
    [Tooltip("Whether to apply this to all instances of the material")]
    [SerializeField] bool applyGlobal;

    //Condition tag relies on Feel. Can be safely removed if needed
    [MMCondition("applyGlobal", true, true)]
    [Tooltip("The type of component to control the material of")]
    [SerializeField] TargetType targetType = TargetType.None;

    [MMEnumCondition("targetType", (int)TargetType.Renderer, Hidden =true)]
    [Tooltip("The renderer whose material to control")]
    [SerializeField] Renderer render;
    [MMEnumCondition("targetType", (int)TargetType.Renderer, Hidden = true)]
    [Tooltip("The index of the material to control")]
    [SerializeField] int materialIndex = 0;

    [MMEnumCondition("targetType", (int)TargetType.Decal, Hidden = true)]
    [Tooltip("The decal whose material to control")]
    [SerializeField] DecalProjector decal;
    

    [MMCondition("applyGlobal", true, false)]
    [Tooltip("The material to control all instances of")]
    [SerializeField] Material globalMaterial;


    [Tooltip("The controlled material")]
    protected Material material;
    [Tooltip("A backup of the material used to restore values when the game ends")]
    protected Material materialBackup;
    #endregion

    #region Unity Methods
    protected void Awake()
    {
        //Get the material to control
        if (applyGlobal)
        {
            material = globalMaterial;
            materialBackup = new Material(material);
        }
        else
        {
            if (targetType == TargetType.Renderer)
                material = render.materials[materialIndex];
            else if (targetType == TargetType.Decal)
                material = decal.material;
            else
                Debug.LogError("No valid target to edit material set");
        }
    }

    protected void OnDestroy()
    {
        //Restore original material values to prevent extraneous version control changes
        if (applyGlobal)
            material.CopyMatchingPropertiesFromMaterial(materialBackup);
    }
    #endregion
}
