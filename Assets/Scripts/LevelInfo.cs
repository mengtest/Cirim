﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class LevelInfo : MonoBehaviour
{
    public int TotalLevels {
        get {
            //Return the total number of levels within the current pack
            return _totalLevels;
        }
    }

    public int CurrentLevel {
        get {
            //Return the current level in the current pack
            return _curLevel;
        }
        set {
            //If the level is within the right range
            if (value < TotalLevels && value >= 0) {
                _curLevel = value;
            } else { //Else set the level to the first level of the pack
                _curLevel = 0;
            }
        }
    }

    public int CurrentPack {
        get {
            //Return the current pack as int; tens represent the packGroup and ones represent the pack within the group
            return _curPack;
        }
        set {
            //Set correct value
            _curPack = value;
            _curLevel = 0;

            UpdateLevelInfo ();
        }
    }

    public int CompletedLevels {
        get {
            //Return how many levels in the current pack are completed
            return _completedLevels;
        }
    }

    public int CompletedPacks {
        get {
            //Return how many packs are completed
            return _completedPacks;
        }
    }

    public int LastPack {
        get {
            //Return the index of the last pack within the current pack group
            return _lastPack;
        }
    }

    public int Rings {
        get {
            return _rings;
        }
    }

    public int Segments {
        get {
            return _segments;
        }
    }

    private int _totalLevels = 1;
    private int _curLevel = 0;
    private int _curPack = 00;
    private int _completedLevels = 0;
    private int _completedPacks = 0;
    private int _lastPack = -1;
    private int _rings = 2;
    private int _segments = 4;

    // Use this for initialization
    void Start ()
    {
        //Make InfoCarrier persistent
        DontDestroyOnLoad (this);

        UpdateLevelInfo (true);
    }

    public void UpdateGameCompletion()
    {
        //Load GameData.xml
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Path.Combine(Application.persistentDataPath, "GameData.xml"));

        //Reset completedPacks counter
        _completedPacks = 0;

        //Counter
        int i = 0;
        foreach (XmlNode group in xmlDoc.SelectNodes("packs/packGroup"))
        {
            //Count completed packs
            _completedPacks += int.Parse(group.Attributes["packsCompleted"].Value);

            //If it is the current group
            if (i == _curPack / 10)
            {
                //Find the levels completed in the current pack
                XmlNode pack = group.ChildNodes[_curPack % 10];
                _completedLevels = int.Parse(pack.Attributes["levelsCompleted"].Value);
            }

            i++;
        }
    }

    private void UpdateLevelInfo (bool firstTime = false)
    {
        //Load Puzzles.xml
        XmlDocument xmlDoc = new XmlDocument ();
        xmlDoc.Load (Path.Combine (Application.dataPath, "Puzzles.xml"));

        int i = 0;
        bool packExists = false;
        //Loop through pack groups
        foreach (XmlNode group in xmlDoc.SelectNodes("packs/packGroup")) {
            
            //If the current pack is in the specified pack group
            if (i == _curPack / 10 && _curPack % 10 < group.ChildNodes.Count)
            {
                packExists = true;

                //Get the last index of packs within the current group
                _lastPack = group.ChildNodes.Count - 1;
                
                //Get the number of rings within the current group
                _rings = int.Parse (group.Attributes ["rings"].Value);
                
                //Select the right pack
                XmlNode pack = group.ChildNodes [_curPack % 10];

                //Find the number of levels in pack
                _totalLevels = pack.SelectNodes ("level").Count;
                
                //Find number of completed levels
                _completedLevels = int.Parse (pack.Attributes ["levelsCompleted"].Value);
                
                //Get the number of segments in the current pack
                _segments = int.Parse (pack.Attributes ["segments"].Value);

            }

            i++;
        }

        if (!packExists) CurrentPack = 0;

        //Sum the number of completed packs
        if (firstTime)
        {
            UpdateGameCompletion();
        }
    }
}