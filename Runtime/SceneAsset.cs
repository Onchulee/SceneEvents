using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.dgn.SceneEvent
{
    public class SceneAsset : MonoBehaviour
    {
        public string assetName;
        public bool hideOnStart;

        private void Start()
        {
            OnStart();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(assetName)) { assetName = this.name; }
        }

        // This function will call before Start but after Awake
        // Therefore SceneAssetManager will have all SceneAsset in scene
        // before SceneEventController initializes the event
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            InitialSetup();
        }
        
        private void InitialSetup()
        {
            if (SceneAssetManager.Instance)
            {
                SceneAssetManager.Instance.AddAsset(this);
            }
            else
            {
                Debug.LogWarning(gameObject.name + " : SceneAssetManager is not available");
            }
        }

        public void OnStart()
        {
            gameObject.SetActive(!hideOnStart);
        }
    }
}