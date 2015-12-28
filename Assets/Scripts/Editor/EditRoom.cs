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

	private Marker _marker;
	private Object _outputFolder;
	private Object _prefab;
	private Vector2 _scrollPosition;
	private Common.RoomType _roomType;

	private void OnGUI ()
	{
		_scrollPosition = EditorGUILayout.BeginScrollView (_scrollPosition);
		if (SetMarker () && SetOutputFolder () && SetPrefab ()) {
			SetType ();
			_markerAnalysis ();
		}
		EditorGUILayout.EndScrollView ();
	}

	private void SetType ()
	{
		EditorLine ();
		_roomType = (Common.RoomType)EditorGUILayout.EnumPopup ("roomType", _roomType);
	}

	private bool SetPrefab ()
	{
		EditorLine ();
		_prefab = EditorGUILayout.ObjectField ("prefab", _prefab, typeof(Object), true);
		if (_prefab != null)
			return true;
		else
			return false;
	}

	private bool SetOutputFolder ()
	{
		EditorLine ();
		_outputFolder = EditorGUILayout.ObjectField ("Output Folder", _outputFolder, typeof(Object), false);
		if (_outputFolder != null)
			return true;
		else
			return false;
	}

	private bool SetMarker ()
	{
		EditorLine ();
		_marker = (Marker)EditorGUILayout.ObjectField ("marker", _marker, typeof(Marker), true);
		if (_marker != null)
			return true;
		else
			return false;
	}

	private void _markerAnalysis ()
	{
		if (GUILayout.Button ("Generate RoomDefine")) {
			
			var cellList = new List<Vector4> ();
			var list = _marker.GetCellList ();

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

			var define = ScriptableObject.CreateInstance<RoomDefine> ();
			define.listCell = cellList;
			define.prefabName = _prefab.name;
			define.limit = _marker.GetLimit ();

			var outputPath = AssetDatabase.GetAssetPath (_outputFolder);
			string assetpath = AssetDatabase.GenerateUniqueAssetPath (outputPath + "/" + _prefab.name + ".asset");
			AssetDatabase.CreateAsset (define, assetpath);
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = define;
		}
	}

	private void EditorLine ()
	{
		GUILayout.Box ("", GUILayout.Width (this.position.width), GUILayout.Height (1));
	}
}