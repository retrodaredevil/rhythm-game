using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi.Extra.Stress {
	public struct TimePeriod {
		public float start;
		public float stop;

		public TimePeriod(float start, float stop) {
			this.start = start;
			this.stop = stop;

		}
	}
}
