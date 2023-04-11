using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Playersboard : MonoBehaviour
{
    public TextMeshProUGUI otherPlayerName;

    public void  SetOtherPlayerName(string name)
    {
        otherPlayerName.text = name;
    }
}
