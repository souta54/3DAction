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
    [SerializeField] float lookSpeedX = 120f;
    [Header("カメラ　ーー　視点の上下の速さ")]
    [SerializeField] float lookSpeedY = 90f;
    [Header("カメラ　ーー　視点の上下の下限")]
    [SerializeField] float pitchMin = 10f;
    [Header("カメラ　ーー　視点の上下の上限")]
    [SerializeField] float pitchMax = 70f;
    [Header("カメラ　ーー　位置の追従の滑らかさ")]
    [SerializeField] float positionSmoothTime = 10f;
}
