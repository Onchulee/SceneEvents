using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.dgn.SceneEvent
{
    public class SceneEventController : Singleton<SceneEventController>
    {
        [HideInInspector]
        public bool onEditMode = true;

        [SerializeField]
        [ConditionalHide("onEditMode", false)]
        private SceneEvent initialEvent;
        public bool IsInitialEventAvailable { get { return initialEvent != null; } }

        [SerializeField]
        [ReadOnly]
        private SceneEvent currentEvent;
        public bool IsCurrentEventAvailable { get { return currentEvent != null; } }

        [SerializeField]
        [ReadOnly]
        [ConditionalHide("onEditMode", true, ConditionalHide.False)]
        [Rename("Delay next process")]
        private float delayProc;
        private bool onStartEvent;

        private bool isEventStart;
        public bool IsEventStart { get { return isEventStart; } }
        private bool pause;
        public bool IsPause { get { return pause; } }

        protected override void Awake()
        {
            base.Awake();
            onEditMode = false;

            delayProc = 0;
            pause = false;
            isEventStart = false;
            onStartEvent = false;
        }

        private void Start()
        {
            if (initialEvent != null) initialEvent.InitEvent();
        }

        private void Update()
        {
            if (currentEvent == null) return;
            if (pause) return;

            if (delayProc > 0)
            {
                delayProc = Mathf.Max(0, delayProc - Time.deltaTime);
                return;
            }
            if (onStartEvent)
            {
                onStartEvent = false;
                currentEvent.StartEvent();
            }
            else
            {
                currentEvent.UpdateEvent();

                if (currentEvent.CheckPassEventCondition())
                {
                    currentEvent.StopEvent();
                    delayProc = currentEvent.GetDelayNextEvent();
                    currentEvent = currentEvent.NextEvent();
                    onStartEvent = true;
                }
            }
        }
        
        public void StartEvent()
        {
            currentEvent = initialEvent;
            onStartEvent = true;
            isEventStart = true;
            pause = false;
        }


        /// <summary>
        /// Skip current event
        /// </summary>
        /// <returns>
        /// true if current event is skippable.
        /// false if it's not skippable or the event doesn't existed.
        /// </returns>
        public bool SkipEvent()
        {
            if(currentEvent) return currentEvent.Skip();
            return false;
        }

        public void PauseEvent()
        {
            pause = true;
            if (currentEvent) currentEvent.Pause();
        }

        public void UnPauseEvent()
        {
            if (currentEvent) currentEvent.UnPause();
            pause = false;
        }

        public void Restart()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

    }
}