using UnityEngine;
using System.Collections;
using RhythmGame.Scripts.Screen;
using System.Collections.Generic;
using CSharpSynth.Midi;
using System;
using RhythmGame.Scripts.MidiUtil;
using RhythmGame.Scripts.Note;
using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.SongData;
using RhythmGame.Scripts.Player;
using RhythmGame.Scripts.SongReferences;

namespace RhythmGame.Scripts.SongUtil {
	public class MasterSong : MonoBehaviour { // this should initialize last btw (It's in the Script Execution Order)


		public MIDIPlayer midiPlayer; // the intialization stuff is in it's Awake method so we should be good
		private ScreenScript[] screens;
		private List<PlayerScript> finalPlayers = new List<PlayerScript>();

		private MidiSongList songList = new MidiSongList();
		//private IList<MidiEvent> events = new List<MidiEvent>();

		public TimeHandler time;

		public Song song;
		public SongPlayData playData;

		private void Awake() {
			midiPlayer.midiListener = new MidiListenerRegister(midiPlayer.midiStreamSynthesizer);
			song = songList.HOUSE_OF_THE_RISING_SUN.Create(midiPlayer);

			screens = this.transform.GetComponentsInChildren<ScreenScript>(); // we should be able to have children other than Screens
			foreach(ScreenScript s in screens) {
				foreach(PlayerScript p in s.players) {
					finalPlayers.Add(p);
				}
			}
			Debug.Log("Called MasterSong awake. screens.Length: " + screens.Length);
		}
		public void StartSong() { // called from a SongScript because that's when we know everything that's initialized is good. (It's also only called once so don't worry about that)
			if(song == null) {
				throw new InvalidOperationException("song is null");
			}
			this.playData = song.Start();
			Debug.Log("Started song. ");
		}

		void Update() {
			if(playData != null) {
				bool continueSong = song.Update(playData, time.songTime, finalPlayers);
				if(!continueSong) {
					song.End(playData, false);
					song = null;
					playData = null;
				}
			}
		}

		//Public Methods
		/* past methods
		void Update() { // pretty much don't need this code in this method because Song's Update method has a better version
			float now = time.songTime;
			for(int i = events.Count - 1; i >= 0; i--) {
				MidiEvent ev = events[i];
				float time = MidiNote.ChangeToSeconds(ev.deltaTime, midiPlayer.midiStreamSynthesizer);
				if(time <= now) { // play the event and remove it

					MidiNote note = midiPlayer.midiListener.GetMidiNote(ev);
					bool play = true;
					if (play && note != null && note.noteOn == ev) { // we should always call the noteOff event just in case.
						foreach (NoteData data in note.noteDataList) {
							if(data.peopleWhoMissed.Count == 0 && data.peopleWhoPlayed.Count == 0) {
								Debug.Log("Gonna return true, but no one did anything.");
							} else {
								Debug.Log("Gonna do the right thing.");
							}
							if (data.peopleWhoPlayed.Count > 0) {
								play = true;
								break;
							} else {
								play = false;
							}
						}
					}
					if (!play) {
						continue;
					}

					midiPlayer.midiListener.ProcessMidiEvent(ev);
					events.Remove(ev);
					Debug.Log("Playing event.");
				}
			}
		}
		
		public void OnLoad(MidiFile file) { // here, we should initalize all the notes since everything else should be initalized. // this functionality should now be in a SongScript
			MidiListener listener = midiPlayer.midiListener;

			foreach(List<MidiEvent> t in listener.tracks) { // usually there's just one track because it feels like jumbling all the channels into one track, so keep that in mind
			   foreach(MidiEvent ev in t) {
					events.Add(ev);
			   }
			}


			NoteHandler song = screens[0].songScript.noteHandler;
			List<MidiEvent> onEvents = new List<MidiEvent>();

			int count = 0;
			foreach(MidiNote note in listener.notes) {
				count++;

				//Debug.Log("Chan: " + note.channel + " Time: " + MidiNote.ChangeToSeconds(note.start, midiPlayer.midiStreamSynthesizer));
				if(note.trackNumber != 2 && note.trackNumber != 3) {
					continue; // for sweet child of mine
				}
				

				//Debug.Log("trackNumber: " + note.trackNumber + " time: " + MidiNote.ChangeToSeconds(note.start, midiPlayer.midiStreamSynthesizer) + " contains: " + onEvents.Contains(note.noteOn));
				onEvents.Add(note.noteOn);
				List<ControlDirection> directions = GetDirectionsForNote(note.noteOn.parameter1);
				float lengthInSeconds = MidiNote.ChangeToSeconds(note.length, midiPlayer.midiStreamSynthesizer);
				float playAtTime = MidiNote.ChangeToSeconds(note.start, midiPlayer.midiStreamSynthesizer);
				if(lengthInSeconds < 3 || lengthInSeconds > 13) {
					lengthInSeconds = 0;
				} 


				float endTime = playAtTime + lengthInSeconds;

				int xx = 0;
				{ // continue if there's a note that's too close to another
					bool doContinue = false;
					for(int i = 0; i < count - 1; i++) { // only test the notes we've already added
						MidiNote note2 = listener.notes[i];

						IList<NoteData> dataList = note2.noteDataList;
						if(dataList.Count > 0) {
							NoteData data = dataList[0];
							float dataStart = data.noteHitTime;
							float dataLength = data.lengthInSeconds;
							float dataEnd = dataStart + dataLength;
							xx++;
							if(Math.Abs(playAtTime - dataStart) < 0.24  // check if the start times are close
								|| (dataStart < playAtTime  && dataEnd > playAtTime) // check if the start of this note is in the longNote of another
								) {
								//Debug.Log("going to continue");
								doContinue = true;
								break;
							}
						}

					}
					if(doContinue) {
						continue;
					}
				}
				//Debug.Log("Tested: " + xx + " times");

				

				NoteScript noteScript = song.CreateNote(directions, playAtTime, lengthInSeconds);

				note.noteDataList.Add(noteScript.noteData);

				//noteScript.songSyncNote = new MidiNoteNote(note);

			}
			
		}*/
		

		//Static Methods
		//Private

		

	}
}