using UnityEngine;
using System.Collections;

public class PlayNowScript : MonoBehaviour
{

    public void OnClick ()
    {
        GameObject infoCarrier = GameObject.Find("InfoCarrier");
        if (infoCarrier != null)
        {
            infoCarrier.GetComponent<InfoCarrierScript>().CurrentLevel = 0;
            Application.LoadLevel("Level00");
        }
    }
}
