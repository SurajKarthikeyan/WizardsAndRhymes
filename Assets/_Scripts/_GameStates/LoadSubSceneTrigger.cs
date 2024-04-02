using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoadSubSceneTrigger : MonoBehaviour
{
    public string subscene;

    public bool isLoaded;

    public bool isLoadingScene;

    public bool isUnloadingScene;

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
        // Once the subscene is loaded, find its root GameObject
        GameObject subsceneRoot = SceneManager.GetSceneByName(subscene).GetRootGameObjects()[0];

        // Parent the subscene root GameObject to an appropriate GameObject in the main scene
        subsceneRoot.transform.SetParent(transform);

        // Optionally, activate the subscene (make it visible)
        subsceneRoot.SetActive(true);

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
            }
            else
            {
                //Unload subscene asynchronously
                UnloadSubscene();
            }
        }
    }
}