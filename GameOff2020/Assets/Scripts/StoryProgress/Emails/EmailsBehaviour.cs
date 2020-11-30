using System.Collections.Generic;
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
            Debug.Log("initialising emails");
            storyEmails = new List<EmailStruct>();
        }
    
        // Start is called before the first frame update
        void Start()
        {
            int emailsCount = 0;
            List<EmailStruct> reverse = storyEmails;
            reverse.Reverse();
            
            foreach (var email in reverse)
            {
                var emailObject = Instantiate(emailPrefab, emailParent, false);
                emailObject.transform.localScale = Vector3.one;
                var emailData = emailObject.GetComponent<Email>();
                emailData.PopulateEmail(email, emailsCount);
                emailsCount++;
            }
        }

        public static  void CreateEmail(TwineParser.StoryNode node)
        {
            var virusStatus = MessageStatus.Unread;
            if (node.stat.virus != null)
            {
                virusStatus = MessageStatus.Glitched;
            }
            EmailStruct email = new EmailStruct(node.username, node.name, virusStatus, node.text, node.scene);
            storyEmails.Add(email);
        }

    }
    
    
    
    
    
    
    
    
    
    
    

    public struct EmailStruct
    {
        public string Sender;
        public string Title;
        public MessageStatus Status;
        public string BodyText;
        public string ExternalLink;
        

        public EmailStruct(string sender, string title, MessageStatus status, string bodyText, string externalLink)
        {
            Sender = sender;
            Title = title;
            Status = status;
            BodyText = bodyText;
            ExternalLink = externalLink;
        }

        public void SetStatus(MessageStatus status)
        {
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