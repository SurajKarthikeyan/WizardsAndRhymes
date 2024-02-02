using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MaterialPropertyController : MonoBehaviour
{

    [Header("Base Material Property Controller Settings")]
    [Tooltip("Whether to apply this to all instances of the material")]
    [SerializeField] bool applyGlobal;

    //Condition tag relies on Feel. Can be safely removed if needed
    [MMCondition("applyGlobal", true, true)]
    [Tooltip("The renderer whose material to control")]
    [SerializeField] Renderer render;
    [Tooltip("The decal whose material to control")]
    [SerializeField] DecalProjector decal;
    [MMCondition("applyGlobal", true, true)]
    [Tooltip("The index of the material to control")]
    [SerializeField] int materialIndex = 0;

    [MMCondition("applyGlobal", true, false)]
    [Tooltip("The material to control all instances of")]
    [SerializeField] Material globalMaterial;

    protected Material material; //The controlled material
    protected Material materialBackup; //A backup of the material used to restore values when the game ends

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
            if (render != null)
                material = render.materials[materialIndex];
            else if (decal != null)
                material = decal.material;
            else
                Debug.LogError("No valid target to edit material");
        }
    }

    protected void OnDestroy()
    {
        //Restore original material values to prevent extraneous version control changes
        if (applyGlobal)
            material.CopyMatchingPropertiesFromMaterial(materialBackup);
    }
}
