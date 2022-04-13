using RhythmGame.Scripts.SongData;
using RhythmGame.Scripts.SongData.Create;
using RhythmGame.Scripts.SongData.Midi;
using RhythmGame.Scripts.SongData.Midi.Extra;
using RhythmGame.Scripts.SongData.Midi.Extra.Stress;
using RhythmGame.Scripts.SongData.NoteType;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongReferences {
	public class MidiSongList : SongList<MidiSong, MIDIPlayer> {

		private static readonly StressPeriod ZERO_STRESS = new StressPeriod(0, 1, 0);
		private static readonly StressSettings settings = new StressSettings(new List<DifficultyStress>() {
			new DifficultyStress(new List<DifficultyValue>(), new StressObject(ZERO_STRESS), new LongNoteConfig(false, LongNoteShortener.REMOVE, ZERO_STRESS))
		}, new TimePeriod(0, float.MaxValue), 1000);

		public SongCreator<MidiSong, MIDIPlayer> HOUSE_OF_THE_RISING_SUN;


		public MidiSongList() {
			Init();
		}


		private void Init() {
			{ // house of the rising sun
				List<DifficultyValue> difficulties = new List<DifficultyValue>() { };
				List<DifficultyStress> stress = new List<DifficultyStress>();
				stress.Add(new DifficultyStress(difficulties, new StressObject(new StressPeriod(3, 1, 0)), new LongNoteConfig(true, LongNoteShortener.SHORTEN_TO_NEXT, new StressPeriod(3, 3, 0))));

				List<StressSettings> settings = new List<StressSettings>();
				settings.Add(new StressSettings(stress, new TimePeriod(0, float.MaxValue), 10));

				HOUSE_OF_THE_RISING_SUN = new MidiSongCreator(
					new SongInfo("House of the Rising Sun", "The Animals", "The Animals"),

					new List<MidiSongPart>{
							new MidiSongPart(
								new List<TrackData>{ new TrackData(3, new PlayTypeSet(PlayType.HIT_NOTE), settings) }
							)
						}, 
					"Midis/hotrs.mid"
				);
			}



		}





	}
}
