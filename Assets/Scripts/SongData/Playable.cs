using RhythmGame.Scripts.Player.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData {
    public interface Playable{

        void Start(SongPlayData source);
        void EndNote(SongPlayData source);

        ///float timePlayed { get; }
        ///float length { get; }
        ////// the actual length of the note on screen in seconds (0 if it's not a long note)
        ///float actualNoteLength { get; }
        
        
        /// Get when the note is started in seconds
        float GetTimePlayed(SongPlayData data);
        /// Get how long the note is in seconds
        float GetLength(SongPlayData data);
        /// the actual length of the note on screen in seconds (0 if it's not a long note)
        float GetActualNoteLength(SongPlayData data);
		


    }
}
