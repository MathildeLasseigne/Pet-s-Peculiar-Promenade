using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
	private Menu menu;
	public GameObject TrampolinePrefab;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Pet")){
			other.GetComponent<AnimalMoveManager>().Jump();
		}
	}
	
	//called on manipulation started
	/*public void RemoveFromMenu(){
		//remove the trampoline's parent
		this.menu = gameObject.transform.parent.GetComponent<Menu>();
		gameObject.transform.parent = null;
		
		GetComponent<Transform>().scale*=2;
	}*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
