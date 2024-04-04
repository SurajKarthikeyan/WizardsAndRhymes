using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningBlockManager : MonoBehaviour
{
    #region Vars

    [Tooltip("List of lighting blocks AND generator (PLACE THEM IN ORDER FROM FIRST TO LAST(GENERATOR IS LAST))")]
    [SerializeField] private List<GameObject> lightBlockList;
    [Tooltip("Reference to the generator of this lightining system")]
    [SerializeField] private Generator lightningGenerator;
    [Tooltip("Eletric arc effect")]
    [SerializeField] private GameObject lightningEffectPrefab;
    [Tooltip("Time between each chain between blocks")]
    [SerializeField] private float lightningChainDelay;
    [Tooltip("Time to wait before destorying all lightning on incorrect puzzle completion")]
    [SerializeField] private float waitLightningDestroy;

    [Tooltip("Y offset for lighting chain spawn")]
    [SerializeField] private float yOffSet;

    [Tooltip("Holographic block list to spawn in location of deactivated block")]
    [SerializeField] private List<GameObject> holoBlockList;
    #endregion


    #region UnityMethods

    private void Start()
    {
        
        SetHoloBlocks();

    }

    #endregion
    

    #region CustomMethods

    /// <summary>
    /// Spawn all the holo blocks and set them active if the eletric block in that location is decativated
    /// </summary>
    public void SetHoloBlocks()
    {
        for (int i = 0; i < holoBlockList.Count; i++)
        {
            Vector3 pos = lightBlockList[i].transform.position;
            GameObject curHoloBlock = holoBlockList[i];
            curHoloBlock.transform.position = pos;
            if (lightBlockList[i].activeInHierarchy == false)
            {
                curHoloBlock.SetActive(true);
            }
        }
    }
    public void CheckAllBlocks()
    {
        StartCoroutine(ChainLightingWithDelayCoroutine());
    }
    
    /// <summary>
    /// Chains lightning between two objects
    /// </summary>
    public GameObject ChainLightning(Vector3 startPos, Vector3 endPos)
    {
        startPos = new Vector3(startPos.x, startPos.y + yOffSet, startPos.z);
        endPos = new Vector3(endPos.x, endPos.y + yOffSet, endPos.z);
        Vector3 deltaPos = endPos - startPos;
        GameObject curLightningEffect = Instantiate(lightningEffectPrefab);
        
        // ensure that the overall GO position is within render location
        curLightningEffect.transform.position = startPos;
        
        // Set start and end positions
        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position =
            startPos;
        curLightningEffect.GetComponent<LightningVFXPosition>().pos4.transform.position =
            endPos;
        
        Vector3 pos2 = startPos + deltaPos * 0.33f;
        Vector3 pos3 = startPos + deltaPos * 0.66f;
        curLightningEffect.GetComponent<LightningVFXPosition>().pos2.transform.position = pos2;
        curLightningEffect.GetComponent<LightningVFXPosition>().pos3.transform.position = pos3;

        return curLightningEffect;
    }

    /// <summary>
    /// Coroutine to chain lightning to all the blocks with a delay to show progression
    /// Will chain to the generator at the end!
    /// </summary>
    /// <returns></returns>
    IEnumerator ChainLightingWithDelayCoroutine()
    {
        List<GameObject> chainList = new List<GameObject>();
        for (int i = 0; i < lightBlockList.Count - 1; i++)
        {
            Vector3 startPos = lightBlockList[i].transform.position;
            // bloom current box
           StartCoroutine(lightBlockList[i].GetComponent<IndividualEmissionChange>().AlterEmissionOverTime(true));
            if (lightBlockList[i + 1].activeSelf)
            {
            Vector3 endPos = lightBlockList[i + 1].transform.position;
            chainList.Add(ChainLightning(startPos, endPos));
            yield return new WaitForSeconds(lightningChainDelay);
            }
            else
            {
                yield return new WaitForSeconds(waitLightningDestroy);
                for (int k = 0; k <= i; k++)
                {
                    StartCoroutine(lightBlockList[k].GetComponent<IndividualEmissionChange>().AlterEmissionOverTime(false));
                }
                chainList.ForEach(obj => Destroy(obj));
                yield break;
            }
        }
        lightningGenerator.Hodor();
    }

    #endregion
}
