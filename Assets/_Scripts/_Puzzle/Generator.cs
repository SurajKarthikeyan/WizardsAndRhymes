using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator : MonoBehaviour
{
   #region Variables
   [SerializeField] private GameObject gate;
   [SerializeField] public bool isOn;
   [SerializeField] private Material generatorOnMaterial;
   [SerializeField] private Renderer generatorRenderer;
   [SerializeField] private List<GameLight> allLights;
   [SerializeField] private AK.Wwise.Event generatorOnSoundEffect;
   #endregion


   #region UnityMethods

   private void Start()
   {
      isOn = false;
   }

   private void Update()
   {
      for (int i = 0; i < allLights.Count(); i++)
      {
         if (!allLights[i].isOn)
         {
            return;
         }
      }

      if (!isOn)
      {
         TurnOn();
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
