using UnityEngine;
using System.Collections;

public class LevelButtonListener : MonoBehaviour
{
	void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				Application.LoadLevel ("MainMenu");
			}
		}
	}

	public void PreviousButton ()
	{
		GameObject infoCarrier = GameObject.Find ("InfoCarrier");

		if (infoCarrier != null) {
			InfoCarrierScript ics = infoCarrier.GetComponent<InfoCarrierScript> ();
			
			if (ics.CurrentLevel - 1 == -1) {
				if (ics.CurrentPack % 10 == 0) {
					if (ics.CurrentPack / 10 != 0) {
						ics.CurrentPack -= 10;
						ics.CurrentPack += ics.LastPack;
					}
				} else {
					ics.CurrentPack--;
				}
			} else {
				ics.CurrentLevel--;
			}

			Application.LoadLevel (string.Format ("Level{0}{1}", ics.Rings - 2, ics.Segments - 4));
		}
	}

	public void NextButton ()
	{
		GameObject infoCarrier = GameObject.Find ("InfoCarrier");

		if (infoCarrier != null) {
			InfoCarrierScript ics = infoCarrier.GetComponent<InfoCarrierScript> ();

			if (ics.CurrentLevel + 1 == ics.TotalLevels) {
				if (ics.CurrentPack % 10 == ics.LastPack) {
					ics.CurrentPack = (ics.CurrentPack / 10) * 10 + 10;
				} else {
					ics.CurrentPack++;
				}
			} else {
				ics.CurrentLevel++;
			}

			Application.LoadLevel (string.Format ("Level{0}{1}", ics.Rings - 2, ics.Segments - 4));
		}
	}

	public void RestartButton ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}