using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.dgn.SceneEvent.Editor
{
    [CustomEditor(typeof(SceneAssetManager))]
    public class SceneAssetManagerEditor : UnityEditor.Editor
    {
        GUIStyle wrapLabelStyle;
        SceneAssetManager AssetManager { get { return target as SceneAssetManager; } }
        List<SceneAsset> sceneAssetList = new List<SceneAsset>();
        bool showAssetList = true;


        public override void OnInspectorGUI()
        {
            wrapLabelStyle = new GUIStyle(GUI.skin.GetStyle("label"))
            {
                wordWrap = true
            };

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            this.DrawDefaultInspector();

            if (Application.isPlaying)
            {
                DrawSceneAssets();
            }
            else
            {
                EditorGUILayout.HelpBox("Add SceneAsset Component into GameObjects in Scene, they will automatically be added into SceneAssetList.", MessageType.Info);
                EditorGUILayout.LabelField("[ List of Scene assets will be available on runtime ]", wrapLabelStyle);
            }

            if (EditorGUI.EndChangeCheck())
            {
                GetSceneAssetList();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawSceneAssets()
        {
            showAssetList = EditorGUILayout.BeginFoldoutHeaderGroup(showAssetList, "Scene Assets");
            if (showAssetList)
            {
                if (sceneAssetList != null && sceneAssetList.Count > 0)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    for (int i = 0; i < sceneAssetList.Count; i++)
                    {
                        EditorGUILayout.ObjectField(i + " : " + sceneAssetList[i].assetName,
                            sceneAssetList[i], typeof(SceneAsset), true);
                    }
                    EditorGUI.BeginDisabledGroup(false);

                }
                else
                {
                    EditorGUILayout.LabelField("[ No scene assets in this scene ]", wrapLabelStyle);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void GetSceneAssetList()
        {
            if (AssetManager && AssetManager.AssetDictionary != null && AssetManager.AssetDictionary.Count > 0)
            {
                sceneAssetList = AssetManager.AssetDictionary.Values.ToList();
            }
            else
            {
                sceneAssetList.Clear();
            }
        }

        // Update asset list before repaint
        private void RedrawGUI()
        {
            GetSceneAssetList();
            this.Repaint();
        }

        private void OnEnable()
        {
            if (AssetManager)
            {
                AssetManager.WantRepaint += RedrawGUI;
                GetSceneAssetList();
            }
        }

        private void OnDisable()
        {
            if (AssetManager) AssetManager.WantRepaint -= RedrawGUI;
        }

        private void OnDestroy()
        {
            if (AssetManager) AssetManager.WantRepaint -= RedrawGUI;
        }
    }

}