using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForumMainPage : MonoBehaviour
{

    public List<GameObject> forumPosts;

    public GameObject frontPageLists;
    // Start is called before the first frame update

    private void OnEnable()
    {
        HideAllPosts();
    }


    public void ShowThread(GameObject post)
    {
        HideAllPosts();
        HideShowAllFrontPage(false);
        post.SetActive(true);
    }

    public void HideAllPosts()
    {
        foreach (var post in forumPosts)
        {
            post.SetActive(false);
        }
    }
    
    public void HideShowAllFrontPage(bool show)
    {
        frontPageLists.SetActive(show);
    }
    
}
