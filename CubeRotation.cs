using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CubeRotation : MonoBehaviour
{
    public Transform start;
    public Transform end;
    Vector3 rotationChange;
    // Start is called before the first frame update
    void Start()
    {
        double x1 = start.position.x;
        double y1 = start.position.y;
        double z1 = start.position.z;
        double x2 = end.position.x;
        double y2 = end.position.y;
        double z2 = end.position.z;
        float r = start.localScale.x;
        double dx = Math.Abs(x1 - x2);
        double dy = Math.Abs(y1 - y2);
        double dz = Math.Abs(z1 - z2);
        transform.localScale = new Vector3((float)Math.Pow(dz * dz + dy * dy + dz * dz, 0.5), r, r);
        transform.position = new Vector3((float)(x1 + x2) / 2f, (float)(y1 + y2) / 2f, (float)(z1 + z2) / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        double x1 = start.position.x;
        double y1 = start.position.y;
        double z1 = start.position.z;
        double x2 = end.position.x;
        double y2 = end.position.y;
        double z2 = end.position.z;
        float r = start.localScale.x;
        //Debug.Log(Tuple.Create(x1, y1, z1, x2, y2, z2));
        double dx = Math.Abs(x1 - x2);
        double dy = Math.Abs(y1 - y2);
        double dz = Math.Abs(z1 - z2);
        //float rx = (float)(Math.Atan2(dz, dy) * (180 / Math.PI));
        //float ry = (float)(Math.Atan2(dz, dx) * (180 / Math.PI));
        //float rz = (float)(Math.Atan2(dy, dx) * (180 / Math.PI));
        //Debug.Log(Tuple.Create(rx, ry, rz));
        //if (y2 < y1 ^ z2 < z1)
        //{
        //    rx = -rx; // rx
        //}
        //if (z2 < z1 ^ x2 < x1)
        //{
        //    ry = -ry; // -ry
        //}
        //if (y2 < y1 ^ x2 < x1)
        //{
        //    rz = -rz;
        //}
        float length = (float)Math.Pow(dx * dx + dy * dy + dz * dz, 0.5);
        transform.localScale = new Vector3(r, r, length);
        transform.position = new Vector3((float)(x1 + x2) / 2f, (float)(y1 + y2) / 2f, (float)(z1 + z2) / 2f);
        // transform.rotation = Quaternion.Euler(new Vector3(rx, ry, rz));

        // transform.eulerAngles = new Vector3(rx, ry, rz);

        // transform.Rotate(new Vector3(rx, ry, rz), Space.Self);

        // rotationChange += new Vector3(rx, ry, rz);
        // transform.eulerAngles = rotationChange;

        Vector3 relativePos = end.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        if (relativePos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }
        Debug.Log("done");
        

        
    }
}
