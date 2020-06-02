using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    //yDir is used to determine whether or not it is player1 or player2
    public abstract void applyItem (PlayerInputController player);

    void OnTriggerEnter2D (Collider2D other){
        if (other.tag == "Bullet") {
            PlayerInputController player = other.transform.parent.GetComponent<BulletMovement> ().yDir == 1 ? GameManagers.gameModel.controller.player1 : GameManagers.gameModel.controller.player2;
            applyItem(player);
            Destroy (gameObject);
        }
    }
}
