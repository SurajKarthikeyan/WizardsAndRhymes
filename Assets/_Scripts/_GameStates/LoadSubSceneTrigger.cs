using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSubSceneTrigger : MonoBehaviour
{
    public string subscene;

    public bool isLoaded;

    public bool isLoadingScene;

    public bool isUnloadingScene;

    public AKChangeState enterPuzzleState;

    public bool isLoader;

    public List<LoadSubSceneTrigger> otherLoaders;

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

    public void UnloadSubscene(string name)
    {
        isUnloadingScene = true;

    
        if (SceneManager.GetSceneByName(name).isLoaded)
        {
            SceneManager.UnloadSceneAsync(name).completed += SubsceneUnloadOperation_completed;
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

        foreach (LoadSubSceneTrigger otherLoader in otherLoaders)
        {
            otherLoader.isLoaded = isLoaded;
        }
        
        
        Debug.Log("Scene loaded");
    }
    
    private void SubsceneUnloadOperation_completed(AsyncOperation obj)
    {

        isUnloadingScene = false;

        isLoaded = false;
        
        foreach (LoadSubSceneTrigger otherLoader in otherLoaders)
        {
            otherLoader.isLoaded = isLoaded;
        }

        Debug.Log("Scene unloaded");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLoadingScene && !isUnloadingScene && other.gameObject.CompareTag("SceneLoader"))
        {
            if (isLoader && !isLoaded)
            {
                // Load the subscene asynchronously
                LoadSubscene();
                enterPuzzleState.ChangeState();
            }
            else if (!isLoader && isLoaded)
            {
                //Unload subscene asynchronously
                UnloadSubscene();
            }
        }
    }
}