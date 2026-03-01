using JetBrains.Annotations;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Rotation : MonoBehaviour
{
    public GameObject objectToWatch;
    public float speed;
    public TextMeshProUGUI statusText;
    public Vector3 direction = new Vector3();
    public float stop1;
    public float stop2;



    void FixedUpdate()
    {
        float angle = objectToWatch.transform.eulerAngles.z;

        if (objectToWatch == null || statusText == null) return;

        transform.Rotate(speed * direction * Time.deltaTime);

        if(angle > stop1)
        {
            direction.z = -1 ;
        }
        if (angle < stop2)
        {
            direction.z = 1;
        }

    }  

}

    
