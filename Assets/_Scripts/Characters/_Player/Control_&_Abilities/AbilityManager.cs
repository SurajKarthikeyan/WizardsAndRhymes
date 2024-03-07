using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that manages all the player ability logic
/// </summary>
public class AbilityManager : MonoBehaviour
{
    #region Variables
    //[Header("Ability Slider Relevant References")]
    //[Tooltip("GameObject that contains the ability slider")]
    //[SerializeField]
    //private GameObject abilitySliderGameObject;

    //[Tooltip("GameObject that contains the ability slider fill area")]
    //[SerializeField]
    //private GameObject sliderFillArea;

    [Header("Ability Slider values")]
    [Tooltip("Interval in seconds of one charge of ability meter (Every x seconds, the ability meter will charge a bit)")]
    [SerializeField]
    private float abilityRechargeThreshold = 0.5f;

    [Tooltip("Amount that the ability gauge fills every time it is charged")]
    [SerializeField]
    private float abilityGaugeFillValue = 3f;

    [Tooltip("Minimum value of the ability bar")]
    [SerializeField]
    private float minimumAbilityValue = 0f;

    [Tooltip("Maximum value of the ability bar")]
    [SerializeField]
    private float maximumAbilityValue = 100f;

    [Tooltip("Current value of the ability bar")]
    public float currentAbilityValue = 0f;

    [Tooltip("Amount of ability gauge used when ranged attacking")]
    public float rangedAbilityCost = 2f;

    [Tooltip("Amount of ability gauge used when ranged attacking")]
    public float meleeAbilityCost = 5f;

    [Tooltip("Amount of ability gauge used when dashing")]
    public float dashAbilityCost = 10f;

    [Tooltip("Float that acts as a timer in between each charge of the ability meter.")]
    private float abilityRechargeTimer;

    [Tooltip("Vector offset that is used for displaying the ability slider at the proper place")]
    private Vector3 abilityPipUIOffset = new(0, 1.5f, -3.75f);


    public GameObject abilityCanvas;

    public List<SpriteRenderer> manaSprites = new();
    #endregion

    #region Unity Methods
    /// <summary>
    /// Unity method that is called on scene start
    /// </summary>
    private void Awake()
    {

        abilityCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        foreach(SpriteRenderer spriteRenderer in abilityCanvas.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            manaSprites.Add(spriteRenderer);
        }
        currentAbilityValue = maximumAbilityValue;
    }

    /// <summary>
    /// Unity Method that is called each frame
    /// </summary>
    private void FixedUpdate()
    {
        //Keeps ability slider with the player below them
        abilityCanvas.transform.position = transform.position + abilityPipUIOffset;

        abilityRechargeTimer += Time.deltaTime;

        //Charges gauge
        if (abilityRechargeTimer >= abilityRechargeThreshold)
        {
            FillAbilityGauge();
        }

        
        currentAbilityValue = Mathf.Clamp(currentAbilityValue,
            minimumAbilityValue, maximumAbilityValue);

        float abilityPercentage = currentAbilityValue / maximumAbilityValue * 100;

        SetManaPips(abilityPercentage); 
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function called routinely to refill ability guage when not dashing or attacking
    /// </summary>
    public void FillAbilityGauge()
    {
        currentAbilityValue += abilityGaugeFillValue;
        abilityRechargeTimer = 0;
    }

    /// <summary>
    /// Function used to reduce the ability value for the ability guage
    /// </summary>
    /// <param name="value"></param>
    public void ReduceAbilityGuage(float value)
    {
        currentAbilityValue -= value;
    }

    /// <summary>
    /// Function used to reset the ability charge timer
    /// </summary>
    public void ResetAbilityRecharge()
    {
        abilityRechargeTimer = 0;
    }

    public void SetManaPips(float abilityPercentage)
    {
        if (abilityPercentage < 100)
        {
            manaSprites[manaSprites.Count - 1].color = Color.red;
        }
        else
        {
            manaSprites[manaSprites.Count - 1].color = Color.green;
        }

        if (abilityPercentage < 75)
        {
            manaSprites[manaSprites.Count - 2].color = Color.red;
        }
        else
        {
            manaSprites[manaSprites.Count - 2].color = Color.green;
        }

        if (abilityPercentage < 50)
        {
            manaSprites[manaSprites.Count - 3].color = Color.red;
        }
        else
        {
            manaSprites[manaSprites.Count - 3].color = Color.green;
        }

        if (abilityPercentage < 25)
        {
            manaSprites[manaSprites.Count - 4].color = Color.red;
        }
        else
        {
            manaSprites[manaSprites.Count - 4].color = Color.green;
        }
    }
    #endregion
}
