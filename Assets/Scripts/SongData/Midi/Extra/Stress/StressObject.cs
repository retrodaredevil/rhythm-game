using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi.Extra.Stress {
	public class StressObject {
		//private const float DEFAULT_FLOAT = -1;

		/// Represents the normal notes per second where useNotesPerSecond is true and maxTime is always 1
		public StressPeriod normalPeriod;
		/// Represents the max notes per second and how long it is allowed to be that along with recovery time // can also be null
		public StressPeriod maxPeriod;

		

		public StressObject(StressPeriod normal, StressPeriod max = null) {
			this.normalPeriod = normal;
			this.maxPeriod = max;
			if(normal == null) {
				throw new NullReferenceException("normal cannot be null.");
			}
			if(!normal.useNotesPerSecond) {
				throw new ArgumentException("normal's maxTime should be 1 because normal is used for notes per second");
			}
		}

		public bool useMax {
			get {
				return maxPeriod != null;
			}
		}


	}
}
