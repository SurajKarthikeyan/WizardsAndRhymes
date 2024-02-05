using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the transform of the attached GameObject by setting all selected parameters to a single value
/// </summary>
//Based on the Feel TransformController script
public class SingleValueTransformcontroller : MonoBehaviour
{
    #region Variables
    [Tooltip("The value to set for all selected controls")]
    public float value;

    [Header("Position")]
    public bool controlPositionX;
    public bool controlPositionY;
    public bool controlPositionZ;

    [Header("Local Position")]
    public bool controlLocalPositionX;
    public bool controlLocalPositionY;
    public bool controlLocalPositionZ;

    [Header("Rotation")]
    public bool controlRotationX;
    public bool controlRotationY;
    public bool controlRotationZ;

    [Header("Local Rotation")]
    public bool controlLocalRotationX;
    public bool controlLocalRotationY;
    public bool controlLocalRotationZ;

    [Header("Scale")]
    public bool controlScaleX;
    public bool controlScaleY;
    public bool controlScaleZ;

    protected Vector3 position;
    protected Vector3 localPosition;
    protected Vector3 rotation;
    protected Vector3 localRotation;
    protected Vector3 scale;
    #endregion

    #region Unity Methods
    //Modify the selected properties every update
    protected virtual void Update()
    {
        position = this.transform.position;
        localPosition = this.transform.localPosition;
        rotation = this.transform.eulerAngles;
        localRotation = this.transform.localEulerAngles;
        scale = this.transform.localScale;

        if (controlPositionX) { position.x = value; this.transform.position = position; }
        if (controlPositionY) { position.y = value; this.transform.position = position; }
        if (controlPositionZ) { position.z = value; this.transform.position = position; }

        if (controlLocalPositionX) { localPosition.x = value; this.transform.localPosition = localPosition; }
        if (controlLocalPositionY) { localPosition.y = value; this.transform.localPosition = localPosition; }
        if (controlLocalPositionZ) { localPosition.z = value; this.transform.localPosition = localPosition; }

        if (controlRotationX) { rotation.x = value; this.transform.eulerAngles = rotation; }
        if (controlRotationY) { rotation.y = value; this.transform.eulerAngles = rotation; }
        if (controlRotationZ) { rotation.z = value; this.transform.eulerAngles = rotation; }

        if (controlLocalRotationX) { localRotation.x = value; this.transform.localEulerAngles = localRotation; }
        if (controlLocalRotationY) { localRotation.y = value; this.transform.localEulerAngles = localRotation; }
        if (controlLocalRotationZ) { localRotation.z = value; this.transform.localEulerAngles = localRotation; }

        if (controlScaleX) { scale.x = value; this.transform.localScale = scale; }
        if (controlScaleY) { scale.y = value; this.transform.localScale = scale; }
        if (controlScaleZ) { scale.z = value; this.transform.localScale = scale; }
    }
    #endregion Unity Methods
}
