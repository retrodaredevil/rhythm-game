using RhythmGame.Scripts.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : SongBehaviour {
    

	private Material current;
    private float rotation;

	// Use this for initialization
	void Start () {

		current = (Material)Resources.Load("Skybox2_8/Skybox2_8");
		RenderSettings.skybox = current;
		songScript.screenScript.camera.gameObject.AddComponent<SmoothMouseLook>();


	}
    private void Update() {
        rotation += 1 * songScript.time.deltaTime;
        current.SetFloat("_Rotation", rotation);


    }

}
