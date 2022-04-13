using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi.Extra.Stress {
	public class StressSettings {

		private IList<DifficultyStress> stress;
		public TimePeriod timePeriod;
		public int priority;

		/// <param name="priority">The lower the number the higher the priority</param>
		/// <param name="stress">The DifficultyStress values for each difficulty. If all difficultystress objects don't have any difficulty values on it's difficulties list, it will choose the first one</param>
		public StressSettings(IList<DifficultyStress> stress, TimePeriod timePeriod, int priority) {
			this.stress = stress;
			this.timePeriod = timePeriod;
			this.priority = priority;
		}

		public DifficultyStress GetStress(DifficultyValue value) {

			foreach(DifficultyStress s in stress) {
				if(s.difficulties.Contains(value)) {
					return s;
				}
			}

			return stress[0];
		}




	}
}
