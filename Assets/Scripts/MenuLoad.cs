﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuLoad : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
		GameObject infoCarrier = GameObject.Find("InfoCarrier");

		if (infoCarrier != null)
		{
			transform.GetChild(0).GetChild(0).GetComponent<Text>().text = infoCarrier.GetComponent<InfoCarrier>().CompletedLevels.ToString();
			transform.GetChild(1).GetChild(0).GetComponent<Text>().text = infoCarrier.GetComponent<InfoCarrier>().CompletedPacks.ToString();
		}
	}
}