using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class GameLoad : MonoBehaviour
{
    // Init
    void Awake()
    {
        //If puzzles.xml isn't saved at the persistentdatapath
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "GameData.xml")))
        {
            //Create xmldoc object
            XmlDocument xmlDoc = new XmlDocument(); 

            //Decleration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            //Root element
            XmlElement element1 = xmlDoc.CreateElement("packs");
            xmlDoc.AppendChild(element1);

            //Save gamedata.xml to persistentdatapath
            xmlDoc.Save(Path.Combine(Application.persistentDataPath, "GameData.xml")); 
        }

        XmlDocument xmlAsset = new XmlDocument(); //Create xmldoc object
        xmlAsset.Load(Path.Combine(Application.dataPath, "Puzzles.xml")); //Load puzzles.xml from datapath (asset storage)

        XmlDocument xmlPersist = new XmlDocument(); //Create xmldoc object
        xmlPersist.Load(Path.Combine(Application.persistentDataPath, "GameData.xml")); //Load gamedata.xml from persistentdatapath

        //Select a list of groups from xmlAsset
        XmlNodeList assetGroups = xmlAsset.SelectNodes("packs/packGroup");

        //Select root from xmlPersist
        XmlNode persistRoot = xmlPersist.DocumentElement;

        //Loop through list
        for (int i = 0; i < assetGroups.Count; i++)
        {
            //If the persistent already has the given the group
            if (i >= persistRoot.ChildNodes.Count)
            {
                //Append the given group
                XmlNode group = xmlPersist.CreateElement("packGroup");
                XmlAttribute packsCompleted = xmlPersist.CreateAttribute("packsCompleted");
                packsCompleted.Value = "0";
                group.Attributes.Append(packsCompleted);
                persistRoot.AppendChild(group);
            }

            //The corresponding group in persisting data storage
            XmlNode persistGroup = persistRoot.ChildNodes[i];

            //node list of the packs in the given group
            XmlNodeList assetPacks = assetGroups[i].ChildNodes;

            //Loop through packs in group
            for (int j = 0; j < assetPacks.Count; j++)
            {
                //If persist already has the given group
                if (j >= persistGroup.ChildNodes.Count)
                {
                    //Append the given pack
                    XmlNode pack = xmlPersist.CreateElement("pack");
                    XmlAttribute levelsCompleted = xmlPersist.CreateAttribute("levelsCompleted");
                    levelsCompleted.Value = "0";
                    pack.Attributes.Append(levelsCompleted);
                    XmlAttribute completed = xmlPersist.CreateAttribute("completed");
                    completed.Value = "false";
                    pack.Attributes.Append(completed);
                    persistGroup.AppendChild(pack);
                }

                //The corresponding pack in persisting data storage
                XmlNode persistPack = persistGroup.ChildNodes[j];

                //node list of the levels in the given pack
                XmlNodeList assetLevels = assetPacks[j].ChildNodes;

                //Loop through the missing levels
                for (int m = persistPack.ChildNodes.Count; m < assetLevels.Count; m++)
                {
                    //If the pack is completed beforehand, the addition of levels causes the pack to not be completed anymore. Correct the values then.
                    if (bool.Parse(persistPack.Attributes["completed"].Value))
                    {
                        persistPack.Attributes["completed"].Value = "false";
                        persistGroup.Attributes["packsCompleted"].Value = (int.Parse(persistGroup.Attributes["packsCompleted"].Value) - 1).ToString();
                    }

                    //Append the given level
                    XmlNode level = xmlPersist.CreateElement("level");
                    XmlAttribute completed = xmlPersist.CreateAttribute("completed");
                    completed.Value = "false";
                    level.Attributes.Append(completed);
                    persistPack.AppendChild(level);
                }
            }
        }

        //Save the document
        xmlPersist.Save(Path.Combine(Application.persistentDataPath, "GameData.xml"));
    }

    void Start()
    {
        Application.LoadLevel("MainMenu");
    }
}
