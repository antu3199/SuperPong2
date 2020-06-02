
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


	[SerializeField] Text scoreText;
	public BallController ball;

    public PlayerInputController player1;
    public PlayerInputController player2;

	[SerializeField] Text player1WinText;
	[SerializeField] Text player2WinText;

    [SerializeField] Image [] background;

    public Transform gameHolder;


	void Awake() {
		GameManagers.gameModel.setController (this);
        updateScoreText ();
	}

	public void updateScoreText (){
        scoreText.text = GameManagers.gameModel.getPlayerInfo(PlayerControlScheme.MULTIPLAYER_1).score + ":" + GameManagers.gameModel.getPlayerInfo(PlayerControlScheme.MULTIPLAYER_2).score;
	}

    public void showWinText (PlayerControlScheme controlScheme){
        Time.timeScale = 1.0f;
		player1WinText.gameObject.SetActive (true);
		player2WinText.gameObject.SetActive (true);
        if (controlScheme == PlayerControlScheme.MULTIPLAYER_1 || controlScheme == PlayerControlScheme.SINGLE_PLAYER) {
			player1WinText.text = "WIN!";
			player2WinText.text = "LOSE";
		} else {
			player1WinText.text = "LOSE";
			player2WinText.text = "WIN!";
		}
	}

	public void resetRound(){
		
		player1WinText.gameObject.SetActive (false);
		player2WinText.gameObject.SetActive (false);
        ball.resetBall ();
	}

    public void setBackgroundColor(Color color){
        //StopAllCoroutines ();
        //StartCoroutine (crossFadeColor (color));
        Color originalColor = background [0].color;
        for (int i = 0; i < background.Length; i++){
            background [i].color = Color.Lerp(originalColor, color, 1.0f);
        }
    }

    IEnumerator crossFadeColor (Color color){
     
        Color originalColor = background [0].color;
        float lerp = 0.0f;

        while (lerp < 1.0f) {
            for (int i = 0; i < background.Length; i++){
                background [i].color = Color.Lerp(originalColor, color, lerp);
            }
            lerp += 100 * Time.deltaTime;
            yield return null;
        }

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        #if UNITY_EDITOR
        if (Input.GetKeyDown (KeyCode.Escape)) {
            GameManagers.gameModel.resetGame();
            SceneManager.LoadScene("MainMenu");
        }
        #endif
	}
}
