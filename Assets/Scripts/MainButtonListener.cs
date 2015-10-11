using UnityEngine;
using System.Collections;

public class MainButtonListener : MonoBehaviour {
	
	public void PlayNow()
	{
		GameObject infoCarrier = GameObject.Find("InfoCarrier");
		if (infoCarrier != null)
		{
			infoCarrier.GetComponent<InfoCarrierScript>().CurrentLevel = 0;
			Application.LoadLevel("Level00");
		}
	}

	public void Levels()
	{

	}

	public void Options()
	{

	}

	public void Help()
	{
		
	}
}
