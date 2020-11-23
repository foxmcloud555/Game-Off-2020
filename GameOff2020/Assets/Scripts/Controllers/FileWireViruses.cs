
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FileWireViruses : MonoBehaviour
{
    public GameObject errorMessage;

    private List<Text> texts;
    private List<string> originalStrings;

    public Transform browserCanvas;
   //Start is called before the first frame update
   void Start()
   {
       texts = new List<Text>();
       originalStrings = new List<string>();
   }

   //// Update is called once per frame
   //void Update()
   //{
   //    
   //}

   public void SpamError()
   {
       StartCoroutine(ErrorStream(errorMessage));
   }

   IEnumerator ErrorStream(GameObject prefab)
   {
       var randomLocation = new Vector3();
       for (int i = 0; i < 10; i++)
       { 
           randomLocation = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 5f), 0);
           yield return Instantiate(prefab, randomLocation, Quaternion.identity, browserCanvas);
       }
   }

   public void CloseWindow(GameObject go)
    {
        GameObject.Destroy(go);
    }




   public void TextBlender()
   {
       ScrambleStrings(GatherText());
   }

   private List<Text> GatherText()
   {
       var textComponents = new List<Text>();
       textComponents = Resources.FindObjectsOfTypeAll<Text>().ToList();
       return textComponents;
   }

   private void ScrambleStrings(List<Text> textComponents)
   {
       foreach (var textComponent in textComponents)
       {
           texts.Add(textComponent);
           var text = textComponent.text;
           originalStrings.Add(text);
           textComponent.text = text.Scramble();
           
       }
   }
   

   
}
