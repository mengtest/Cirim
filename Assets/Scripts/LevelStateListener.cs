using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Xml;

public class LevelStateListener : MonoBehaviour
{
    //Internal variables
    private bool samePattern;
    private int[] rotationDiff;
    private bool won = false;
    private int segments;

    //public readonly variable
    public bool Won { get { return won; } }

    //Objects to be filled from unity
    public GameObject WinCanvas;
    public GameObject[] rings;
    public Button[] buttons;

    // Init
    void Start()
    {
        segments = rings[0].GetComponent<Transform>().childCount; //Set the segments
        rotationDiff = new int[rings.Length - 1]; //Instantiate array for rotation diffs

        samePattern = SamePattern(); //Lookup if the pattern is the same across all rings

        //Subscribe to events
        SwapColor.OnSwap += SwapEvent;
        Movement.OnRotate += RotateEvent;
    }

    //"Destructor"
    void OnDestroy()
    {
        //Unsubscribe
        SwapColor.OnSwap -= SwapEvent;
        Movement.OnRotate -= RotateEvent;
    }

    //Rotate event procedure
    void RotateEvent()
    {
        //Check if player has won the level
        CheckForWin();
    }

    //Swap event procedure
    void SwapEvent()
    {
        //Lookup if the pattern is the same across all rings
        samePattern = SamePattern();

        //Check if player has won the level
        CheckForWin();
    }

    //Check for win function
    private void CheckForWin()
    {
        //If it is the samme pattern
        if (samePattern)
        {
            //Validate if the rings are alligned correctly
            bool correctRotation = true;

            for (int i = 0; i < rotationDiff.Length; i++)
            {
                if (rings[i].GetComponent<Movement>().rotation + rotationDiff[i] != rings[i + 1].GetComponent<Movement>().rotation)
                {
                    correctRotation = false;
                }
            }

            //If the rotations are correct
            if (correctRotation)
            {
                //Unsubscribe
                SwapColor.OnSwap -= SwapEvent;
                Movement.OnRotate -= RotateEvent;

                //Save that the level is complete
                GameObject infoCarrier = GameObject.Find("InfoCarrier");

                //If there is an infocarrier
                if (infoCarrier != null)
                {
                    //Get the script
                    LevelInfo ics = infoCarrier.GetComponent<LevelInfo>();

                    //Load GameData.xml
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(Path.Combine(Application.persistentDataPath, "GameData.xml"));

                    //Get the group, pack, and level of the won game
                    XmlNode group = xmlDoc.SelectNodes("packs/packGroup")[ics.CurrentPack / 10];
                    XmlNode pack = group.ChildNodes[ics.CurrentPack % 10];
                    XmlNode level = pack.ChildNodes[ics.CurrentLevel];

                    //Update level and pack attributes
                    if (!bool.Parse(level.Attributes["completed"].Value))
                    {
                        level.Attributes["completed"].Value = "true";
                        int newVal = int.Parse(pack.Attributes["levelsCompleted"].Value) + 1;
                        pack.Attributes["levelsCompleted"].Value = newVal.ToString();

                        //If all the levels in the pack are completed
                        if (newVal == pack.ChildNodes.Count)
                        {
                            //Update a group attribute and an additional pack attribute
                            pack.Attributes["completed"].Value = "true";
                            group.Attributes["packsCompleted"].Value = (int.Parse(group.Attributes["packsCompleted"].Value) + 1).ToString();
                        }

                        xmlDoc.Save(Path.Combine(Application.persistentDataPath, "GameData.xml"));

                        ics.UpdateGameCompletion();
                    }
                }

                //Disable the bottom buttons during win canvas
                foreach (Button b in buttons)
                {
                    b.interactable = false;
                }

                //Set level to won
                won = true;

                //Create the win canvas
                Instantiate(WinCanvas);
            }
        }
    }

    //Recursive function to find out if the rings are all the same pattern
    private bool SamePattern(int m = 0)
    {
        bool temp = false;

        //Loop through the segments of the inner ring
        for (int i = 0; i < segments; i++)
        {
            temp = true;

            //Loop through the segments of the outer ring
            for (int j = 0; j < segments; j++)
            {
                //If the components are not the same
                if (rings[m].GetComponent<Transform>().GetChild((i + j) % segments).gameObject.GetComponent<SegmentColor>().color != rings[m + 1].GetComponent<Transform>().GetChild(j).gameObject.GetComponent<SegmentColor>().color)
                {
                    temp = false;
                    break;
                }
            }

            //If the same pattern has been found
            if (temp)
            {
                rotationDiff[m] = i; //Save the rotation difference
                break;
            }
        }

        //If there are more rings
        if (rings.Length > m + 2)
        {
            return temp && SamePattern(m + 1); //Continue recursively
        }
        else  //Else
        {
            return temp;
        }
    }
}