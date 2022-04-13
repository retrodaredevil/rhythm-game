using UnityEngine;
using System.Collections;

namespace RhythmGame.Scripts {
    public class TimeHandler : MonoBehaviour {

        private float timeOnStart = 0;
        private float lastPause = -1; // the last moment paused from the time property
        private float currentPauseTime = 0;
        private bool _isPaused = false;

        /// The amount of time in seconds to start the time after the time variable starts incrementing
        public float songStartTime = 4f;

        // Use this for initialization
        void Start() {
            timeOnStart = Time.time;
        }

        // Update is called once per frame
        void Update() {

        }

        #region Properties
        public float time {
            get {
                return Time.time - timeOnStart;
            }
        }

        public float pauseTime {
            get {
                if(!isPaused) {
                    return currentPauseTime;
                }
                return currentPauseTime + (time - lastPause);
            }
        }
        /// <summary>The time minus the amount of time paused</summary>
        public float playTime {
            get {
                return time - pauseTime;
            }
        }

        public float songTime {
            get {
                return playTime - songStartTime;
            }
        }

        public float deltaTime {
            get {
                return Time.deltaTime;
            }
        }

        public bool isPaused {
            get {
                return _isPaused;
            }
            set {
                bool last = _isPaused;
                _isPaused = value;
                if(last == value) {
                    return;
                }
                if(value) {
                    lastPause = time;
                } else {
                    currentPauseTime += (time - lastPause);
                }
            }
        }

        #endregion



    }
}