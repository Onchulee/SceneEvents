using UnityEngine;

namespace com.dgn.SceneEvent
{
    [System.Serializable]
    public abstract class SceneSubEvent : ScriptableObject, ISceneEvent
    {
        protected bool passEventCondition;
        [SerializeField]
        protected float delayNextEvent;


        public virtual void InitEvent()
        {
            passEventCondition = false;
        }

        public abstract void StartEvent();
        public abstract void UpdateEvent();
        public abstract void StopEvent();

        public virtual bool Skip() {
            return false;
        }

        public virtual bool CheckPassEventCondition()
        {
            return passEventCondition;
        }
        public virtual float GetDelayNextEvent()
        {
            return delayNextEvent;
        }
        public virtual void OnDestroy() { }
    }
}