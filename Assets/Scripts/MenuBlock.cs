using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum ItemType { Cube, Trampoline, Trap, Other }
public class MenuBlock : MonoBehaviour
{
	private Menu menu;
	//public GameObject MenuItemPrefab;
	public ItemType itemType;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	//TO DO, remove this object from menu after manipulation and add new object to menu
	
	//called on manipulation started
	public void RemoveFromMenu(){
		//remove the block's parent
		this.menu = gameObject.transform.parent.GetComponent<Menu>();
		if(itemType==ItemType.Trap){ //menu is the parent of the parent of this script for the trap
			this.menu = gameObject.transform.parent.parent.GetComponent<Menu>();
			gameObject.transform.parent.parent = null;
			return;
		}
		gameObject.transform.parent = null;

	}

	
	//called on manipulation ended
	public void RecreateBlockInMenu(){
		if(menu){
		menu.RecreateBlockInMenu(itemType);
		} else {
			Debug.Log(menu);
		}
		
		Destroy(GetComponent<MenuBlock>());
		/*//create new block in menu
		Transform menuTransform=MenuContent.GetComponent<Transform>();
		GameObject newMenuBlock = Instantiate(MenuItemPrefab, menuTransform);
		newMenuBlock.GetComponent<MenuBlock>().MenuContent=MenuContent;
		//GameObject newMenuBlock = Instantiate(this.gameObject, menuTransform.position, menuTransform.rotation, menuTransform);
		//destroy this script
		Destroy(GetComponent<MenuBlock>());*/
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
