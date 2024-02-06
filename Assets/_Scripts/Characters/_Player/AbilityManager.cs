using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Class that manages all the player ability logic
/// </summary>
public class AbilityManager : MonoBehaviour
{
    #region Variables
    [Header("Ability Slider Relevant References")]
    [Tooltip("GameObject that contains the ability slider")]
    [SerializeField]
    private GameObject abilitySliderGameObject;

    [Tooltip("GameObject that contains the ability slider fill area")]
    [SerializeField]
    private GameObject sliderFillArea;

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

    [Tooltip("Slider that is obtained from its GameObject, used for value changes and display")]
    private Slider abilitySlider;

    [Tooltip("Vector offset that is used for displaying the ability slider at the proper place")]
    private Vector3 abilitySliderUIOffset = new(-0.1f, -0.45f, -1.25f);
    #endregion

    #region Unity Methods
    /// <summary>
    /// Unity method that is called on scene start
    /// </summary>
    private void Awake()
    {
        abilitySlider = abilitySliderGameObject.GetComponent<Slider>();
        currentAbilityValue = maximumAbilityValue;
        abilitySlider.value = currentAbilityValue;
    }

    /// <summary>
    /// Unity Method that is called each frame
    /// </summary>
    private void FixedUpdate()
    {
        //Keeps ability slider with the player below them
        abilitySliderGameObject.transform.position = transform.position + abilitySliderUIOffset;

        abilityRechargeTimer += Time.deltaTime;

        //Charges gauge
        if (abilityRechargeTimer >= abilityRechargeThreshold)
        {
            FillAbilityGauge();
        }

        
        currentAbilityValue = Mathf.Clamp(currentAbilityValue,
            minimumAbilityValue, maximumAbilityValue);

        abilitySlider.value = currentAbilityValue;

        //These checks essentially make the bar go away entirely if it's empty, it doesn't do that on its own
        if (abilitySlider.value <= 0)
        {
            sliderFillArea.SetActive(false);
        }
        else if (abilitySlider.value > 0 && !sliderFillArea.activeInHierarchy)
        {
            sliderFillArea.SetActive(true);
        }
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
    #endregion
}
