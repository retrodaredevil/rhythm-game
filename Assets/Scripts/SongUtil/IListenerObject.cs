using UnityEngine;
using System.Collections;
using CSharpSynth.Midi;

public interface IListenerObject {

    /// <summary>
    /// Gets called when a note is about to play
    /// </summary>
    /// <param name="midiEvent">The midi note that's about to play</param>
    /// <returns>Return true if you want to play the note, false otherwise</returns>
    bool PlayEvent(MidiEvent midiEvent);
    void OnLoad(MidiFile file);
    void OnBeforeCombineTracks(MidiFile file);
}
