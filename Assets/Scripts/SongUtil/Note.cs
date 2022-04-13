using UnityEngine;
using UnityEditor;
using System;
using RhythmGame.Scripts.MidiUtil;
using RhythmGame.Scripts.Note;

namespace RhythmGame.Scripts.SongUtil {
    public class Note {
        public readonly MidiNote midiNote;
        public readonly NoteData noteData;

        public Note(MidiNote midiNote, NoteData noteData = null) {
            this.midiNote = midiNote;
            this.noteData = noteData;
        }
        

    }
}