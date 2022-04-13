using RhythmGame.Scripts.Player;
using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.SongData.NoteType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Note {
    public class NoteData {

        private List<ControlDirection> _lineDirections;
        public List<ControlDirection> lineDirections {
            get {
                if (_lineDirections == null || _lineDirections.Count == 0) { // only initialize once
                    InitLineDirections();
                    if(note.noteParts.Count == 0) {
                        throw new Exception("This property returned the wrong value. Please wait for the NoteParts to update. Also, there may be too many notes on screen and the fps may be too low. ");
                    }
                }
                return _lineDirections;
            }
        }
        private void InitLineDirections() {
            _lineDirections = new List<ControlDirection>();
            foreach(NotePart part in note.noteParts) {
                _lineDirections.Add(part.currentLine.direction);
            }
        }

        private float _length;
        public float lengthInSeconds {
            get {
                if (!isDestroyed) {
                    _length = note.noteLength;
                }
                return _length;
            }
        }
        public bool isDestroyed {
            get {
                return note == null;
            }
        }
        public float height {
            get {
                if (isDestroyed) {
                    return -5;
                }
                return note.height;
            }
        }

        public float viewedHeight {
            get {
                if (isDestroyed) {
                    return -0.1f;
                }

                //return 1 - ((1 - height) * (1 - height)); // use this if you want a laugh. I don't know what I was thinking
                //return Mathf.Pow(height, -1); haha nope
                //return Mathf.Sqrt(height); // to fast at the end // same as Mathf.Pow(heigh, 1 / 2);
                return CalculateViewHeight(height);
            }
        }
        public static float CalculateViewHeight(float height) {
            const float pow = 0.9f;
            if (height < 0) {
                return Mathf.Pow(height * -1, pow) * -1;
            }
            return Mathf.Pow(height, pow);
        }

        private float noteHit;
        public float noteHitTime {
            get {
                if (!isDestroyed) {
                    NoteMover mover = note.gameObject.GetComponent<NoteMover>();
                    return (noteHit = mover.hitAtTime);
                }

                return noteHit;
            }
            set {
                noteHit = value;
                if (!isDestroyed) {
                    NoteMover mover = note.gameObject.GetComponent<NoteMover>();
                    noteHit = value;
                    mover.hitAtTime = value;
                }
            }

        }

        public List<PlayerScript> peopleWhoPlayed = new List<PlayerScript>();
        public List<PlayerScript> peopleWhoMissed = new List<PlayerScript>();
        

        /// <summary>
        /// The noteScript script. Please note that, it may be null when this NoteData object isn't destroyed
        /// </summary>
        public NoteScript note;

        public NoteData(NoteScript note) {
            this.note = note;
            InitLineDirections();
        }
        

    }
}
