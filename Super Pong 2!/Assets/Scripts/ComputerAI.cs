using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComputerDifficulty {
    NONE,
    EASY,
    MEDIUM, 
    HARD
}


public class ComputerAI : MonoBehaviour {

    public ComputerDifficulty difficulty = ComputerDifficulty.MEDIUM;

    PlayerInputController controller;

    float moveSpeed = 0;

    RectTransform rect;

    public float easyAIBreakTimer = 2f;
    float easyAIBreakCounter = 0;

    public int normalAIBreakChance = 3;

    public float timeBeforeUltimate = 5.0f;
    float timeBeforeUltimateCounter = 0;
   
	bool canNormalAIBreak = false;

	// Use this for initialization
	void Start () {
        if ( GameManagers.gameModel.computerDiffiulty == ComputerDifficulty.NONE) {
            this.enabled = false;
            return;
        }
        easyAIBreakTimer += Random.Range (0, 10);
        controller = GetComponent<PlayerInputController> ();	
        rect = GetComponent<RectTransform> ();
        difficulty = GameManagers.gameModel.computerDiffiulty;
        setDifficulty (GameManagers.gameModel.computerDiffiulty);
        controller.computerState = ComputerState.NEUTRAL;
		int breakChance = Random.Range (0, normalAIBreakChance);
		if (breakChance == 0) {
			canNormalAIBreak = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (controller.canMove) {
            if (difficulty == ComputerDifficulty.EASY || difficulty == ComputerDifficulty.MEDIUM) {
                if (Mathf.Abs (rect.anchoredPosition.x - GameManagers.gameModel.controller.ball.rect.anchoredPosition.x) > 5) {
                    if (rect.anchoredPosition.x < GameManagers.gameModel.controller.ball.rect.anchoredPosition.x) {
                        rect.anchoredPosition += Vector2.right * moveSpeed * Time.deltaTime;
                    } else {
                        rect.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
                    }
                }
            } else if (difficulty == ComputerDifficulty.HARD) {
                rect.anchoredPosition = new Vector2(GameManagers.gameModel.controller.ball.rect.anchoredPosition.x, rect.anchoredPosition.y);
            }
        }

        if (controller.hasBreak == true) {
            if (difficulty == ComputerDifficulty.EASY) {
                easyAIBreakCounter += Time.deltaTime;
                if (easyAIBreakCounter > easyAIBreakTimer) {
                    easyAIBreakCounter = 0;
                    controller.computerState = ComputerState.DO_BREAK;
                }
            } else if (difficulty == ComputerDifficulty.MEDIUM) {

                float bound = GameManagers.consts.gameSize.y / 2 - GameManagers.gameModel.controller.ball.rect.sizeDelta.y / 2 - (GameManagers.gameModel.controller.ball.getYSpeed() * Time.deltaTime);
				if (canNormalAIBreak == true && GameManagers.gameModel.controller.ball.rect.anchoredPosition.y >= bound) {
                    controller.computerState = ComputerState.DO_BREAK;
                }

            } else {
                float bound = GameManagers.consts.gameSize.y / 2 - GameManagers.gameModel.controller.ball.rect.sizeDelta.y / 2 - (GameManagers.gameModel.controller.ball.getYSpeed() * Time.deltaTime);
                if (GameManagers.gameModel.controller.ball.rect.anchoredPosition.y >= bound) {
                    controller.computerState = ComputerState.DO_BREAK;
                }
            }
        }


        if (GameManagers.gameModel.getPlayerInfo (PlayerControlScheme.MULTIPLAYER_2).hasUltimate && difficulty != ComputerDifficulty.EASY) {
            if (GameManagers.gameModel.player1Info.score == GameManagers.gameModel.winCondition - 1 || GameManagers.gameModel.player2Info.score == GameManagers.gameModel.winCondition - 1) {
                timeBeforeUltimateCounter += Time.deltaTime;
                if (timeBeforeUltimateCounter >= timeBeforeUltimate) {
                    controller.computerState = ComputerState.DO_ULTIMATE;
                }
            }
        }
    }


    public void setDifficulty (ComputerDifficulty difficulty){
        switch (difficulty) {
            
        case ComputerDifficulty.EASY:
            moveSpeed = 150;
            break;
        case ComputerDifficulty.MEDIUM:
            moveSpeed = 350;
            break;
        case ComputerDifficulty.HARD:
            moveSpeed = 0;
            break;

        default:
            break;


        }

    }
}
