using RhythmGame.Scripts.Player.Controls;
using RhythmGame.Scripts.SongData.Midi.Extra.Stress;
using RhythmGame.Scripts.SongData.NoteType;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData {
    public interface SongPart{

        PlayTypeSet GetPlayType(Playable playable);

		IList<StressSettings> GetStressSettings(Playable playable, SongPlayData playData);


		/// Returns a non null value even if it's not meant to be played on a line.
		IList<ControlDirection> GetLineDirection(Playable playable, Difficulty diff);



    }
    
}
