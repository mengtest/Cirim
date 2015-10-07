using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;

public class LevelLoadScript : MonoBehaviour
{

	private GameObject infoCarrier;
	public Text LevelText;
	public Transform[] rings;

	void Awake ()
	{
		infoCarrier = GameObject.Find ("InfoCarrier");

		if (infoCarrier != null) {
			//Set text components
			InfoCarrierScript ics = infoCarrier.GetComponent<InfoCarrierScript> ();
			LevelText.text = string.Format ("{0}/{1}", 
			                               ics.CurrentLevel + 1, 
			                               ics.TotalLevels);

			XmlDocument xmlDoc = new XmlDocument ();

			//Load puzzles.xml
			xmlDoc.Load (Path.Combine (Application.persistentDataPath, "Puzzles.xml"));

			//Select first group, then pack and last level
			XmlNode level = xmlDoc.SelectNodes ("packs/packGroup") [ics.CurrentPack / 10].ChildNodes [ics.CurrentPack % 10].ChildNodes [ics.CurrentLevel];

			for (int i = 0; i < level.ChildNodes.Count; i++) {

				XmlNode ring = level.ChildNodes [i];
				for (int j = 0; j < ring.ChildNodes.Count; j++) {

					rings [i].GetChild (j).gameObject.GetComponent<SegmentColor> ().SetColor (int.Parse (ring.ChildNodes [j].InnerXml));
				}
			}
		}
	}
}
