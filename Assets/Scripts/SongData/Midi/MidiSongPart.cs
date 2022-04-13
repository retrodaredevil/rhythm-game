using RhythmGame.Scripts.SongData.NoteType;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RhythmGame.Scripts.Player.Controls;
using UnityEngine;
using RhythmGame.Scripts.SongData.Midi.Extra.Stress;
using RhythmGame.Scripts.SongData.Midi.Extra;

namespace RhythmGame.Scripts.SongData.Midi {
    ///This is used to define a part of the song that one player can play
    public class MidiSongPart : SongPart {
		public static readonly TrackData DEFAULT_DATA = new TrackData(-1, new PlayTypeSet(PlayType.BACKGROUND_NOTE), null);

		public static readonly string[] noteNames = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

		

		public IList<TrackData> trackData;

        public MidiSongPart(IList<TrackData> trackData) {
            this.trackData = trackData;
			if(trackData.Count == 0) {
				throw new ArgumentException("trackData must have content in the list.");
			}
        }

		public IList<ControlDirection> GetLineDirection(Playable playable, Difficulty diff) {
			MidiPlayable play = GetPlayable(playable);
			return GetDirectionsForNote(play.note.noteOn.parameter1, diff);
		}

		public PlayTypeSet GetPlayType(Playable playable) {
            MidiPlayable play = GetPlayable(playable);
            foreach(TrackData data in trackData) {
                if(DoesApply(data, play)) {
					Debug.Log("Ehhay. Not returning the default typeset. useChannel: " + data.useChannel + " data.number " + data.number + " trackNumber: " + play.note.trackNumber + " channel number: " + play.note.channelNumber);

                    return data.playTypeSet;
                }
            }

            return DEFAULT_DATA.playTypeSet;
        }
		public IList<StressSettings> GetStressSettings(Playable playable, SongPlayData playData) {
			MidiPlayable p = GetPlayable(playable);
			List<StressSettings> r = new List<StressSettings>();

			foreach(TrackData data in trackData) {
				if(!DoesApply(data, p)){
					continue;
				}
				foreach(StressSettings s in data.stressSettings) {
					TimePeriod time = s.timePeriod;
					float timePlayed = p.GetTimePlayed(playData);
					if(timePlayed >= time.start && timePlayed <= time.stop) {
						r.Add(s);
					}
				}
			}

			return r;
		}
		private bool DoesApply(TrackData data, MidiPlayable play) {
			return (!data.useChannel && data.number == play.note.trackNumber) || (data.useChannel && data.number == play.note.channelNumber);
		}


        private static MidiPlayable GetPlayable(Playable playable) {
            MidiPlayable r = playable as MidiPlayable;
            if(r == null) {
                throw new InvalidOperationException("The playable must be a Midi Playable.");
            }
            return r;
        }


		public static string GetNote(byte note) {
			return noteNames[note % 12];
		}
		private static List<ControlDirection> GetDirectionsForNote(byte note, Difficulty diff) { // note is 0 - 127
			List<ControlDirection> r = new List<ControlDirection>();
			string name = GetNote(note); // {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A,", "A#", "B"};
			string test = name.ToLower();
			switch(test) {
			case "c":
			case "d#":
			case "e":
				if(test == "d#") {
					if(diff.value == DifficultyValue.HARD || diff.value == DifficultyValue.EXTREME) {
						r.Add(ControlDirection.RIGHT);
					} 
				}
				r.Add(ControlDirection.UP);
				break;
			case "c#":
			case "f":
			case "a":
				if(test == "f") {
					if(diff.value == DifficultyValue.HARD || diff.value == DifficultyValue.EXTREME) {
						r.Add(ControlDirection.LEFT);
					}
				}
				r.Add(ControlDirection.DOWN);
				break;
			case "g#":
			case "b":
			case "d":
				if(test == "b") {
					if(diff.value == DifficultyValue.HARD || diff.value == DifficultyValue.EXTREME) {
						r.Add(ControlDirection.DOWN);
					}
				}
				r.Add(ControlDirection.LEFT);
				break;
			case "a#":
			case "f#":
			case "g":
				if(test == "f#") {
					if(diff.value == DifficultyValue.HARD || diff.value == DifficultyValue.EXTREME) {
						r.Add(ControlDirection.UP);
					}
				}
				r.Add(ControlDirection.RIGHT);
				break;
			}
			if(r.Count == 0) {
				Debug.LogWarning("Didn't add any direction. name: " + name + ", note: " + note);
			}
			return r;
		}
	}
}
