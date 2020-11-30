using System.Collections;
using StoryProgress.Emails;
using UnityEngine;

namespace Controllers
{
    public class ActProgresser : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameController.CurrentAct = 2;
            foreach (var node in GameController.Act2Nodes)
            {
                if (node.email && ((IList) GameController._actTwoEmails).Contains(node.pid))
                {
                    EmailsBehaviour.CreateEmail(node);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
