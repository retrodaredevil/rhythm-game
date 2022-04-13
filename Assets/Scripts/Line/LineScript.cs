using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Player.Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame.Scripts.Line {
    public class LineScript : SongBehaviour {

        [SerializeField]
        public ControlDirection direction;

        private GameObject _lineObject;

        public GameObject lineObject {
            get {
                if (_lineObject == null) {
                    int toGet = 0;
                    if (toGet >= this.transform.childCount) {
                        Debug.Log("LineScript: " + this.name + " only has " + this.transform.childCount + " child(s). This method will still try to get it though. Expect an excpetion.");
                    }
                    _lineObject = this.transform.GetChild(toGet).gameObject;
                }
                return _lineObject;
            }
        }
        [HideInInspector]
        public GameObject noteSpotObject;

        private float targetZ = 0;
        
        

        public float flyInSpeed = 6f;
        public float startFlyZPositionAddition = 15f;

        private float originalXScale;

        private bool _pressedDown = false;
        public bool isPressedDown {
            get {
                return _pressedDown;
            }
            set {
                if(_pressedDown == value) {
                    return;
                }
                _pressedDown = value;

                Vector3 scale = this.lineObject.transform.localScale;
                scale.x = _pressedDown ? scale.z : originalXScale;
                this.lineObject.transform.localScale = scale;

                //Vector3 angles = this.transform.localEulerAngles;
                //angles.z = _pressedDown ? 90 : 0;
                //this.transform.localEulerAngles = angles;
            }
        }


        /// <summary>
        /// True if the line is done being flown in
        /// </summary>
        public bool flownIn {
            get;
            private set;
        }
        /// <summary>
        /// If true, the line will continue to fly in. If false, it will stay where it is
        /// If the line has already flownIn or is in the process, changing this will not reset it's position
        /// Also note, that changing this only affects this line, not any other lines.
        /// </summary>
        public bool flyIn = true;



        // Use this for initialization
        void Start() {
            targetZ = this.lineObject.transform.localPosition.z;

            Vector3 current = this.lineObject.transform.localPosition;
            current.z += startFlyZPositionAddition;
            this.lineObject.transform.localPosition = current;
            originalXScale = lineObject.transform.localScale.x;

            noteSpotObject = this.transform.GetChild(1).GetChild(0).gameObject;
        }

        // Update is called once per frame
        void Update() {
            if (!flownIn && flyIn) {
                {
                    Vector3 current = this.lineObject.transform.localPosition;
                    current.z -= flyInSpeed * songScript.time.deltaTime;
                    if (current.z < this.targetZ) {// now the lines will stop moving
                        current.z = targetZ;
                        flownIn = true;
                        
                    }
                    this.lineObject.transform.localPosition = current;
                }
                
            }
            

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="height">A number (usually from 0 to 1) where 0 is the bottom and 1 is the top.</param>
        /// <returns>The spot on the line using the height</returns>
        public Vector3 GetSpotOnLine(float height) {
            float use = height * 2f - 1f;

            Vector3 add = new Vector3(0, use, 0);

            add.Scale(lineObject.transform.localScale); // the lineObject's scale is greatest in the Y

            add = lineObject.transform.rotation * add;

            //add.Scale(this.transform.lossyScale); // just in case we decide so edit the scale of a supersupersuperobject, we will be prepared
            //Debug.Log("add final: " + add);

            Vector3 r = lineObject.transform.position + add;

            //Debug.Log("Returning: " + r + " add: " + add + " pos: " + lineObject.transform.position);

            return r;
        }



    }
}