using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
	// public for debug purpose
	public GameObject CollidingGameObject;

    void Start()
    {
        
    }
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Block")){
			CollidingGameObject=other.gameObject;
			Debug.Log("block collided with bin");
			//other.gameObject.transform.parent = gameObject.transform;
			//other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Block") && other.gameObject==CollidingGameObject){
			CollidingGameObject=null;
		}
	}
	
	public void collidedWithBin(){
		if(CollidingGameObject){
			Destroy(CollidingGameObject);
			CollidingGameObject=null;
			Debug.Log("block deleted in bin");
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
