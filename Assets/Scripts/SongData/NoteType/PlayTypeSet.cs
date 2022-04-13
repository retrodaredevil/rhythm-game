using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.NoteType {
    public class PlayTypeSet {

        public readonly PlayType playType;
        /// If the playType is HIT_NOTE then this is not null and tells you what the notes that aren't hit are (if a note is too close to another this is what that note will be)
        public readonly PlayType? notHitType;

        ///If notHitType is null and playType is a HIT_NOTE, notHitType will then default to STREAK_IMPORTANT
        public PlayTypeSet(PlayType playType, PlayType? notHitType = null) {
            this.playType = playType;
            this.notHitType = notHitType;
            if(playType == PlayType.HIT_NOTE && notHitType == null) {
                notHitType = PlayType.STREAK_IMPORTANT;
            }
        }


    }
}
