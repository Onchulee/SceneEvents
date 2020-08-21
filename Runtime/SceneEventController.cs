using com.dgn.UnityAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.dgn.SceneEvent
{
    public class SceneEventController : Singleton<SceneEventController>
    {
        [HideInInspector]
        public bool onEditMode = true;

        [HideInInspector]
        [SerializeField]
        private List<SceneEvent> sequenceEvents;
        public List<SceneEvent> SequenceEvents
        {
            get {
                return sequenceEvents;
            }
        }
        private int seqID = 0;
        [Tooltip("Whether controller automatically continues next scene event in SequenceEvents")]
        [ConditionalHide("onEditMode", false)]
        public bool autoRun = false;

        [SerializeField]
        [ReadOnly]
        [ConditionalHide("onEditMode", true, ConditionalHide.False)]
        private SceneEvent initialEvent = null;
        public bool IsInitialEventAvailable { get { return initialEvent != null; } }

        [SerializeField]
        [ReadOnly]
        [ConditionalHide("onEditMode", true, ConditionalHide.False)]
        private SceneEvent currentEvent = null;
        public bool IsCurrentEventAvailable { get { return currentEvent != null; } }
        public bool IsNextSequenceEventAvailable { get { return sequenceEvents.IsValidIndex(seqID); } }

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
            foreach (SceneEvent sceneEvent in sequenceEvents)
            {
                if (sceneEvent) sceneEvent.InitEvent();
            }
            if (sequenceEvents.Count > 0) {
                seqID = 0;
                initialEvent = sequenceEvents[0];
            }
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
                    if (currentEvent)
                    {
                        onStartEvent = true;
                    }
                    else
                    {
                        seqID = seqID + 1;
                        if (IsNextSequenceEventAvailable) {
                            initialEvent = sequenceEvents[seqID];
                            if (autoRun) StartEvent();
                            else delayProc = 0;
                        }
                    }
                }
            }
        }
        
        public void StartEvent()
        {
            currentEvent = initialEvent;
            onStartEvent = true;
            isEventStart = true;
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