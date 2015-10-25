using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public FloorCell RestartCell;
    private AIPath aiPath;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
    }

    public void KillPlayer()
    {
        aiPath.target = RestartCell.transform;
        transform.position = RestartCell.transform.position;
    }

    public void Unpause()
    {
        if (aiPath.target == null)
        {
            aiPath.target = TargetsManager.GetTarget();
        }
    }


}
