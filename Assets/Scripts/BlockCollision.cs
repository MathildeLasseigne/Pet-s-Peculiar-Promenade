using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI; //to locate ObjectManipulator script
using Microsoft.MixedReality.Toolkit.Input; //to locate NearInteractionGrabbable script

public class BlockCollision : MonoBehaviour
{
	
	//public for debug purpose in the inspector
	public GameObject CollidingGameObject;
	//public bool enableManipulator=false;
	public bool isCollidingBin;
	
	//getting speed without using rigid body
	private Vector3 previous;
	public Vector3 velocity;

	void Start(){
		//stickedBlocks.Add(this.gameObject);
	}

    private void Update()
    {
		//update speed
		velocity = (transform.position - previous) / Time.deltaTime;
		previous = transform.position;
		
		//only the highest *Block* parent can be manipulated thanks to the ObjectManipulator script,
		//it's disabled for its children
		if (transform.parent != null && transform.parent.gameObject.layer == LayerMask.NameToLayer("Block"))
		{
			transform.GetComponent<ObjectManipulator>().enabled = false;
			transform.GetComponent<NearInteractionGrabbable>().enabled = false;

		} else{
			transform.GetComponent<ObjectManipulator>().enabled = true;
			transform.GetComponent<NearInteractionGrabbable>().enabled = true;
		}

	}
	
	//old code
	/*void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Block")){
            //ignore the OnTriggerEnter event for the object immobile/slower than the other object
            //if(GetComponent<Rigidbody>().velocity.magnitude < other.gameObject.GetComponent<Rigidbody>().velocity.magnitude)
            if(velocity.magnitude < other.gameObject.GetComponent<BlockCollision>().velocity.magnitude)
            {
                return;
            }
            
            other.gameObject.transform.parent = gameObject.transform;
            other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
			Destroy(other.gameObject.GetComponent<Rigidbody>());
        }
    }*/

    void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Block")){

			//ignore the OnTriggerEnter event for the object immobile/slower than the other object
			if(other.gameObject.GetComponent<BlockCollision>()!=null &&
			velocity.magnitude < other.gameObject.GetComponent<BlockCollision>().velocity.magnitude)
			{
				return;
			}
			CollidingGameObject=other.gameObject;
			Debug.Log("blocks collided");
			
			//other.gameObject.transform.parent = gameObject.transform;
			//other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
			//enableManipulator = true;

		} else if (other.gameObject.layer == LayerMask.NameToLayer("Bin")){
			isCollidingBin=true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Block") && other.gameObject==CollidingGameObject){
			CollidingGameObject=null;
			//enableManipulator = false;
		} else if (other.gameObject.layer == LayerMask.NameToLayer("Bin")){
			isCollidingBin=false;
		}
	}
	
	public void mergeBlocks(){

		if(CollidingGameObject==null)
		{
			return;
		} /*else if(GetComponent<MenuBlock>().itemType!=CollidingGameObject.GetComponent<MenuBlock>().itemType){
			Debug.Log("different types, not merged");
			return;
		}*/
		
		// merge blocks between their highest parent
		while(CollidingGameObject.transform.parent!=null
		&& CollidingGameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Block")){
			Destroy(CollidingGameObject.GetComponent<Rigidbody>());
			CollidingGameObject=CollidingGameObject.transform.parent.gameObject;
		}
		CollidingGameObject.transform.parent = gameObject.transform;
		Destroy(CollidingGameObject.GetComponent<Rigidbody>());
		Debug.Log("blocks merged");
		
		//CollidingGameObject.GetComponent<ObjectManipulator>().enabled = false;
		//enableManipulator = false;
		//the highest parent of this block (root) becomes the parent
		//of the highest parent of the colliding block (CollidingGameObject...root)
		//CollidingGameObject.transform.root.parent = gameObject.transform.root;

	}
	
	// called on manipulation ended, checks if object was released in the bin
	public void deleteOrNotInBin(){
		//checks if this object or its children are colliding bin
		if (isCollidingBin || ChildIsCollidingBin(this.gameObject)){
			Destroy(this.gameObject);
			Debug.Log("object deleted in bin");
		}
			/*while(transform!=null && transform.parent!=null
		&& transform.parent.gameObject.layer == LayerMask.NameToLayer("Block")){
				CollidingGameObject=CollidingGameObject.transform.parent.gameObject;
				Destroy(CollidingGameObject);
				Debug.Log("object deleted in bin");
			}*/
	}
	
	// recursively checks if any child block is colliding bin
	private bool ChildIsCollidingBin(GameObject obj)
    {
        if (obj == null)
            return false;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            if (child.GetComponent<BlockCollision>()) {
                if(child.GetComponent<BlockCollision>().isCollidingBin){
					return true;
				}
            }
            return (false || ChildIsCollidingBin(child.gameObject));
        }
		return false;
    }
	
	
	
	
	
	


	/*protected Transform stuckTo = null;
	protected Vector3 offset = Vector3.zero;
	protected Rigidbody rb;
	
	private bool stuckLock = true;

	public void LateUpdate()
	{
		if(stuckLock){
			if (stuckTo != null)
				transform.position = stuckTo.position - offset;

		}
		
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log("collision");
		rb = GetComponent<Rigidbody>();
		rb.isKinematic = true;

		if(stuckTo == null 
			|| stuckTo != col.gameObject.transform){
			offset = col.gameObject.transform.position - transform.position;

		stuckTo = col.gameObject.transform;
			}

	}
	
	///Lock the moving of the object
	public void LockStuck(bool on){
		Debug.Log("Stuck :"+on);
		this.stuckLock = on;
	}*/
}
