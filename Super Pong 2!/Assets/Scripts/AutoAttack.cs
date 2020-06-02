using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour {


    public bool canAttack = false;

    public GameObject bulletPrefab;
    public Transform parentTransform;
    public Transform spawnLocation;


    public bool goesUp = true;

    private float autoAttackCounter = 0;
    public float autoAttackSpeed = 1;
    public float autoAttackTimer = 2;

    float bulletRechargeMultiplier = 1;
    float bulletSpeedMultiplier = 1; 

    float speedBuffDuration = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        autoAttackCounter += autoAttackSpeed * bulletRechargeMultiplier * Time.deltaTime;
        if (autoAttackCounter >= autoAttackTimer) {
            autoAttackCounter = 0;
            shoot ();
        }

        if (speedBuffDuration > 0) {
            speedBuffDuration -= Time.deltaTime;
            if (speedBuffDuration < 0) {
                bulletRechargeMultiplier = 1;
                bulletSpeedMultiplier = 1;
            }

        }


	}

    void shoot (){
        GameObject bullet = GameObject.Instantiate (bulletPrefab, spawnLocation.position, Quaternion.identity, parentTransform) as GameObject;
        BulletMovement bulletMovement = bullet.GetComponent<BulletMovement> ();
        bulletMovement.setMovementDirection (goesUp);
        bulletMovement.setBulletSpeedMultiplier (bulletSpeedMultiplier);


    }

    public void addSpeedBuff (float bulletSpeed, float bulletRecharge, float duration){
        this.bulletSpeedMultiplier = bulletSpeed;
        this.bulletRechargeMultiplier = bulletRecharge;
        this.speedBuffDuration = duration;
    }

}
