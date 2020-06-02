using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour {

    [SerializeField] GameObject [] screenShots;
    [SerializeField] Text pageNumText;

    int pageNum = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void updatePageNum (){
        pageNumText.text = pageNum + "/" + screenShots.Length;
    }

    public void nextPage (){
        if (pageNum == screenShots.Length) {
            return;
        }

        screenShots [pageNum - 1].SetActive (false);
        pageNum++;
        screenShots[pageNum -1].SetActive(true);
        updatePageNum ();
    }
    public void previousPage(){
        if (pageNum == 1) {
            return;
        }

        screenShots [pageNum - 1].SetActive (false);
        pageNum--;
        screenShots[pageNum -1].SetActive(true);
        updatePageNum ();
    }


}
