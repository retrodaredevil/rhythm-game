using RhythmGame.Scripts.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Player.Controls {
    public class PlayerController : ScreenBehaviour, ControlListener {

        private List<ControlListener> listeners = new List<ControlListener>();


        public void AddListener(ControlListener listener) {
            listeners.Add(listener);
        }
        public void RemoveListener(ControlListener listener) {
            listeners.Remove(listener);
        }
        public bool IsRegistered(ControlListener listener) {
            return listeners.Contains(listener);
        }

//        private bool CheckListener(ControlListener l) {
//            if(l == null) {
//                RemoveListener(l); // if this is a gameObject and it has been destroyed, checking if it is null will return true because the == is overloaded so really, it's not null
//                return true;
//            }
//            return false;
//        }


        public void OnCancelPress() {
            foreach (ControlListener l in listeners) {
                l.OnCancelPress();
            }
        }

        public void OnChange(List<ControlDirection> direction) {
            foreach (ControlListener l in listeners) {
                l.OnChange(direction);
            }
        }

        public void OnPausePress() {
            foreach (ControlListener l in listeners) {
                l.OnPausePress();
            }
        }

        public void OnStrum(List<ControlDirection> direction) {
            foreach (ControlListener l in listeners) {
                l.OnStrum(direction);
            }
        }

        public void OnSelectPress() {
            foreach (ControlListener l in listeners) {
                l.OnSelectPress();
            }
        }

        public void OnMenuDirection(ControlDirection direction) {

            foreach (ControlListener l in listeners) {
                l.OnMenuDirection(direction);
            }
        }



    }
}
