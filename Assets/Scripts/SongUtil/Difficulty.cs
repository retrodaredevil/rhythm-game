using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.SongUtil {
 
    public class Difficulty {
        // around 0.04 to 0.03 when seeing with viewedHeight(Pow0.9)
        public static readonly Difficulty BEGINNER = new Difficulty(DifficultyValue.BEGINNER, true, false, "beginner", 0.07f * 1.5f, 0.015f, 0.060f, 0.28f);
        public static readonly Difficulty SIMPLE = new Difficulty  (DifficultyValue.SIMPLE, false, true, "simple",     0.07f * 1.5f, 0.015f, 0.060f, 0.28f);
        public static readonly Difficulty EASY = new Difficulty    (DifficultyValue.EASY, true, true, "easy",          0.10f * 1.5f, 0.015f, 0.060f, 0.28f);
        public static readonly Difficulty NORMAL = new Difficulty  (DifficultyValue.NORMAL, true, true, "normal",      0.13f * 1.8f, 0.015f, 0.060f, 0.23f);
        public static readonly Difficulty HARD = new Difficulty    (DifficultyValue.HARD, true, true, "hard",          0.16f * 2,    0.010f, 0.063f, 0.2f);
        public static readonly Difficulty EXTREME = new Difficulty (DifficultyValue.EXTREME, true, false, "extreme",   0.20f * 2,    0.009f, 0.064f, 0.14f);

        public static readonly Difficulty[] values = { BEGINNER, SIMPLE, EASY, NORMAL, HARD, EXTREME };

        private Difficulty(DifficultyValue value, bool strum, bool point, string name, float speed, float minHeight, float maxHeight, float minTimeBetween) { // yeah yeah I know the fields are supposed to be above the constructor. Shut up.
            this.strum = strum;
            this.point = point;
            this.name = name;
            this.speed = speed;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;

            this.value = value;
			this.minimumTimeBetweenNotes = minTimeBetween;
        }

        public bool IsHitAtHeight(float height, SongScript songScript = null) {
            return CanBeHitAtLaterHeight(height, songScript) && height <= GetMax(songScript);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="songScript"></param>
        /// <returns>Returns true if it can be hit at a later height assumming height keeps decreasing</returns>
        public bool CanBeHitAtLaterHeight(float height, SongScript songScript = null) {
            return GetMin(songScript) <= height;
        }
        private float GetMin(SongScript song) {
            float min = minHeight;
            if(song != null) {
                min -= GetDistanceDelay(song);
            }

            return min;
        }
        private float GetMax(SongScript song) {
            float max = maxHeight;
            if(song != null) {
                max -= GetDistanceDelay(song);
            }
            return max;
        }
        /// <returns>The distance, a value 0 to 1 </returns>
        private float GetDistanceDelay(SongScript song) {
            return song.delayInSeconds * speed;
        }

        public DifficultyValue value;

        /// <summary>
        /// The minimum height for allowing to strum
        /// </summary>
        private float minHeight;

        /// <summary>
        /// The maximum height for allowing to strum
        /// </summary>
        private float maxHeight;

        /// <summary>
        /// True if you have to press a button when the note hits.
        /// False if all you have to do is make sure you're pointing in the right direction when the note hits
        /// </summary>
        public bool strum;

        /// <summary>
        /// True if you have to point when the note hits for it to count
        /// False if all you have to do is strum.
        /// </summary>
        public bool point;

        /// <summary>
        /// The name of the difficulty
        /// </summary>
        public string name;

        /// <summary>
        /// The speed for how fast the notes will fall per second
        /// if it is 0.1 then every second the note will fall 10% of the line
        /// </summary>
        public float speed;

		///<summary>
		///The minimum time between notes in seconds ex: if there are two notes X seconds apart, the second one is ignored.
		/// </summary>
		public float minimumTimeBetweenNotes;

        public static Difficulty GetDifficulty(DifficultyValue? value) {
            if(value == null) {
                return null;
            }
            foreach(Difficulty d in values) {
                if(d.value == value) {
                    return d;
                }
            }
            Debug.Log("A Difficulty was requested using the value: " + value + " but we did not return a value.");
            return null;
        }


    }
	///used to represent the different Difficulty values but cannot be used unless you call Difficulty.GetDifficulty
    [Serializable]
    public enum DifficultyValue : int {
        BEGINNER = 1, SIMPLE = 2, EASY = 3, NORMAL = 4, HARD = 5, EXTREME = 6
        

    }
}
