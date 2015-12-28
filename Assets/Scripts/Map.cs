using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
	[SerializeField] private Player _player;
	private Room _room;
	private float _durartion;
	private Coroutine _timerCoroutine;

	private void Start ()
	{
		StartCoroutine (SequenceInit ());
	}

	private IEnumerator SequenceInit ()
	{
		var common = Common.Instance;
		_durartion = common.GetDuration ();

		var definePath = common.GetDefineString () + "/room";
		var roomDefine = Resources.Load (definePath) as RoomDefine;

		var prefabsPath = common.GetPrefabsString () + "/" + roomDefine.prefabName;
		var roomPrefab = Resources.Load (prefabsPath) as GameObject;

		var roomObject = Instantiate (roomPrefab);
		roomObject.transform.SetParent (transform);

		_room = roomObject.GetComponent<Room> ();
		_room.name = roomDefine.prefabName;

		yield return StartCoroutine (_room.SequenceInit (roomDefine));
		yield return StartCoroutine (_player.SequenceInit (_room));

		yield return new WaitForFixedUpdate ();

		_timerCoroutine = StartCoroutine (SequenceTimer ());
		yield return _timerCoroutine;

		yield break;
	}

	private IEnumerator SequenceTimer ()
	{
		yield return new WaitForSeconds (_durartion);
		yield return StartCoroutine (SequenceTurn ());
		_timerCoroutine = StartCoroutine (SequenceTimer ());
		yield return _timerCoroutine;
		yield break;
	}

	private IEnumerator SequenceTurn ()
	{
		_player.Action ();
		yield break;
	}

	private IEnumerator SequenceChangeRoom ()
	{
		StopCoroutine (_timerCoroutine);

		yield return new WaitForFixedUpdate ();
		yield return StartCoroutine (SequenceTimer ());

		yield break;
	}
}
