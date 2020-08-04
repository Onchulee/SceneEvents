using UnityEditor;
using UnityEngine;

namespace com.dgn.SceneEvent
{
    [CustomEditor(typeof(SceneEventController))]
    public class SceneEventControllerEditor : Editor
    {
        SceneEventController EventController { get { return target as SceneEventController; } }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            this.DrawDefaultInspector();

            if (Application.isPlaying)
            {
                if (EventController.IsEventStart)
                {
                    if (EventController.IsCurrentEventAvailable)
                    {
                        if (EventController.IsPause && GUILayout.Button("Continue"))
                        {
                            EventController.UnPauseEvent();
                        }
                        else if (!EventController.IsPause)
                        {
                            if (GUILayout.Button("Pause"))
                            {
                                EventController.PauseEvent();
                            }
                            if (GUILayout.Button("Skip"))
                            {
                                EventController.SkipEvent();
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No event left to continue.", MessageType.Info);
                        if (GUILayout.Button("Restart"))
                        {
                            EventController.Restart();
                        }
                    }
                }
                else
                {
                    if (!EventController.IsInitialEventAvailable)
                    {
                        EditorGUILayout.HelpBox("Need initial event to start.", MessageType.Warning);
                    }
                    else if (GUILayout.Button("Start"))
                    {
                        EventController.StartEvent();
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}