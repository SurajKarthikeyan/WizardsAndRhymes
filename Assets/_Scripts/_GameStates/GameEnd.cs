using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that handles current game state(win / loss)
/// </summary>
public class GameEnd : MonoBehaviour
{
    #region Variable

    public static GameEnd gameEnd;
    
    [Header("Player")]
    [Tooltip("Reference to the player object to attain health")]
    [SerializeField] private GameObject player;
    [Tooltip("If the player has died or not")]
    [HideInInspector] private bool hasDied;
    
    [Header("Exit Door")]
    [Tooltip("Door to Instantiate")]
    [SerializeField] private GameObject doorRef;
    [Tooltip("This is a debug boolean used to test if the level is the 'final' level in the game(UnusedATM)")]
    [SerializeField] private bool isLastRoom;
    [Tooltip("Door open SFX")]
    [SerializeField] private AK.Wwise.Event doorOpenEvent;
    [Tooltip("This is here to ensure that once the room has been cleared it doesn't clear again")]
    [HideInInspector] private bool hasPassed;


    [Header("UI Elements")]
    [Tooltip("Restart button")] 
    [SerializeField] private Button restartButton;
    [Tooltip("TMP text for opening the door to the next room")]
    [SerializeField] private TextMeshProUGUI doorText;
    [Tooltip("TMP Pro text for dying")]
    [SerializeField] private TextMeshProUGUI deathText;
    [Tooltip("Toggle showing the room is cleared - interactable set to false")]
    [SerializeField] private Toggle roomClearedCheckbox;
    [SerializeField] private Color toggleOnColor;
    [SerializeField] private Color toggleOffColor;

    [Header("Etc.")]
    [Tooltip("Reference to scene fadeToBlack")]
    [SerializeField] private FadeToBlack fadeToBlack;
    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        EnemyManager.RoomCleared += RoomCleared;
    }

    private void OnDisable()
    {
        EnemyManager.RoomCleared -= RoomCleared;
    }

    /// <summary>
    /// Called at the first frame: ensures booleans are at correct starting values
    /// </summary>
    private void Start()
    {
        if (gameEnd == null)
        {
            gameEnd = this;
        }
        else
        {
            Debug.LogError("Two GameEnd scripts in scene");
        }

        roomClearedCheckbox.interactable = false;
        roomClearedCheckbox.isOn = false;
        
        var tempColorBlock = roomClearedCheckbox.colors;
        tempColorBlock.disabledColor = toggleOffColor;
        roomClearedCheckbox.colors = tempColorBlock;
        
        hasPassed = true;
        hasDied = false;
    }

    /// <summary>
    /// Called every frame
    /// Goes to player death screen when the player dies
    /// </summary>
    private void Update()
    {
        if (player.GetComponent<Health>().HP <= 0 && !hasDied) // Running the check here but it might be able to be abstracted
        {
            fadeToBlack.OnFadeOut();
            hasDied = true; 
            PlayerDeathUI();
        }
    }
    

    #endregion

    #region Custom Methods

    /// <summary>
    /// Invoked by the room cleared callback. Activates the exit door
    /// </summary>
    private void RoomCleared()
    {
        if (hasPassed)
        {
            var tempColorBlock = roomClearedCheckbox.colors;
            tempColorBlock.disabledColor = toggleOnColor;
            roomClearedCheckbox.colors = tempColorBlock;
            
            doorRef.SetActive(true);
            doorOpenEvent.Post(this.gameObject);
            hasPassed = false;
            roomClearedCheckbox.isOn = true;
        }
    }

    /// <summary>
    /// Activates appropriate UI elements when a end-room door is activated
    /// </summary>
    public void OpenDoorUI()
    {
        fadeToBlack.OnFadeOut();
        restartButton.gameObject.SetActive(true);
        doorText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates appropriate UI elements when player dies
    /// </summary>
    public void PlayerDeathUI()
    {
        restartButton.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Reset button that is activated the PlayerDeathUI() and OpenDoorUI() is pressed
    /// Reset all UI elements, reload scene
    /// </summary>
    public void ResetButton()
    {
        restartButton.gameObject.SetActive(false);
        doorText.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(currentSceneName);
    }
    #endregion
}
