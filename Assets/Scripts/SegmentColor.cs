using UnityEngine;
using System.Collections;

public class SegmentColor : MonoBehaviour {

	public int color;

	public void SetColor(int i)
	{
		color = i;

		switch(i)
		{
			case 0:
				gameObject.GetComponent<SpriteRenderer>().color = SegmentColors.Red;
				break;
			case 1:
				gameObject.GetComponent<SpriteRenderer>().color = SegmentColors.Yellow;
				break;
			case 2:
				gameObject.GetComponent<SpriteRenderer>().color = SegmentColors.Blue;
				break;
			case 3:
				gameObject.GetComponent<SpriteRenderer>().color = SegmentColors.Green;
				break;
		}
	}
}

class SegmentColors
{
	public readonly static Color Red = new Color32(255, 0, 72, 255);
	public readonly static Color Green = new Color32(0, 255, 128, 255);
	public readonly static Color Yellow = new Color32(235, 246, 52, 255);
	public readonly static Color Blue = new Color32(0, 192, 255, 255);
}