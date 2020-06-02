using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

	private float speedX;
	private float directionX;
	private float speedY;
	private float directionY;

	public float initialYSpeed;
	public float maxInitSpeedX;
	
    public float initialSpeedYIncrease = 50;
    public float speedYIncrease;
    public float minSpeedYIncrease = 10;

    public float speedYIncreaseSlowdown = 10f;
    public float speedYIncreaseSlowdownSpeed = 5;
    public float maxSlowDownSpeed = 7;


	public float maxSpeedXIncrease;

	public float xDisplacementScale = 200f;

	public float timeSlowOffset = 100f;

	public RectTransform rect;

    public float slowedTime = 0.5f;
    public float standardTime = 1.0f;

	public float maxSpeedX = 300f;
	TrailRenderer trailRenderer;

	public float baseSpeedXIncrease = 20f;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform> ();
		trailRenderer = GetComponent<TrailRenderer> ();
		resetBall ();
	}

	public void resetBall (){

		trailRenderer.Clear ();
		rect.anchoredPosition = new Vector2 (0, 0);

		int startPosY = Random.Range (0, 2);
		speedY = initialYSpeed;
		//0 = ball start up, 1 = down
		directionY = startPosY == 0 ? 1 : -1;
		speedX = maxInitSpeedX;

		int startPosX = Random.Range (0, 2);
		directionX = startPosX == 0 ? 1 : -1;

        speedX = Random.Range (0, maxInitSpeedX);
        speedYIncrease = initialSpeedYIncrease;
		trailRenderer.enabled = true;
	}


	// Update is called once per frame
	void Update () {
        if (!GameManagers.gameModel.hasWon) {
            rect.anchoredPosition += new Vector2 (speedX * directionX, speedY * directionY) * Time.deltaTime; 
            if ( (rect.anchoredPosition.y >= GameManagers.consts.gameSize.y / 2 - timeSlowOffset && directionY == 1) || (rect.anchoredPosition.y <= -GameManagers.consts.gameSize.y / 2 + timeSlowOffset && directionY == -1)) {
               // Time.timeScale = slowedTime;
            } else {
                //Time.timeScale = standardTime;
            }
        }

        if ( (rect.anchoredPosition.x >= GameManagers.consts.gameSize.x/2 - rect.sizeDelta.x/2 && directionX == 1) || (rect.anchoredPosition.x - rect.sizeDelta.x/2 <= -GameManagers.consts.gameSize.x/2 && directionX == -1)) {
            directionX *= -1;
        }

        if (rect.anchoredPosition.y >= (GameManagers.consts.gameSize.y / 2 + rect.sizeDelta.y / 2 + 200) && GameManagers.gameModel.hasWonRound == false) {
            //trailRenderer.Clear ();
            GetComponent<Image> ().enabled = false;
            GameManagers.gameModel.addScore (PlayerControlScheme.MULTIPLAYER_1);
        } else if (rect.anchoredPosition.y <= -(GameManagers.consts.gameSize.y / 2 + rect.sizeDelta.y / 2 + 200) && GameManagers.gameModel.hasWonRound == false) {
            //trailRenderer.enabled = false;
            GetComponent<Image> ().enabled = false;
            GameManagers.gameModel.addScore (PlayerControlScheme.MULTIPLAYER_2);
        }

         

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
            if (other.GetComponent<PlayerInputController> ().getYDir () == directionY) {
                return;
            }
			directionY *= -1;
			speedY += speedYIncrease;
            speedYIncrease -= speedYIncreaseSlowdown;
            speedYIncrease = Mathf.Clamp (speedYIncrease, minSpeedYIncrease, speedYIncrease);
            speedYIncreaseSlowdown += speedYIncreaseSlowdownSpeed;
            speedYIncreaseSlowdown = Mathf.Clamp (speedYIncreaseSlowdown, 0, maxSlowDownSpeed);

			float xDisplacement = (transform.position - other.transform.position).x;
            float extraSpeed = Mathf.Abs (xDisplacement) * xDisplacementScale + maxSpeedXIncrease; //Random.Range (0.0f, maxSpeedXIncrease) + Mathf.Abs (xDisplacement) * xDisplacementScale;

            if (xDisplacement < 0 && directionX == 1 || xDisplacement >= 0 && directionX == -1) {
                extraSpeed *= -1;
            }

			float baseSpeedIncrease = baseSpeedXIncrease * directionX;

			float nextSpeed = extraSpeed + speedX + baseSpeedXIncrease;

            if (nextSpeed < 0 ) {
                nextSpeed *= -1;
                directionX *= -1;
            }

            if (nextSpeed < maxSpeedX ) {
                speedX = nextSpeed;
            } else {
                speedX = maxSpeedX;
            }
		}
	}

	public void flipDirectionY(){
		directionY *= -1;
	}

	public float getYDirection (){
		return directionY;
	}
	public void addSpeedY (float addAmount) {
		speedY += addAmount;
		if (speedY < 0 ) {
			speedY *= -1;
			directionY *= -1;
		}

    }
    public void addSpeedX (float addAmount) {
        speedX += addAmount;
        if (speedX < 0 ) {
            speedX *= -1;
            directionX *= -1;
        }

    }

    public float getYSpeed (){
        return speedY;
    }

}
