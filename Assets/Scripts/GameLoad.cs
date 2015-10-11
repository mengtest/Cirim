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
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "Puzzles.xml")))
        {
            XmlDocument xmlDoc = new XmlDocument(); //Create xmldoc object
            xmlDoc.Load(Path.Combine(Application.dataPath, "Puzzles.xml")); //Load puzzles.xml from datapath (asset storage)
            xmlDoc.Save(Path.Combine(Application.persistentDataPath, "Puzzles.xml")); //Save puzzles.xml to persistentdatapath
        }
        else
        {
            XmlDocument xmlAsset = new XmlDocument(); //Create xmldoc object
            xmlAsset.Load(Path.Combine(Application.dataPath, "Puzzles.xml")); //Load puzzles.xml from datapath (asset storage)

            XmlDocument xmlPersist = new XmlDocument(); //Create xmldoc object
            xmlPersist.Load(Path.Combine(Application.persistentDataPath, "Puzzles.xml")); //Load puzzles.xml from persistentdatapath

            //Select a list of groups from xmlAsset
            XmlNodeList assetGroups = xmlAsset.SelectNodes("packs/packGroup");

            //Select root from xmlPersist
            XmlNode persistRoot = xmlPersist.DocumentElement;

            //Loop through list
            for (int i = 0; i < assetGroups.Count; i++)
            {
                //If the persistent already has the given the group
                if (i < persistRoot.ChildNodes.Count)
                {
                    //The corresponding group in persisting data storage
                    XmlNode persistGroup = persistRoot.ChildNodes[i];

                    //node list of the packs in the given group
                    XmlNodeList assetPacks = assetGroups[i].ChildNodes;

                    //Loop through packs in group
                    for (int j = 0; j < assetPacks.Count; j++)
                    {
                        //If persist already has the given group
                        if (j < persistGroup.ChildNodes.Count)
                        {
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
                                    persistPack.Attributes["completed"].Value = false.ToString();
                                    persistGroup.Attributes["packsCompleted"].Value = (int.Parse(persistGroup.Attributes["packsCompleted"].Value) - 1).ToString();
                                }

                                //Append the given level
                                persistPack.AppendChild(xmlPersist.ImportNode(assetLevels[m], true));
                            }
                        }
                        else //else
                        {
                            //Append the given pack
                            persistGroup.AppendChild(xmlPersist.ImportNode(assetPacks[j], true));
                        }
                    }
                }
                else //else
                {
                    //Append the given group
                    persistRoot.AppendChild(xmlPersist.ImportNode(assetGroups[i], true));
                }
            }

            //Save the document
            xmlPersist.Save(Path.Combine(Application.persistentDataPath, "Puzzles.xml"));
        }
    }

    void Start()
    {
        Application.LoadLevel("MainMenu");
    }
}
