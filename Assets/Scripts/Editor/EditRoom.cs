using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditRoom : EditorWindow
{
	[MenuItem ("Window/Room Editor")]
	public static void Open ()
	{
		var window = EditorWindow.GetWindow<EditRoom> ("Room Editor");
		window.Show ();
	}

	public Marker marker;
	public Object outputFolder;
	public Object prefab;
	private Vector2 _scrollPosition;

	private void OnGUI ()
	{
		_scrollPosition = EditorGUILayout.BeginScrollView (_scrollPosition);
		if (SetMarker () && SetOutputFolder () && SetPrefab ()) {
			MarkerAnalysis ();
		}
		EditorGUILayout.EndScrollView ();
	}

	private bool SetPrefab ()
	{
		LineMarker ();
		prefab = EditorGUILayout.ObjectField ("Prefab", prefab, typeof(Object), true);
		if (prefab != null)
			return true;
		else
			return false;
	}

	private bool SetOutputFolder ()
	{
		LineMarker ();
		outputFolder = EditorGUILayout.ObjectField ("Output Folder", outputFolder, typeof(Object), false);
		if (outputFolder != null)
			return true;
		else
			return false;
	}

	private bool SetMarker ()
	{
		LineMarker ();
		marker = (Marker)EditorGUILayout.ObjectField ("Marker", marker, typeof(Marker), true);
		if (marker != null)
			return true;
		else
			return false;
	}

	private void MarkerAnalysis ()
	{
		if (GUILayout.Button ("Generate RoomDefine")) {
			
			var cellList = new List<Vector4> ();
			var list = marker.GetCellList ();

			for (int i = 0; i < list.Count; i++) {
				var mark = list [i];
				var markID = mark.GetSolidID ();
				var x = markID.x;
				var y = markID.y;
				var z = markID.z;
				var w = (int)mark.GetCellType ();
				Vector4 cellinfo = new Vector4 (x, y, z, w);
				cellList.Add (cellinfo);
			}

			var roomDefine = ScriptableObject.CreateInstance<RoomDefine> ();
			roomDefine.listCell = cellList;
			roomDefine.prefabName = prefab.name;
			roomDefine.limit = marker.GetLimit ();
			var outputPath = AssetDatabase.GetAssetPath (outputFolder);
			string assetpath = AssetDatabase.GenerateUniqueAssetPath (outputPath + "/" + prefab.name + ".asset");
			AssetDatabase.CreateAsset (roomDefine, assetpath);
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = roomDefine;
		}
	}

	private void LineMarker ()
	{
		GUILayout.Box ("", GUILayout.Width (this.position.width), GUILayout.Height (1));
	}
}