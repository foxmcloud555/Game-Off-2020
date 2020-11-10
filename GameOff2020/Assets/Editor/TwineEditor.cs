using UnityEditor;
using UnityEngine;
public class TwineEditor : EditorWindow {

    //Path to twine saves
    string twineSource;
    string twineSourceFolder;
    //Path to settings
    //Path to save twine files
    string twineOutput;

    [MenuItem ("Window/TwineEditor")]
    public static void ShowWindow () {
        EditorWindow.GetWindow (typeof (TwineEditor));
    }
    private void OnGUI () {
        GUILayout.Label ("Output Folder", EditorStyles.largeLabel);
        GUILayout.Label (twineOutput, EditorStyles.label);
        //twineOutput = EditorGUILayout.TextField ("Output", twineOutput);
        if (GUILayout.Button ("Browse")) {
            string newLocation = "";
            if (twineOutput != "") {
                newLocation = EditorUtility.OpenFolderPanel ("Browse", twineOutput, "");

            } else {
                newLocation = EditorUtility.OpenFolderPanel ("Browse", Application.dataPath, "");
            }
            if (newLocation != "") {
                twineOutput = newLocation;
            }
        }

        GUILayout.Label ("-------------------------", EditorStyles.largeLabel);
        GUILayout.Label ("Twine File", EditorStyles.largeLabel);
        GUILayout.Label (twineSource, EditorStyles.label);
        if (GUILayout.Button ("Browse")) {
            string newLocation;
            if (twineSource != "") {
                newLocation = EditorUtility.OpenFilePanel ("Browse", System.IO.Path.GetDirectoryName (twineSource), "html");
            } else {
                newLocation = EditorUtility.OpenFilePanel ("Browse", System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments) + "/Twine/Stories", "html");
            }

            if (newLocation != "") {
                twineSource = newLocation;
            }

        }

        if (GUILayout.Button ("Import")) {
            Import ();
        }
        GUILayout.Label ("-------------------------", EditorStyles.largeLabel);
        GUILayout.Label ("Twine Folder", EditorStyles.largeLabel);
        GUILayout.Label (twineSourceFolder, EditorStyles.label);
        if (GUILayout.Button ("Browse for Folder")) {
            string newLocation;
            if (twineSourceFolder != "") {
                newLocation = EditorUtility.OpenFolderPanel ("Browse", System.IO.Path.GetDirectoryName (twineSourceFolder), "");
            } else {
                newLocation = EditorUtility.OpenFolderPanel ("Browse", System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments) + "/Twine/Stories", "");
            }
            if (newLocation != "") {
                twineSourceFolder = newLocation;
            }

        }

        if (GUILayout.Button ("Import Folder")) {
            ImportAll ();
        }
    }

    void Import () {
        //AssetDatabase.StopAssetEditing();
        //string[] files = System.IO.Directory.GetFiles (twineSource);
        string newFile = twineOutput + "/" + System.IO.Path.GetFileNameWithoutExtension (twineSource) + ".twine";
        System.IO.File.Copy (twineSource, newFile, true);
        //System.IO.Directory.GetFile
        // foreach (string file in files) {
        //     if (file.EndsWith (".html")) {
        //         string newFile = twineOutput + "/" + System.IO.Path.GetFileNameWithoutExtension (file) + ".twine";
        Debug.Log (newFile);
        //         System.IO.File.Copy (file, newFile, true);
        //     }
        // }

        AssetDatabase.Refresh ();
        //AssetDatabase.StartAssetEditing();
    }

    void ImportAll () {
        string[] files = System.IO.Directory.GetFiles (twineSourceFolder);
        foreach (string file in files) {
            if (file.EndsWith (".html")) {
                string newFile = twineOutput + "/" + System.IO.Path.GetFileNameWithoutExtension (file) + ".twine";
                Debug.Log (newFile);
                System.IO.File.Copy (file, newFile, true);
            }
        }
        AssetDatabase.Refresh ();
    }
}