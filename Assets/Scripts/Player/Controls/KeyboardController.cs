using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Player.Controls {
    public class KeyboardController : PlayerController {

        //        private KeyCode[] up = { KeyCode.W, KeyCode.UpArrow};
        //        private KeyCode[] right = { KeyCode.D, KeyCode.RightArrow };
        //        private KeyCode[] down = { KeyCode.S, KeyCode.DownArrow };
        //        private KeyCode[] left = { KeyCode.A, KeyCode.LeftArrow };


        protected List<ControlDirection> lastDirection = new List<ControlDirection>();

        void Update() {
            CheckPauseAndCancel();
            

            CheckSubmitAndStrum(CheckDirection(GetAxis()));
        }
        protected virtual void CheckPauseAndCancel() {
            if (Input.GetButtonDown("Cancel")) { // cancel and pause events
                OnCancelPress();
            }
            if (Input.GetButtonDown("Pause")) {
                OnPausePress();
            }
        }
        protected virtual List<ControlDirection> CheckDirection(Vector2 axis) {
            // Debug.Log("Axis: " + axis + " hor: " + Input.GetAxis("Horizontal")); // I'm not that stupid all I had to do was unplug my phone.
            
            
            List<ControlDirection> current = new List<ControlDirection>();

            ControlDirection hDirection = ControlDirection.NONE;
            ControlDirection vDirection = ControlDirection.NONE;
            {
                float hor = axis.x;
                float ver = axis.y;
                hor = Mathf.Ceil(hor);
                ver = Mathf.Ceil(ver);
                if (hor > 0) {
                    hDirection = ControlDirection.RIGHT;
                } else if (hor < 0) {
                    hDirection = ControlDirection.LEFT;
                }
                if (ver > 0) {
                    vDirection = ControlDirection.UP;
                } else if (ver < 0) {
                    vDirection = ControlDirection.DOWN;
                }
            }
            if (hDirection != vDirection) { // neither are NONE // there may be a simpler way to do this. 
                if (hDirection == ControlDirection.NONE) {
                    current.Add(vDirection);
                } else if (vDirection == ControlDirection.NONE) {
                    current.Add(hDirection);
                } else { // now, the the inputs are being pressed at once
                    current.Add(vDirection);
                    current.Add(hDirection);
                }
            }
            if (!lastDirection.SequenceEqual(current)) {
                OnChange(current);
                lastDirection = current;
            }
            return current;
        }
        protected virtual Vector2 GetAxis() {
            return new Vector2(GetHorizontal(), GetVertical());
        }
        private float GetHorizontal() {
            return Input.GetAxis("Horizontal");
        }
        private float GetVertical() {
            return Input.GetAxis("Vertical");
        }

        protected virtual void CheckSubmitAndStrum(List<ControlDirection> current) {
            if (Input.GetButtonDown("Strum")){ 
                OnStrum(current);
            }
            if (Input.GetButtonDown("Submit")){
                OnSelectPress();
            }
        }

    }
}
