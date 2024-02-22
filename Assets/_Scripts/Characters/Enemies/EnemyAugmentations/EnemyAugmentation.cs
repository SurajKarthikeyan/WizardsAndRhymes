using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu(fileName = "EnemyAugmentationData", menuName = 
    "EnemyAugmentation/EnemyAugmentationScriptableObject", order = 1)]
public class EnemyAugmentation : ScriptableObject
{
    public enum AugmentationEffects
    {
        MovementSpeed,
        //AttackSpeed,
        AttackDamage,
        Health,
        //FireDamage,
        //LightningDamage,
        //IceDamage
    }
    #region Editor
#if UNITY_EDITOR
    public AugmentationEffects augmentationEffect;

    float movementSpeedIncrease;

    float attackSpeedIncrease;

    float healthIncrease;

    float attackDamageIncrease;

    public float GetHealthIncrease() { return healthIncrease; }

    public float GetAttackDamageIncrease() { return attackDamageIncrease; }

    public float GetAttackSpeedIncrease() { return attackSpeedIncrease; }

    public float GetMovementSpeedIncrease() { return movementSpeedIncrease; }

    [CustomEditor(typeof(EnemyAugmentation))]
    public class AugmentationEffectsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAugmentation enemyAugmentation = (EnemyAugmentation)target;

            DrawDetails(enemyAugmentation);
        }

        private void DrawDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Augmentation Values");
            EditorGUILayout.BeginVertical();

            switch (enemyAugmentation.augmentationEffect)
            {
                case AugmentationEffects.MovementSpeed:
                    DrawMovementSpeedDetails(enemyAugmentation);
                    break;
                //case AugmentationEffects.AttackSpeed:
                //    DrawAttackSpeedDetails(enemyAugmentation);
                //    break;
                case AugmentationEffects.Health:
                    DrawHealthDetails(enemyAugmentation);
                    break;
                case AugmentationEffects.AttackDamage:
                    DrawAttackDamageDetails(enemyAugmentation);
                    break;
                default:
                    break;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawMovementSpeedDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Movement Speed Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.movementSpeedIncrease = EditorGUILayout.FloatField(enemyAugmentation.movementSpeedIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAttackSpeedDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Attack Speed Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.attackSpeedIncrease = EditorGUILayout.FloatField(enemyAugmentation.attackSpeedIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawHealthDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Health Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.healthIncrease = EditorGUILayout.FloatField(enemyAugmentation.healthIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAttackDamageDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Attack Damage Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.attackDamageIncrease = EditorGUILayout.FloatField(enemyAugmentation.attackDamageIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}
