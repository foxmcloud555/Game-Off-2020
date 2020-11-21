﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;


public class ForumStory : MonoBehaviour
{
    public string start;
    public string[] end;
    public string characterToReplyTo;

    public GameObject forumPost;
    public GameObject forumReply;
    public GameObject forumBoard;
    public GameObject replyButton;

    [Header("User Avatars")] public Texture2D[] avatars;

    private double timer = 0;
    private TwineParser.StoryNode currentNode;
    private List<TwineParser.StoryNode> _nodes;
    private bool nextPostReady = true;
    private GameObject currentReply;

    public void Start()
    {
        var twine = GetComponent<TwineParser>();
        twine.parseJSON();
        _nodes = twine.storyNodes;
        var startNode = _nodes.First(n => n.name == start);
        
        currentNode = startNode;
        CreatePost(startNode);
    }

    public void CreatePost(TwineParser.StoryNode node)
    {
        var post = Instantiate(forumPost, forumBoard.transform);
        var text = post.transform.GetChild(1).GetComponent<Text>();
        text.text = node.text;
        var avatar = post.GetComponentInChildren<RawImage>();
        avatar.texture = avatars.FirstOrDefault(a => String.Equals(a.name, node.username, StringComparison.CurrentCultureIgnoreCase));
        avatar.GetComponentInChildren<Text>().text = node.username;
    }
    
    public void CreateReply()
    {
        var reply = Instantiate(forumReply, forumBoard.transform);
        var panel = reply.transform.GetChild(1);
        for (int i = 0; i < currentNode.links.Count; i++)
        {
            var button = Instantiate(replyButton, panel.transform);
            button.name = currentNode.links[i].passageID.ToString();
            button.GetComponentInChildren<Text>().text = currentNode.links[i].label;
            var uiButton = button.GetComponent<Button>();
            uiButton.onClick.AddListener(delegate { ProcessReply(button);});
        }

        currentReply = reply;

    }
    
    

    public void ProcessReply(GameObject reply)
    {
       currentNode = _nodes.First(n => n.pid.ToString() == reply.name);
       nextPostReady = true;
       if (currentReply)
           Destroy(currentReply);
       CreatePost(currentNode);
       timer = 0;

    }

    private void Update()
    {
        //if (expr)
        {
            
        }
        timer += 0.01;
        if (timer > 10 && nextPostReady)
        {
            
            if (!String.Equals(currentNode.username, characterToReplyTo, StringComparison.CurrentCultureIgnoreCase))
            {
                ProgressConvo(); 
            }
            else
            {
                nextPostReady = false;
                CreateReply();
            }

        }

    }

    private void ProgressConvo()
    {
        timer = 0;
        var node = _nodes.First(n => n.pid == currentNode.links[0].passageID);
        CreatePost(node);
        UpdateStats(node);
        currentNode = node;
        nextPostReady = true;
    }

    private void UpdateStats(TwineParser.StoryNode node)
    {
        GameController.PlayerStats.fame += node.stat.fame;
        GameController.PlayerStats.happiness += node.stat.happiness;
        GameController.PlayerStats.confidence += node.stat.confidence;
        GameController.PlayerStats.productivity += node.stat.productivity;
    }
}
