using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Player.Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RhythmGame.Scripts.SongUtil;
using RhythmGame.Scripts.Line;

namespace RhythmGame.Scripts.Note {
    //[ExecuteInEditMode]
    public class NoteScript : SongBehaviour {


        public bool isFake = false;

        [SerializeField]
        private List<ControlDirection> initialDirectionsToAdd;

        internal List<NotePart> noteParts = new List<NotePart>();

        public float height = 1;
        //public INote songSyncNote = null;




        private bool lastShowingNote = false;
        [SerializeField]
        private bool _isShowingNote = false;




        private List<NotePart> _noteObjects = new List<NotePart>();
        private GameObject between = null;

        /// <summary>
        /// The length of the note in seconds. If this is not a long note, then this is 0
        /// </summary>
        public float noteLength = 0;

        [HideInInspector]
        public NoteData noteData;

        void Awake() {

            noteData = new NoteData(this);
        }

        // Use this for initialization
        void Start() {

            AddInitialNoteParts();
            AddCurrentNoteParts();

        }


        public bool isShowingNote {
            get {
                return _isShowingNote;
            }
            set {
                bool toSet = value;
                if (toSet == _isShowingNote) {
                    return;
                }
                _isShowingNote = toSet;
                foreach (NotePart part in noteParts) {
                    part.ShowNote(toSet);
                    //if(part.longLineObject != null) {
                    //    part.longLineObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    //    
                    //}
                }
            }
        }
        public bool isLongNote {
            get {
                return noteLength > 0;
            }
        }
        public List<NotePart> noteObjects {
            get {
                return _noteObjects;
            }
        }

        // Update is called once per frame
        void Update() {
            if (lastShowingNote != _isShowingNote) {
                this.isShowingNote = _isShowingNote;
                lastShowingNote = _isShowingNote;
            }

            GameObject betweenPrefab;
            if (height < 1 && noteObjects.Count == 2 && (betweenPrefab = songScript.noteHandler.multiNoteBetweenPrefab) != null) { // make sure this is a multi note and the prefab isn't null
                if (between == null) {
                    between = Instantiate(betweenPrefab);
                    between.transform.parent = this.transform;
                }
                NotePart a = noteObjects[0];
                NotePart b = noteObjects[1];

                Vector3 position = (a.transform.position + b.transform.position) / 2;
                between.transform.position = position;

                Vector3 toSet = between.transform.localScale;
                toSet.z = Vector3.Distance(a.transform.position, b.transform.position);
                between.transform.localScale = toSet; // scale it to make it long

                // Quaternion q = Quaternion.LookAt(a.transform.position, b.transform.position);
                //between.transform.position = a.transform.position;
                between.transform.LookAt(b.transform.position); // rotate it so it's in the right direction

            }


            //Debug.Log("Updating. _:" + _isShowingNote + " reg: " + isShowingNote); // works when getting

            // Debug.Log("Actual: " + this.transform.position);

        }

        public void AddNoteOnLine(LineScript lineScript) {
            if (lineScript == null) {
                throw new NullReferenceException("lineScript is null.!!!");
            }
            //Debug.Log("Running");



            GameObject prefab = songScript.noteHandler.noteNoteObjectPrefab;



            GameObject newObject = Instantiate(prefab);
            newObject.transform.parent = this.transform;

            NotePart newNote = newObject.GetComponent<NotePart>();
            if (newNote == null) {
                Debug.LogWarning("We tried to create an object that needs a NotePart, but it didn't work!!!");
            }
            newNote.currentLine = lineScript;
            newNote.note = this;

            newNote.ShowNote(this.isShowingNote);

            this.noteParts.Add(newNote);

            //Debug.Log("did it");
        }
        private void AddCurrentNoteParts() {
            for (int i = 0; i < this.transform.childCount; i++) {
                GameObject gameObject = this.transform.GetChild(i).gameObject;
                NotePart notePart = gameObject.GetComponent<NotePart>();
                if (notePart.currentLine == null) {
                    Debug.Log("While adding current note parts, a note part didn't have a currentLine. This will cause errors in the future.");
                }
                _noteObjects.Add(notePart);
            }
        }
        private void AddInitialNoteParts() {
            foreach (ControlDirection d in initialDirectionsToAdd) {
                LineScript line = songScript.noteHandler.lineHandler.GetLine(d);
                AddNoteOnLine(line);
            }
        }
    }
}