using UnityEditor;

namespace com.dgn.SceneEvent.Editor
{
    [CustomEditor(typeof(SceneSubEvent), true)]
    public class SceneSubEventDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            if (target)
            {
                EditorGUILayout.ObjectField("Asset", target, typeof(SceneSubEvent), false);
            }
            EditorGUI.EndDisabledGroup();
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}