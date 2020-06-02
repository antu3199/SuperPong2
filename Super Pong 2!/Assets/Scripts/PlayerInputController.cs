using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerControlScheme {
	SINGLE_PLAYER,
	MULTIPLAYER_1,
	MULTIPLAYER_2
};


public enum ControllerState{
	NONE,
	LEFT,
	RIGHT,
	BOTH
}

public enum ComputerState {
    NOT_COMPUTER,
    NEUTRAL,
    DO_BREAK,
    DO_ULTIMATE,
}

public class PlayerInputController : MonoBehaviour {


    public Color color;

	[SerializeField] private PlayerControlScheme playerControlScheme;

	[SerializeField] private float moveSpeed;

    public float stunDuration = 1;

	public RingAnimator ringAnim;

	public PlayerUltimateController ultimateController;
	public float ultimateChargeTime = 2.0f;
	public float breakChargeTime = 0.5f;

    public bool canDoActions = true;

	public KeyCode ultimateKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    RectTransform rectTransform;

  
    float playerWidth;

    Image sprite;
    float ultimateChargeCounter = 0.0f;
    float stunCounter = 0;

    public bool hasBreak = true;
    public bool canMove = true;
    private bool forceKeyUp = false;




	bool startCharge = false;

	public float bothTouchThresh = 0.1f;
	private float bothThreshCounter = 0;
	bool touchedLeft = false;
	bool touchedRight = false;
	bool bothTouched = false;

    public RectTransform AIBotLocation;


    public ComputerState computerState = ComputerState.NOT_COMPUTER;


	// Use this for initialization
	void Awake () {


	}

	void Start() {


		//left = new Rect (0, 0, Screen.width / 2, Screen.height);
	//	right = new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height);
		rectTransform = GetComponent<RectTransform> ();
		sprite = GetComponent<Image> ();
		sprite.color = color;
		canDoActions = true;
		playerWidth = GetComponent<RectTransform> ().sizeDelta.x;
	}

	public TouchPhase quadrantPressed (Rect rect){
		if (Input.touchCount == 0) {
			return  TouchPhase.Canceled;
		}
	
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch (i);
		
			if (rect.Contains(touch.position) ) {
				return touch.phase;
			}
		}

		//return Input.GetTouch (0).phase;
		return TouchPhase.Canceled;
	}


	private TouchPhase getLeftTouch (){
        if (computerState != ComputerState.NOT_COMPUTER) {
            return TouchPhase.Canceled;
        }

		if (playerControlScheme == PlayerControlScheme.MULTIPLAYER_1) {
			return GameManagers.gameModel.bottomLeftTouch;
		} else {
			return GameManagers.gameModel.topLeftTouch;
		}
	}

	private TouchPhase getRightTouch(){
        if (computerState != ComputerState.NOT_COMPUTER) {
            return TouchPhase.Canceled;
        }

		if (playerControlScheme == PlayerControlScheme.MULTIPLAYER_1) {
			return GameManagers.gameModel.bottomRightTouch;
		} else {
			return GameManagers.gameModel.topRightTouch;
		}
	}

	private TouchPhase isBothTouch(){
        if (computerState != ComputerState.NOT_COMPUTER) {
            return TouchPhase.Canceled;
        }

		return  (getLeftTouch () != TouchPhase.Canceled && getRightTouch () != TouchPhase.Canceled) ? getLeftTouch() : TouchPhase.Canceled;
	}



	// Update is called once per frame
	void Update () {


        if (canMove) {
	
            if (getKeyBoardKey(leftKey) ||  getLeftTouch() != TouchPhase.Canceled  ) {
                moveLeft ();
            }
            if (getKeyBoardKey (rightKey) || getRightTouch() != TouchPhase.Canceled) {
                moveRight ();
            }

        } else {
            stunCounter += Time.deltaTime;
            if (stunCounter >= stunDuration) {
                stunCounter = 0;
                canMove = true;
                sprite.color = color;
            }
        }
	    

		if (getLeftTouch () == TouchPhase.Began) {
			bothTouched = bothThreshCounter > 0;
			bothThreshCounter = bothTouchThresh;
		}
		if (getRightTouch () == TouchPhase.Began) {
			bothTouched = bothThreshCounter > 0;
			bothThreshCounter = bothTouchThresh;
		}

		if (bothThreshCounter >= 0) {
			bothThreshCounter -= Time.deltaTime;
		}
	
		if (canDoActions && forceKeyUp == false) {
            if (bothTouched || getKeyBoardKeyDown (ultimateKey) || computerState == ComputerState.DO_BREAK || computerState == ComputerState.DO_ULTIMATE ) {
                if (GameManagers.gameModel.getPlayerInfo (playerControlScheme).hasUltimate || hasBreak) {
                    GameManagers.gameModel.controller.setBackgroundColor (Color.gray);
                    if (GameManagers.gameModel.getPlayerInfo (playerControlScheme).hasUltimate == true) {
                        ultimateController.playBase ();
                    }
                    Time.timeScale = 0.5f;
                }
				startCharge = true;
			}
			if (startCharge) {

                if (  (getLeftTouch() == TouchPhase.Stationary && getRightTouch() == TouchPhase.Stationary) || getKeyBoardKey (ultimateKey) || computerState == ComputerState.DO_ULTIMATE) {
					ultimateChargeCounter += Time.deltaTime;
                   
				}

                if ((getLeftTouch() == TouchPhase.Ended || getRightTouch() == TouchPhase.Ended ) || getKeyBoardKeyUp (ultimateKey) || computerState == ComputerState.DO_BREAK) {
                if (ultimateChargeCounter <= breakChargeTime && hasBreak == true) {
                    hasBreak = false;
                    ringAnim.playAnimation (playerControlScheme);
                    resetComputerToNeutral ();
                    
                }
                resetBreakCharge ();
                } else if ( (ultimateChargeCounter > breakChargeTime && GameManagers.gameModel.getPlayerInfo (playerControlScheme).hasUltimate == false) || (ultimateChargeCounter > breakChargeTime && GameManagers.gameModel.isUltimateActive == true)) {
                resetBreakCharge ();
                forceKeyUp = true;
            }

                if (ultimateChargeCounter >= ultimateChargeTime && GameManagers.gameModel.getPlayerInfo(playerControlScheme).hasUltimate == true && GameManagers.gameModel.isUltimateActive == false) {
                    GameManagers.gameModel.getPlayerInfo(playerControlScheme).hasUltimate = false;
                    GameManagers.gameModel.isUltimateActive = true;
					canDoActions = false;
					startCharge = false;
					forceKeyUp = true;
                    Time.timeScale = 1.0f;
					ultimateController.playBeam (getYDir(), ()=> { 
					  canDoActions = true;
					  ultimateChargeCounter = 0;
                      GameManagers.gameModel.controller.setBackgroundColor (Color.white);
                        GameManagers.gameModel.isUltimateActive = false;
				    });
                    resetComputerToNeutral ();

				}
			}
				
		}

        if ( (getLeftTouch() == TouchPhase.Ended || getRightTouch() == TouchPhase.Ended )  || getKeyBoardKeyUp (ultimateKey)) {
			forceKeyUp = false;
			ultimateChargeCounter = 0;
		}

	

	}



    private void resetBreakCharge(){
        ultimateChargeCounter = 0;
        ultimateController.stopBase ();
        GameManagers.gameModel.controller.setBackgroundColor (Color.white);
		startCharge = false;
        Time.timeScale = 1.0f;
		bothTouched = false;
    }

	private void moveLeft (){
		float nextPos = rectTransform.anchoredPosition.x -  (moveSpeed * Time.deltaTime);
		nextPos = Mathf.Clamp (nextPos, -GameManagers.consts.screenSize.x/2 + playerWidth/2,  GameManagers.consts.screenSize.x/2 - playerWidth/2);
        rectTransform.anchoredPosition = new Vector2 (nextPos, rectTransform.anchoredPosition.y);	
	}

	private void moveRight(){
		float nextPos = rectTransform.anchoredPosition.x +  (moveSpeed * Time.deltaTime);
		nextPos = Mathf.Clamp (nextPos, -GameManagers.consts.screenSize.x/2 + playerWidth/2,  GameManagers.consts.screenSize.x/2 - playerWidth/2);
        rectTransform.anchoredPosition = new Vector2 (nextPos, rectTransform.anchoredPosition.y);   
	}

	void charge(){

	}

    void OnTriggerEnter2D (Collider2D other){
        if (other.tag == "Bullet" && other.transform.parent.GetComponent<BulletMovement>().yDir != getYDir()) {
            canMove = false;
            stunCounter = 0;
            sprite.color = color * new Color (0.4f, 0.4f, 0.4f);

        }

    }

	public void setCanDoActions(bool val){
		canDoActions = val;
	}

	public float getYDir (){
		if (playerControlScheme == PlayerControlScheme.MULTIPLAYER_1 || playerControlScheme == PlayerControlScheme.SINGLE_PLAYER) {
			return 1;
		} else {
			return -1;
		}
	}



    bool getKeyBoardKey (KeyCode key){
        if (computerState != ComputerState.NOT_COMPUTER) {
            return false;
        }
        return Input.GetKey (key);

    }

    bool getKeyBoardKeyDown (KeyCode key){
        if (computerState != ComputerState.NOT_COMPUTER) {
            return false;
        }
        return Input.GetKeyDown (key);

    }

    bool getKeyBoardKeyUp (KeyCode key){
        if (computerState != ComputerState.NOT_COMPUTER) {
            return false;
        }
        return Input.GetKeyUp (key);
    }



    void resetComputerToNeutral(){
        if (computerState != ComputerState.NOT_COMPUTER) {
            computerState = ComputerState.NEUTRAL;
        }
    }

}


