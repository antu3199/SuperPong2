using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstsController : MonoBehaviour {
    public Canvas canvas;
    public RectTransform gameHolder;

	// Use this for initialization
	void Awake () {
		StartCoroutine (updateConsts ());
	}

	IEnumerator updateConsts (){
		while (GameManagers.consts == null) {
			yield return null;
		}
		GameManagers.consts.gameSize = new Vector2( gameHolder.sizeDelta.x, canvas.GetComponent<CanvasScaler>().referenceResolution.y );
		GameManagers.consts.screenSize = canvas.GetComponent<CanvasScaler> ().referenceResolution;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
