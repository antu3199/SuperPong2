using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneChanger : MonoBehaviour {

    [SerializeField] string nextScene;
    [SerializeField] GameMode setMode;



    public void changeScene(){
        GameManagers.gameModel.mode = setMode;
        SceneManager.LoadScene (nextScene);
    }

	
}
