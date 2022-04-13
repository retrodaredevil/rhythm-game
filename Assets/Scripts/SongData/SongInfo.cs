namespace RhythmGame.Scripts.SongData {
    /// a struct representing song info like the name artist album etc.
    public struct SongInfo {


        public string name;
        public string artist;
        public string album;


        public SongInfo(string name, string artist, string album) {
            this.name = name;
            this.artist = artist;
            this.album = album;
        }


    }
}