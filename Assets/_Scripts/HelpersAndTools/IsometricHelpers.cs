using UnityEngine;

/// <summary>
/// Class for any helper functions for isometric related code
/// </summary>
public static class IsometricHelpers
{
    #region Variables
    //Will be changed to actually account for exact isometric angle later
    private static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    #endregion

    #region Methods
    #region Custom Methods
    public static Vector3 ToIso(this Vector3 input) => isoMatrix.MultiplyPoint3x4(input);
    #endregion
    #endregion
}
