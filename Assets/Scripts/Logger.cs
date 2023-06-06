using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

public class Logger : MonoBehaviour
{
    private float startTime;
    private RealtimeAvatarManager realtimeAvatarManager;

    private bool shouldLog = false;

    // Start is called before the first frame update
    void Start()
    {
        realtimeAvatarManager = GameObject.Find("Realtime + VR Player").GetComponent<RealtimeAvatarManager>();
        startTime = Time.time;
        string filePath = "DataLog.csv";
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine("Time,PlayerPositionX,PlayerPositionY,PlayerPositionZ");
        writer.Close();
    }

    public void StartLogging()
    {
        shouldLog = true;
    }


    void Update()
    {
        if (!shouldLog) return;


        float currentTime = Time.time - startTime;
        Vector3 playerPosition = realtimeAvatarManager.avatars[0] ? realtimeAvatarManager.avatars[0].transform.position : new Vector3();

        Debug.Log(playerPosition);

        string filePath = "DataLog.csv";
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(currentTime + "," + playerPosition.x + "," + playerPosition.y + "," + playerPosition.z);
        writer.Close();
    }
}
