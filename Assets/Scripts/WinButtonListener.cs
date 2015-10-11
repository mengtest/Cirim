using UnityEngine;
using System.Collections;

public class WinButtonListener : MonoBehaviour
{
	private GameObject infoCarrier;

	void Start ()
	{
		infoCarrier = GameObject.Find ("InfoCarrier");
	}

	public void Next ()
	{
		if (infoCarrier != null) {
			LevelInfo ics = infoCarrier.GetComponent<LevelInfo> ();
			
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

	public void Replay ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void MainMenu ()
	{
		if (infoCarrier != null) {
			LevelInfo ics = infoCarrier.GetComponent<LevelInfo> ();
			
			ics.CurrentPack = 0;
			ics.CurrentLevel = 0;
			
			Application.LoadLevel ("MainMenu");
		}
	}
}
