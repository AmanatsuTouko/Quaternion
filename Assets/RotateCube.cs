using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public GameObject unityObj;
    public GameObject originalObj;
    
    public Quaternion unityQuaternion;
    public Quaternion originalQuaternion;

    public GameObject targetObj;

    [Header("Test")]
    [SerializeField] TestPattern testPattern = TestPattern.LOOK_ROTATION;
    public enum TestPattern
    {
        ANGLE_AXIS,

        FROM_ROTATION,
        LOOK_ROTATION,

        LERP,
        LERP_UNCLAMPED,
        SLERP,
        SLERP_UNCLAMPED,

        ROTATE_TOWARDS,        
    }

    [Header("AngleAxis")]
    [Range(-360, 360)]
    public float angle = 0;
    public Vector3 axis = Vector3.zero;

    [Header("FromDirection and ToDirection")]
    public Vector3 fromDirection;
    public Vector3 toDirection;

    [Header("Lerp / Slerp")]
    [Range(0, 1)]
    public float t = 0;
    [Range(-10, 10)]
    public float t_unclamped = 0;
    public float fromAngle = 0;
    public Vector3 fromAxis = Vector3.zero;
    public float toAngle = 0;
    public Vector3 toAxis = Vector3.zero;

    void Start()
    {
        fromAngle =  30; fromAxis = new Vector3(0, 1, 0);
        toAngle   = 270; toAxis   = new Vector3(0, 1, 0);
    }

    void Update()
    {
        switch (testPattern)
        {
            case TestPattern.ANGLE_AXIS:
                // Inspector の AngleAxisの項目を変更してテストする
                {
                    unityQuaternion = Quaternion.AngleAxis(angle, axis);
                    unityObj.transform.rotation = unityQuaternion;
                    originalQuaternion = FThingSoftware.Quaternion.AngleAxis(angle, axis);
                    originalObj.transform.rotation = originalQuaternion;
                }
                break;

            case TestPattern.LOOK_ROTATION:
                {
                    Vector3 lookDirection = targetObj.transform.position - this.transform.position;
                    unityQuaternion = Quaternion.LookRotation(lookDirection.normalized);
                    unityObj.transform.rotation = unityQuaternion;

                    originalQuaternion = FThingSoftware.Quaternion.LookRotation(lookDirection);
                    originalObj.transform.rotation = originalQuaternion;
                }
                break;

            case TestPattern.LERP:
                // Inspector の Lerp/Slerp の項目を変更してテストする
                {
                    Quaternion fromRotation_1 = Quaternion.AngleAxis(fromAngle, fromAxis);
                    Quaternion toRotation_1 = Quaternion.AngleAxis(toAngle, toAxis);
                    Quaternion lerp_1 = Quaternion.Lerp(fromRotation_1, toRotation_1, t);
                    unityObj.transform.rotation = lerp_1;

                    FThingSoftware.Quaternion fromRotation_2 = FThingSoftware.Quaternion.AngleAxis(fromAngle, fromAxis);
                    FThingSoftware.Quaternion toRotation_2 = FThingSoftware.Quaternion.AngleAxis(toAngle, toAxis);
                    FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.Lerp(fromRotation_2, toRotation_2, t);
                    originalObj.transform.rotation = lerp_2;
                }
                break;

            case TestPattern.LERP_UNCLAMPED:
                // Inspector の Lerp/Slerp の項目を変更してテストする
                {
                    Quaternion fromRotation_1 = Quaternion.AngleAxis(fromAngle, fromAxis);
                    Quaternion toRotation_1 = Quaternion.AngleAxis(toAngle, toAxis);
                    Quaternion lerp_1 = Quaternion.LerpUnclamped(fromRotation_1, toRotation_1, t_unclamped);
                    unityObj.transform.rotation = lerp_1;

                    FThingSoftware.Quaternion fromRotation_2 = FThingSoftware.Quaternion.AngleAxis(fromAngle, fromAxis);
                    FThingSoftware.Quaternion toRotation_2 = FThingSoftware.Quaternion.AngleAxis(toAngle, toAxis);
                    FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.LerpUnclamped(fromRotation_2, toRotation_2, t_unclamped);
                    originalObj.transform.rotation = lerp_2;
                }
                break;

            case TestPattern.SLERP:
                // Inspector の Lerp/Slerp の項目を変更してテストする
                {
                    Quaternion fromRotation_1 = Quaternion.AngleAxis(fromAngle, fromAxis);
                    Quaternion toRotation_1 = Quaternion.AngleAxis(toAngle, toAxis);
                    Quaternion lerp_1 = Quaternion.Slerp(fromRotation_1, toRotation_1, t);
                    unityObj.transform.rotation = lerp_1;

                    FThingSoftware.Quaternion fromRotation_2 = FThingSoftware.Quaternion.AngleAxis(fromAngle, fromAxis);
                    FThingSoftware.Quaternion toRotation_2 = FThingSoftware.Quaternion.AngleAxis(toAngle, toAxis);
                    FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.Slerp(fromRotation_2, toRotation_2, t);
                    originalObj.transform.rotation = lerp_2;

                    Debug.Log(lerp_1);
                    Debug.Log(lerp_2);
                }
                break;

            case TestPattern.SLERP_UNCLAMPED:
                // Inspector の Lerp/Slerp の項目を変更してテストする
                {
                    Quaternion fromRotation_1 = Quaternion.AngleAxis(fromAngle, fromAxis);
                    Quaternion toRotation_1 = Quaternion.AngleAxis(toAngle, toAxis);
                    Quaternion lerp_1 = Quaternion.SlerpUnclamped(fromRotation_1, toRotation_1, t_unclamped);
                    unityObj.transform.rotation = lerp_1;

                    FThingSoftware.Quaternion fromRotation_2 = FThingSoftware.Quaternion.AngleAxis(fromAngle, fromAxis);
                    FThingSoftware.Quaternion toRotation_2 = FThingSoftware.Quaternion.AngleAxis(toAngle, toAxis);
                    FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.SlerpUnclamped(fromRotation_2, toRotation_2, t_unclamped);
                    originalObj.transform.rotation = lerp_2;

                    Debug.Log(lerp_1);
                    Debug.Log(lerp_2);
                }
                break;

            case TestPattern.ROTATE_TOWARDS:
                {
                    var step = 1.0f * Time.deltaTime;

                    Quaternion toRotation_1 = Quaternion.AngleAxis(toAngle, toAxis);
                    Quaternion lerp_1 = Quaternion.RotateTowards(unityObj.transform.rotation, toRotation_1, step);
                    unityObj.transform.rotation = lerp_1;

                    FThingSoftware.Quaternion fromRotation_2 = new FThingSoftware.Quaternion(
                            originalObj.transform.rotation.x,
                            originalObj.transform.rotation.y,
                            originalObj.transform.rotation.z,
                            originalObj.transform.rotation.w
                        );
                    FThingSoftware.Quaternion toRotation_2 = FThingSoftware.Quaternion.AngleAxis(toAngle, toAxis);
                    FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.RotateTowards(fromRotation_2, toRotation_2, step);
                    originalObj.transform.rotation = lerp_2;
                }
                break;            
        }
    }
}
