using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public int AlarmRadius;
    public float AlarmCount;
    public float AlarmRiseSpeed;
    public float AlarmFadeSpeed;

    [Serializable]
    public struct EnemyWayPoint
    {
        public FloorCell WayPoint;
        public float WaitTime;
    }

    public EnemyWayPoint[] WayPoints;

    private int targetWayPointIndex;
    private AIPath aiPath;

    private bool targetReallyReached = false;
    
    private GameObject alarmRadiusGO;
    private GameObject alarmBar;
    private float alarmAmount;
    private bool seePlayer;

    private Transform playerPos;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        targetWayPointIndex = 0;
        aiPath.speed = Speed;
        alarmRadiusGO = transform.FindChild("AlarmRad").gameObject;
        alarmRadiusGO.transform.localScale *= AlarmRadius*2;
        alarmBar = transform.Find("alarm_timer/alarm bar").gameObject;
        SetNewTarget();
        seePlayer = false;

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (aiPath.TargetReached && !targetReallyReached)
        {
            targetReallyReached = true;
            
            int ind = ((targetWayPointIndex - 1) + WayPoints.Length) % WayPoints.Length;
            Invoke("SetNewTarget", WayPoints[ind].WaitTime);
        }
        else if (!aiPath.TargetReached)
        {
            targetReallyReached = false;
        }

        float d = Vector3.Distance(transform.position, playerPos.position);

        if (d < AlarmRadius)
        {
            seePlayer = true;
        }
        else
        {
            seePlayer = false;
        }

        if (seePlayer)
        {
            
            alarmAmount += AlarmRiseSpeed*Time.deltaTime;
            if (alarmAmount >= AlarmCount)
            {
                playerPos.GetComponent<Player>().KillPlayer();
            }
        }
        else
        {
            alarmAmount -= AlarmFadeSpeed * Time.deltaTime;
        }
        alarmAmount = Mathf.Clamp(alarmAmount, 0f, AlarmCount);
        Vector3 v = Vector3.one;
        v.y = alarmAmount/AlarmCount;
        alarmBar.transform.localScale = v;
    }

    void SetNewTarget()
    {
        aiPath.target = WayPoints[targetWayPointIndex].WayPoint.transform;
        targetWayPointIndex++;
        targetWayPointIndex = targetWayPointIndex % WayPoints.Length;
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        for (int i = 0; i < WayPoints.Length; i++)
        {
            Gizmos.DrawSphere(WayPoints[i].WayPoint.transform.position, 0.2f);
            int to = (i + 1)%WayPoints.Length;
            Gizmos.DrawLine(WayPoints[i].WayPoint.transform.position, WayPoints[to].WayPoint.transform.position);
        }

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position // position
                                      , transform.up                       // normal
                                      , AlarmRadius);                              // radius
        
    }


}
