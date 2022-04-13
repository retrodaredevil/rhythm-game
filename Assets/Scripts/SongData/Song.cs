using RhythmGame.Scripts.Player;
using RhythmGame.Scripts.Player.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData {
    /// A Song object is responcible for holding info about the song (SongInfo), all of the notes off the song (even if they aren't played on a line) and holding one or more Parts of the song (SongPart)
    ///                             where the part lets you know what kind of note a Playable is (one part could be guitar, and another could be piano.
    /// Objects inside the SongData namespace do not interact with objects outside. Objects outside must call Song methods like Start Update and End. Some outside parameters may be needed to create the
    ///                             song like a MIDIPlayer object
    /// 
    public abstract class Song {
        /* Notes about the SongData namespace
         * So you're going to try to understand what the SongData namespace stuff does huh?
         * Basically, it's an abstraction with stuff that implements it like the Midi namespace
         * It's used to store and play songs with basic method calls like Start, Update and End that a member outside the namespace calls
         * The SongDataClass is where the data is stored for each time a song is played.
         * When I first was going to create this, I learned a lot about C#'s generics and in some places, it may seem that I should have used them
         *              but trust me, you do not want to use generics in this senario or you'd have <SongPlayData> everywhere in the code and type parameters on classes that don't make sense
         *              so you'll see a lot of casting in the different implementations of Song and its data.
         */

        #region events
        /// called before all the SongPart's OnStart are called
        protected delegate void OnStart();
        /// Parameters started and ended are null or are not empty
        protected delegate void OnUpdate(SongPlayData songData, float songTime, List<Playable> started, List<Playable> ended);
        protected delegate void OnEnd(SongPlayData songData);

        protected event OnStart onStartEvent;
        protected event OnUpdate onUpdateEvent;
        protected event OnEnd onEndEvent;
        #endregion

        //Public Variables;
        public IList<Playable> notes { get; private set; }
        public IList<SongPart> parts { get; private set; }

        public readonly SongInfo songInfo;

        //Constructor
        protected Song(IList<Playable> notes, IList<SongPart> parts, SongInfo songInfo) {
            this.songInfo = songInfo;
			this.notes = notes;
			this.parts = parts;

			if(parts.Count == 0) {
				throw new ArgumentException("parts.Count cannot be 0");
			}
        }



        // Public Methods
        public virtual SongPlayData Start() {
            SongPlayData re = CreateSongData();
            if(onStartEvent != null) {
                onStartEvent();
            }
            

            return re;
        }
        /// returns false if the song is over true otherwise
        public virtual bool Update(SongPlayData songData, float songTime, List<SongPlayer> players) {

            if(songData.playables == null || songData.toEnd == null) {
                songData.playables = new List<Playable>(notes);
                songData.toEnd = new List<Playable>();
            }
            List<Playable> toPlay = null; // not initialized to save memory
            { // for starting
                foreach(Playable play in songData.playables) {
                    float start = play.GetTimePlayed(songData);
                    if(start <= songTime) {
                        if(toPlay == null) {
                            toPlay = new List<Playable>();
                        }
                        toPlay.Add(play);
						play.Start(songData); // play note here
                    }

                }
                if(toPlay != null) {
                    foreach(Playable play in toPlay) {
                        songData.playables.Remove(play);
                        songData.toEnd.Add(play);
                    }
                }
            }
            List<Playable> toEnd = null;
            {// for ending
                foreach(Playable play in songData.toEnd) {
                    float end = play.GetTimePlayed(songData) + play.GetLength(songData);
                    if(end <= songTime) {
                        if(toEnd == null) {
                            toEnd = new List<Playable>();
                        }
                        toEnd.Add(play);
						play.EndNote(songData); // end note here
                    }
                }
                if(toEnd != null) {
                    foreach(Playable play in toEnd) {
                        songData.toEnd.Remove(play);
                    }
                }
            }
            

            if(onUpdateEvent != null) {
                onUpdateEvent(songData, songTime, toPlay, toEnd);
            }

            return !(songData.playables.Count == 0 && songData.toEnd.Count == 0); // normally returns false
        }
        
		/// A way to convert a list of PlayerScripts to SongPlayers. This uses a simple cache using the SongPlayData too
        public bool Update(SongPlayData songData, float songTime, List<PlayerScript> players) {

			if(songData.players == null) {
				songData.players = new List<SongPlayer>();

				foreach(PlayerScript p in players) {
					SongPlayer songPlayer = new Func<SongPlayer>(() => {
						foreach(SongPlayer songp in songData.players) {
							if(songp.part == p.songPart)
								return songp;
						}
						return null;
					})();
					StreakType type;
					if(songPlayer == null) {
						songPlayer = new SongPlayer(p.songPart);
						songData.players.Add(songPlayer);
						type = StreakType.NONE;
					} else {
						type = songPlayer.streakType;
						if(type != StreakType.NONE) {
							continue;
						}
					}
					switch(type) { // if one person missed and one person hit, it will still play the note
					case StreakType.MISS: case StreakType.NONE: type = p.score.streakType; break;

					default: break;
					}
					songPlayer.streakType = type;

				}
			}
            return Update(songData, songTime, songData.players);
        }
		///<param name="forced">False if the song is ending normally (false if reached the end of song)</param>

		public virtual void End(SongPlayData songData, bool forced) {
            if(onEndEvent != null) {
                onEndEvent(songData);
            }
        }


        protected abstract SongPlayData CreateSongData();



    }
    /// A class that's used to combine players playing a song
    public class SongPlayer {

        public StreakType streakType = StreakType.NONE;

        public readonly SongPart part;

        /// <param name="onMissStreak">True if on a miss streak</param>
        public SongPlayer(SongPart part) {
            this.part = part;
        }



    }
}
