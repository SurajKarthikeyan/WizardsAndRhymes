using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class that holds all enemy augmentations
/// </summary>
[CreateAssetMenu(fileName = "EnemyAugmentationData", menuName = 
    "EnemyAugmentation/EnemyAugmentationScriptableObject", order = 1)]
public class EnemyAugmentation : ScriptableObject
{
    #region Variables
    [Tooltip("Enum representing all categories of enemy augmentations")]
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

    [Tooltip("Augmentation Effects that are possible to give to enemies")]
    public AugmentationEffects augmentationEffect;

    [Tooltip("Percentage increase in movement speed applied to all enemies")]
    float movementSpeedIncrease;

    [Tooltip("Percentage increase in attack speed applied to all enemies")]
    float attackSpeedIncrease;

    [Tooltip("Percentage increase in health applied to all enemies")]
    float healthIncrease;

    [Tooltip("Percentage increase in attack damage applied to all enemies")]
    float attackDamageIncrease;
    #endregion

    #region Custom Methods
    /// <summary>
    /// Returns the health increase percentage
    /// </summary>
    /// <returns>Float representing the percentage increase in health to give to enemies</returns>
    public float GetHealthIncrease() { return healthIncrease; }

    /// <summary>
    /// Returns the attack damage increase percentage
    /// </summary>
    /// <returns>Float representing the percentage increase in attack damage to give to enemies</returns>
    public float GetAttackDamageIncrease() { return attackDamageIncrease; }

    /// <summary>
    /// Returns the attack speed increase percentage
    /// </summary>
    /// <returns>Float representing the percentage increase in attack speed to give to enemies</returns>
    public float GetAttackSpeedIncrease() { return attackSpeedIncrease; }

    /// <summary>
    /// Returns the movement speed increase percentage
    /// </summary>
    /// <returns>Float representing the percentage increase in movement speed to give to enemies</returns>
    public float GetMovementSpeedIncrease() { return movementSpeedIncrease; }
    #endregion

    #region Editor
#if UNITY_EDITOR
    /// <summary>
    /// Editor class for creating instances of the EnemyAugmentation class
    /// </summary>
    [CustomEditor(typeof(EnemyAugmentation))]
    public class AugmentationEffectsEditor : Editor
    {
        /// <summary>
        /// Function that handles any inspector UI related code
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAugmentation enemyAugmentation = (EnemyAugmentation)target;

            DrawDetails(enemyAugmentation);
        }

        /// <summary>
        /// Function that draws the enemy augmentation details based on what is 
        /// </summary>
        /// <param name="enemyAugmentation"></param>
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

        /// <summary>
        /// Function that draws the movement speed augmentation details
        /// </summary>
        /// <param name="enemyAugmentation">EnemyAugmentation object to write to</param>
        private void DrawMovementSpeedDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Movement Speed Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.movementSpeedIncrease = EditorGUILayout.FloatField(enemyAugmentation.movementSpeedIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Function that draws the attack speed augmentation details
        /// </summary>
        /// <param name="enemyAugmentation">EnemyAugmentation object to write to</param>
        private void DrawAttackSpeedDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Attack Speed Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.attackSpeedIncrease = EditorGUILayout.FloatField(enemyAugmentation.attackSpeedIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Function that draws the health augmentation details
        /// </summary>
        /// <param name="enemyAugmentation">EnemyAugmentation object to write to</param>
        private void DrawHealthDetails(EnemyAugmentation enemyAugmentation)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Health Increase", GUILayout.MaxWidth(150));
            enemyAugmentation.healthIncrease = EditorGUILayout.FloatField(enemyAugmentation.healthIncrease,
                GUILayout.MaxWidth(40));
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Function that draws the attack damage augmentation details
        /// </summary>
        /// <param name="enemyAugmentation">EnemyAugmentation object to write to</param>
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
