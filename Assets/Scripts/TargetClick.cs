using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TargetClick : MonoBehaviour 
{
    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Time.timeScale == 1f)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<AIPath>().target = transform;
                GetComponent<FloorCell>().isPlayerTarget = true;
            }
            else
            {
                TargetsManager.AddTarget(transform);
            }
            
        }
        
    }
}
