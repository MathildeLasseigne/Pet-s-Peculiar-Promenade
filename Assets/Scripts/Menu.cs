using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public GameObject MenuBlockPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	//called on manipulation ended
	public void RecreateBlockInMenu(){
		//create new block in menu
		Transform menuTransform=GetComponent<Transform>();
		GameObject newMenuBlock = Instantiate(MenuBlockPrefab, menuTransform);
		//GameObject newMenuBlock = Instantiate(this.gameObject, menuTransform.position, menuTransform.rotation, menuTransform);
		//destroy this script
	}
	
	public void setMenu(Menu menu){
	
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnManipulationEnded(){
		
	}
}
