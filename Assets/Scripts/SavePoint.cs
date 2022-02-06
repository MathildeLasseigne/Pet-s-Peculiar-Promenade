using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allow to register a save point to teleport the object back to, and allow external activation of savePoint
/// </summary>
public class SavePoint : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (checkY && gameObject.transform.position.y < posYMinBeforeReset)
        {
            goToSavePoint();
        }
        if (checkX && gameObject.transform.position.x < posXMinBeforeReset)
        {
            goToSavePoint();
        }
        if (checkZ && gameObject.transform.position.z < posZMinBeforeReset)
        {
            goToSavePoint();
        }
    }

    /// <summary>
    /// Set the position and rotation of the attached object to be the same as the save point
    /// </summary>
    public void goToSavePoint()
    {
        if (savePoint)
        {
            gameObject.transform.position = savePoint.position;
            gameObject.transform.rotation = savePoint.rotation;
        } else
        {
            gameObject.transform.position = originPosition;
            gameObject.transform.rotation = originRotation;
        }
        
    }
}
