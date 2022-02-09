using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allow to register a save point to teleport the object back to, and allow external activation of savePoint
/// </summary>
public class SavePoint : MonoBehaviour
{
    private GameObject savedObject;

    [SerializeField]
    private Transform savePoint;

    private Vector3 originPosition;
    private Quaternion originRotation;


    
    [SerializeField]
    private float posXMinBeforeReset = -1000f;
    [SerializeField]
    private bool checkX = true;
    [SerializeField]
    private float posYMinBeforeReset = -1000f;
    [SerializeField]
    private bool checkY = false;
    [SerializeField]
    private float posZMinBeforeReset = -1000f;
    [SerializeField]
    private bool checkZ = false;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = gameObject.transform.position;
        originRotation = gameObject.transform.rotation;

        if(!savedObject)
        {
            savedObject = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update, position x : "+ savedObject.transform.position.x);
        //Debug.Log(savedObject.transform.position);
        //Debug.Log("save point : " + savePoint.position);
        if (checkY && savedObject.transform.position.y < posYMinBeforeReset)
        {
            goToSavePoint();
        }
        if (checkX && savedObject.transform.position.x < posXMinBeforeReset)
        {
            goToSavePoint();
        }
        if (checkZ && savedObject.transform.position.z < posZMinBeforeReset)
        {
            goToSavePoint();
        }
    }

    /// <summary>
    /// Set the position and rotation of the attached object to be the same as the save point
    /// </summary>
    public void goToSavePoint()
    {
        AnimalMoveManager moveManager = savedObject.GetComponent<AnimalMoveManager>();
        if (moveManager)
        {
            //Debug.Log("Got move manager");
            moveManager.Sit(true);

            moveManager.Cry();
        }
        CharacterController cc = savedObject.GetComponent<CharacterController>();
        if (cc)
        {
            cc.enabled = false;
        }
        Debug.Log("Save point loaded");
        if (savePoint)
        {
            savedObject.transform.position = savePoint.position;
            savedObject.transform.rotation = savePoint.rotation;
        } else
        {
            savedObject.transform.position = originPosition;
            savedObject.transform.rotation = originRotation;
        }

        
        if (moveManager)
        {
            moveManager.resetVelocity();
            moveManager.Meow();
        }
        if (cc)
        {
            cc.enabled = true;
        }

    }
}
