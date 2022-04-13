using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Player.Controls {
    public interface ControlListener {
        

        void OnStrum(List<ControlDirection> direction);
        void OnChange(List<ControlDirection> direction);

        void OnPausePress();
        void OnSelectPress();

        void OnCancelPress();
        void OnMenuDirection(ControlDirection direction);
    }
}
