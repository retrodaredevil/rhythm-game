using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.Note {
    [RequireComponent(typeof(NoteScript))]
    public class NoteMover : MonoBehaviour{

        [SerializeField]
        private float _hitAtTime = -1;
        public float hitAtTime {
            get {
                return _hitAtTime;
            }
            set {
                _hitAtTime = value;
                if (started) {
                    SetHitAtTime(value);
                }
            }
        }

        private NoteScript noteScript;
        private bool started = false;

        public float speed {
            get {
                //if (noteScript == null) {
                //    Debug.Log("NoteScript");
                //} else if (noteScript.songScript == null) {
                //    Debug.Log("Song");
                //} else if (noteScript.songScript.difficulty == null) {
                //    Debug.Log("diff");
                //}
                
                return noteScript.songScript.difficulty.speed;
            }
        }

        void Start() {
            started = true;
            this.noteScript = GetComponent<NoteScript>();
            if(hitAtTime >= 0) {
                this.SetHitAtTime(hitAtTime);
            }
        } 

        void Update() {
            noteScript.height -= speed * noteScript.songScript.time.deltaTime;
            //noteScript.isShowingNote //
        }

        public void SetHitAtTime(float songTime) {
            noteScript.height = GetHitAtTime(songTime, speed, noteScript.songScript.time.songTime);
        }
        /// <returns>Returns the height that a note is at based on when it will play in the song</returns>
        public static float GetHitAtTime(float hitAt, float speed, float songTimeNow) {
            

            float diff = hitAt - songTimeNow;
            return speed * diff;

        }


    }
}
