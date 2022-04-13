using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.SongData.Create {
    public interface SongCreator<out T, in D> where T : Song {


        T Create(D data);


    }
}
