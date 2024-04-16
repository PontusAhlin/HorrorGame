using UnityEngine.AdaptivePerformance;

namespace UnityEditor.AdaptivePerformance.Editor
{
    [CustomEditor(typeof(AdaptivePerformanceManagerSettings))]
    internal class AdaptivePerformanceManagerSettingsEditor : UnityEditor.Editor
    {
        AdaptivePerformanceLoaderOrderUI m_LoaderUi = new AdaptivePerformanceLoaderOrderUI();

        internal BuildTargetGroup BuildTarget
        {
            get;
            set;
        }

        public void Reload()
        {
            m_LoaderUi.CurrentBuildTargetGroup = BuildTargetGroup.Unknown;
        }

        /// <summary>
        /// <see href="https://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html">Editor Documentation</see>
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
                return;

            serializedObject.Update();

            m_LoaderUi.OnGUI(BuildTarget);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
