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
    #endregion


    #region CustomMethods

    public void CheckAllBlocks()
    {
        StartCoroutine(ChainLightingWithDelayCoroutine());
        /*if (lightBlockList.All(obj => obj.activeSelf))  // checks if all objects are enabled in the list
        {
        }*/
    }
    
    /// <summary>
    /// Chains lightning between two objects
    /// </summary>
    public GameObject ChainLightning(Vector3 startPos, Vector3 endPos)
    {
        Vector3 deltaPos = endPos - startPos;
        GameObject curLightningEffect = Instantiate(lightningEffectPrefab);
        
        // ensure that the overall GO position is within render location
        curLightningEffect.transform.position = startPos;
        
        // Set start and end positions
        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position =
            startPos;
        curLightningEffect.GetComponent<LightningVFXPosition>().pos4.transform.position =
            endPos;
        
        Vector3 pos2 = new Vector3(
            curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.x + deltaPos.x * 0.33f,
            curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.y + deltaPos.y * 0.33f,
            curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.z + deltaPos.z * 0.33f);
        
        Vector3 pos3 = new Vector3(
            curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.x + deltaPos.x * 0.66f,
            curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.y + deltaPos.y * 0.66f,
            curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.z + deltaPos.z * 0.66f);

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
            if (lightBlockList[i + 1].activeSelf)
            {
            Vector3 endPos = lightBlockList[i + 1].transform.position;
            chainList.Add(ChainLightning(startPos, endPos));
            yield return new WaitForSeconds(lightningChainDelay);
            }
            else
            {
                yield return new WaitForSeconds(waitLightningDestroy);
                chainList.ForEach(obj => Destroy(obj));
                yield break;
            }
        }
        lightningGenerator.Hodor();
    }

    #endregion
}
