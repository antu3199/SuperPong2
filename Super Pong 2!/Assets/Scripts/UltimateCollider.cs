using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateCollider : MonoBehaviour {


	BoxCollider2D collider;

	public float growSpeed = 1000f;

	float maxSize;
	bool growCollider = false;

	float yDir = 1;
	public float power = 10f;

	// Use this for initialization
	void Start () {
		maxSize = GameManagers.consts.gameSize.y * 3;
		collider = GetComponent<BoxCollider2D> ();
		collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (growCollider && collider.size.y < maxSize) {
			collider.size += Vector2.up * growSpeed * Time.deltaTime;
		}
	}

	public void GrowCollider(float yDir){
		collider.enabled = true;
		growCollider = true;

		this.yDir = yDir; 
	}

	void OnTriggerStay2D (Collider2D other){
        if (other.tag == "Ball") {
            BallController ballController = other.GetComponent<BallController> ();

            float extraSpeed = power;
            if (yDir != ballController.getYDirection ()) {
                extraSpeed *= -1;
                ballController.addSpeedY (extraSpeed / 2);
            }

            ballController.addSpeedY (extraSpeed);
            ballController.addSpeedX (extraSpeed / 2);

        } else if (other.tag == "AIBotHelper") {
            Destroy (other.gameObject);
        }

	}

	public void disableCollider (){
		collider.enabled = false;
	}



}
