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

    [Header("Angle and Axis")]
    public float angle = 0;
    public Vector3 axis = Vector3.zero;

    [Header("FromDirection and ToDirection")]
    public Vector3 fromDirection;
    public Vector3 toDirection;

    void Start()
    {

    }

    void Update()
    {
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
        unityQuaternion = Quaternion.Euler(20, 45, 175);
        originalQuaternion = FThingSoftware.Quaternion.Euler(20, 45, 175);
        Debug.Log(Quaternion.Inverse(unityQuaternion));
        Debug.Log(FThingSoftware.Quaternion.Inverse(originalQuaternion));
    }
}
