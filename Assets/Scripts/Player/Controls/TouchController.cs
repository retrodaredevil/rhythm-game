using RhythmGame.Scripts.Player.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Player.Controls {
    public class TouchController : KeyboardController {

        private Vector2? last;
        private Vector2 orig;
        private List<ControlDirection> directions = new List<ControlDirection>();
        private bool strum = false;

        public float minimumMagnitudeInPixels = 25;



        private void Start() {
            Input.multiTouchEnabled = true;
        }

        void Update() { // override keyboard controller's update so it won't do anything on it's own
            //#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE #endif

            if(Input.touchCount > 0) {
                Touch recent = Input.touches[0];
                

                /*
                 * if (Input.touchCount > 1 && last != null) {
                    float d2 = float.MaxValue;
                    Touch set = recent;
                    foreach(Touch t in Input.touches) {
                        float distance = DistanceSquared((Vector2)last, t.position);
                        if (distance < d2) { // set the touch that is closest from the last
                            d2 = distance;
                            set = t;
                        }
                    }
                    recent = set;
                } 
                */

                Vector2 current = recent.position;

                bool isBeginning = false;

                //if(last != null && Vector3.Distance(current, (Vector2)last) > 100) {
                //    StopTouch(); // this is a brand new touch or there is another touch on screen but we are stopping that one.
                //    isBeginning = true;
                //}
                last = current;

                
                if(recent.phase == TouchPhase.Began || isBeginning) {
                    orig = recent.position;
                    //Debug.Log("orig: " + orig);
                } else if(recent.phase == TouchPhase.Ended) {
                    StopTouch();
                    return;
                }
                float mag = Vector3.Distance(current, orig);
                Vector2 normal;
                float angle;
                { // update directions
                    normal = current - orig;
                    //normal = normal.normalized; // we don't need to normalize it

                    {
                        angle = Mathf.Rad2Deg * Mathf.Atan2(normal.y, normal.x) ;

                        //Debug.Log("angle: " + angle);

                    }

                    /*{ // using floats to guess the angle, not as accurate as above
                        float ax = Mathf.Abs(normal.x);
                        float ay = Mathf.Abs(normal.y);

                        float dead = .4f;

                        if (ax < dead) {
                            normal.x = 0;
                        } else {
                            normal.x /= ax; // make it either 1 or -1 depending if it's negative
                        }
                        if (ay < dead) {
                            normal.y = 0;
                        } else {
                            normal.y /= ay;
                        }
                    }*/
                }
                List<ControlDirection> directions = CheckDirection(angle);

                if(lastDirection == null || !directions.SequenceEqual(lastDirection)) {

                    OnChange(directions);
                    lastDirection = directions;
                }

                //Debug.Log("Current: " + current + " orig: " + orig + " directions: " + ScoreScript.ListToString(directions) + " normal: " + normal);
                if (!strum && mag > minimumMagnitudeInPixels && directions.Count > 0) {
                    OnStrum(directions);
                    strum = true;
                }
                    
                
            }



        }
        
        public static List<ControlDirection> CheckDirection(float angle) {
            /*      90
             * 180       0
             *      270
             */
           

            List<ControlDirection> r = new List<ControlDirection>();
            const float between = 360 / 8; // 45

            //angle -= between / 2;
            while(angle < 0) {
                angle += 360;
            } // angle is between 0 and 360

            int result = (int)Math.Round(angle / between);
            switch (result) {
                case 0:
                    r.Add(ControlDirection.RIGHT);
                    break;
                case 1:
                    r.Add(ControlDirection.RIGHT);
                    r.Add(ControlDirection.UP);
                    break;
                case 2:
                    r.Add(ControlDirection.UP);
                    break;
                case 3:
                    r.Add(ControlDirection.UP);
                    r.Add(ControlDirection.LEFT);
                    break;
                case 4:
                    r.Add(ControlDirection.LEFT);
                    break;
                case 5:
                    r.Add(ControlDirection.LEFT);
                    r.Add(ControlDirection.DOWN);
                    break;
                case 6:
                    r.Add(ControlDirection.DOWN);
                    break;
                case 7:
                    r.Add(ControlDirection.RIGHT);
                    r.Add(ControlDirection.DOWN);
                    break;

            }
            //Debug.Log("angle: " + angle + " result: " + result + " r: " + ScoreScript.ListToString(r));

            return r;
        }

        private void StopTouch() {
            strum = false;
            orig = Vector3.zero;
            directions.Clear();
            OnChange(directions);
            //Debug.Log("Stopping...");
        }
        private static float DistanceSquared(Vector2 a, Vector2 b) {
            return Mathf.Pow(a.x + b.x, 2) + Mathf.Pow(a.y + b.y, 2);
        }

    }
}
