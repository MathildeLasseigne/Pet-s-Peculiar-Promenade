using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer != LayerMask.NameToLayer("UI")){
			other.gameObject.transform.parent = gameObject.transform;
			//CombineMesh();
			
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
