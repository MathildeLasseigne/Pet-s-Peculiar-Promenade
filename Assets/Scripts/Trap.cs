using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	/*[SerializeField]
	Vector3 v3Force;*/
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	/*
	void FixedUpdate()
	{
		GetComponent<Rigidbody>().velocity+= v3Force;
	}*/
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Pet")){
			GetComponentInChildren<Animator>().Play("close");
			//GetComponentInChildren<Animator>().SetTrigger("Trap");
			Debug.Log("trap moved");
		}
	}
	/*
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Pet")){
			transform.parent.GetComponentInChildren<Animator>().Play("movingBridge");
			//GetComponentInChildren<Animator>().SetTrigger("Trap");
			Debug.Log("collision trap lifted");
		}
	}*/
}
