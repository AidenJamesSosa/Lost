using UnityEngine;
using System;
using UnityEditor.Rendering.LookDev;
using Unity.XR.OpenVR;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour, IViewClient
{
    [SerializeField] CameraRig mCameraRigPrefab;
    [SerializeField] GameplayWidget mGameplayWidgetPrefab;
    GameplayWidget mGameplayWidget;
    private PlayerInputAction mPlayerInputAction;

    private MovementController mMovementController;
    private BattlePartyComponent mBattlePartyCompontent;
    private BattleState mBattleState;
    CameraRig mCameraRig;
    void Awake()
    {
        mCameraRig = Instantiate(mCameraRigPrefab);
        mCameraRig.SetFollowTransform(transform);

        mMovementController = GetComponent<MovementController>();

        mPlayerInputAction = new PlayerInputAction();
        mPlayerInputAction.Gameplay.Jump.performed += mMovementController.PerformJump;

        mPlayerInputAction.Gameplay.Move.performed += mMovementController.HandledMoveInput;
        mPlayerInputAction.Gameplay.Move.canceled += mMovementController.HandledMoveInput;

        mPlayerInputAction.Gameplay.Look.performed += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());
        mPlayerInputAction.Gameplay.Look.canceled += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());

        mBattlePartyCompontent = GetComponent<BattlePartyComponent>();

        mGameplayWidget = Instantiate(mGameplayWidgetPrefab);

    }
    // void HandleLookInput(PlayerInputAction.CallBackContext context)
    // {
    //mCameraRig.SetLookInput(CameraRig.Something)
    // }
    void OnEnable()
    {
        mPlayerInputAction.Enable();
    }

    void OnDisable()
    {
        mPlayerInputAction.Disable();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject)
        {
            return;
        }

        BattlePartyComponent otherBattlePartyComponent = other.GetComponent<BattlePartyComponent>();
        if (otherBattlePartyComponent && !IsInBattle())
        {
            GameMode.MainGameMode.BattleManager.StartBattle(mBattlePartyCompontent, otherBattlePartyComponent);
            SwitchToBattleState(BattleState.InBattle);
        }
    }
    private void SwitchToBattleState(BattleState battleState)
    {
        if (battleState == BattleState.InBattle)
        {
            mPlayerInputAction.Gameplay.Disable();
        }
        if (battleState == BattleState.Roaming)
        {
            mPlayerInputAction.Gameplay.Enable();
        }
        mGameplayWidget.DipToBlack(1, 1, DippedToBlack);
    }
    void DippedToBlack()
    {
        Debug.Log($"Dipped To Black Called");
    }
    private bool IsInBattle()
    {
        return mBattleState == BattleState.InBattle;
    }
    public void SetViewTarget(Transform viewTarget)
    {
        mCameraRig.SetFollowTransform(viewTarget);
        mCameraRig.transform.rotation = viewTarget.transform.rotation;
    }
    public void ResetViewAngle()
    {
        mCameraRig.ResetViewAngle();
    }
}
