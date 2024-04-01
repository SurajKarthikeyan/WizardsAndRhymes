using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoadTrigger : MonoBehaviour
{
    public string sceneToLoad;
    
    public bool isSceneLoaded;

    public bool loadingScene = false;

    public bool unloadingScene = false;

    private Scene currentlyLoadedScene;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !loadingScene && !unloadingScene)
        {
            if (!FlagManager.instance.GetFlag(("icePuzzle1Loaded")))
            {
                StartCoroutine(LoadSubScene(sceneToLoad));
                FlagManager.instance.SetFlag("icePuzzle1Loaded", true);
            }
                
            else
            {
                StartCoroutine(UnloadSubScene(sceneToLoad));
                FlagManager.instance.SetFlag("icePuzzle1Loaded", false);
            } 
        }
    }

    IEnumerator LoadSubScene(string sceneName)
    {
        loadingScene = true;
        
        var asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        asyncLoadScene.allowSceneActivation = false;
        
        while (!asyncLoadScene.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        asyncLoadScene.allowSceneActivation = true;

        isSceneLoaded = true;
        Debug.Log("Scene loaded");
        loadingScene = false;
    }

    IEnumerator UnloadSubScene(string sceneName)
    {
        unloadingScene = true;
        var asyncUnloadScene = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncUnloadScene.isDone)
        {
            yield return null;
        }

        isSceneLoaded = false;
        Debug.Log("Scene unloaded");
        unloadingScene = false;
    }
}
