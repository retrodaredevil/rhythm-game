using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmGame.Scripts.Player.Controls {
    /// <summary>
    ///
    /// </summary>
    public enum ControlDirection : int {
        NONE = -1, UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3


    }
    public class ControlAxis {
        public static readonly ControlAxis xAxis = new ControlAxis("x", new ControlDirection[] { ControlDirection.RIGHT, ControlDirection.LEFT }.OfType<ControlDirection>().ToList() );
        public static readonly ControlAxis yAxis = new ControlAxis("y", new ControlDirection[] { ControlDirection.UP, ControlDirection.DOWN }.OfType<ControlDirection>().ToList());


        public List<ControlDirection> directions;
        public string name;

        private ControlAxis(string name, List<ControlDirection> directions) {
            this.directions = directions;
            this.name = name;
        }

        public static bool IsSameAxis(ControlDirection a, ControlDirection b) { // I could write this in one line but I'm too lazy
            if(xAxis.directions.Contains(a) && xAxis.directions.Contains(b)) {
                return true;
            }
            if (yAxis.directions.Contains(a) && yAxis.directions.Contains(b)) {
                return true;
            }
            return false;
        }

    }
}
