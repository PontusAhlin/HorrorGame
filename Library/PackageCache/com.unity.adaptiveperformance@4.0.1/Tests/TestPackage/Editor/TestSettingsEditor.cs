using UnityEditor;
using UnityEngine.AdaptivePerformance.TestPackage;

namespace UnityEditor.AdaptivePerformance.TestPackage.Editor
{
    [CustomEditor(typeof(TestSettings))]
    public class TestSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Test only...");
        }
    }
}
