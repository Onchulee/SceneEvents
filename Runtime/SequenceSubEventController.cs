using UnityEngine;

namespace com.dgn.SceneEvent
{
    public class SequenceSubEventController : Singleton<SequenceSubEventController>
    {

        #region Variable
        public SceneSubEvent[] subEvents;
        private int eventIter;
        private SceneSubEvent SceneEvent { get { return subEvents[eventIter]; } }

        [Header(" ")]
        public int procIter;
        public bool callNextProc;
        public float delayProc;
        [Header(" ")]
        //public OVRInput.RawButton skipButton = OVRInput.RawButton.Start;
        public bool skip;
        #endregion

        void Start()
        {
            eventIter = 0;
            callNextProc = true;
            delayProc = 0;
            skip = false;
            foreach (SceneSubEvent sceneEvent in subEvents)
            {
                if (sceneEvent != null) sceneEvent.InitEvent();
            }
        }

        void Update()
        {
            if (eventIter >= subEvents.Length) return;
            //if (OVRInput.GetUp(skipButton)) skip = true;
            if (delayProc > 0)
            {
                delayProc -= Time.deltaTime;
                return;
            }

            if (callNextProc)
            {
                callNextProc = false;
                skip = false;
                SceneEvent.StartEvent();
            }
            else
            {
                SceneEvent.UpdateEvent();
                if (SceneEvent.CheckPassEventCondition())
                {
                    SceneEvent.StopEvent();
                    delayProc = SceneEvent.GetDelayNextEvent();
                    eventIter++;
                    callNextProc = true;
                }
            }

            if (skip)
            {
                SceneEvent.Skip();
                skip = false;
            }
        }
    }
}