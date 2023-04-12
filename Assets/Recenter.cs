using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recenter : MonoBehaviour
{
    public GameObject headPlaceHolder;
    public GameObject leftHandPlaceHolder;
    public GameObject rightHandPlaceHolder;

    // Start is called before the first frame update
    public void HidePlaceHolders()
    {
        headPlaceHolder.SetActive(false);
        leftHandPlaceHolder.SetActive(false);
        rightHandPlaceHolder.SetActive(false);
    }

    public void ShowPlaceHolders()
    {
        headPlaceHolder.SetActive(true);
        leftHandPlaceHolder.SetActive(true);
        rightHandPlaceHolder.SetActive(true);
    }
}
