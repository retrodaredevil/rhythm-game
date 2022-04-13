using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Midi.Extra.Stress {
	public class LongNoteConfig {

		public readonly bool hasLongNotes;


		public readonly LongNoteShortener notesInsideShortener;
		/// if applied and is SHORTEN_TO_NEXT, do adjust the length to minLength, or if that interferes with another note, make the length 0
		public readonly LongNoteShortener tooShortShortener;
		/// if applied and is SHORTEN_TO_NEXT, shorten the length to maxLength
		public readonly LongNoteShortener tooLongShortener;

		/// Represents the amount of notes there can be within maxTime
		public readonly StressPeriod maxNotesInside;
		/// Assumming the length of a long note is shorter than this, and notesInsideShortener is not REMOVE, use tooShortShortener to determine what to do
		public readonly float minLength;
		/// Assuming the length of a long note is greater than this and notesInsideShortener has not been applied, use tooLongShortener to determine what to do
		public readonly float maxLength;

		public LongNoteConfig(bool hasLongNotes, LongNoteShortener notesInsideShortener, StressPeriod maxNotesInside, float minLength = 1f, float maxLength = 15f, LongNoteShortener tooShort = LongNoteShortener.SHORTEN_TO_NEXT, LongNoteShortener tooLong = LongNoteShortener.SHORTEN_TO_NEXT) {
			this.hasLongNotes = hasLongNotes;
			this.notesInsideShortener = notesInsideShortener;
			this.maxNotesInside = maxNotesInside;

			this.minLength = minLength;
			this.maxLength = maxLength;

			this.tooShortShortener = tooShort;
			this.tooLongShortener = tooLong;

		}

		public LongNoteConfig() : this(false, LongNoteShortener.REMOVE, new StressPeriod(0, 1, 0)) {

		}

	}

	public enum LongNoteShortener {
		REMOVE, LENGTH_ZERO, SHORTEN_TO_NEXT
	}

}
