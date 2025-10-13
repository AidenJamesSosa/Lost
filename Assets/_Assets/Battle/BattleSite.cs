using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class BattleSite : MonoBehaviour
{
    [SerializeField] float mSiteRadius;
    [SerializeField, Range(0, 5)] int mSiteCapacity;
    public bool IsPlayerSite = false;
    //public bool IsPlayerSite => mIsPlayerSite;
    public Vector3 GetPositionForUnit(int index)
    {
        if (mSiteCapacity <= 1)
        {
            return transform.position;
        }
        float gap = mSiteRadius * 2 / (mSiteCapacity - 1);
        Vector3 startingPoint = transform.position - transform.right * mSiteRadius;

        return startingPoint + index * gap * transform.right;
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < mSiteCapacity; i++)
        {
            Gizmos.DrawSphere(GetPositionForUnit(i), 0.5f);
        }
    }

}
