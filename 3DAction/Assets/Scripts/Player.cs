using UnityEngine;

using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [Header("転がり　ーー　力の強さ")]
    [SerializeField] float moveForce = 12f;
    [Header("転がり　ーー　速さの上限（抜け防止にも効く）")]
    [SerializeField] float maxSpeed = 10f;
    [Header("ジャンプ　ーー　一瞬の力（使わないなら0）")]
    [SerializeField] float jumpImpulse = 5f;
    [Header("接地判定　ーー　足元からの距離（ボール半径よりすこしだけ大きい）")]
    [SerializeField] float groundCheckDistance = 0.6f;
    [Header("接地判定　ーー　接地判定用のレイヤー")]
    [SerializeField] LayerMask groundLayer;
    [Header("カメラ（力を加える前方向の準備。未設定なら Main Camera）")]
    [SerializeField] Transform cameraTransform;

    Rigidbody rigidBody;
    InputAction moveAction;
    InputAction jumpAction;
    Vector2 moveInput;
    bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        if (jumpAction != null)
        {
            jumpAction.Enable();
        }
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        UpdateGrounded();
        TryJump();
    }
    void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 forceDir = forward * moveInput.y + right * moveInput.x;
        if(forceDir.sqrMagnitude > 1f)
        {
            forceDir.Normalize();
        }

        rigidBody.AddForce(forceDir * moveForce, ForceMode.Force);

        Vector3 velocity = rigidBody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if(horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rigidBody.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);

        }

    }

    void UpdateGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }

    void TryJump()
    {
        if (jumpAction == null || jumpImpulse <= 0f)
        {
            return;
        }
        if (!jumpAction.WasPerformedThisFrame())
        {
            return;
        }

        if (!isGrounded)
        {
            return;
        }
        rigidBody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 origin = transform.position + Vector3.up * 0.05f;
        Gizmos.DrawLine(origin, origin + Vector3.down * groundCheckDistance);
    }
}
