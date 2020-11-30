using System;
using System.Collections;
using System.Collections.Generic;
using StoryProgress.Emails;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Email : MonoBehaviour
{

    public int listID;
    public Image icon;
    public Text sender;
    public Text title;
    public Text bodyText;
    public GameObject body;
    public Sprite[] emailIcons = new Sprite[4];
    public Animator animator;
    public MessageStatus status;
    public GameObject externalLink;

    private bool isOpen;

    public void ClickEmail()
    {
        animator.enabled = false;
        
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
        
        icon.sprite = emailIcons[(int) status];
        EmailsBehaviour.storyEmails[listID] = new EmailStruct(sender.text, title.text, MessageStatus.Read, bodyText.text, externalLink.name );
    }

    public void PopulateEmail(EmailStruct data, int id)
    {
        listID = id;
        status = data.Status;
        icon.sprite = emailIcons[(int) status];
        sender.text = data.Sender;
        title.text = data.Title;
        bodyText.text = data.BodyText;
        externalLink.name = data.ExternalLink;
        externalLink.SetActive(data.ExternalLink != null);
    }

    public void Open()
    {
        isOpen = true;
        body.SetActive(true);
        status = MessageStatus.Open;
    }

    public void FollowExternalLink(GameObject email)
    {
        
        SceneManager.LoadScene(email.name);
    }


    public void Close()
    {
        isOpen = false;
        body.SetActive(false);
        status = MessageStatus.Read;

    }
}
