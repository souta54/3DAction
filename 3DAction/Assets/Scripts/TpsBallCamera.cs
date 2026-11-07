using UnityEngine;

using UnityEngine.InputSystem;
public class TpsBallCamera : MonoBehaviour
{
    [Header("追う対象（ボール）")]
    [SerializeField] Transform target;
    [Header("カメラ　ーー　後ろへの距離")]
    [SerializeField] float distance = 8f;
    [Header("カメラ　ーー　高さ")]
    [SerializeField] float height = 5f;
    [Header("カメラ　ーー　視点の左右の速さ")]
    [SerializeField] float lookSensitivityX = 120f;
    [Header("カメラ　ーー　視点の上下の速さ")]
    [SerializeField] float lookSensitivityY = 90f;
    [Header("カメラ　ーー　視点の上下の下限")]
    [SerializeField] float pitchMin = 10f;
    [Header("カメラ　ーー　視点の上下の上限")]
    [SerializeField] float pitchMax = 70f;
    [Header("カメラ　ーー　位置の追従の滑らかさ")]
    [SerializeField] float positionSmooth = 10f;

    InputAction lookAction;
    float yaw;
    float pitch = 25f;

    void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Player/Look");
    }

    void OnEnable()
    {
     if(lookAction != null)
        {
            lookAction.Enable();
        }   
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        UpdateLookAngles();

        Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = orbit * new Vector3(0f, 0f, -distance) + Vector3.up * height;
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
           transform.position,
           desiredPosition,
           1f - Mathf.Exp(-positionSmooth * Time.deltaTime));

        transform.LookAt(target.position, Vector3.up);
    }
    void UpdateLookAngles()
    {
        if(lookAction == null)
        {
            return;
        }

        Vector2 look = lookAction.ReadValue<Vector2>();
        yaw += look.x * lookSensitivityX * Time.deltaTime;
        pitch -= look.y * lookSensitivityY * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }
}
