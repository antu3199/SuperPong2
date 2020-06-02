using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameMode {
    SINGLE_PLAYER,
    MULTI_PLAYER,
    CLASSIC,
    OTHER
};

/*
 * TODO:
 * 
 * Recommended AW
 * Add the three items
 * Add computer opponent
 * 
 * 
 * Recommended AH
 * Add mobile input support
 * Create screenshots & decorate the Main menu screen
 * 
 * */


//Player information that transfers across rounds
public class PlayerInfo {
    public int score = 0;
    public bool hasUltimate = true;
};

public class GameModel : MonoBehaviour, IGameManager {

	public int winCondition = 7;

	public GameController controller;

    public GameMode mode = GameMode.OTHER;

    public PlayerInfo player1Info = new PlayerInfo();
    public PlayerInfo player2Info = new PlayerInfo();

    public bool hasWonRound = false;
    public bool hasWon = false;


	Rect topLeft;
	Rect bottomLeft;
	Rect topRight;
	Rect bottomRight;


	public TouchPhase topLeftTouch = TouchPhase.Canceled;
	public TouchPhase bottomLeftTouch = TouchPhase.Canceled;

	public TouchPhase topRightTouch = TouchPhase.Canceled;
	public TouchPhase bottomRightTouch = TouchPhase.Canceled;

    public ComputerDifficulty computerDiffiulty = ComputerDifficulty.NONE;


    public bool isUltimateActive = false;

	public void Startup(){
        resetGame ();
		topLeft = new Rect (0, Screen.height / 2, Screen.width / 2.0f, Screen.height / 2.0f);
		bottomLeft = new Rect (0, 0, Screen.width / 2.0f, Screen.height / 2.0f);
		topRight = new Rect (Screen.width / 2.0f, Screen.height / 2, Screen.width / 2.0f, Screen.height / 2.0f);
		bottomRight = new Rect (Screen.width / 2.0f, 0, Screen.width / 2.0f, Screen.height / 2.0f);
	}

    public void resetGame(){
        player1Info = new PlayerInfo ();
        player2Info = new PlayerInfo ();
        hasWon = false;
        hasWonRound = false;
        isUltimateActive = false;
    }

    public void addScore (PlayerControlScheme controlScheme){
        getPlayerInfo (controlScheme).score++;
        updateScoreText ();
        StartCoroutine (waitAndCheckWin (controlScheme));
    }


	private void updateScoreText(){
		controller.updateScoreText ();
	}

	public void setController (GameController control) {
		controller = control;
	}

    private void checkWin(PlayerControlScheme controlScheme){
        if (getPlayerInfo (controlScheme).score >= winCondition) {
            controller.showWinText (controlScheme);
            StartCoroutine (backToMainMenu ());
        } 
	}

	public void resetRound (){
        hasWonRound = false;
        isUltimateActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

    private IEnumerator waitAndCheckWin(PlayerControlScheme controlScheme){
        hasWonRound = true;
		//TODO: Play cheering sound effect
        checkWin (controlScheme);

		yield return new WaitForSeconds (1f);
        if (getPlayerInfo (controlScheme).score < winCondition) {
			resetRound ();
		}

	}

    private IEnumerator backToMainMenu (){
        hasWon = true;
        yield return new WaitForSeconds (2);
        hasWonRound = false;
        resetGame ();
        SceneManager.LoadScene ("MainMenu");
    }

    public PlayerInfo getPlayerInfo( PlayerControlScheme controlScheme){
        if (controlScheme == PlayerControlScheme.MULTIPLAYER_1 || controlScheme == PlayerControlScheme.SINGLE_PLAYER) {
            return player1Info;
        } else {
            return player2Info;
        }

    }

	public void getTouches (){
		topLeftTouch = TouchPhase.Canceled;
		topRightTouch = TouchPhase.Canceled;
		bottomLeftTouch = TouchPhase.Canceled;//
		bottomRightTouch = TouchPhase.Canceled;
		if (Input.touchCount > 0) {
			

			for (int i = 0; i < Input.touchCount; i++) {
				Touch touch = Input.GetTouch (i);

				if (topLeft.Contains (touch.position)) {
					topLeftTouch = touch.phase;
				} else if (bottomLeft.Contains (touch.position)) {
					bottomLeftTouch = touch.phase;
				} else if (topRight.Contains (touch.position)){
					topRightTouch = touch.phase;
				} else if (bottomRight.Contains(touch.position)){
					bottomRightTouch = touch.phase;
				}
			}
		}
	}

	void Update(){
		getTouches ();
	}


}
