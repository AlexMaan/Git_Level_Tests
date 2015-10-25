using UnityEngine;
using System.Collections;

public class EnemyAlarm : MonoBehaviour
{

    private Enemy enemyParent;

    void Start()
    {
        //enemyParent = transform.parent.GetComponent<Enemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            //enemyParent.SeePlayer();
            //other.gameObject.GetComponent<Player>().KillPlayer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            //Debug.Log("EXIT");
            //enemyParent.LosePlayer();
        }
    }
}
