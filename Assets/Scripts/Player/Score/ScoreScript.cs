using RhythmGame.Scripts.Behaviours;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.Note;
using RhythmGame.Scripts.SongUtil;
using System.Collections;
using RhythmGame.Scripts.Line;

namespace RhythmGame.Scripts.Player.Score {
    public class ScoreScript : PlayerBehaviour, ControlListener {

        

        [HideInInspector]
        public float score = 0, health = 1;

        [HideInInspector]
        public int streak = 0, missStreak = 0, multiplier = 1;


        private NoteData currentData = null;

        [HideInInspector]
        private List<NoteData> past = new List<NoteData>();
        [HideInInspector]
        private List<NoteData> played = new List<NoteData>();
        [HideInInspector]
        private List<NoteData> playable = new List<NoteData>();

        private Vector3 original;
        private int loc = 0;


        public StreakType streakType {
            get {
                if(streak == 0) {
                    return StreakType.NONE;
                } else if(streak > 0) {
                    return StreakType.POSITIVE;
                } else {
                    return StreakType.MISS;
                }
            }
        }

        void Start() {
            this.original = this.playerScript.songScript.centerBall.transform.localPosition;
            //Debug.Log("Startin");
            foreach(NoteScript note in playerScript.songScript.noteHandler.allNotes) {
                playable.Add(note.noteData);
                //Debug.Log("Added.");
            }
        }
        void OnGUI() {
            GUI.Box(new Rect(0, 0, 500, 30), "Score: " + (score + CalculateScoreAdditionForLongNote()));
            GUI.Box(new Rect(0, 30, 500, 30), "Health: " + ((int) (health * 100)) + "%");
            GUI.Box(new Rect(0, 60, 500, 30), "Streak: " + streak);
            GUI.Box(new Rect(0, 90, 500, 30), "MissStreak: " + missStreak);
            GUI.Box(new Rect(0, 120, 500, 30), "currentData null: " + (currentData == null));
        }


        private bool IsPaused() {
            return playerScript.songScript.time.isPaused;
        }
        private int CalculateScoreAdditionForLongNote() {
            if(currentData == null) {
                return 0;
            }
            float start = currentData.noteHitTime;
            float now = playerScript.songScript.time.songTime;
            if(start > now) {
                return 0;
            }
            float diff = now - start;
            
            return (int)(diff * 40);
        }

        void Update() {

            for(int i = playable.Count - 1; i >= 0; i--) {
                NoteData note = playable[i];
                if(note.lengthInSeconds > 0 && note.height < 1) {
                    if(!playerScript.songScript.difficulty.CanBeHitAtLaterHeight(note.height, playerScript.songScript)) {
                        
                        if (note.peopleWhoPlayed.Count == 0) {
                            foreach (NotePart part in note.note.noteParts) {
                                if(part == null || part.longLineObject == null) {
                                    
                                    break;
                                }
                                Destroy(part.longLineObject);
                            }
                        }
                    }
                }
                if (playerScript.songScript.noteHandler.passedNotes.Contains(note) && !played.Contains(note)) { // the note is past and we didn't play it
                    note.peopleWhoMissed.Add(playerScript);
                    playable.Remove(note); // make this unplayable
                    past.Add(note); // add it to past
                    //Debug.Log("A note has past without playing it");
                    OnMiss(false);
                    if(note.peopleWhoMissed.Count == playerScript.screenScript.players.Count && note.note != null) {
                        note.note.isShowingNote = false;
                        Destroy(note.note.gameObject);
                    }
                    continue;
                }
            }
            if (currentData != null) {
                if (currentData.isDestroyed) {
                    StopLongNote();
                } else {
                    float endTime = currentData.lengthInSeconds + currentData.noteHitTime;
                    if (!playerScript.songScript.difficulty.CanBeHitAtLaterHeight(NoteMover.GetHitAtTime(endTime, playerScript.songScript.difficulty.speed, playerScript.songScript.time.songTime), playerScript.songScript)) {
                        StopLongNote();
                    }
                }
            }
        }


        public void OnCancelPress() {

        }

        public void OnChange(List<ControlDirection> directions) {
            int many = 0;
            foreach (LineScript line in playerScript.songScript.noteHandler.lineHandler.lines) {
                line.isPressedDown = directions.Contains(line.direction);
                if(currentData != null) {
                    if (line.isPressedDown && currentData.lineDirections.Contains(line.direction)) {
                        many++;
                    }
                }
            }
            if(currentData != null && !EqualsIgnoreOrder(directions, currentData.lineDirections)) { // currentNote is no longer being held
                StopLongNote();
            }
            
            //if(directions.ContainsAll(currentNote.noteData.lineDirections))
            
        }
        private void StopLongNote() {
            score += CalculateScoreAdditionForLongNote();
            foreach (NotePart part in currentData.note.noteParts) {
                part.longLineObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            currentData = null;
            Debug.Log("Should have stopped long note");
        }

        public void OnMenuDirection(ControlDirection direction) {
            // not used
        }

        public void OnPausePress() {
            // we'll just let the songScript handle this one
        }

        public void OnStrum(List<ControlDirection> directions) {
            if(directions.Count == 0) {
                OnMiss(true);
                return;
            }
            //Debug.Log("directions: " + ListToString(directions));
            Difficulty difficulty = playerScript.songScript.difficulty;
            bool didPlay = false;
            for (int i = playable.Count - 1; i >= 0; i--) {
                NoteData note = playable[i];
                if(note == null) {
                    playable.Remove(note);
                    Debug.LogWarning("Removed 1 note. Should not have happened!!!");
                    continue;
                }
                //Debug.Log("directions of note: " + ListToString(note.lineDirections));
                //Debug.Log("i: " + i + "height: " + note.height);
                if (difficulty.IsHitAtHeight(note.height, playerScript.songScript) 
                    && EqualsIgnoreOrder(note.lineDirections, directions)) {
                    playable.Remove(note);
                    played.Add(note);
                    score += 10;
                    didPlay = true;
                    note.peopleWhoPlayed.Add(playerScript);
                    if(note.peopleWhoPlayed.Count == playerScript.screenScript.players.Count) {
                        note.note.isShowingNote = false;
                    }
                    Debug.Log("Added to play count.");
                    if(note.lengthInSeconds > 0) {
                        currentData = note;
                    }
                }
            }
            if (didPlay) {
                OnPlay();

            } else {
                OnMiss(true);
                if(currentData != null) {
                    StopLongNote();
                }
            }
        }
        IEnumerator PumpCenterBall(bool up = true) {
            int yAdd = up ? 1 : -1;
            GameObject centerBall = this.playerScript.songScript.centerBall;

            Vector3 toSet = centerBall.transform.localPosition;
            toSet.y += yAdd;
            centerBall.transform.localPosition = toSet;

            bool didYield = false;
            if(currentData != null) {
                didYield = true;
            }
            yield return new WaitWhile(() => { return currentData != null; });

            if (!didYield) {
                yield return new WaitForSeconds(0.15f);

            }
            centerBall.transform.localPosition = original;
            
        }

        
        protected void OnPlay() {
            StartCoroutine(PumpCenterBall());
            streak++;
            missStreak = 0;
            if(streak > 50) {
                multiplier = 10;
            } else if(streak > 30) {
                multiplier = 8;
            } else if(streak > 20) {
                multiplier = 4;
            } else if(streak > 8) {
                multiplier = 2;
            } else {
                multiplier = 1;
            }

            score += 10 * multiplier;
            health += streak / 100f;
            if(health > 1) {
                health = 1;
            }

            
        }
        protected void OnMiss(bool strum) {
            if (strum) {
                OnStrumMiss();
            } else {
                OnPastMiss();
            }

            missStreak++;
            streak = 0;
            multiplier = 1;

            health -= missStreak / 50f;
            if(health < 0) {
                OnDeath();
            }
        } 
        protected void OnStrumMiss() {
            StartCoroutine(PumpCenterBall(false));

        }
        protected void OnPastMiss() {

        }
        protected void OnDeath() {

        }



        public void OnSelectPress() {
        }
        private static bool EqualsIgnoreOrder(List<ControlDirection> a, List<ControlDirection> b) {
            // a and b 0<= .Count <= 2
            if(a.Count != b.Count) {
                return false;
            } // both are the same values
            if(a.Count > 2) {
                return false;
            }

            if(a.Count == 0) {
                //Debug.Log("False");
                return false;
            }
            if(a.Count == 1) {

                return a[0] == b[0];
            }
            

            return a.Contains(b[0]) && a.Contains(b[1]);
        }
        public static string ListToString<T>(List<T> list) {
            string r = "List<" + typeof(T).Name + ">{";
            int count = 0;
            foreach(T thing in list) {
                if(count != 0) {
                    r += ",";
                }
                r += thing.ToString();
                count++;
            }

            return r + "}";
        }
    }
}
