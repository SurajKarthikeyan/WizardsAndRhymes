using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Class that manages all the player ability logic
/// </summary>
public class AbilityManager : MonoBehaviour
{
    #region Class Variables
    /// <summary>
    /// GameObject that contains the ability slider, used for positioning
    /// </summary>
    [Header("Ability Slider Relevant References")]
    [Tooltip("GameObject that contains the ability slider")]
    [SerializeField]
    private GameObject m_AbilitySliderGameObject;

    /// <summary>
    /// Slider that is obtained from that GameObject, used for value changes and display
    /// </summary>
    private Slider m_AbilitySlider;

    /// <summary>
    /// Vector offset that is used for displaying the ability slider at the proper place
    /// </summary>
    private Vector3 m_AbilitySliderUIOffset = new (-0.1f, -0.45f, -1.25f);

    /// <summary>
    /// GameObject that contains the ability slider fill area
    /// </summary>
    [Tooltip("GameObject that contains the ability slider fill area")]
    [SerializeField]
    private GameObject m_SliderFillArea;

    /// <summary>
    /// Maximum value of the ability bar
    /// </summary>
    [Tooltip("Maximum value of the ability bar")]
    [SerializeField]
    private float m_MaximumAbilityValue = 100f;

    /// <summary>
    /// Minimum value of the ability bar
    /// </summary>
    [Tooltip("Minimum value of the ability bar")]
    [SerializeField]
    private float m_MinimumAbilityValue = 0f;

    /// <summary>
    /// Current value of the ability bar
    /// </summary>
    [Tooltip("Current value of the ability bar")]
    public float m_CurrentAbilityValue = 0f;

    /// <summary>
    /// Interval in seconds of one charge of ability meter (Every x seconds, the ability meter will charge a bit)
    /// </summary>
    [Tooltip("Interval in seconds of one charge of ability meter (Every x seconds, the ability meter will charge a bit)")]
    [SerializeField]
    private float abilityRechargeThreshold = 0.5f;

    public float rangedAbilityCost = 2f;

    public float meleeAbilityCost = 5f;

    public float dashAbilityCost = 10f;

    /// <summary>
    /// Float that acts as a timer in between each charge of the ability meter.
    /// </summary>
    private float abilityRechargeTimer;
    #endregion

    #region Methods

    #region Unity Methods
    /// <summary>
    /// Unity method that is called on scene start
    /// </summary>
    private void Awake()
    {
        m_AbilitySlider = m_AbilitySliderGameObject.GetComponent<Slider>();
        m_CurrentAbilityValue = m_MaximumAbilityValue;
        m_AbilitySlider.value = m_CurrentAbilityValue;
    }

    /// <summary>
    /// Unity Method that is called each frame
    /// </summary>
    private void FixedUpdate()
    {
        //Keeps ability slider with the player below them
        m_AbilitySliderGameObject.transform.position = transform.position + m_AbilitySliderUIOffset;

        abilityRechargeTimer += Time.deltaTime;

        if (abilityRechargeTimer >= abilityRechargeThreshold)
        {
            FillAbilityGauge();
        }

        
        m_CurrentAbilityValue = Mathf.Clamp(m_CurrentAbilityValue,
            m_MinimumAbilityValue, m_MaximumAbilityValue);

        m_AbilitySlider.value = m_CurrentAbilityValue;

        //These checks essentially make the bar go away entirely if it's empty, it doesn't do that on its own
        if (m_AbilitySlider.value <= 0)
        {
            m_SliderFillArea.SetActive(false);
        }
        else if (m_AbilitySlider.value > 0 && !m_SliderFillArea.activeInHierarchy)
        {
            m_SliderFillArea.SetActive(true);
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function called routinely to refill ability guage when not dashing or attacking
    /// </summary>
    public void FillAbilityGauge()
    {
        m_CurrentAbilityValue += 3;
        abilityRechargeTimer = 0;
    }

    /// <summary>
    /// Function used to reduce the ability value for the ability guage
    /// </summary>
    /// <param name="value"></param>
    public void ReduceAbilityGuage(float value)
    {
        m_CurrentAbilityValue -= value;
    }

    /// <summary>
    /// Function used to reset the ability charge timer
    /// </summary>
    public void ResetAbilityRecharge()
    {
        abilityRechargeTimer = 0;
    }
    #endregion
    #endregion
}
