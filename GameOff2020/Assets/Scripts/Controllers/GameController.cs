using System;
using System.Collections.Generic;
using System.Linq;
using StoryProgress.Emails;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {

        public static int CurrentAct = 2;
        public static TwineParser.CharacterStats PlayerStats;
        public static List<string> StoryVariables;
        public static Dictionary<int, bool> ScenesCompleteAct1;
        public static Dictionary<int, bool> ScenesCompleteAct2;
        public static List<TwineParser.StoryNode> Act1Nodes;
        public static List<TwineParser.StoryNode> Act2Nodes;

        private readonly int[] _startingEmails =  {136, 137, 138, 139};
        private int[] _actTwoEmails = {8, 11, 12};

        //this list contains acts already encountered in the story
        //to autocomplete forum posts if already played
        public static List<StoryAct> Acts;

        //first time the game is run?
        private static bool firstSetup = true;


        // Start is called before the first frame update
        void Awake()
        {

            if (firstSetup)
            {
                Acts = new List<StoryAct>();
                StoryVariables = new List<string>();
                ScenesCompleteAct1 = new Dictionary<int, bool>();
                ScenesCompleteAct2 = new Dictionary<int, bool>();
                
                ///act 1 setup
                var twine1 = GetComponent<TwineParser>();
                twine1.parseJSON(1);
                Act1Nodes = twine1.storyNodes;
                var scenes = Act1Nodes.Where(a1n => a1n.trigger == "start");
                
                foreach (var scene in scenes)
                {
                    ScenesCompleteAct1.Add(scene.pid, false);
                }
                
                ///act 2 setup
                var twine2 = GetComponent<TwineParser>();
                twine2.parseJSON(2);
                Act2Nodes = twine2.storyNodes;
                scenes = Act2Nodes.Where(a2n => a2n.trigger == "start");
                
                foreach (var scene in scenes)
                {
                    ScenesCompleteAct2.Add(scene.pid, false);
                }
                
                InitEmails();
        
                firstSetup = false;
            }
                //BeginGame();
        }

        private void Start()
        {
            var urlBar = GameObject.Find("urlBar");
            if (!urlBar) return;
            string sitename = SceneManager.GetActiveScene().name.ToLower();
            sitename = sitename.Replace(' ', '\0');
            var siteNameParts = sitename.Split(new string[] { "act" }, StringSplitOptions.None);
            sitename = siteNameParts[0];
            urlBar.GetComponent<InputField>().text = $"www.{sitename}.com";
        }

        public void LoadScene(GameObject button)
        {
            var sceneToLoad = button.name;
            if (CurrentAct != 1 && !sceneToLoad.Contains("E-Mail") && !sceneToLoad.Contains("Home"))
            {
                sceneToLoad = $"{sceneToLoad} Act {CurrentAct}";
            }
            
            SceneManager.LoadScene(sceneToLoad);
        }


        public void SwitchAct(int act)
        {
            CurrentAct = act;
        }

        private void InitEmails()
        {
            EmailsBehaviour.ConstructListOfEmails();
            
            foreach (var node in Act1Nodes)
            {
                if (node.email && _startingEmails.Contains(node.pid))
                {
                    EmailsBehaviour.CreateEmail(node);
                }
            }
        }


        public void ShowObject(GameObject objectToShow)
        {
            objectToShow.SetActive(!objectToShow.activeInHierarchy);
        }

        public struct StoryAct
        {
            public string StartName;
            public List<int> PassageIDs;
            public bool Completed;

            public StoryAct(string startName, List<int> passageIDs, bool completed )
            {
                StartName = startName;
                PassageIDs = passageIDs;
                Completed = completed;
            }
        }
        
        

    
    

    }
}
