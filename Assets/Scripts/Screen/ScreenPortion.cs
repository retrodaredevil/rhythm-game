using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RhythmGame.Scripts.Screen {
    public static class ScreenPortion {// 0, 0 is the button left
        public static readonly Rect full = new Rect(0, 0, 1, 1);

        public static readonly Rect leftHalf = new Rect(0, 0, 0.5f, 1);
        public static readonly Rect rightHalf = new Rect(0.5f, 0, 0.5f, 1);

        public static readonly Rect upperLeft = new Rect(0, 0.5f, 0.5f, 0.5f);
        public static readonly Rect bottomLeft = new Rect(0, 0, 0.5f, 0.5f);

        public static readonly Rect upperRight = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        public static readonly Rect bottomRight = new Rect(0.5f, 0, 0.5f, 0.5f);


        public static readonly Rect[] values = { full, leftHalf, rightHalf, upperLeft, bottomLeft, upperRight, bottomRight };
        public static Rect FromValue(ScreenPortionValue value) {
            return values[(long)value];
        }
    }
    public enum ScreenPortionValue : int {
        full = 0, leftHalf = 1, rightHalf = 2, upperLeft = 3, bottomLeft = 4, upperRight = 5, bottomRight = 6



    }
}
