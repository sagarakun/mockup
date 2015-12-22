using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marker : MonoBehaviour
{
	[SerializeField] private GameObject _markPrefab;
	[SerializeField] private List<Mark> _list;
	[SerializeField] private Vector3 _limit = new Vector3 (20, 20, 20);

	public List<Mark> GetCellList ()
	{
		return _list;
	}

	private void Start ()
	{
		StartCoroutine (SequenceInit ());
	}

	public Vector3 GetLimit ()
	{
		return _limit;
	}

	private IEnumerator CreateMarker ()
	{
		var common = Common.Instance;
		_list = new List<Mark> ();

		var x = _limit.x;
		var y = _limit.y;
		var z = _limit.z;

		for (var i = 0; i < x; i++) {
			for (var j = 0; j < y; j++) {
				for (var k = 0; k < z; k++) {
					var mo = Instantiate (_markPrefab);
					mo.transform.SetParent (transform);
					var mark = mo.GetComponent<Mark> ();
					mark.CreateMarker (new Vector3 (i, j, k));
					_list.Add (mark);
				}
			}
		}
		yield break;
	}

	private IEnumerator CheckMarker ()
	{
		for (var i = 0; i < _list.Count; i++) {
			var mark = _list [i];
			mark.Initialize ();
		}
		yield break;
	}

	private IEnumerator SequenceInit ()
	{
		yield return StartCoroutine (CreateMarker ());
		yield return new WaitForFixedUpdate ();
		yield return StartCoroutine (CheckMarker ());
		yield break;
	}
}
