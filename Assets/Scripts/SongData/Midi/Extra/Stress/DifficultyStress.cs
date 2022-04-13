using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi.Extra.Stress {
	/// Represents stress values for a single difficulty valuethg
	public class DifficultyStress {

		public readonly IList<DifficultyValue> difficulties;
		public readonly StressObject stress;
		public readonly LongNoteConfig longNoteConfig;

		public DifficultyStress(IList<DifficultyValue> diff, StressObject stress, LongNoteConfig longNoteConfig) {
			this.difficulties = diff;
			this.stress = stress;
			this.longNoteConfig = longNoteConfig;
		}

	}
}
