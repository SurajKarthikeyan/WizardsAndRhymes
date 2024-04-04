using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSubSceneTrigger : MonoBehaviour
{
    public string subscene;

    public bool isLoaded;

    public bool isLoadingScene;

    public bool isUnloadingScene;

    public AKChangeState enterPuzzleState;

    private void LoadSubscene()
    {
        isLoadingScene = true;
        if (subscene != null)
        {
            // Start loading the subscene asynchronously if it's not already loaded
            if (!SceneManager.GetSceneByName(subscene).isLoaded)
            {
                SceneManager.LoadSceneAsync(subscene, LoadSceneMode.Additive).completed
                    += SubsceneLoadOperation_completed;
            }
        }
        else
        {
            Debug.LogError("Subscene string not provided");
        }
    }

    private void UnloadSubscene()
    {
        isUnloadingScene = true;

        if (subscene != null)
        {
            if (SceneManager.GetSceneByName(subscene).isLoaded)
            {
                SceneManager.UnloadSceneAsync(subscene).completed += SubsceneUnloadOperation_completed;
            }
        }
        else
        {
            Debug.LogError("Subscene string not provided");
        }
    }

    private void SubsceneLoadOperation_completed(AsyncOperation obj)
    {
        isLoaded = true;

        isLoadingScene = false;
        
        Debug.Log("Scene loaded");
    }
    
    private void SubsceneUnloadOperation_completed(AsyncOperation obj)
    {

        isUnloadingScene = false;

        isLoaded = false;

        Debug.Log("Scene unloaded");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLoadingScene && !isUnloadingScene && other.gameObject.CompareTag("Player"))
        {
            if (!isLoaded)
            {
                // Load the subscene asynchronously
                LoadSubscene();
                enterPuzzleState.ChangeState();
            }
            else
            {
                //Unload subscene asynchronously
                UnloadSubscene();
            }
        }
    }
}