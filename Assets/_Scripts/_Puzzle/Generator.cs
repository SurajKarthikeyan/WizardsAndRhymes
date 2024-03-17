using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator : MonoBehaviour
{
   #region Variables
   [Tooltip("The gate to despawn when generator is turned on")]
   [SerializeField] private GameObject gate;
   [Tooltip("If the generator is on")]
   [SerializeField] public bool isOn;
   [Tooltip("Material to set generator to when its turned on")]
   [SerializeField] private Material generatorOnMaterial;
   [Tooltip("Renderer of the generator")]
   [SerializeField] private Renderer generatorRenderer;
   [Tooltip("Reference to the lights if not using lightning blocks")]
   [SerializeField] private List<GameLight> allLights;

   [Tooltip("Are you using lightning blocks or spotlights to turn on the generator?")]
   [SerializeField] private bool useLightningBlocks;
   [Tooltip("AK Sound event for generator sfx")]
   [SerializeField] private AK.Wwise.Event generatorOnSoundEffect;
   #endregion


   #region UnityMethods

   private void Start()
   {
      isOn = false;
   }

   private void Update()
   {

      //If your using spotlights instead of lightning blocks
      if (!useLightningBlocks)
      {
         if (allLights.All(gameLight => gameLight.isOn))
         {
            if (!isOn)
            {
               TurnOn();
            }
         }
      }

      else
      {
         
      }


   }

   #endregion

   #region CustomMethods

   public void TurnOn()
   {
      isOn = true;
      gate.SetActive(false);
      generatorRenderer.material = generatorOnMaterial;
      generatorOnSoundEffect.Post(this.gameObject);
   }

   #endregion
}
