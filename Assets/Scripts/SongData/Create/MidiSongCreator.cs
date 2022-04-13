using RhythmGame.Scripts.SongData.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpSynth.Midi;
using RhythmGame.Scripts.MidiUtil;
using UnityEngine;

namespace RhythmGame.Scripts.SongData.Create {
    public class MidiSongCreator : SongCreator<MidiSong, MIDIPlayer>, IListenerObject {

        private string filePath;
        private SongInfo info;

        private IList<SongPart> parts;// initialized in Constructor
        private IList<Playable> notes; // initialized in Create

        public MidiSongCreator(SongInfo info, List<MidiSongPart> parts, string filePath) {
            this.info = info;
            this.parts = parts.ConvertAll(x => (SongPart) x);
			this.filePath = filePath;

			if(parts.Count == 0) {
				throw new InvalidOperationException("parts.Count is 0");
			}
        }
        
        
        private void Clear(MIDIPlayer player) { // resets all the data so we can create another Song if we need to (even without entering stuff in)
            if(player.midiListener.listener == this) {
                player.midiListener.listener = null;
            }
        }



        public MidiSong Create(MIDIPlayer player) {
			if(player == null) {
				Debug.LogWarning("midiPlayer is null");
			} else if(player.midiListener == null) {
				Debug.LogWarning("midiListener is null");
			}
            player.midiListener.listener = this;
            notes = new List<Playable>();
            player.LoadSong(filePath);

			foreach(MidiNote note in player.midiListener.notes) {
				MidiPlayable play = new MidiPlayable(note);
				notes.Add(play);
			}
			List<MidiEvent> extraEvents = new List<MidiEvent>();
			foreach(MidiEvent ev in player.midiListener.midi.Tracks[0].MidiEvents) {
				/*if(!notes.Any(x => { // buggy as hell
					MidiPlayable p = (MidiPlayable)x;
					return p.note.noteOn == ev || p.note.noteOff == ev; // returns true if that event is in a Playable/MidiNote
				})) {
					// gets fired if no Playable/MidiNote has ev as an event
					extraEvents.Add(ev);

				}*/
				if(ev.midiChannelEvent != MidiHelper.MidiChannelEvent.Note_On) {
					extraEvents.Add(ev);
				}
			}
			

			if(parts.Count == 0) {
				Debug.Log("parts.Count is 0 here");
			}
			Debug.Log("notes.Count: " + notes.Count);
            MidiSong r = new MidiSong(info,notes, parts, extraEvents, player, filePath);
            Clear(player);
            return r;
        }

        public void OnBeforeCombineTracks(MidiFile file) {

        }

        public void OnLoad(MidiFile file) {

        }

        public bool PlayEvent(MidiEvent midiEvent) {
            throw new InvalidOperationException("PlayEvent should not be called.");
        }
    }
}
