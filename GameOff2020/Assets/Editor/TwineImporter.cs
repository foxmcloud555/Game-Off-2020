using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

[ScriptedImporter (1, "twine")]
public class TwineImporter : ScriptedImporter {
	public override void OnImportAsset (AssetImportContext ctx) {
		TwineStory so_story = ScriptableObject.CreateInstance<TwineStory> ();
		HtmlDocument document = new HtmlDocument ();
		string path = ctx.assetPath;
		document.Load (path);
		//Debug.Log (path + "HTML Document: " + document.Text);

		HtmlNode html_storydata = document.DocumentNode.SelectNodes ("//tw-storydata").First ();
		TwineStorydata storyData = new TwineStorydata ();
		storyData.name = html_storydata.Attributes["name"].Value;
		storyData.ifid = html_storydata.Attributes["ifid"].Value;
		storyData.startNode = Int32.Parse (html_storydata.Attributes["startnode"].Value);
		storyData.creator = html_storydata.Attributes["creator"].Value;
		storyData.creatorVersion = html_storydata.Attributes["creator-version"].Value;
		storyData.format = html_storydata.Attributes["format"].Value;
		storyData.formatVersion = html_storydata.Attributes["format-version"].Value;
		storyData.zoom = float.Parse (html_storydata.Attributes["zoom"].Value);

		HtmlNode html_style = document.DocumentNode.SelectNodes("//tw-storydata//style").First();
		storyData.style = new TwineStyle();
		storyData.style.id =  html_style.Attributes["id"].Value;
		storyData.style.role =  html_style.Attributes["role"].Value;
		storyData.style.type =  html_style.Attributes["type"].Value;

		HtmlNodeCollection html_passages = document.DocumentNode.SelectNodes ("//tw-storydata//tw-passagedata");
		List<TwinePassage> twinePassages = new List<TwinePassage> ();
		foreach (var item in html_passages) {
			TwinePassage p = new TwinePassage ();
			p.position = StringsToVector2Int (item.Attributes["position"].Value.Split (','));
			p.size = StringsToVector2Int (item.Attributes["size"].Value.Split (','));
			p.name = item.Attributes["name"].Value;
			p.pid = Int32.Parse (item.Attributes["pid"].Value);
			p.lines = item.InnerHtml.Split ('\n');
			for (int i = 0; i < p.lines.Length; i++) {
				p.lines[i] = System.Net.WebUtility.HtmlDecode(p.lines[i]);
			}
			p.tags = item.Attributes["tags"].Value.Split (' ');
			twinePassages.Add (p);
		}
		storyData.passages = twinePassages.ToArray ();

		so_story.storyData = storyData;
		ctx.AddObjectToAsset ("TwineStory", so_story);
		ctx.SetMainObject (so_story);
		//ScriptableObjectUtility.CreateAsset<TwineStory>(ctx.assetPath, ctx.);

	}

	Vector2 StringsToVector2Int (string[] value) {
		return new Vector2 (float.Parse (value[0]), float.Parse (value[0]));;
	}
}