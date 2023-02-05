using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;
using System;

public static class Extns
{
    public static IEnumerator Tweeng(this float duration,
               System.Action<float> var, float aa, float zz)
    {
        float sT = Time.time;
        float eT = sT + duration;

        while (Time.time < eT)
        {
            float t = (Time.time - sT) / duration;
            var(Mathf.SmoothStep(aa, zz, t));
            yield return null;
        }

        var(zz);
    }

    public static IEnumerator Tweeng(this float duration,
               System.Action<Vector3> var, Vector3 aa, Vector3 zz)
    {
        float sT = Time.time;
        float eT = sT + duration;

        while (Time.time < eT)
        {
            float t = (Time.time - sT) / duration;
            var(Vector3.Lerp(aa, zz, Mathf.SmoothStep(0f, 1f, t)));
            yield return null;
        }

        var(zz);
    }
}

public class code : MonoBehaviour
{
    Thread mThread;
    public GameObject[] points = null;
    int[,] CoordinatesForPoint = new int[21, 3];
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    public float speed = 300;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos = Vector3.zero;
    bool running;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        points = GameObject.FindGameObjectsWithTag("points");

        for (int i = 0; i < 21; i++)
        {
            Vector3 prevPosition = points[i].transform.position;
            Vector3 targetPosition = new Vector3(CoordinatesForPoint[i, 0], CoordinatesForPoint[i, 1], CoordinatesForPoint[i, 2]);
            double x1 = prevPosition.x;
            double y1 = prevPosition.y;
            double z1 = prevPosition.z;
            double x2 = targetPosition.x;
            double y2 = targetPosition.y;
            double z2 = targetPosition.z;
            double dx = Math.Abs(x1 - x2);
            double dy = Math.Abs(y1 - y2);
            double dz = Math.Abs(z1 - z2);
            float length = (float)Math.Pow(dx * dx + dy * dy + dz * dz, 0.5);
            speed = length / 10f;
            points[i].transform.position = Vector3.MoveTowards(prevPosition, targetPosition, speed); // * Time.deltaTime;
            // StartCoroutine(DoMove(points[i], Time.deltaTime, prevPosition, new Vector3(10, 10, 10)));
            // points[i].transform.Translate(targetPosition - prevPosition, Space.World);
            // points[i].transform.position = Vector3.Lerp(prevPosition, targetPosition, Time.deltaTime);
        }
        Debug.Log("ESHKERE!!!");
    }

    private IEnumerator DoMove(GameObject point, float duration, Vector3 startPosition, Vector3 targetPosition)
    {
        float timeElapsed = 0f;
        while (timeElapsed <= duration)
        {
            point.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        point.transform.position = targetPosition;
        Debug.Log("moved point to");
        Debug.Log(targetPosition);
    }

    void GetInfo()
    {
        Debug.Log("GetInfo");
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }
    void SendAndReceiveData()
    {
        Debug.Log("SendAndRecieveData");
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];


        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Debug.Log(dataReceived);
        if (dataReceived != null)
        {
            string[] coords = dataReceived.Split(' ');


            int PointCounter = 0;

            for (int i = 0; i < coords.Length; i += 3)
            {
                CoordinatesForPoint[PointCounter, 0] = int.Parse(coords[i]);
                CoordinatesForPoint[PointCounter, 1] = int.Parse(coords[i + 1]);
                CoordinatesForPoint[PointCounter, 2] = int.Parse(coords[i + 2]);

                PointCounter += 1;

            }
            Debug.Log("kjdshi");


        }
    }
}