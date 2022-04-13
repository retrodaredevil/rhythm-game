using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi.Extra.Stress {
	public class StressPeriod {

		public readonly float notesPerMaxTime;
		public readonly float maxTime;


		public readonly float recoveryTime;
		public readonly float? recoveryTimeAfterAnotherPeriod;


		/// <param name="convertToNotesPerSecond">Default value is false. If true, notesPerMaxTime will be treated at notesPerSecond and will initalize the variables according to that value and maxTime</param>
		public StressPeriod(float notesPerMaxTime, float maxTime, float recoveryTime, float? recoveryTimeAfterAnotherPeriod = null, bool convertToNotesPerSecond = false) {
			this.maxTime = maxTime;
			this.recoveryTime = recoveryTime;
			this.recoveryTimeAfterAnotherPeriod = recoveryTimeAfterAnotherPeriod;
			if(convertToNotesPerSecond) {
				this.notesPerMaxTime = notesPerMaxTime * maxTime;
			} else {
				this.notesPerMaxTime = notesPerMaxTime;
			}
		}
		///// Calls <see cref="StressPeriod(float notesPerMaxTime, float maxTime, float recoveryTime, float? recoveryTimeAfterAnotherPeriod = null)"/> constructor with maxTime as 1
		//public StressPeriod(float notesPerSecond, float recoveryTime, float? recoveryTimeAfterAnotherPeriod = null) : 
		//	this(notesPerSecond, 1, recoveryTime, recoveryTimeAfterAnotherPeriod) { }

		/// Returns <see cref="notesPerMaxTime"/> / <see cref="maxTime"/>
		public float notesPerSecond {
			get {
				if(maxTime == 0) {
					return 0;
				}
				return notesPerMaxTime / maxTime;
			}
		}
		public bool useNotesPerSecond {
			get {
				return maxTime == 1;
			}
		}



	}
}
