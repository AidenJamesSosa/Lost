using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    GameObject mTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject Target
    {
        get { return mTarget; }
        set
        {   if (Target == value)
            {
                    return;
            }
            if (value == null)
                {
                    mBehaviorGraphAgent.BlackboardReference.SetVariableValue("HasLastSeenLocation", true);
                    mBehaviorGraphAgent.BlackboardReference.SetVariableValue("TargetLastSeenLocation", mTarget.transform.position);
                }
            mTarget = value;
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue("Target", mTarget);
        }

    }
    [SerializeField] float mEyeHeight = 5f;
    [SerializeField] float mSightDistance = 1.5f;
    [SerializeField] float mViewAngle =30f;
    [SerializeField] float mAlwaysAwareDistance =1.5f;

    BehaviorGraphAgent mBehaviorGraphAgent;
   void Awake()
    {
        mBehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePerception();
    }
    private void UpdatePerception()
    {
       
        Player player = GameMode.MainGameMode.mPlayer;
        if (!player)
        {
             Debug.Log("Player not existant");
            Target = null;
            return;
        }
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= mAlwaysAwareDistance)
        {
            Target = player.gameObject;
            return;
        }
        //if(distanceToPlayer)
            if (Vector3.Distance(player.transform.position, transform.position) > mSightDistance)
            {
                Debug.Log("Player is is out of range");
                Target = null;
                return;
            }
        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(playerDir, transform.forward) > mViewAngle)
        {
             Debug.Log("Player out of angle");
            Target = null;
            return;
        }
        

        Vector3 eyeViewpoint = transform.position + Vector3.up * mEyeHeight;
        if (Physics.Raycast(eyeViewpoint, playerDir, out RaycastHit hitInfo, mSightDistance))
        {
            if (hitInfo.collider.gameObject != player.gameObject)
            {
                 Debug.Log("Player is blocked behind");
                Target = null;
                return;
            }
        }
        Debug.Log("Player is seen");
        Target = player.gameObject;
    }
    void OnDrawGizmos()
    {
        Vector3 eyeViewpoint = transform.position + Vector3.up * mEyeHeight;
        Gizmos.DrawWireSphere(eyeViewpoint, mSightDistance);
        Gizmos.DrawWireSphere(eyeViewpoint, mAlwaysAwareDistance);

        Vector3 leftLineDir = Quaternion.AngleAxis(mViewAngle, Vector3.up) * transform.forward;
        Vector3 rightLineDir = Quaternion.AngleAxis(-mViewAngle, Vector3.up) * transform.forward;
        if (Target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Target.transform.position);
            Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        }

       // Gizmos.DrawLine(eyeViewPoint, eyeViewpoint + leftLineDir * mSightDistance);
        //Gizmos.DrawLine(eyeViewPoint, eyeViewpoint + rightLineDir * mSightDistance);
    }
}
