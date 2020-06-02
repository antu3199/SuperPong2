using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomItem : Item {

	
    public float sizeIncrease = 30;

    public override void applyItem (PlayerInputController player){
        player.GetComponent<RectTransform> ().sizeDelta += Vector2.right * sizeIncrease; 
		player.GetComponent<ResizeCollider> ().adjustCollider ();
    }
}
