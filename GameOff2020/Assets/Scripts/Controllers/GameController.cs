using System.Collections.Generic;
using System.Linq;
using StoryProgress.Emails;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
    
        public static TwineParser.CharacterStats PlayerStats;
        public static List<string> StoryVariables;
        public static Dictionary<int, bool> ScenesComplete;
        public static List<TwineParser.StoryNode> Act1Nodes;

        public static List<StoryAct> Acts;

        private static bool firstSetup = true;


        // Start is called before the first frame update
        void Awake()
        {

            if (firstSetup)
            {
                Acts = new List<StoryAct>();
                StoryVariables = new List<string>();
                ScenesComplete = new Dictionary<int, bool>();
                var twine1 = GetComponent<TwineParser>();
                twine1.parseJSON();
                Act1Nodes = twine1.storyNodes;

                var scenes = Act1Nodes.Where(a1n => a1n.trigger == "start");

                foreach (var scene in scenes)
                {
                    ScenesComplete.Add(scene.pid, false);
                }
                
                InitEmails();
        
                firstSetup = false;
            }
                //BeginGame();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void LoadScene(GameObject button)
        {
            SceneManager.LoadScene(button.name);
        }


        private void BeginGame()
        {
            
        }

        private void InitEmails()
        {
            EmailsBehaviour.ConstructListOfEmails();
            
            foreach (var node in Act1Nodes)
            {
                EmailsBehaviour.CreateEmail(node);
            }
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
