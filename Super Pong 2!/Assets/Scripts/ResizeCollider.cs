using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResizeCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        adjustCollider ();
	}

    public void adjustCollider(){
        RectTransform rect = GetComponent<RectTransform> ();
        GetComponent<BoxCollider2D> ().size = new Vector3 (rect.rect.width, rect.rect.height, 1);
    }
}
