using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Player.Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RhythmGame.Scripts.Player.Score;
using RhythmGame.Scripts.SongData;

namespace RhythmGame.Scripts.Player {
    [RequireComponent(typeof(ScoreScript))]
    public class PlayerScript : ScreenBehaviour {

        [SerializeField]
        private GameObject _controllerPrefab;

        [HideInInspector]
        public ScoreScript score;


        [HideInInspector]
        public GameObject childControllerGameObject;

        private PlayerController _controller;
        public PlayerController controller { // the controller is the object that holds ControlListeners and calls them. 
            get {
                if (_controller == null) {
                    _controller = childControllerGameObject.GetComponent<PlayerController>();
                    if (_controller == null) {
                        Debug.LogWarning("The controller is null!!!!!!!!!!!!");
                        return null;
                    }
                    _controller.screenScript = screenScript; // gotta give it the neccessary things it doesn't have
                    _controller.songScript = songScript;
                }
                return _controller;
            }
        }

        public SongPart songPart { get { return this.songScript.songPart; } }

        // Use this for initialization
        void Start() {
            ChangeController(_controllerPrefab);
            score = GetComponent<ScoreScript>();
            score.playerScript = this;
            controller.AddListener(score);

            screenScript.players.Add(this);
        }

        // Update is called once per frame
        void Update() {

        }

        public PlayerController ChangeController(GameObject controllerPrefab) {
            this._controllerPrefab = controllerPrefab;
            if (_controller != null) {
                Destroy(_controller); // controller should be attached to childController that we will destroy next
            }
            if (childControllerGameObject != null) {
                Destroy(childControllerGameObject);
            }
            _controller = null;
            childControllerGameObject = null;
            // both just got destroyed so we'll just set them to null


            childControllerGameObject = Instantiate(_controllerPrefab); // create the childController // the prefab has the script on it and anything else it needs.
            childControllerGameObject.transform.parent = this.transform; // set the object we just created to our child
            return controller; // by calling the get of the controller, it will get the PlayerController object
        }
    }
}