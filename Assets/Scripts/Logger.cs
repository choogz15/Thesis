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
        writer.WriteLine("Time," +
            "P0PX,P0PY,P0PZ,P0RX,P0RY,P0RZ,P0LPX,P0LPY,P0LPZ,P0LRX,P0LRY,P0LRZ,P0RPX,P0RPY,P0RPZ,P0RRX,P0RRY,P0RRZ," +
            "P1PX,P1PY,P1PZ,P1RX,P1RY,P1RZ,P1LPX,P1LPY,P1LPZ,P1LRX,P1LRY,P1LRZ,P1RPX,P1RPY,P1RPZ,P1RRX,P1RRY,P1RRZ," +
            "P2PX,P2PY,P2PZ,P2RX,P2RY,P2RZ,P2LPX,P2LPY,P2LPZ,P2LRX,P2LRY,P2LRZ,P2RPX,P2RPY,P2RPZ,P2RRX,P2RRY,P2RRZ,");
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

        string playerData = "";

        for(int i = 0; i < 3; i++)
        {
            Vector3 playerPosition = realtimeAvatarManager.avatars.ContainsKey(i) ? realtimeAvatarManager.avatars[i].head.transform.position : new Vector3();
            Vector3 playerRotation = realtimeAvatarManager.avatars.ContainsKey(i) ? realtimeAvatarManager.avatars[i].head.transform.rotation.eulerAngles : new Vector3();

            Vector3 playerLHandPosition = realtimeAvatarManager.avatars.ContainsKey(i) ? realtimeAvatarManager.avatars[i].leftHand.transform.position : new Vector3();
            Vector3 playerLHandRotation = realtimeAvatarManager.avatars.ContainsKey(i) ? realtimeAvatarManager.avatars[i].leftHand.transform.rotation.eulerAngles : new Vector3();

            Vector3 playerRHandPosition = realtimeAvatarManager.avatars.ContainsKey(i) ? realtimeAvatarManager.avatars[i].rightHand.transform.position : new Vector3();
            Vector3 playerRHandRotation = realtimeAvatarManager.avatars.ContainsKey(i) ? realtimeAvatarManager.avatars[i].rightHand.transform.rotation.eulerAngles : new Vector3();

            playerData = playerData + playerPosition.x + "," + playerPosition.y + "," + playerPosition.z + "," + playerRotation.x + "," + playerRotation.y + "," + playerRotation.z + ","
                + playerLHandPosition.x + "," + playerLHandPosition.y + "," + playerLHandPosition.z + "," + playerLHandRotation.x + "," + playerLHandRotation.y + "," + playerLHandRotation.z + ","
                + playerRHandPosition.x + "," + playerRHandPosition.y + "," + playerRHandPosition.z + "," + playerRHandRotation.x + "," + playerRHandRotation.y + "," + playerRHandRotation.z + ",";

        }

        string filePath = "DataLog.csv";
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(currentTime + "," + playerData);
        writer.Close();
    }
}
