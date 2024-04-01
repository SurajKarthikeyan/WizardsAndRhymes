using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSubSceneTrigger : MonoBehaviour
{
    public string subsceneAsset;

    private bool isLoaded = false;

    private void LoadSubscene()
    {
        isLoaded = true;
        if (subsceneAsset != null)
        {
            // Start loading the subscene asynchronously if it's not already loaded
            if (!SceneManager.GetSceneByName(subsceneAsset).isLoaded)
            {
                SceneManager.LoadSceneAsync(subsceneAsset, LoadSceneMode.Additive).completed += SubsceneLoadOperation_completed;
            }
        }
        else
        {
            Debug.LogError("Subscene Asset is not assigned!");
        }
    }

    private void SubsceneLoadOperation_completed(AsyncOperation obj)
    {
        // Once the subscene is loaded, find its root GameObject
        GameObject subsceneRoot = SceneManager.GetSceneByName(subsceneAsset).GetRootGameObjects()[0];

        // Parent the subscene root GameObject to an appropriate GameObject in the main scene
        subsceneRoot.transform.SetParent(transform);

        // Optionally, activate the subscene (make it visible)
        subsceneRoot.SetActive(true);

        Debug.Log("Scene loaded");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLoaded && other.gameObject.CompareTag("Player"))
        {
            // Load the subscene asynchronously
            LoadSubscene();
        }
    }
}