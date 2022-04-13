using RhythmGame.Scripts.Behaviours;
using RhythmGame.Scripts.Player;
using RhythmGame.Scripts.SongUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Screen {
    public class ScreenScript : SongBehaviour{

        public new Camera camera;
        public List<PlayerScript> players = new List<PlayerScript>();

        public MasterSong masterSong;

        [SerializeField]
        private ScreenPortionValue _screenPortionValue = ScreenPortionValue.full;
        private Rect? _screenPortion = null;

        
        void Start() {
            camera.rect = screenPortion;

        }


        public Rect screenPortion {
            get {
                if(_screenPortion == null) {
                    _screenPortion = ScreenPortion.FromValue(_screenPortionValue);
                }

                return (Rect)_screenPortion;
            }
            set {
                camera.rect = value;
                _screenPortion = value;
            }
        }

    }
}
