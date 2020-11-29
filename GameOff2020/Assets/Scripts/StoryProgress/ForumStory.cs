﻿using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using StoryProgress.Emails;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace StoryProgress
{
    public class ForumStory : MonoBehaviour
    {
        public string start;
        public string characterToReplyTo;
        public bool autoPost;

        public GameObject forumPost;
        public GameObject forumReply;
        public GameObject forumBoard;
        public GameObject replyButton;

        public TwineParser parser;

        [Header("User Avatars")] public Texture2D[] avatars;

        private double timer = 0;
        private TwineParser.StoryNode currentNode;
        private List<TwineParser.StoryNode> _nodes;
        private bool nextPostReady = true;
        private GameObject currentReply;
        private List<int> passageIDs;
        private Dictionary<int, bool> scenesComplete;

        public void Start()
        {
            SetAct();
            passageIDs = new List<int>();
            parser = GameObject.Find("Browser").GetComponent<TwineParser>();
            parser.parseJSON(GameController.CurrentAct);
            _nodes = parser.storyNodes;
            var startNode = _nodes.Find(n => n.name == start);
            scenesComplete.TryGetValue(startNode.pid, out var post);
            if (post)
            {
                AutomaticallyCreatePosts();
            }
            else
            {
                currentNode = startNode;
                CreatePost(startNode);
            }
            
        }

        private void SetAct()
        {
            switch (GameController.CurrentAct)
            {
                case 1:
                    scenesComplete = GameController.ScenesCompleteAct1;
                    break;
                case 2:
                    scenesComplete = GameController.ScenesCompleteAct2;
                    break;
                case 3:
                   // _nodes = GameController.Act3Nodes;
                break;
            }

        }

        private void AutomaticallyCreatePosts()
        {
            var act = GameController.Acts.First(a => a.StartName == start);
            foreach (var passageID in act.PassageIDs)
            {
                var node = _nodes.Find(n => n.pid == passageID);
                CreatePost(node);
            }
        } 


        public void CreatePost(TwineParser.StoryNode node)
        {
            var post = Instantiate(forumPost, forumBoard.transform);
            var text = post.transform.GetChild(1).GetComponent<Text>();
            text.text = node.text;
            var avatar = post.GetComponentInChildren<RawImage>();
            avatar.texture = avatars.FirstOrDefault(a => String.Equals(a.name, node.username, StringComparison.CurrentCultureIgnoreCase));
            avatar.GetComponentInChildren<Text>().text = node.username;
            passageIDs.Add(currentNode.pid);
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
            UpdateStoryVariables(currentNode);
            timer = 0;

        }

        private void EndScene(TwineParser.StoryNode node)
        {
        
        
        }

        private void Update()
        {
        
            FirstTimeUpdate();
        }


        private void FirstTimeUpdate()
        {
            timer += 0.01;
            if ((!(timer > 10) || !nextPostReady) && !autoPost) return;
            if (currentNode.trigger == "end")
            {
                if (scenesComplete.ContainsKey(currentNode.pid))
                {
                    scenesComplete[currentNode.pid] = true;
                }

                GameController.Acts.Add(new GameController.StoryAct(start, passageIDs, true));
                nextPostReady = false;
                SendEmailIfNecessary(currentNode);
                    
                //TODO check if next post is email!
                    
                return;
            }
            
            //if (!String.Equals(currentNode.username, characterToReplyTo, StringComparison.CurrentCultureIgnoreCase))
            if (currentNode.links == null || currentNode.links.Count < 2)
            {
                ProgressConvo(); 
            }
            else
            {
                nextPostReady = false;
                CreateReply();
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
            UpdateStoryVariables(currentNode);
        }

        private void SendEmailIfNecessary(TwineParser.StoryNode node)
        {
            if (node.links.Count == 1)
            {
                var link = node.links[0];
                node = _nodes.Find(n => n.pid == link.passageID);
                EmailsBehaviour.CreateEmail(node);
            }
        }

        private void UpdateStats(TwineParser.StoryNode node)
        {
            GameController.PlayerStats.fame += node.stat.fame;
            GameController.PlayerStats.happiness += node.stat.happiness;
            GameController.PlayerStats.confidence += node.stat.confidence;
            GameController.PlayerStats.productivity += node.stat.productivity;
        }

        private void UpdateStoryVariables(TwineParser.StoryNode node)
        {
            if (node.storyVariable != null)
            {
                GameController.StoryVariables.Add(node.storyVariable);
            }
        }
    }
}
