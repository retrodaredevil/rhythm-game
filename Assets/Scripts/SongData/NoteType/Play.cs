using RhythmGame.Scripts.SongData.NoteType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.NoteType {
    [Obsolete]
    public class Play {

        public readonly PlayTypeSet playTypeSet;
        public readonly SongPart songPart;
        public readonly Playable playable;

        public Play(SongPart part, PlayTypeSet playTypeSet, Playable playable) {
            this.songPart = part;
            this.playTypeSet = playTypeSet;
            this.playable = playable;
        }


    }
}
