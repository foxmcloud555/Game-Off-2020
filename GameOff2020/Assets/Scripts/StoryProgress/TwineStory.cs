using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TwineStorydata {
    public string name;
    public int startNode;
    public string creator;
    public string creatorVersion;
    public string ifid;
    public float zoom;
    public string format;
    public string formatVersion;
    //Options
    //StyleData
    public TwinePassage[] passages;
    public TwineStyle style;

    //Constant Null TwineStoryData
    public static TwineStorydata Null {
        get {
            TwineStorydata retval = new TwineStorydata ();
            retval.startNode = -1;
            return retval;
        }
    }

    public bool IsNull () {
        return startNode < 0;
    }
}

[System.Serializable]
public struct TwineStyle {
    public string role;
    public string id;
    public string type;
}

[System.Serializable]
public struct TwinePassage {
    public Vector2 position;
    public Vector2 size;
    public string name;
    public int pid;
    public string[] tags;
    public string[] lines;

    public static TwinePassage Null {
        get {
            TwinePassage retval = new TwinePassage ();
            retval.pid = -1;
            return retval;
        }
    }

    public bool IsNull () {
        return pid < 0;
    }
}

public class TwineStory : ScriptableObject {
    public TwineStorydata storyData;
    public TwinePassage GetPassage (string name) {
        for (int i = 0; i < storyData.passages.Length; i++) {
            if (storyData.passages[i].name == name) {
                return storyData.passages[i];
            }
        }
        return TwinePassage.Null;
    }

    public bool ContainsPassage(string value){
        return !GetPassage(value).IsNull() && TwinePassage.Null.IsNull();
    }
}