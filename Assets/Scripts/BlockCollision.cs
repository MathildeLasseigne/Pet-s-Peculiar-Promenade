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
	public List <GameObject> stickedBlocks = new List<GameObject> ();

	void Start(){
		stickedBlocks.Add(this.gameObject);
	}

    private void Update()
    {
		//only the highest parent can be manipulated thanks to the ObjectManipulator script,
		//it's disabled for its children
		if (transform.parent != null)
		{
			transform.parent.GetComponent<ObjectManipulator>().enabled = true;
			transform.parent.GetComponent<NearInteractionGrabbable>().enabled = true;

			transform.GetComponent<ObjectManipulator>().enabled = false;
			transform.GetComponent<NearInteractionGrabbable>().enabled = false;

		}

	}

    void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Block")){

			// make the object with the most children be parent of the other object
			/*if (gameObject.transform.childCount > other.gameObject.transform.childCount)
            {
                Transform lowestChild = gameObject.transform;
                while (lowestChild.childCount > 0)
                {
					lowestChild=lowestChild.GetChild(0);
				}
				other.gameObject.transform.parent = lowestChild;

            }
            else
            {
				Transform lowestChild = other.gameObject.transform;
				while (lowestChild.childCount > 0)
				{
					lowestChild = lowestChild.GetChild(0);
				}
				gameObject.transform.parent = lowestChild;

			}*/
			//Destroy(other.gameObject.GetComponent<Rigidbody>());
			
			
			//ignore the OnTriggerEnter event for the object immobile/slower than the other object
			if(GetComponent<Rigidbody>().velocity.magnitude < other.gameObject.GetComponent<Rigidbody>().velocity.magnitude)
			{
				return;
			}
			
			other.gameObject.transform.parent = gameObject.transform;
			other.gameObject.GetComponent<ObjectManipulator>().enabled = false;

			Debug.Log("blocks collided");
		}
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
