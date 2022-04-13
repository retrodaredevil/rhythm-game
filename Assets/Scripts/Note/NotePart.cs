using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.Line;

namespace RhythmGame.Scripts.Note {
    public class NotePart : MonoBehaviour{

        public NoteScript note;
        public LineScript currentLine;

        private bool didUpdateOnce = false;

        public GameObject longLineObject = null;


        private MeshRenderer render;
        


        void Awake() {

            render = this.GetComponent<MeshRenderer>();
        }
        void Start() {
            if (currentLine == null) {
                Debug.Log("At the time of starting a NotePart, it doesn't have a currentLine!!!");
            }
            UpdateRotation(this.transform, currentLine);
        }

        void Update() {

            if (didUpdateOnce && note.noteData.height > 1) {
                ShowNote(false); 
                return;
            }
            if (didUpdateOnce) {
                ShowNote(note.isShowingNote);
            }

                

            Vector3 pos = currentLine.GetSpotOnLine(note.noteData.viewedHeight);
            this.transform.localPosition = pos;

            //Debug.Log("Updating. en: " + render.enabled + " long: " + note.isLongNote + " length: " + note.noteLength);

            if (note.noteData.height < 1 && didUpdateOnce && longLineObject == null) {
                if (note.isLongNote ) {
                    GameObject longLinePrefab;
                    if ((longLinePrefab = note.songScript.noteHandler.longLinePrefab) != null) { // make sure this is a multi note and the prefab isn't null
                        if (longLineObject == null) {
                            longLineObject = Instantiate(longLinePrefab);
                            longLineObject.transform.parent = this.transform.parent; // has notescript as a parent
                            //Debug.Log("Created.");
                            LongLineScript longScript = longLineObject.GetComponent<LongLineScript>();
                            longScript.notePart = this;
                            longScript.songScript = this.note.songScript;
                        }
                        // edit y pos and y scale
                        // make sure y is pos and the x and z scale are 1
                        

                    }
                }
            }
            didUpdateOnce = true;
        }
        public static void UpdateRotation(Transform transform, LineScript line) {
            Quaternion current = transform.localRotation;

            Vector3 change = current.eulerAngles;
            //Debug.Log("Current angles: " + change);
            change.y = line.gameObject.transform.localRotation.eulerAngles.y;
            //Debug.Log("After: " + change + " asdf: " + this.currentLine.gameObject.transform.localRotation.eulerAngles);
            current.eulerAngles = change;

            transform.localRotation = current;
        }
        internal void ShowNote(bool show) { // this is called every time isShowingNote is changed
            render.enabled = show;
        }
    }
}
