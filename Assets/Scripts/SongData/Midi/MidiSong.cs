using CSharpSynth.Midi;
using RhythmGame.Scripts.MidiUtil;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi {
    public class MidiSong : Song {

        private MIDIPlayer midiPlayer;
		private IList<MidiEvent> extraEvents;

		/// <param name="extraEvents">This list should have all events that were not put onto a Playable (usually special events or end events</param>
        public MidiSong(SongInfo info,IList<Playable> notes, IList<SongPart> parts, IList<MidiEvent> extraEvents, MIDIPlayer player, string filePath) : // we'll take a filePath just for future rewriting stuff
            base(notes, parts, info){ // set the notes and parts to null because we still have to figure them out
            this.midiPlayer = player;
			this.extraEvents = extraEvents;
        }

		public override bool Update(SongPlayData songData, float songTime, List<SongPlayer> players) {
			MidiSongData data = GetData(songData);
			List<MidiEvent> toPlay = new List<MidiEvent>();
			foreach(MidiEvent e in extraEvents) {
				float time = MidiNote.ChangeToSeconds(e.deltaTime, data.midiPlayer.midiStreamSynthesizer);
				if(time <= songTime) {
					toPlay.Add(e);
				}
			}
			foreach(MidiEvent play in toPlay) {
				data.midiPlayer.midiListener.ProcessMidiEvent(play);
				extraEvents.Remove(play); // this will stop the song from being capable to play it twice in a row without recreating it btw
			}

			return base.Update(songData, songTime, players);
		}


		protected override SongPlayData CreateSongData() {
            return new MidiSongData(midiPlayer);
        }

		internal static MidiSongData GetData(SongPlayData source) {
			MidiSongData data = source as MidiSongData;
			if(data == null) {
				throw new InvalidOperationException("Must use midi song data.");
			}
			return data;
		}
	}
}
