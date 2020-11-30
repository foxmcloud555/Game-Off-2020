using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace StoryProgress
{
    public class EndingController : MonoBehaviour
    {
        private TwineParser parser;
        private List<TwineParser.StoryNode> _nodes;

        public List<string> svs;

        public GameObject postHolder;
        public GameObject afterLifePrefab;

        // Start is called before the first frame update
        void Start()
        {
            //svs = new List<string> {"HouseholdIncomeYES", "PsychicReading", "AlligatorGood"};
            //GameController.StoryVariables = svs;
            parser = GameObject.Find("Browser").GetComponent<TwineParser>();
            parser.parseJSON(3);
            _nodes = parser.storyNodes;
            AutomaticallyCreatePosts();
        }
        
        private void AutomaticallyCreatePosts()
        {
            var endingVariables = _nodes.Where(sv => GameController.StoryVariables.Contains(sv.endVariable));
            
            foreach (var node in endingVariables)
            {
                CreatePost(node);
            }
        }

        public void CreatePost(TwineParser.StoryNode node)
        {
            var post = Instantiate(afterLifePrefab, postHolder.transform);

            var title = post.transform.GetChild(0);
            var bodyText = post.transform.GetChild(1);
            title.GetComponent<Text>().text = node.name;
            bodyText.GetComponent<Text>().text = node.text;
        }


        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
