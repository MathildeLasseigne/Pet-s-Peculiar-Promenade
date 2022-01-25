using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI; //to locate ObjectManipulator script
using Microsoft.MixedReality.Toolkit.Input; //to locate NearInteractionGrabbable script

/*using System.Collections;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]*/
public class BlockCollision : MonoBehaviour
{
	//public List <GameObject> stickedBlocks = new List<GameObject> ();
	
	//public for debug purpose in the inspector
	public GameObject CollidingGameObject;
	public bool enableManipulator=false;

	void Start(){
		//stickedBlocks.Add(this.gameObject);
	}
	
	private Vector3 previous;
	public Vector3 velocity;

    private void Update()
    {
		velocity = (transform.position - previous) / Time.deltaTime;
		previous = transform.position;
		
		//only the highest parent can be manipulated thanks to the ObjectManipulator script,
		//it's disabled for its children
		if (transform.parent != null && !enableManipulator)
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
			if(velocity.magnitude < other.gameObject.GetComponent<BlockCollision>().velocity.magnitude)
			{
				return;
			}
			
			//other.gameObject.transform.parent = gameObject.transform;
			//other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
			
			CollidingGameObject=other.gameObject;
			enableManipulator = true;
			Debug.Log("blocks collided");
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Block") && other.gameObject==CollidingGameObject){
			CollidingGameObject=null;
			enableManipulator = false;
		}
	}
	
	public void mergeBlocks(){

		if(CollidingGameObject==null)
		{
			return;
		}
			
		//CollidingGameObject.transform.parent = gameObject.transform;
		//CollidingGameObject.GetComponent<ObjectManipulator>().enabled = false;
		
		//the highest parent of this block (root) becomes the parent
		//of the highest parent of the colliding block (CollidingGameObject...root)
		//CollidingGameObject.transform.root.parent = gameObject.transform.root;
		
		CollidingGameObject.transform.parent = gameObject.transform;
		Destroy(CollidingGameObject.GetComponent<Rigidbody>());
		
		//CollidingGameObject.GetComponent<ObjectManipulator>().enabled = false;
		enableManipulator = false;
		Debug.Log("blocks merged");
	}
	
	
	
	
	
	
	
	

	/*void CombineMesh()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false);
        transform.gameObject.SetActive(true);
    }*/

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
