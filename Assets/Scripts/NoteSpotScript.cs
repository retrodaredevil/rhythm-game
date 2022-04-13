using RhythmGame.Scripts.Line;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame.Scripts {
    [ExecuteInEditMode]
    public class NoteSpotScript : MonoBehaviour {

        private LineScript line;
        private MeshRenderer lineRender;
        private MeshRenderer render;

        // Use this for initialization
        void Start() {
            line = this.transform.parent.gameObject.GetComponent<LineScript>();
            render = this.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
			if(line == null) {
				Debug.Log("line is null"); // problem with missing MonoBehaviour fixed
			}
            lineRender = line.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

            Color c = lineRender.sharedMaterial.color;
            c.a = 0.3f;
            render.sharedMaterial.color = c;
        }
        
    }
}