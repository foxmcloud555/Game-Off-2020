using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
//using System.Json;

public class TwineParser : MonoBehaviour
{
    public TextAsset storyFile;

    public List<StoryNode> storyNodes;
    // Start is called before the first frame update
    void Start()
    {
        
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void parseJSON()
    {
        storyNodes = new List<StoryNode>();
        JObject o1 = JObject.Parse(storyFile.ToString());

        var passages = o1["passages"];

        foreach (var passage in passages)
        {
            var node = new StoryNode();
            node.pid = Int32.Parse(passage["pid"]?.ToString() ?? "-1");
            node.name = passage["name"]?.ToString();
            node.text = passage["text"]?.ToString();
            node.username = passage["tags"][0]["user"].ToString();
            node.links = new List<StoryLink>();
            var linksJSON = passage["links"].ToList();
            foreach (var link in linksJSON)
            {
                var storyLink = new StoryLink();
                storyLink.ID = link["id"]?.ToString();
                storyLink.label = link["label"]?.ToString();
                storyLink.passageID = Int32.Parse(link["passageId"]?.ToString() ?? "-1");
                node.links.Add(storyLink);
            }
            
            storyNodes.Add(node);
        }

    }

    public struct StoryNode
    {
        public int pid;
        public string name;
        public string text;
        public string username;
        public List<StoryLink> links;
    }

    public struct StoryLink
    {
        public string ID;
        public string label;
        public int passageID;
    }
}
