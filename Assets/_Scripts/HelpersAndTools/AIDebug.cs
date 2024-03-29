using TMPro;
using UnityEngine;

/// <summary>
/// Class used to debug the behavior of the AI
/// </summary>
public class AIDebug : MonoBehaviour
{
    #region Variables
    [Tooltip("Text used to debug the state")]
    private TextMeshProUGUI debugText;

    [Tooltip("Canvas the text is a part of")]
    private Canvas debugCanvas;

    [Tooltip("Enemy script reference to get the behavior from")]
    private BaseEnemyBehavior enemyBehavior;

    [Tooltip("Enemy script reference to get the health from")]
    private BaseEnemyHealth enemyHealth;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Method called on scene load
    /// </summary>
    void Start()
    {
        debugCanvas = transform.GetComponentInChildren<Canvas>();
        debugCanvas.worldCamera = Camera.main;
        debugText = debugCanvas.GetComponentInChildren <TextMeshProUGUI>();
        enemyBehavior = GetComponent<BaseEnemyBehavior>();
        enemyHealth = GetComponent<BaseEnemyHealth>();
    }

    /// <summary>
    /// Unity method called once per frame
    /// </summary>
    void Update()
    {
        if (!enemyBehavior.activated)
        {
            ClearDebugText();
        }
        else
        {
            debugText.text = enemyBehavior.behaviorState.ToString();
            Vector3 rotation = Quaternion.LookRotation(Camera.main.transform.position, Vector3.up).eulerAngles;
            // rotation.y = 180;
            rotation.x = 0;
            debugCanvas.transform.forward = -debugCanvas.worldCamera.transform.forward;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that clears the debug text
    /// </summary>
    public void ClearDebugText()
    {
        debugText.text = "";
    }
    #endregion
}
