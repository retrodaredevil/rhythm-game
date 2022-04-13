using CSharpSynth.Midi;
using CSharpSynth.Synthesis;
using RhythmGame.Scripts.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.MidiUtil {
    public class MidiNote {

        //Public variables
        public MidiEvent noteOn;
        public MidiEvent noteOff;
        /// <summary>The number of the index of the Note for the current track (starts at 0 for each track)</summary>
        public int number;
        
        [Obsolete]
        public readonly IList<NoteData> noteDataList = new List<NoteData>();
      

        //Constructors
        public MidiNote(int number) {
            this.number = number;
        }

        // Public properties
        public uint start {
            get { return noteOn.deltaTime; }
        }
        public uint end {
            get { return noteOff.deltaTime; }
        }
        public uint length {
            get { return end - start; }
        }
        //[Obsolete]
        public byte channelNumber {
            get { return noteOn.channel; }
        }
        public int trackNumber {
            get { return noteOn.trackNumber; }
        }

        //Public Methods

        public override string ToString() {
            return "MidiNote{trackNumber:" + trackNumber + ",number:" + number + ",length:" + length + "}";
        }

        //Static Methods
        public static float ChangeToSeconds(uint time, StreamSynthesizer syn) {

            //float a = (file.BeatsPerMinute * file.MidiHeader.DeltaTiming);

            //Debug.Log("a: " + a);

            return ((float)time) / syn.SampleRate;
        }

    }
}
