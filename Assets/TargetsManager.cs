using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetsManager : MonoBehaviour
{
    public static List<Transform> FutureTargets;

    public GameObject TargetMarker;

    private List<GameObject> markers; 
 
	// Use this for initialization
	void Start () 
    {
	    FutureTargets = new List<Transform>();
        markers = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    for (int i = 0; i < markers.Count; i++)
	    {
	        markers[i].SetActive(false);
	    }

	    for (int i = 0; i < FutureTargets.Count; i++)
	    {
	        if (i < markers.Count)
	        {
                markers[i].SetActive(true);
	            markers[i].transform.position = FutureTargets[i].position;
	        }
	        else
	        {
                GameObject go = Instantiate(TargetMarker, FutureTargets[i].position, Quaternion.identity) as GameObject;
                markers.Add(go);
	        }
	    }
	}

    public static void AddTarget(Transform target)
    {
        FutureTargets.Add(target);
    }

    public static Transform GetTarget()
    {
        if (FutureTargets.Count > 0)
        {
            Transform t = FutureTargets[0];
            FutureTargets.RemoveAt(0);
            t.GetComponent<FloorCell>().isPlayerTarget = true;
            return t;
        }
        
        return null;
    }
}
