using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CameraRig mCameraRigPrefab;

    private PlayerInputAction mPlayerInputAction;

    private MovementController mMovementController;
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
}
