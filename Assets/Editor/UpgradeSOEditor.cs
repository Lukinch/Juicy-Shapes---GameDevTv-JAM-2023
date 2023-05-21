using UnityEditor;

[CustomEditor(typeof(UpgradeSO))]
public class UpgradeSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Icon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Title"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UpgradeType"));

        UpgradeType upgradeType = (UpgradeType)serializedObject.FindProperty("UpgradeType").enumValueIndex;

        switch (upgradeType)
        {
            case UpgradeType.Movement:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MovementUpgradeType"));

                MovementUpgradeType movementUpgradeType = (MovementUpgradeType)serializedObject.FindProperty("MovementUpgradeType").enumValueIndex;

                switch (movementUpgradeType)
                {
                    case MovementUpgradeType.MovementSpeed:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AddMoveSpeed"));
                        break;
                    case MovementUpgradeType.Acceleration:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AddAcceleration"));
                        break;
                }
                break;

            case UpgradeType.Fire:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FireUpgradeType"));

                FireUpgradeType fireUpgradeType = (FireUpgradeType)serializedObject.FindProperty("FireUpgradeType").enumValueIndex;

                switch (fireUpgradeType)
                {
                    case FireUpgradeType.ShotsAmount:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AddShotsAmount"));
                        break;

                    case FireUpgradeType.ShotsPerSecond:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AddShotsPerSecond"));
                        break;

                    case FireUpgradeType.Damage:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AddDamage"));
                        break;
                    case FireUpgradeType.Pattern:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("ShootingPattern"));
                        break;
                }
                break;

            case UpgradeType.Health:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HealthUpgradeType"));

                HealthUpgradeType healthUpgradeType = (HealthUpgradeType)serializedObject.FindProperty("HealthUpgradeType").enumValueIndex;

                if (healthUpgradeType == HealthUpgradeType.MaxHealthPoints)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("AddCurrentMaxHealthPoints"));
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
