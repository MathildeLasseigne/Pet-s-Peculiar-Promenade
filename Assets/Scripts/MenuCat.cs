using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCat : MonoBehaviour
{
	//text saying to move the cat box
	public GameObject tutorialLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	//called on manipulated started
	public void RemoveFromMenu(){
		gameObject.transform.parent = null;
		Destroy(tutorialLabel);
		//destroy this script
		Destroy(GetComponent<MenuCat>());
	}
}
