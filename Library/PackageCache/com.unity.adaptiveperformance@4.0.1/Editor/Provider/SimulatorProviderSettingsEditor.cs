using UnityEngine;
using UnityEditor.AdaptivePerformance.Editor;

namespace UnityEditor.AdaptivePerformance.Simulator.Editor
{
    /// <summary>
    /// This is custom Editor for Simulator Provider Settings.
    /// </summary>
    [CustomEditor(typeof(SimulatorProviderSettings))]
    public class SimulatorProviderSettingsEditor : ProviderSettingsEditor
    {
        /// <summary>
        /// Override of Editor callback to display custom settings.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (!DisplayBaseSettingsBegin())
                return;

            BuildTargetGroup selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();

            if (selectedBuildTargetGroup == BuildTargetGroup.Standalone)
            {
                EditorGUIUtility.labelWidth = 180; // some property labels are cut-off
                DisplayBaseRuntimeSettings();
                EditorGUILayout.Space();
                DisplayBaseDeveloperSettings();
            }
            else
            {
                EditorGUILayout.HelpBox("Adaptive Performance Simulator settings not available on this platform.", MessageType.Info);
                EditorGUILayout.Space();
            }
            DisplayBaseSettingsEnd();
        }
    }
}
