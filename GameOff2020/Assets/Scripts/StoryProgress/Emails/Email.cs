using System.Collections;
using System.Collections.Generic;
using StoryProgress.Emails;
using UnityEngine;
using UnityEngine.UI;

public class Email : MonoBehaviour
{

    public SpriteRenderer icon;
    public Text sender;
    public Text title;
    public Text bodyText;
    public GameObject body;
    public Sprite[] emailIcons = new Sprite[4];
    public Animator animator;
    public MessageStatus status;

    private bool isOpen;

    private void OnMouseDown()
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
    }

    public void PopulateEmail(EmailStruct data)
    {
        status = data.Status;
        icon.sprite = emailIcons[(int) status];
        sender.text = data.Sender;
        title.text = data.Title;
        bodyText.text = data.BodyText;
    }

    public void Open()
    {
        isOpen = true;
        body.SetActive(true);
        status = MessageStatus.Open;
    }


    public void Close()
    {
        isOpen = false;
        body.SetActive(false);
        status = MessageStatus.Read;

    }
}
