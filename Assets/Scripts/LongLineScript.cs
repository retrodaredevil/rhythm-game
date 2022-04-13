using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Line;
using RhythmGame.Scripts.Note;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame.Scripts {
    public class LongLineScript : SongBehaviour {

        public NotePart notePart;

        private LineScript line;
        private GameObject longObject;

        [HideInInspector]
        public float lastActualHeight;

        



        private void Awake() {
            longObject = this.transform.GetChild(0).gameObject;

            this.transform.position = new Vector3(100, 100, 100);// to make sure it's not shown weirdly a frame before we change it
        }

        // Use this for initialization
        void Start() {
            line = notePart.currentLine;

            NotePart.UpdateRotation(this.transform, line);
            
        }
        public bool canHold {
            get {
                return songScript.difficulty.CanBeHitAtLaterHeight(lastActualHeight, songScript);
            }
        }

        // Update is called once per frame
        void Update() { // this.transform.parent.position should be 0 0 0 

            NoteData data = notePart.note.noteData;

            float hitAt = data.lengthInSeconds + data.noteHitTime;
            float timeNow = songScript.time.songTime; // once got a bug where this was -5 because of pauseTime in songScript

            float height = NoteMover.GetHitAtTime(hitAt, songScript.difficulty.speed, timeNow);
            if(height < 0.03) {
                this.longObject.GetComponent<MeshRenderer>().enabled = false;

                return;
            }
            this.lastActualHeight = height;

            if (height > 1) {
                height = 1;
                //Debug.Log("one height: " + height + " hitAt: " + hitAt + " timeNow: " + timeNow + " pauseTime: " + songScript.pauseTime);
            } else {
                height = NoteData.CalculateViewHeight(height);
                if (height > 1) {
                    height = 1;
                   // Debug.Log("one");
                }
            }
            Vector3 spot = line.GetSpotOnLine(height); // where the note ends
            Vector3 here = notePart.transform.position;

            {
                // sorry, this is ugly. Basically, if the person is playing it, it'll look like it's stopping on the note spot, and if the note is missed, it'll stop on
                // the center thing which should be at 0 0 0. 
                bool someonePlayed = data.peopleWhoPlayed.Count > 0;
                bool past = this.songScript.noteHandler.passedNotes.Contains(notePart.note.noteData);
                bool showing = this.notePart.note.isShowingNote;
                if ((this.notePart.note.noteData.height < 0 || !showing || past)
                    && someonePlayed) {
                    here = line.noteSpotObject.transform.position;
                   // Debug.Log("using noteSpot's position: " + here);
                } else if (!someonePlayed && !showing) {
                    here = new Vector3(0, 0, 0);
                }
            }

            //Debug.Log("here: " + here + " spot: " + spot + " height: " + height);
            float distance = Vector3.Distance(spot, here);
            this.transform.position = Vector3.Lerp(spot, here, 0.5f);
            Vector3 scale = this.longObject.transform.localScale;
            scale.y = distance / 2;
            this.longObject.transform.localScale = scale;



        }
    }
}