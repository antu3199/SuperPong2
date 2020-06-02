using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBotItemController : MonoBehaviour {

    public float moveSpeed;
    public int maxHealth = 3;

    RectTransform rect;


    public float yDir = 1;
    float health = 3;

    public float duration = 10f;
    float durationCounter = 0;

    public Color player1Color;
    public Color player2Color;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform> ();
        health = maxHealth;
	}
	

    public void setYDir (float dir){
        yDir = dir;
        if (yDir == 1) {
            GetComponent<Image> ().color = player1Color;
        } else {
            GetComponent<Image> ().color = player2Color;
        }
    }

	// Update is called once per frame
	void Update () {
		
        if (Mathf.Abs (rect.anchoredPosition.x - GameManagers.gameModel.controller.ball.rect.anchoredPosition.x) > 5) {
            if (rect.anchoredPosition.x < GameManagers.gameModel.controller.ball.rect.anchoredPosition.x) {
                rect.anchoredPosition += Vector2.right * moveSpeed * Time.deltaTime;
            } else {
                rect.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            }
        }

        durationCounter += Time.deltaTime;
        if (durationCounter > duration) {
            durationCounter = 0;
            health = 0;
            checkIfDestroyed ();
        }

	}

    void OnTriggerEnter2D (Collider2D other ){
        if (other.tag == "Ball") {
            if (GameManagers.gameModel.controller.ball.getYDirection () != yDir) {
                GameManagers.gameModel.controller.ball.flipDirectionY ();
            }
        } else if (other.tag == "Bullet") {
            BulletMovement bullet = other.transform.parent.GetComponent<BulletMovement> ();
            if (bullet.yDir != yDir) {
                health--;
                Destroy (other.transform.parent.gameObject);
                checkIfDestroyed ();

            }

        } else if (other.tag == "Ultimate") {
            health = 0;
            checkIfDestroyed ();
        }

    }

    void checkIfDestroyed(){
        if (health <= 0) {
            Destroy (gameObject);
        }
    }

}
