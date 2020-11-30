using System;
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

        //local stats
        private List<string> localStoryVariables;
        private int fame;
        private int happiness;
        private int productivity;
        private int confidence;

        public TwineParser parser;

        private Texture2D[] avatars;

        private bool complete;

        private double timer = 0;
        private TwineParser.StoryNode currentNode;
        private TwineParser.StoryNode startNode;
        private List<TwineParser.StoryNode> _nodes;
        private bool nextPostReady = true;
        private GameObject currentReply;
        private List<int> passageIDs;
        private Dictionary<int, bool> scenesComplete;

        public void Start()
        {
            localStoryVariables = new List<string>();
            avatars = Resources.LoadAll<Texture2D>("avatars");
            SetAct();
            passageIDs = new List<int>();
            parser = GameObject.Find("Browser").GetComponent<TwineParser>();
            parser.parseJSON(GameController.CurrentAct);
            _nodes = parser.storyNodes;
            startNode = _nodes.Find(n => n.name == start);
            scenesComplete.TryGetValue(startNode.pid, out var post);
            if (post)
            {
                complete = true;
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
            
            var avatar = post.GetComponentInChildren<RawImage>();
            if (avatar)
            {
                var text = post.transform.GetChild(1).GetComponent<Text>();
                text.text = node.text;
                avatar.texture = avatars.FirstOrDefault(a => String.Equals(a.name, node.username, StringComparison.CurrentCultureIgnoreCase));
                avatar.GetComponentInChildren<Text>().text = node.username;
            }
            else
            {
                post.transform.GetChild(0).GetComponent<Text>().text = node.name;
                post.transform.GetChild(1).GetComponent<Text>().text = node.username; 
                post.transform.GetChild(2).GetComponent<Text>().text = node.text;
            }

            var image = post.transform.Find("ForumImage");
            if (image && node.image != null)
            {
                image.GetComponent<Image>().sprite = Resources.Load<Sprite>($"images/{node.image}");
                image.gameObject.SetActive(true);
            }
            
            passageIDs.Add(node.pid);
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
            UpdateStoryVariables(currentNode);
            
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
            if (complete) return;
            timer += 0.01;
            
            if (currentNode.trigger == "end")
            {
                if (scenesComplete.ContainsKey(startNode.pid) && scenesComplete[startNode.pid])
                    return;
                
                GameController.Acts.Add(new GameController.StoryAct(start, passageIDs, true));
                nextPostReady = false;
                SendEmailIfNecessary(currentNode);
                
                if (scenesComplete.ContainsKey(startNode.pid))
                {
                    scenesComplete[startNode.pid] = true;

                    FinaliseStoryDetails();
                }
                
                

                return;
            }
            if ((!(timer > 10) || !nextPostReady) && !autoPost) return;
            //if (!String.Equals(currentNode.username, characterToReplyTo, StringComparison.CurrentCultureIgnoreCase))
            if (currentNode.links.Count < 2)
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
                var emailPossible = _nodes.Find(n => n.pid == node.links[0].passageID);
                if (emailPossible.email)
                {
                    var link = node.links[0];
                    node = _nodes.Find(n => n.pid == link.passageID);
                    EmailsBehaviour.CreateEmail(node);
                    var audioPlayer = gameObject.AddComponent<AudioSource>();
                    audioPlayer.clip = Resources.Load<AudioClip>("email/e-mail");
                    audioPlayer.Play();
                }
            }
        }

        private void UpdateStats(TwineParser.StoryNode node)
        {
            //GameController.PlayerStats.fame += node.stat.fame;
            //GameController.PlayerStats.happiness += node.stat.happiness;
            //GameController.PlayerStats.confidence += node.stat.confidence;
            //GameController.PlayerStats.productivity += node.stat.productivity;
            fame += node.stat.fame;
            happiness += node.stat.happiness;
            confidence += node.stat.confidence;
            productivity += node.stat.productivity;
            
            
        }

        private void UpdateStoryVariables(TwineParser.StoryNode node)
        {
            if (node.storyVariable != null && !GameController.StoryVariables.Contains(node.storyVariable))
            {
                localStoryVariables.Add(node.storyVariable);
                
            }
        }

        private void FinaliseStoryDetails()
        {
            GameController.PlayerStats.fame += fame;
            GameController.PlayerStats.happiness += happiness;
            GameController.PlayerStats.confidence += confidence;
            GameController.PlayerStats.productivity += productivity;
            
            
            GameController.StoryVariables.AddRange(localStoryVariables);
        }
    }
}
