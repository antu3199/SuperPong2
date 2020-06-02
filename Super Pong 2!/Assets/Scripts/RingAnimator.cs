using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingAnimator : MonoBehaviour {

    public int segments;
    public float radius;
    LineRenderer line;
    public float speed = 1000f;

	bool playAnim = false;
	CircleCollider2D collider;

	float yDirection = 1;

	public float ballSpeedBoost = 300f;

	float maxRadius;

	public ParticleSystem particles;
    public Transform animatorTransform;
    public Transform parentTransform;

	public void playAnimation(PlayerControlScheme controlScheme){
        RectTransform rect = GetComponent<RectTransform> ();
        transform.SetParent (animatorTransform);
        rect.anchoredPosition = Vector3.zero;

		gameObject.SetActive (true);
        transform.SetParent (parentTransform);
		line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = (segments + 1);
		line.useWorldSpace = false;
		collider = GetComponent<CircleCollider2D> ();
		playAnim = true;
		radius = 100;
		maxRadius = Mathf.Max (GameManagers.consts.gameSize.x, GameManagers.consts.gameSize.y) * 3;
		collider.radius = radius;
		if (controlScheme == PlayerControlScheme.MULTIPLAYER_1 || controlScheme == PlayerControlScheme.SINGLE_PLAYER) {
			yDirection = 1; 
		} else {
			yDirection = -1;
		}
		particles.Clear ();
		particles.Play ();
	}

	void reset(){
		playAnim = false;
		radius = 100;
		CreatePoints ();
		collider.radius = radius;
		gameObject.SetActive (false);
	}

    void CreatePoints ()
    {
        float x;
        float y;
		float z = 0;

        float angle = 0f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle);
            y = Mathf.Cos (Mathf.Deg2Rad * angle);
            line.SetPosition (i,new Vector3(x,y,z) * radius);
            angle -= (360f / segments);
        }
    }

    void Update (){
		if (playAnim) {
			if (radius < maxRadius) {
				radius += speed * Time.deltaTime;
				CreatePoints ();
				collider.radius = radius;
			} else {
				reset();
			}
		}
    }

	void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Ball" && GameManagers.gameModel.hasWon == false) {
			BallController ballController = other.GetComponent<BallController> ();
			if (yDirection != ballController.getYDirection ()) {
				ballController.flipDirectionY ();
                ballController.addSpeedY (ballSpeedBoost * (1 - (radius / maxRadius )) );
			} else {
                ballController.addSpeedY (ballSpeedBoost * (1 - (radius / maxRadius ))  * 2);
			}
		}
	}

  

}
