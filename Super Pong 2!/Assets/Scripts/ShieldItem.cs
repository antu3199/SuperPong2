using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item {

    public GameObject AIBotPrefab;

    public override void applyItem (PlayerInputController player){
        //TODO: shield item

        GameObject aiBotHelper = Instantiate (AIBotPrefab) as GameObject;

        float randomX = Random.Range (-GameManagers.consts.gameSize.x / 2f, GameManagers.consts.gameSize.x / 2f);

        RectTransform rect = aiBotHelper.GetComponent<RectTransform> ();
        aiBotHelper.transform.SetParent (player.AIBotLocation, false);
        rect.anchoredPosition = Vector2.zero;

        aiBotHelper.transform.SetParent (GameManagers.gameModel.controller.gameHolder);
        rect.anchoredPosition = new Vector2 (randomX, rect.anchoredPosition.y);


        AIBotItemController controller = aiBotHelper.GetComponent<AIBotItemController> ();
        controller.setYDir (player.getYDir ());

    }
}
