using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
	[SerializeField] private Cell _cellPrefab;
	private RoomDefine _define;
	private Cell[,,] _listCell;
	private Vector3 _limit;
	private Cell _enter;
	private Cell _exit;

	public IEnumerator SetRoomDefine (RoomDefine define)
	{
		_define = define;
		_limit = _define.limit;

		var listDefine = _define.listCell;
		yield return StartCoroutine (CreateCells (listDefine));
		yield return StartCoroutine (CheckCells ());
		yield break;
		//CheckCells ();
	}

	public Vector3 GetLimit ()
	{
		return _limit;
	}

	public Cell GetCell (Vector3 v3)
	{
		int x = (int)v3.x;
		int y = (int)v3.y;
		int z = (int)v3.z;

		var cell = (Cell)_listCell [x, y, z];
		return cell;
	}

	public Cell GetEnterCell ()
	{
		return _enter;
	}

	private IEnumerator CreateCells (List<Vector4> listDefine)
	{
		_listCell = new Cell [(int)_limit.x, (int)_limit.y, (int)_limit.z];

		for (var i = 0; i < listDefine.Count; i++) {
			var v4 = listDefine [i];

			int x = (int)v4.x;
			int y = (int)v4.y;
			int z = (int)v4.z;
			int w = (int)v4.w;

			var cell = Instantiate (_cellPrefab);
			cell.name = "cell";
			cell.transform.SetParent (transform);
			cell.SetSolidID (new Vector3 (x, y, z)); 
			cell.SetCellType (w);

			if (w == 4) {
				_enter = cell;
				Debug.Log ("Enter" + cell.GetSolidID ());
			} else if (w == 5) {
				_exit = cell;
				Debug.Log ("Exit" + cell.GetSolidID ());
			}

			_listCell [x, y, z] = cell;
		}

		yield break;
	}

	private IEnumerator CheckCells ()
	{
		var x = (int)_limit.x;
		var y = (int)_limit.y;
		var z = (int)_limit.z;

		for (var i = 0; i < x; i++) {
			for (var j = 0; j < y; j++) {
				for (var q = 0; q < z; q++) {
					var cell = (Cell)_listCell [i, j, q];
					Debug.Log ("list" + cell.GetSolidID ());
				}
			}
		}
		yield break;
	}
}
