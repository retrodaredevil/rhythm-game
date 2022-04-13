using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Note;
using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.SongData;
using RhythmGame.Scripts.SongData.Midi;
using RhythmGame.Scripts.SongData.Midi.Extra.Stress;
using RhythmGame.Scripts.SongData.NoteType;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RhythmGame.Scripts {
    public class SongScript : ScreenBehaviour {

        //Public
        public NoteHandler noteHandler;
        public GameObject centerBall;
        [HideInInspector] // if for whatever reason unity starts supporting showing this, it shouldn't show in the editor
        public Difficulty difficulty;
        /// <summary>The amount of seconds it takes for something to happen and for that to show up on screen (lag)</summary>
        public float delayInSeconds = 0;

        [HideInInspector]
        public SongPart songPart = null;

        //Private
        [SerializeField]
        private DifficultyValue _difficultyValue = DifficultyValue.BEGINNER; // this value is available to edit in the inspector
        


        private void Awake() {
            // sets the difficulty using the difficulty value given in the inspector
            var d = Difficulty.GetDifficulty(_difficultyValue);
            if(d != null) {
                this.difficulty = d;
            }
        }
		private void Start() {
			OnSongLoad();
		}

		private void OnSongLoad() {
			Song song = screenScript.masterSong.song;
			SongPlayData data = screenScript.masterSong.playData;
			if(song == null || data == null) {
				screenScript.masterSong.StartSong();
				song = screenScript.masterSong.song;
				data = screenScript.masterSong.playData;
				//throw new InvalidOperationException("masterSong's song is null!");
			}
			if(songPart == null) {
				if(song.parts.Count == 0) {
					Debug.LogWarning("we don't have any parts to choose from.");
				}
				songPart = song.parts[0];
				Debug.LogWarning("Used first part of Song's parts variable");
			}

			IList<StressSettings> settings;

			int count = 0;
			float lastEnd = float.MinValue;
			foreach(Playable p in song.notes) { // in this loop we will Create the different notes we want using NoteHandler's methods and then ignore most of the playable's we don't want
				count++;

				if(songPart.GetPlayType(p).playType != PlayType.HIT_NOTE) {
					continue;
				}
				// p is a hittable note, but before adding it, we want to check that it is and change stuff based on difficulty
				float timePlayed = p.GetTimePlayed(data);
				float actualLength = p.GetActualNoteLength(data);

				if(actualLength > 5) {
					float diff = 0;
					float nTimePlayed;
					Playable next = new Func<Playable>(() => {
						float minDiff = float.MaxValue;
						Playable r = null;
						for(int i = count; i < song.notes.Count; i++) { // start at the playable after this one and find the next one
							Playable n = song.notes[i];
							if(songPart.GetPlayType(n).playType == PlayType.HIT_NOTE) {
								nTimePlayed = n.GetTimePlayed(data);
								if(nTimePlayed < timePlayed) {
									continue; // we're dealing with a past note and don't care about it
								}
								diff = nTimePlayed - timePlayed; // nTimePlayed is >= timePlayed
								if(diff < minDiff) {
									minDiff = diff;
									r = n;
								}
							}
						}
						diff = minDiff;
						return r;
					})();
					if(next == null) {
						continue;
					}
					nTimePlayed = next.GetTimePlayed(data); 

					if(diff > 3) {
						float nLength = diff - 0.3f;
						if(nLength > actualLength) {
							actualLength = 0;
						} else {
							actualLength = nLength;
						}
					} else {
						actualLength = 0;
					}
					
					
				} // end testing for really long note

				float end = timePlayed + actualLength; // actual note length is the one we want since it will usually be 0
				if(lastEnd >= end) {
					continue; // it's a long note and we don't want a note right in the middle of it
				}
				float minTime = difficulty.minimumTimeBetweenNotes;
				if(end - lastEnd < minTime || timePlayed - lastEnd < minTime) { // makes sure there are no notes too close together
					continue;
				}

				// The Playable object, p, has gone through the neccessary security and does not seem to be hiding any weapons of any kind in it's suitcase or person. It is free to board the plane to the sphere in the middle. 
				IList<ControlDirection> lines = songPart.GetLineDirection(p, difficulty);
				if(lines.Count == 0) {
					Debug.LogWarning("Got the line directions and there are none on the list.");
				}
				NoteScript note = noteHandler.CreateNote(lines, timePlayed, actualLength);
				Debug.Log("Created a note");
				lastEnd = end;
			}
			
		}

        public DifficultyValue difficultyValue {
            get {
                return _difficultyValue;
            }
            set {
                _difficultyValue = value;
                difficulty = Difficulty.GetDifficulty(_difficultyValue);
            }
        }
        public TimeHandler time{
            get { return screenScript.masterSong.time; } 
        }

        [System.Obsolete("You don't need to use this because it gives you an instance of itself.")]
        public new SongScript songScript {
            get {
                return base.songScript;
            }
            set {
                base.songScript = value;
            }
        }

        
    }
}