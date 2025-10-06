using UnityEngine;

public class CameraRig : MonoBehaviour
{

    [SerializeField] float mHeightOffset = 0.5f;
    [SerializeField] float mFollowLerpRate = 20f;
    [SerializeField] float mRotationRate;
    [SerializeField] float mPitchmin = -89f;
    [SerializeField] float mPitchMax = 89f;

    

    [SerializeField] Transform mYawTransform;
    [SerializeField] Transform mPitchTransform;
    Transform mFollowTrasform;
    Vector2 mLookInput;
    private float mPitch;
    public void SetLookInput(Vector2 lookInput)
    {
        mLookInput = lookInput;
    }
    public void SetFollowTransform(Transform followTransform)
    {
        mFollowTrasform = followTransform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, mFollowTrasform.position + mHeightOffset * Vector3.up, mFollowLerpRate * Time.deltaTime);
        mYawTransform.rotation *= Quaternion.AngleAxis(mLookInput.x * mRotationRate * Time.deltaTime, Vector3.up);
        // gives a quaternion to rotate towards this current transform
        //Debug.Log($"Pitch is {newPitch}");
        mPitch = mPitch + mRotationRate * Time.deltaTime * mLookInput.y;
        mPitch = Mathf.Clamp(mPitch, mPitchmin, mPitchMax);
        mPitchTransform.localEulerAngles = new Vector3(mPitch, 0f, 0f);

    }
}
