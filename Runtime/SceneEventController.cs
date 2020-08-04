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
        [ModifiableProperty]
        private SceneEvent initialEvent;
        public bool IsInitialEventAvailable { get { return initialEvent != null; } }

        [SerializeField]
        [ReadOnly]
        [ModifiableProperty]
        private SceneEvent currentEvent;
        public bool IsCurrentEventAvailable { get { return currentEvent != null; } }

        [SerializeField]
        [ReadOnly]
        [ConditionalHide("onEditMode", true, ConditionalHideAttribute.Condition.False)]
        [Rename("Delay next process")]
        private float delayProc;
        private bool onStartEvent;

        private bool isEventStart;
        public bool IsEventStart { get { return isEventStart; } }
        private bool pause;
        public bool IsPause { get { return pause; } }
        private bool skip;

        protected override void Awake()
        {
            base.Awake();
            onEditMode = false;

            delayProc = 0;
            skip = false;
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
            //if (SKIP_BUTTON) skip = true;

            if (delayProc > 0)
            {
                delayProc = Mathf.Max(0, delayProc - Time.deltaTime);
                return;
            }
            if (onStartEvent)
            {
                onStartEvent = false;
                skip = false;
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

            if (skip)
            {
                currentEvent.Skip();
                skip = false;
            }
        }
        
        public void StartEvent()
        {
            currentEvent = initialEvent;
            onStartEvent = true;
            isEventStart = true;
            pause = false;
        }

        public void SkipEvent()
        {
            skip = true;
        }

        public void PauseEvent()
        {
            pause = true;
            currentEvent.Pause();
        }

        public void UnPauseEvent()
        {
            currentEvent.UnPause();
            pause = false;
        }

        public void Restart()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

    }
}