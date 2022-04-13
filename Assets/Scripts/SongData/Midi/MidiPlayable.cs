using System;
using RhythmGame.Scripts.SongUtil;
using RhythmGame.Scripts.MidiUtil;

namespace RhythmGame.Scripts.SongData.Midi {
    public class MidiPlayable : Playable {

        public readonly MidiNote note;

        public MidiPlayable(MidiNote note) {
            this.note = note;
        }


        public float GetTimePlayed(SongPlayData data) {
            MidiSongData d = MidiSong.GetData(data);
            return MidiNote.ChangeToSeconds(note.start, d.midiPlayer.midiStreamSynthesizer);
        }

        public float GetLength(SongPlayData data) {
            MidiSongData d = MidiSong.GetData(data);
            return MidiNote.ChangeToSeconds(note.length, d.midiPlayer.midiStreamSynthesizer);
        }

        public float GetActualNoteLength(SongPlayData data) {
            float length = GetLength(data);
            if(length < 3) {
                return 0;
            }
            return length;
        }

        public void Start(SongPlayData source) {
            MidiSongData data = MidiSong.GetData(source);
            data.midiPlayer.midiListener.ProcessMidiEvent(note.noteOn);
        }
        public void EndNote(SongPlayData source) {
            MidiSongData data = MidiSong.GetData(source);
            data.midiPlayer.midiListener.ProcessMidiEvent(note.noteOff);

        }


    }
}