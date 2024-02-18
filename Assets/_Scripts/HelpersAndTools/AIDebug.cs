using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIDebug : MonoBehaviour
{
    #region Variables
    private TextMeshProUGUI debugText;

    private Canvas debugCanvas;

    private BaseEnemyBehavior enemyBehavior;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        debugCanvas = transform.GetComponentInChildren<Canvas>();
        debugCanvas.worldCamera = Camera.main;
        debugText = debugCanvas.GetComponentInChildren <TextMeshProUGUI>();
        enemyBehavior = GetComponent<BaseEnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = enemyBehavior.behaviorState.ToString();
    }

    private void OnDisable()
    {
        debugText.text = "";
    }
}
