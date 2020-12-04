using System.Collections.Generic;
using UnityEngine;

namespace com.dgn.SceneEvent
{
    public class SceneAssetManager : MonoBehaviour
    {
        private Dictionary<string, SceneAsset> assetDictionary;
        public Dictionary<string, SceneAsset> AssetDictionary { get { return assetDictionary; } }

        private static SceneAssetManager instance;
        public static SceneAssetManager Instance { get { return instance; } }
        

#if UNITY_EDITOR
        // Declare the method signature of the delegate to call.
        // For a void method with no parameters you could just use System.Action.
        public delegate void RepaintAction();
        // Declare the event to which editor code will hook itself.
        public event RepaintAction WantRepaint;
#endif

        private void Awake()
        {
            instance = this;
            assetDictionary = new Dictionary<string, SceneAsset>();
        }

        public void AddAsset(SceneAsset asset)
        {
            if (!assetDictionary.ContainsKey(asset.assetName))
            {
                assetDictionary.Add(asset.assetName, asset);
#if UNITY_EDITOR
                WantRepaint?.Invoke();
#endif
            }
            else
            {
                Debug.LogWarning(asset.assetName + " has already existed.");
            }
        }

        public static bool GetAsset(string name, out SceneAsset sceneAsset)
        {
            sceneAsset = default;
            if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
            {
                bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
                return found;
            }
            return false;
        }

        public static bool GetGameObjectAsset(string name, out GameObject retGameObject)
        {
            retGameObject = default;
            if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
            {
                if (SceneAssetManager.instance.assetDictionary.TryGetValue(name, out SceneAsset sceneAsset))
                {
                    retGameObject = sceneAsset.gameObject;
                    bool found = retGameObject != null;
                    if (!found)
                    {
                        Debug.LogWarning("Scene Asset [" + name + "] is not found!");
                    }
                    return found;
                }
            }
            return false;
        }

        public static bool GetAssetComponent<T>(string name, out T asset)
        {
            asset = default;
            if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
            {
                SceneAsset sceneAsset = default;
                bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
                if (!found) {
                    Debug.LogWarning("Component [" + typeof(T).Name + "] from Scene Asset [" + name + "] is not found!");
                }
                if (found && sceneAsset.gameObject != null)
                {
                    asset = sceneAsset.gameObject.GetComponent<T>();
                    return asset != null;
                }
            }
            return false;
        }

        public static bool GetAssetComponentInChildren<T>(string name, out T asset)
        {
            asset = default;
            if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
            {
                SceneAsset sceneAsset = new SceneAsset();
                bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
                if (!found)
                {
                    Debug.LogWarning("Child component ["+ typeof(T).Name + "] from Scene Asset [" + name + "] is not found!");
                }
                if (found && sceneAsset.gameObject != null)
                {
                    asset = sceneAsset.gameObject.GetComponentInChildren<T>();
                    return asset != null;
                }
            }
            return false;
        }

        public static bool GetAssetComponentsInChildren<T>(string name, out T[] assets)
        {
            assets = default;
            if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
            {
                SceneAsset sceneAsset = new SceneAsset();
                bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
                if (!found)
                {
                    Debug.LogWarning("Children component [" + typeof(T).Name + "] from Scene Asset [" + name + "] is not found!");
                }
                if (found && sceneAsset.gameObject != null)
                {
                    assets = sceneAsset.gameObject.GetComponentsInChildren<T>();
                    return assets != null;
                }
            }
            return false;
        }
    }
}