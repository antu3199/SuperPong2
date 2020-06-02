using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

    public float yDir = 1;
    public float ySpeed = 100;
    public float additionalSpeed = 0;

    private RectTransform rect;
    public Material player1Mat;
    public Material player2Mat;

    float bulletSpeedMultiplier = 1;

    public void setMovementDirection (bool goesUp){
        yDir = goesUp == true ? 1 : -1;
        transform.localScale = new Vector3 (transform.localScale.x, yDir, transform.localScale.z);
        GetComponent<TrailRenderer> ().material = yDir == 1 ? player1Mat : player2Mat;
    }

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform> ();	
	}
	
	// Update is called once per frame
	void Update () {
        rect.anchoredPosition += Vector2.up * yDir * (ySpeed + additionalSpeed) * bulletSpeedMultiplier * Time.deltaTime; 
        if (Mathf.Abs(rect.anchoredPosition.y) >= GameManagers.consts.gameSize.y * 2) {
            Destroy (gameObject);
        }
	}

    public void setBulletSpeedMultiplier(float speed){
        bulletSpeedMultiplier = speed;
    }
}
