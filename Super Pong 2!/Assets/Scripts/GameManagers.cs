using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagers : MonoBehaviour {

	public static ConstsModel consts {get; private set;}
	public static GameModel gameModel {get; private set;}

	private List<IGameManager> _startSequence;

	public string firstSceneName;


	void Awake(){
        if (GameManagers.gameModel != null) { //For debugging purposes only
            this.gameObject.SetActive (false);
            return;
        }
        _startSequence = new List<IGameManager> ();

		consts = GetComponent<ConstsModel> ();
		gameModel = GetComponent<GameModel> ();

		_startSequence.Add (consts);
		_startSequence.Add (gameModel);
		StartCoroutine(StartupManagers());

		DontDestroyOnLoad (this);
	}

	private IEnumerator StartupManagers() {
		foreach (IGameManager manager in _startSequence) {
			manager.Startup();
			yield return null;
		}

		if (firstSceneName != "") {
			SceneManager.LoadScene (firstSceneName);
		}

	}


}
