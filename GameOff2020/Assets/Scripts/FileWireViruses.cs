
using System.Collections;
using UnityEngine;

public class FileWireViruses : MonoBehaviour
{
    public GameObject errorMessage;

    public Transform browserCanvas;
    // Start is called before the first frame update
   //void Start()
   //{
   //    
   //}

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
}
