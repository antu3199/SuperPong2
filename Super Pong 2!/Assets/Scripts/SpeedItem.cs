using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item {

    public float duration = 0;
    public float bulletSpeedMultiplier = 2;
    public float bulletRechargeMultipler = 2;

    public override void applyItem (PlayerInputController player){
        player.GetComponent<AutoAttack> ().addSpeedBuff (bulletSpeedMultiplier, bulletRechargeMultipler, duration);
    }
}
