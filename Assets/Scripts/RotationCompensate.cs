using UnityEngine;
using System.Collections;

public class RotationCompensate : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float ry = -transform.parent.rotation.eulerAngles.y;

	    transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
	        ry,
	        transform.rotation.eulerAngles.z);
	}
}
