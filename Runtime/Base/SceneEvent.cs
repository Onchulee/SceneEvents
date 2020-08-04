using UnityEngine;
namespace com.dgn.SceneEvent
{
    [System.Serializable]
    public abstract class SceneEvent : SceneSubEvent, ICustomSceneEventNext
    {
        /*
        protected bool passEventCondition;
        [SerializeField]
        protected float delayNextEvent;

        public virtual void InitEvent()
        {
            passEventCondition = false;
        }
        
        public override void StartEvent();
        public override void UpdateEvent();
        public override void StopEvent();
        public abstract void Skip();

        public virtual bool CheckPassEventCondition()
        {
            return passEventCondition;
        }
        public virtual float GetDelayNextEvent()
        {
            return delayNextEvent;
        }
        public virtual void OnDestroy() { }
        */
        
        public abstract void Pause();
        public abstract void UnPause();

        public abstract SceneEvent NextEvent();

        public virtual void InitScene(SceneEvent sceneEvent)
        {
            if (sceneEvent == null) return;
            if (this.GetInstanceID() == sceneEvent.GetInstanceID()) return;
            sceneEvent.InitEvent();
        }
        
    }
}