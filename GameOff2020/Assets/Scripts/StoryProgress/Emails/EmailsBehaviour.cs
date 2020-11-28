using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace StoryProgress.Emails
{
    public class EmailsBehaviour : MonoBehaviour
    {

        public static List<EmailStruct> storyEmails;
        public Transform emailParent;

        public GameObject emailPrefab;

        public static void ConstructListOfEmails()
        {
            storyEmails = new List<EmailStruct>();
        }
    
        // Start is called before the first frame update
        void Start()
        {
            foreach (var email in storyEmails)
            {
                var emailObject = Instantiate(emailPrefab, emailParent, false);
                emailObject.transform.localScale = Vector3.one;
                var emailData = emailObject.GetComponent<Email>();
                emailData.PopulateEmail(email);
            }
        }

        public static  void CreateEmail(TwineParser.StoryNode node)
        {
            var virusStatus = MessageStatus.Unread;
            if (node.stat.virus != null)
            {
                virusStatus = MessageStatus.Glitched;
            }
            EmailStruct email = new EmailStruct(node.username, node.name, virusStatus, node.text);
            storyEmails.Add(email);
        }

    }
    
    
    
    
    
    
    
    
    
    
    

    public struct EmailStruct
    {
        public string Sender;
        public string Title;
        public MessageStatus Status;
        public string BodyText;

        public EmailStruct(string sender, string title, MessageStatus status, string bodyText)
        {
            Sender = sender;
            Title = title;
            Status = status;
            BodyText = bodyText;
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