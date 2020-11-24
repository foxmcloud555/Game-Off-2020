using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace StoryProgress.Emails
{
    public class EmailsBehaviour : MonoBehaviour
    {

        //prefab
        public string Sender;
        public string Title;
    

        public static List<EmailStruct> storyEmails;

        public Texture2D[] emailIcons;
    
        // Start is called before the first frame update
        void Start()
        {
            foreach (var email in storyEmails)
            {
            
            }
        }

        void CreateEmail()
        {
        
        }


    }

    public struct EmailStruct
    {
        public string Sender;
        public string Title;
        public MessageStatus Status;

        public EmailStruct(string sender, string title, MessageStatus status)
        {
            Sender = sender;
            Title = title;
            Status = status;
        }
    }
    
    public enum MessageStatus
    {
        Unread = 0,
        Read = 1,
        Glitched,
        Open
    }
}