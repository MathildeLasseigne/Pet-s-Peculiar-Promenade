using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public GameObject CubePrefab;
	public GameObject TrampolinePrefab;
	public GameObject TrapPrefab;

    // Start is called before the first             frame update
    void Start()
    {
        
    }
	
	//called on manipulation ended
	public void RecreateBlockInMenu(ItemType itemType){
		//create new block in menu
		Transform menuTransform=GetComponent<Transform>();
		GameObject newMenuItem;
		switch (itemType)
		{
			case ItemType.Cube:
				newMenuItem = Instantiate(CubePrefab, menuTransform);
				Debug.Log("cube created");
				break;
				
			case ItemType.Trampoline:
				//newMenuItem = Instantiate(TrampolinePrefab, menuTransform.position, menuTransform.rotation, menuTransform);
				newMenuItem = Instantiate(TrampolinePrefab, menuTransform);
				Debug.Log("trampoline created");
				break;
				
			case ItemType.Trap:
				newMenuItem = Instantiate(TrapPrefab, menuTransform);
				Debug.Log("trap created");
				break;

			default:
				newMenuItem = Instantiate(CubePrefab, menuTransform);
				break;
		}
		//GameObject newMenuItem = Instantiate(CubePrefab, menuTransform);
		//GameObject newMenuItem = Instantiate(this.gameObject, menuTransform.position, menuTransform.rotation, menuTransform);
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
