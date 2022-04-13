using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Player.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Line {
    public class LineHandler : MonoBehaviour {

        public List<LineScript> lines;// create in editor


        public LineScript GetLine(ControlDirection direction) {
            foreach (LineScript line in lines) {
                if(line.direction == direction) {
                    return line;
                }
            }

            return null;
        }

    }
}