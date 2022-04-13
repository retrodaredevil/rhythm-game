using UnityEngine;
using System.Collections;
using CSharpSynth.Midi;
using System;
using CSharpSynth.Synthesis;

namespace RhythmGame.Scripts.MidiUtil {
    /// allows you to set an IListenerObject which will call the methods whenever a method is called in this class asumming IListenerObject isn't null
    public class MidiListenerRegister : MidiListener {

        public IListenerObject listener;

        public MidiListenerRegister(StreamSynthesizer synth) : base(synth) {
        }

        public override void OnBeforeCombineTracks(MidiFile file) {
            if(listener != null) {
                listener.OnBeforeCombineTracks(file);
            }
        }

        public override void OnLoad(MidiFile file) {
            if(listener == null) {
                Debug.Log("Listener is not set up");
                return;
            }
            listener.OnLoad(file);
        }

        public override bool PlayEvent(MidiEvent midiEvent) {
            if (listener == null) {
                return true;
            }
            return listener.PlayEvent(midiEvent);
        }
    }
}
