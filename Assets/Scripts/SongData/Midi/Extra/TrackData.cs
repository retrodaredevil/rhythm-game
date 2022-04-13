//using NUnit.Framework;
using RhythmGame.Scripts.SongData.Midi.Extra.Stress;
using RhythmGame.Scripts.SongData.NoteType;
using System.Collections.Generic;

namespace RhythmGame.Scripts.SongData.Midi.Extra {
    public class TrackData {

        public readonly bool useChannel;
        public readonly int number;
        public readonly PlayTypeSet playTypeSet;
		/// The stress settings. May be empty or null if playTypeSet.playType isn't HIT_NOTE
		public readonly IList<StressSettings> stressSettings;

        public TrackData(int number, PlayTypeSet playTypeSet, IList<StressSettings> stressSettings, bool useChannel = false) {
            this.number = number;
            this.playTypeSet = playTypeSet;
            this.useChannel = useChannel;
			this.stressSettings = stressSettings;

//			Assert.IsNotNull(playTypeSet);
//			if(playTypeSet.playType == PlayType.HIT_NOTE) {
//				Assert.IsNotNull(stressSettings);
//				Assert.IsNotEmpty(stressSettings);
//			}


        }

    }
}
