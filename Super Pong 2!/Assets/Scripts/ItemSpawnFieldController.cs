using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnFieldController : MonoBehaviour {

    public GameObject [] items;

    public float itemSpawnTime = 5.0f;
    float itemSpawnCounter = 0;

    BoxCollider2D bounds;

    public Transform parentTransform;

    void Start(){
        bounds = GetComponent<BoxCollider2D> ();
    }

	
	// Update is called once per frame
	void Update () {
        itemSpawnCounter += Time.deltaTime;
        if (itemSpawnCounter >= itemSpawnTime) {
            itemSpawnCounter = 0;
            spawnItem ();
        }
	}

    void spawnItem (){

        int itemIndex = Random.Range (0, items.Length);
        GameObject newItem = Instantiate (items [itemIndex] ) as GameObject;

        RectTransform newItemRect = newItem.GetComponent<RectTransform> ();

        float randomLocationX = Random.Range (-bounds.size.x / 2 + newItemRect.sizeDelta.x/2, bounds.size.x / 2 - newItemRect.sizeDelta.x/2);
        float randomLocationY = Random.Range (-bounds.size.y / 2 + newItemRect.sizeDelta.y/2, bounds.size.y / 2 - newItemRect.sizeDelta.y/2);//

        newItem.transform.SetParent (parentTransform, false);
        newItemRect.anchoredPosition = new Vector2 (randomLocationX, randomLocationY);
    }
}
