﻿using System;
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
    public TextAsset[] storyFiles;

    public List<StoryNode> storyNodes;

    public void parseJSON(int actNumber)
    {
        storyNodes = new List<StoryNode>();
        if (storyFiles.Length  == 0) return;
        JObject o1 = JObject.Parse(storyFiles[actNumber - 1].ToString());

        var passages = o1["passages"];

        foreach (var passage in passages)
        {
            var node = new StoryNode();
            
            //base vars
            node.pid = Int32.Parse(passage["pid"]?.ToString() ?? "-1");
            node.name = passage["name"]?.ToString();
            node.text = passage["text"]?.ToString();
            //tags
            if (passage["tags"].Any())
            {
                node.username = passage["tags"][0]["user"]?.ToString() ?? "anon";
                node.trigger = passage["tags"][0]["trigger"]?.ToString();
                node.storyVariable = passage["tags"][0]["storyVariable"]?.ToString();
                node.email = passage["tags"][0]["email"]?.Type == JTokenType.Boolean;
                node.scene = passage["tags"][0]["scene"]?.ToString();
                node.image = passage["tags"][0]["image"]?.ToString();
            }

            //links
            node.links = new List<StoryLink>();
            var linksJSON = passage["links"]?.ToList();

            if (node.pid == 152)
            {
                Debug.Log("guhh");
            }
            
            if (!linksJSON.Any() || linksJSON[0].ToString() == "{}" )
            {
                node.trigger = "end";
            }
            foreach (var link in linksJSON)
            {
                var storyLink = new StoryLink();
                storyLink.ID = link["id"]?.ToString();
                storyLink.label = link["label"]?.ToString();
                storyLink.passageID = Int32.Parse(link["passageId"]?.ToString() ?? "-1");
                node.links.Add(storyLink);
            }

            if (passage["tags"].Any())
            {
                Int32.TryParse(passage["tags"][0]["productivity"]?.ToString(), out node.stat.productivity);
                Int32.TryParse(passage["tags"][0]["fame"]?.ToString(), out node.stat.fame);
                Int32.TryParse(passage["tags"][0]["happiness"]?.ToString(), out node.stat.happiness);
                Int32.TryParse(passage["tags"][0]["confidence"]?.ToString(), out node.stat.confidence);
            }

            storyNodes.Add(node);
        }
    }

    public struct StoryNode
    {
        //base vars
        public int pid;
        public string name;
        public string text;
        
        //tags
        public string username;
        public string trigger;
        public string storyVariable;
        public bool email;
        public string scene;
        public string image;
        
        //links
        public List<StoryLink> links;
        
        //stats
        public CharacterStats stat;
    }
    
    public struct CharacterStats
    {
        public int fame;
        public int productivity;
        public int confidence;
        public string virus;
        public int happiness;
    }

    public struct StoryLink
    {
        public string ID;
        public string label;
        public int passageID;
    }
}
