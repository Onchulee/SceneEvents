using UnityEngine;
namespace com.dgn.SceneEvent
{
    [System.Serializable]
    public struct SceneObjectSetup
    {
        public string name;
        public bool useInitialTransform;
        public Vector3 position;
        public Vector3 eulerAngles;
        public Vector3 position_UI;
    }
}