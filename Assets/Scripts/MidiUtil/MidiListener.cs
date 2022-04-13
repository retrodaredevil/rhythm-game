using CSharpSynth.Sequencer;
using System.Collections.Generic;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using UnityEngine;

namespace RhythmGame.Scripts.MidiUtil {
    /// doesn't interact with things outside of the CSharSynth stuff and makes a few calls the Debug.Log from UnityEngine
    /// Assumming that this is attached to the MIDIPlayer, once the MIDIPlayer loads the song, the public list, notes will be added to.
    public abstract class MidiListener : MidiSequencer, IListenerObject {
        // Public 
        public MidiFile midi;
        private List<List<MidiEvent>> tracks;
        
        //Private
        public List<MidiNote> notes = new List<MidiNote>();

        //Constructor
        public MidiListener(StreamSynthesizer synth) : base(synth) {
            
        }
        //Public Methods
        public override void ProcessMidiEvent(MidiEvent midiEvent) {
            if (PlayEvent(midiEvent)) {
                base.ProcessMidiEvent(midiEvent); // play the note
            }
        }
        public override void BeforeCombineTracks(MidiFile file) {
            
            {
                int errors = 0;
                tracks = new List<List<MidiEvent>>();
                int track = 0;
                Debug.Log("Tracks Length: " + midi.Tracks.Length);
                foreach (MidiTrack t in midi.Tracks) {
                    List<MidiEvent> events;
                    tracks.Add(events = new List<MidiEvent>());
                    foreach (MidiEvent e in t.MidiEvents) {
                        //MidiHelper.MidiChannelEvent eventType = e.midiChannelEvent;
                        //if (eventType != MidiHelper.MidiChannelEvent.Note_On && eventType != MidiHelper.MidiChannelEvent.Note_Off) {
                        //    continue;
                        //}

                        //byte channel = e.channel;
                        //e.trackNumber = track;
                        events.Add(e);

                    }
                    track++;

                }
                foreach (List<MidiEvent> trackEvents in tracks) {
                    int number = 0;

                    
                    foreach (MidiEvent e in trackEvents) {

                        bool on = e.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On;
                        MidiNote current = null;
                        
                        if (on) {
                            current = new MidiNote(number);
                            number++;
                            current.noteOn = e;

                            MidiEvent off = null;
                            uint leastDiff = uint.MaxValue;
                            foreach (MidiEvent e2 in trackEvents) {
                                if (e2.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off && e.parameter1 == e2.parameter1) { // if off
                                    uint diff = e2.deltaTime - e.deltaTime;
                                    if (diff < leastDiff && diff >= 0) {
                                        off = e2;
                                        leastDiff = diff;
                                    }
                                }
                            }
                            current.noteOff = off;
                            if (current.noteOff != null) {
                                notes.Add(current);
                            } else {
                                errors++;
                            }
                        }
                    }

                }


                Debug.Log("There were: " + errors + " errors where current was null");
            }
            this.OnBeforeCombineTracks(file);
        }
        public override bool LoadMidi(MidiFile midi, bool UnloadUnusedInstruments) {
            this.midi = midi;
            bool r = base.LoadMidi(midi, UnloadUnusedInstruments);
            if (!r) {
                return false;
            }
            this.OnLoad(midi);
            
            return true;
        }

        public MidiNote GetMidiNote(MidiEvent midiEvent) {
            MidiHelper.MidiChannelEvent eventType = midiEvent.midiChannelEvent;
            if(eventType != MidiHelper.MidiChannelEvent.Note_On && eventType != MidiHelper.MidiChannelEvent.Note_Off) {
                return null;
            }
            foreach(MidiNote note in notes) {
                if(eventType == MidiHelper.MidiChannelEvent.Note_On) {
                    if(note.noteOn == midiEvent) {
                        return note;
                    }
                } else if(eventType == MidiHelper.MidiChannelEvent.Note_Off) {
                    if (note.noteOff == midiEvent) {
                        return note;
                    }
                }
            }
            return null;
        }

        public abstract bool PlayEvent(MidiEvent midiEvent);
        public abstract void OnLoad(MidiFile file);
        public abstract void OnBeforeCombineTracks(MidiFile file);
    }
}
