using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace com.dgn.SceneEvent.Editor
{
    [CustomEditor(typeof(SceneEventController))]
    public class SceneEventControllerEditor : UnityEditor.Editor
    {
        private const string C_LIST_NAME = "sqe-";

        SceneEventController EventController { get { return target as SceneEventController; } }
        private ReorderableList reorderableList;
        private string focusField;

        private void OnEnable()
        {
            reorderableList = new ReorderableList(EventController.SequenceEvents, typeof(SceneEvent), true, true, true, true);

            // Add listeners to draw events
            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;
            reorderableList.onSelectCallback += OnSelectCallback;

            reorderableList.onAddCallback += AddItem;
            reorderableList.onRemoveCallback += RemoveItem;
        }

        private void OnDisable()
        {
            // Make sure we don't get memory leaks etc.
            reorderableList.drawHeaderCallback -= DrawHeader;
            reorderableList.drawElementCallback -= DrawElement;
            reorderableList.onSelectCallback -= OnSelectCallback;

            reorderableList.onAddCallback -= AddItem;
            reorderableList.onRemoveCallback -= RemoveItem;
        }

        private void OnSelectCallback(ReorderableList itemList) {

        }

        /// <summary>
        /// Draws the header of the list
        /// </summary>
        /// <param name="rect"></param>
        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Sequence Events");
        }

        /// <summary>
        /// Draws one element of the list (ListItemExample)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="index"></param>
        /// <param name="active"></param>
        /// <param name="focused"></param>
        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            var e = Event.current;
            SceneEvent item = EventController.SequenceEvents[index];
            EditorGUI.BeginChangeCheck();
            Rect itemField = new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height);
            // Set control name to check on focus field
            GUI.SetNextControlName(C_LIST_NAME + index);
            item = EditorGUI.ObjectField(itemField, item, typeof(SceneEvent), false) as SceneEvent;
            EventController.SequenceEvents[index] = item;
            if (!itemField.Contains(e.mousePosition) && e.type == EventType.MouseDown && reorderableList.index == index)
            {
                //e.Use();
               // reorderableList.ReleaseKeyboardFocus();
            }
            // right click open menu
            if (itemField.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 1)
            {
                e.Use();
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add"), false, CallAddItem, index);
                menu.AddItem(new GUIContent("Remove"), false, CallRemoveItem, index);
                menu.ShowAsContext();
            }
        
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void AddItem(ReorderableList list)
        {
            EventController.SequenceEvents.Add(null);
            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list)
        {
            EventController.SequenceEvents.RemoveAt(list.index);
            EditorUtility.SetDirty(target);
        }

        private void CallRemoveItem(object target) {
            int index = (int)target;
            if (EventController.SequenceEvents.IsValidIndex(index))
                EventController.SequenceEvents.RemoveAt(index);
        }

        private void CallAddItem(object target) {
            int index = (int)target;
            if (EventController.SequenceEvents.IsValidIndex(index))
                EventController.SequenceEvents.Insert(index, EventController.SequenceEvents[index]);
        }

        private void OnElementFocusedHandler() {
            if (focusField != GUI.GetNameOfFocusedControl())
            {
                focusField = GUI.GetNameOfFocusedControl();
                if (string.IsNullOrEmpty(focusField)) {
                    return;
                }
                if (focusField.Contains(C_LIST_NAME)) {
                    int index = Int32.Parse(focusField.Replace(C_LIST_NAME, ""));
                    if (EventController.SequenceEvents.IsValidIndex(index)) {
                        reorderableList.index = index;
                       // reorderableList.GrabKeyboardFocus();
                    }
                }
            }
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            // Actually draw the list in the inspector
            reorderableList.DoLayoutList();
            // what to do if field focus
            OnElementFocusedHandler();

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
                    else if(EventController.IsNextSequenceEventAvailable)
                    {
                        EditorGUILayout.HelpBox("Next event is available.", MessageType.Info);
                        if (GUILayout.Button("Start Next Event"))
                        {
                            EventController.StartEvent();
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