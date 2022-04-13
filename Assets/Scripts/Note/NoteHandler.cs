using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Line;
using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame.Scripts.Note {
    public class NoteHandler : SongBehaviour {

        // Public variables
        public LineHandler lineHandler;

        [HideInInspector]
        public List<NoteScript> allNotes = new List<NoteScript>();
        [HideInInspector]
        public List<NoteScript> playableNotes = new List<NoteScript>();

        public GameObject notePrefab;
        public GameObject noteNoteObjectPrefab;
        public GameObject multiNoteBetweenPrefab;
        public GameObject longLinePrefab;

        [HideInInspector]
        public List<NoteData> passedNotes = new List<NoteData>();

        // Initialization
        void Start() {


        }

        // Private methods
        /// <summary>Gets the current children and adds them to the notes variable</summary>
        private void AddCurrentNotes() {
            for(int i = 0; i < this.transform.childCount; i++) {
                GameObject child = this.transform.GetChild(i).gameObject;
                NoteScript add = child.GetComponent<NoteScript>();
                if(add == null) {
                    Debug.Log("A child of a NoteHandler does not have a NoteScript attached.");
                    continue;
                }
                //add.AddNoteOnLine(lineHandler.GetLine(ControlDirection.UP));
                allNotes.Add(add);
                playableNotes.Add(add);

            }
        }
        void Update() {
            for(int i = playableNotes.Count - 1; i >= 0; i--) {
                NoteScript note = playableNotes[i];
                //if(note.currentLine.direction == ControlDirection.DOWN) {
                //    Debug.Log("Updating for down. Height: " + note.height);
                //}
                if(note.height <= 0) {
                    note.isShowingNote = false; // make invisible
                    
                    if (!songScript.difficulty.CanBeHitAtLaterHeight(NoteMover.GetHitAtTime(note.noteData.lengthInSeconds + note.noteData.noteHitTime + 5, songScript.difficulty.speed, songScript.time.songTime), songScript)) {
                        passedNotes.Add(note.noteData);
                        playableNotes.Remove(note);
                        //Debug.Log("Going to destroy note. Height: " + note.height + " direction: " + note.currentLine.direction);
                        Destroy(note.gameObject); // to update the note data.
                        continue;
                    }
                }
            }
        }

        //Public Methods
        public NoteScript CreateNote(IList<ControlDirection> lineDirections, float playAtTime, float noteLengthInSeconds = 0) {
            GameObject instance = Instantiate(notePrefab);
            instance.transform.parent = this.transform; // set this as our child
            NoteScript note = instance.GetComponent<NoteScript>();
            

            note.height = 1;
            note.songScript = songScript; // it doesn't have a songScript

            note.noteLength = noteLengthInSeconds;

            foreach (ControlDirection d in lineDirections) {
                note.AddNoteOnLine(lineHandler.GetLine(d));

            }
            
            instance.GetComponent<NoteMover>().hitAtTime = playAtTime;
            //note.noteData.noteHitTime = playAtTime; // causes NRE NPE

            allNotes.Add(note);
            playableNotes.Add(note);

            return note;
        }
        public void CreateRandomSong() {
            float starting = 5;
            float last = starting;
            for(int i = 0; i < 2000; i++) {
                float add = 1;

                ControlDirection extra = ControlDirection.NONE;
                ControlDirection[] values = (ControlDirection[])Enum.GetValues(typeof(ControlDirection));

                float length = 0;

                int value;
                if((value = i % 30)  < 3) {
                    if (value == 0) {
                        add = 0.5f;
                    } else {
                        add = 0.3f;
                    }
                } else if(i % 5 == 0) {
                    add = 2;
                } else if(i % 10 == 1) {
                    add = 3;
                } else if(i % 15 == 1) {
                    add = 5;
                }
                if(i % 10 < 2) {
                    extra = values[UnityEngine.Random.Range(0, values.Length)];
                } else if(i % 10 == 3) {
                    length = UnityEngine.Random.Range(1, 5);
                }
                float play = last + add;
                ControlDirection d = ControlDirection.NONE;
                while(d == ControlDirection.NONE) {
                    d = values[UnityEngine.Random.Range(0, values.Length)];
                }
                List<ControlDirection> directions = new List<ControlDirection>();
                directions.Add(d);
                if(extra != d && extra != ControlDirection.NONE && !ControlAxis.IsSameAxis(d, extra)) {
                    directions.Add(extra);
                }

                NoteScript current = CreateNote(directions, play, length);
                last = play + length;
            }
        }
        public void RemoveAllNotes() {
            for(int i = allNotes.Count - 1; i >= 0; i--) {
                NoteScript note = allNotes[i];
                Destroy(note.gameObject);
                allNotes.Remove(note);
                playableNotes.Remove(note);
                passedNotes.Remove(note.noteData);
            }
        }
    }
}