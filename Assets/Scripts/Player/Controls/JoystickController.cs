using GamepadInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace RhythmGame.Scripts.Player.Controls {
    [System.Obsolete]
    public class JoystickController : KeyboardController{
        
        public GamePad.Index playerNumber;

        private float lastAxisCall = 0;
        private Vector2 lastAxis = Vector2.zero;

        private void Start() {
            Debug.Log("JoystickController does not function as intended. Please review code before using.");
        }

        protected override void CheckPauseAndCancel() {

            { // pause
                if(GamePad.GetButtonDown(GamePad.Button.Start, playerNumber)) {
                    OnPausePress();
                }
            }
            { // cancel
                if (GamePad.GetButtonDown(GamePad.Button.B, playerNumber)) {
                    OnCancelPress();
                }
            }
            
            
        }
        protected override Vector2 GetAxis() {
            lastAxis = GamePad.GetAxis(GamePad.Axis.Dpad, playerNumber);
            return lastAxis;
        }


    }
}
