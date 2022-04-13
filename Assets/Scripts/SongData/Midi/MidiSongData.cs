using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi {
    public class MidiSongData : SongPlayData{
        public readonly MIDIPlayer midiPlayer;


        public MidiSongData(MIDIPlayer midiPlayer) : base() {
            this.midiPlayer = midiPlayer;

        }

    }
}
