using System.Collections.Generic;
using UnityEngine;

namespace StoryProgress
{
    public class ForumMainPage : MonoBehaviour
    {

        public List<GameObject> forumPosts;
        public GameObject threads;

        public GameObject frontPageLists;
        // Start is called before the first frame update

        private void OnEnable()
        {
            HideShowAllPosts(false);
        }


        public void ShowThread(GameObject post)
        {
            HideShowAllPosts(false);
            HideShowAllFrontPage(false);
            post.SetActive(true);
        }

        public void HideShowAllPosts(bool show )
        {
            foreach (Transform post in threads.transform)
            {
                post.gameObject.SetActive(show);
            }
        }
    
        public void HideShowAllFrontPage(bool show)
        {
            frontPageLists.SetActive(show);
            HideShowAllPosts(false);
        }
    
    }
}
