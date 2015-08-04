using UnityEngine;
using System.Collections;

public class TargetClick : MonoBehaviour 
{
    public void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<AIPath>().target = transform;
    }
}
