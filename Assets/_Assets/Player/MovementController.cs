using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]


public class MovementController : MonoBehaviour
{
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] float mMoveSpeed = 10f;
    [SerializeField] float mMaxMoveSpeed = 5f;
    [SerializeField] float mGroundSpeedAcceleration = 5f;
    [SerializeField] float mAirSpeedAcceleration = 5f;
    [SerializeField] float maxfallspeed = 50f;
    [SerializeField] float LerpRate = 1f;
    [SerializeField] float mAirCheckRadius = 0.5f;
    [SerializeField] LayerMask mAirCheckLayerMask = 1;
    private Animator mAnimator;
    private bool mShouldTryJump;
    private bool mInAir;

    bool wasGrounded;

    private CharacterController mCharacterController;
    private Vector3 mHorizontalVelocity;
    private Vector3 mVerticalVelocity;


    private Vector2 mMoveInput;

    void Awake()
    {
        mAnimator = GetComponent <Animator>();
        mCharacterController = GetComponent<CharacterController>();

    }
    public void HandledMoveInput(InputAction.CallbackContext context)
    {
        mMoveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move input is: {mMoveInput}");
    }
    public void PerformJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumping");
        if (mCharacterController.isGrounded)
        {
            if (!mInAir)
            {
                mShouldTryJump = true;
            }
        }

    }
    bool IsInAir()
    {
        if (mCharacterController.isGrounded)
        {
            return false;
        }
        Collider[] airCheckColliders = Physics.OverlapSphere(transform.position, mAirCheckRadius, mAirCheckLayerMask);
        foreach (Collider collider in airCheckColliders)
        {
            if(collider.gameObject != gameObject)
             return false;
        }
        return true;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mInAir = IsInAir();
        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();
        UpdateTransform();
        UpdateAnimation();
        
    }
    private void UpdateAnimation()
    {
        mAnimator.SetFloat("Speed", mHorizontalVelocity.magnitude);
        mAnimator.SetBool("Landed", !mInAir);
    }
    private void UpdateTransform()
    {
        mCharacterController.Move((mHorizontalVelocity + mVerticalVelocity) * Time.deltaTime);
        if (mHorizontalVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mHorizontalVelocity.normalized, Vector3.up), Time.deltaTime * LerpRate);
        }
        //mAnimator.SetFloat("Speed" Something);
        //if (wasGrounded && mCharacterController.isGrounded)
        //{
        //if (mVerticalVelocity.y <= maxfallspeed)
        //{
        ///mVerticalVelocity.y = 0f;
        //}

        //}
        //wasGrounded = mCharacterController.isGrounded;
    }

    private void UpdateVerticalVelocity()
    {
        if (mShouldTryJump && !mInAir)
        {
            mVerticalVelocity.y = mJumpSpeed;
            mAnimator.SetTrigger("Jump");
            mShouldTryJump = false;
            return;
        }
        if (mCharacterController.isGrounded)
        {
            mAnimator.ResetTrigger("Jump");
            mVerticalVelocity.y = -1f;
            return;
        }

        if (mVerticalVelocity.y > -maxfallspeed)
        {
            mVerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

    }
    void UpdateHorizontalVelocity()
    {
        Vector3 moveDir = PlayerInputToWorldDir(mMoveInput);
        float acceleration = mCharacterController.isGrounded ? mGroundSpeedAcceleration : mAirSpeedAcceleration;
        //Debug.Log($"move dir is: {moveDir} acceleration is: {acceleration}");
        if (moveDir.sqrMagnitude > 0)
        {
            mHorizontalVelocity += moveDir * acceleration * Time.deltaTime;
            mHorizontalVelocity = Vector3.ClampMagnitude(mHorizontalVelocity, mMaxMoveSpeed);
        }
        else
        {
            if (mHorizontalVelocity.sqrMagnitude > 0)
            {
                mHorizontalVelocity -= mHorizontalVelocity.normalized * acceleration * Time.deltaTime;
                if (mHorizontalVelocity.sqrMagnitude < 0.1f)
                {
                    mHorizontalVelocity = Vector3.zero;
                }
            }
        }


    }

    Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = Camera.main.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);

        return rightDir * inputVal.x + fwdDir * inputVal.y;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = mInAir ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, mAirCheckRadius);
    }



}
