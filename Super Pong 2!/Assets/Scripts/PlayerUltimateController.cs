using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUltimateController : MonoBehaviour {



	public ParticleSystem ultimateBase;
	public ParticleSystem ultimateBeam;
	public UltimateCollider ultimateCollider;

	bool isPlaying = false;
	public float duration = 5f;
	float durationCounter = 0;

	Action callback = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlaying) {
			durationCounter += Time.deltaTime;
			if (durationCounter >= duration) {
				durationCounter = 0;
				ultimateBase.Stop ();
				ultimateBeam.Stop ();
				ultimateCollider.disableCollider ();

				if (callback != null)
					callback ();
			}
		}
	}

	public void reset () {
		ultimateBase.Clear ();
		ultimateBeam.Clear ();
	}

	public void playBase(){
		ultimateBase.Play ();
	}

	public void stopBase(){
		ultimateBase.Stop ();
	}

	public void playBeam(float yDir, Action onFinished){
		ultimateBeam.Play ();

		ultimateCollider.GrowCollider (yDir);
		callback = onFinished;
		isPlaying = true;
	}
}
