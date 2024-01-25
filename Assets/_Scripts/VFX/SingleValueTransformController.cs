using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on the Feel TransformController script
public class SingleValueTransformController : MonoBehaviour
{
    [Tooltip("The value to set for all selected controls")]
    public float value;

    [Header("Position")]
    public bool ControlPositionX;
    public bool ControlPositionY;
    public bool ControlPositionZ;

    [Header("Local Position")]
    public bool ControlLocalPositionX;
    public bool ControlLocalPositionY;
    public bool ControlLocalPositionZ;

    [Header("Rotation")]
    public bool ControlRotationX;
    public bool ControlRotationY;
    public bool ControlRotationZ;

    [Header("Local Rotation")]
    public bool ControlLocalRotationX;
    public bool ControlLocalRotationY;
    public bool ControlLocalRotationZ;

    [Header("Scale")]
    public bool ControlScaleX;
    public bool ControlScaleY;
    public bool ControlScaleZ;

    protected Vector3 _position;
    protected Vector3 _localPosition;
    protected Vector3 _rotation;
    protected Vector3 _localRotation;
    protected Vector3 _scale;

    /// <summary>
    /// At update, modifies the requested properties
    /// </summary>
    protected virtual void Update()
    {
        _position = this.transform.position;
        _localPosition = this.transform.localPosition;
        _rotation = this.transform.eulerAngles;
        _localRotation = this.transform.localEulerAngles;
        _scale = this.transform.localScale;

        if (ControlPositionX) { _position.x = value; this.transform.position = _position; }
        if (ControlPositionY) { _position.y = value; this.transform.position = _position; }
        if (ControlPositionZ) { _position.z = value; this.transform.position = _position; }

        if (ControlLocalPositionX) { _localPosition.x = value; this.transform.localPosition = _localPosition; }
        if (ControlLocalPositionY) { _localPosition.y = value; this.transform.localPosition = _localPosition; }
        if (ControlLocalPositionZ) { _localPosition.z = value; this.transform.localPosition = _localPosition; }

        if (ControlRotationX) { _rotation.x = value; this.transform.eulerAngles = _rotation; }
        if (ControlRotationY) { _rotation.y = value; this.transform.eulerAngles = _rotation; }
        if (ControlRotationZ) { _rotation.z = value; this.transform.eulerAngles = _rotation; }

        if (ControlLocalRotationX) { _localRotation.x = value; this.transform.localEulerAngles = _localRotation; }
        if (ControlLocalRotationY) { _localRotation.y = value; this.transform.localEulerAngles = _localRotation; }
        if (ControlLocalRotationZ) { _localRotation.z = value; this.transform.localEulerAngles = _localRotation; }

        if (ControlScaleX) { _scale.x = value; this.transform.localScale = _scale; }
        if (ControlScaleY) { _scale.y = value; this.transform.localScale = _scale; }
        if (ControlScaleZ) { _scale.z = value; this.transform.localScale = _scale; }
    }
}
