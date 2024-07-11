using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public GameObject unityObj;
    public GameObject originalObj;

    public GameObject targetObj;

    public Quaternion unityQuaternion;
    public FThingSoftware.Quaternion originalQuaternion;

    [Header("Test")]
    [SerializeField] TestPattern testPattern = TestPattern.SLERP;
    public enum TestPattern
    {
        ANGLE_AXIS,
        LERP,
        SLERP
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
    public float fromAngle = 0;
    public Vector3 fromAxis = Vector3.zero;
    public float toAngle = 0;
    public Vector3 toAxis = Vector3.zero;

    void Start()
    {
        fromAngle = 30;
        fromAxis = new Vector3(0, 1, 0);
        //toAngle = 270;
        //toAxis = new Vector3(1, 0, 1);
        toAngle = 200;
        toAxis = new Vector3(0, 1, 0);

        FThingSoftware.Quaternion quaternion_1 = FThingSoftware.Quaternion.AngleAxis(270, new Vector3(1, 0, 1));
        FThingSoftware.Quaternion quaternion_2 = FThingSoftware.Quaternion.AngleAxis(270-360, new Vector3(1, 0, 1));

        Debug.Log(quaternion_1.ToString());
        Debug.Log(quaternion_2.ToString());

        Quaternion quaternion_3 = Quaternion.AngleAxis(270, new Vector3(1, 0, 1));
        Quaternion quaternion_4 = Quaternion.AngleAxis(270-360, new Vector3(1, 0, 1));

        Debug.Log(quaternion_3.ToString());
        Debug.Log(quaternion_4.ToString());
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

            case TestPattern.SLERP:
                // Inspector の Lerp/Slerp の項目を変更してテストする
                {
                    Quaternion fromRotation_1 = Quaternion.AngleAxis(fromAngle, fromAxis);
                    Quaternion toRotation_1 = Quaternion.AngleAxis(toAngle, toAxis);
                    Quaternion lerp_1 = Quaternion.Slerp(fromRotation_1, toRotation_1, t);
                    unityObj.transform.rotation = lerp_1;

                    FThingSoftware.Quaternion fromRotation_2 = FThingSoftware.Quaternion.AngleAxis(fromAngle, fromAxis);
                    FThingSoftware.Quaternion toRotation_2 = FThingSoftware.Quaternion.AngleAxis(toAngle, toAxis);
                    FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.Squad(fromRotation_2, toRotation_2, t);
                    originalObj.transform.rotation = lerp_2;
                }
                break;
        }

        // Test AngleAxis()
        //unityQuaternion = Quaternion.AngleAxis(angle, axis);
        //unityObj.transform.rotation = unityQuaternion;
        //originalQuaternion = FThingSoftware.Quaternion.AngleAxis(angle, axis);
        //originalObj.transform.rotation = originalQuaternion;


        // Test SetFromToRotation()
        // unityQuaternion.SetFromToRotation(fromDirection, toDirection);
        //originalQuaternion.SetFromToRotation(fromDirection, toDirection);


        // Test eulerAngle
        //unityQuaternion = Quaternion.AngleAxis(angle, axis);
        //unityObj.transform.rotation = unityQuaternion;
        //Debug.Log(unityQuaternion.eulerAngles);

        //originalQuaternion = FThingSoftware.Quaternion.AngleAxis(angle, axis);
        //originalObj.transform.rotation = originalQuaternion;
        //Debug.Log(originalQuaternion.eulerAngle);

        //unityQuaternion.eulerAngles = axis;
        //unityObj.transform.rotation = unityQuaternion;

        //originalQuaternion.eulerAngle = axis;
        //originalObj.transform.rotation = originalQuaternion;


        // Test LookRotation
        //Vector3 look = targetObj.transform.position - this.transform.position;
        //unityQuaternion = Quaternion.LookRotation(look);
        //unityObj.transform.rotation = unityQuaternion;

        //originalQuaternion = FThingSoftware.Quaternion.LookRotation(look);
        //originalObj.transform.rotation = originalQuaternion;


        // Test ToAngleAxis
        //Quaternion qua1 = Quaternion.AngleAxis(30, Vector3.up);
        //Quaternion qua2 = Quaternion.AngleAxis(45, Vector3.right);
        //Quaternion qua3 = Quaternion.AngleAxis(90, Vector3.forward);
        //unityQuaternion = qua3 * qua2 * qua1;

        //FThingSoftware.Quaternion qua_1 = FThingSoftware.Quaternion.AngleAxis(30, Vector3.up);
        //FThingSoftware.Quaternion qua_2 = FThingSoftware.Quaternion.AngleAxis(45, Vector3.right);
        //FThingSoftware.Quaternion qua_3 = FThingSoftware.Quaternion.AngleAxis(90, Vector3.forward);
        //originalQuaternion = qua_3 * qua_2 * qua_1;

        //float angle = 0;
        //Vector3 axis = Vector3.zero;

        //unityQuaternion.ToAngleAxis(out angle, out axis);
        //Debug.Log((angle, axis));
        //originalQuaternion.ToAngleAxis(out angle, out axis);
        //Debug.Log((angle, axis));


        // Test ToString
        //unityQuaternion = Quaternion.identity;
        //Debug.Log(unityQuaternion.ToString());

        //originalQuaternion = FThingSoftware.Quaternion.identity;
        //Debug.Log(originalQuaternion.ToString());


        // Test Angle
        //float angle_1 = Quaternion.Angle(Quaternion.AngleAxis(30, Vector3.up), Quaternion.AngleAxis(45, Vector3.up));
        //float angle_2 = FThingSoftware.Quaternion.Angle(
        //    FThingSoftware.Quaternion.AngleAxis(30, Vector3.up),
        //    FThingSoftware.Quaternion.AngleAxis(45, Vector3.up)
        //    );
        //Debug.Log((angle_1, angle_2));


        // Test Euler
        //unityQuaternion = Quaternion.Euler(20, 45, 175);
        //originalQuaternion = FThingSoftware.Quaternion.Euler(20, 45, 175);
        //Debug.Log((unityQuaternion.ToString(), originalQuaternion.ToString()));


        // Test Inverse
        //unityQuaternion = Quaternion.Euler(20, 45, 175);
        //originalQuaternion = FThingSoftware.Quaternion.Euler(20, 45, 175);
        //Debug.Log(Quaternion.Inverse(unityQuaternion));
        //Debug.Log(FThingSoftware.Quaternion.Inverse(originalQuaternion));


        
        // Test Slerp
        //Quaternion qua_1 = Quaternion.AngleAxis(30, Vector3.up);
        //Quaternion qua_2 = Quaternion.AngleAxis(180, Vector3.right);
        ////Quaternion qua_2 = Quaternion.AngleAxis(45, Vector3.right);
        //Quaternion lerp_1 = Quaternion.Slerp(qua_1, qua_2, 0.5f);

        //FThingSoftware.Quaternion qua_11 = FThingSoftware.Quaternion.AngleAxis(30, Vector3.up);
        //FThingSoftware.Quaternion qua_22 = FThingSoftware.Quaternion.AngleAxis(180, Vector3.right);
        ////FThingSoftware.Quaternion qua_22 = FThingSoftware.Quaternion.AngleAxis(45, Vector3.right);
        //FThingSoftware.Quaternion lerp_2 = FThingSoftware.Quaternion.Slerp(qua_11, qua_22, 0.5f);

        //Debug.Log(lerp_1.ToString());
        //Debug.Log(lerp_2.ToString());
    }
}
