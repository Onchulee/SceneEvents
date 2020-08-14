using UnityEngine;

namespace com.dgn.SceneEvent
{
    //see https://github.com/UnityCommunity/UnitySingleton
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {

        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private static T instance;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                return instance;
            }
        }
        

        #endregion

        #region Methods

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(instance);
            }
            instance = this as T;
        }
        #endregion

    }
}