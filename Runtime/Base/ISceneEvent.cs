namespace com.dgn.SceneEvent
{
    public interface ISceneEvent
    {
        void InitEvent();
        void StartEvent();
        void UpdateEvent();
        void StopEvent();
        bool CheckPassEventCondition();
        float GetDelayNextEvent();
        bool Skip();
        void OnDestroy();
    }
}