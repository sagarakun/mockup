using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
	[SerializeField] private Player _player;
	private Room _room;
	private float _durartion;

	private void Start ()
	{
		StartCoroutine (SequenceInit ());
	}

	private IEnumerator SequenceInit ()
	{
		yield return StartCoroutine (SequenceInitMap ());
		yield return new WaitForFixedUpdate ();
		yield return StartCoroutine (SequenceTimer ());
		yield break;
	}

	private IEnumerator SequenceInitMap ()
	{
		var common = Common.Instance;
		_durartion = common.GetBaseDuration ();

		yield return StartCoroutine (SequenceCreateRoom ());
		yield return new WaitForFixedUpdate ();

		_player.SetActiveRoom (_room);
		_player.Action ();

		yield break;
	}

	private IEnumerator SequenceCreateRoom ()
	{
		var common = Common.Instance;

		var definePath = common.GetDefineString () + "/room";
		var roomDefine = Resources.Load (definePath) as RoomDefine;

		var prefabsPath = common.GetPrefabsString () + "/" + roomDefine.prefabName;
		var roomPrefab = Resources.Load (prefabsPath) as GameObject;

		var roomObject = Instantiate (roomPrefab);
		roomObject.transform.SetParent (transform);

		_room = roomObject.GetComponent<Room> ();
		_room.name = roomDefine.prefabName;

		yield return StartCoroutine (_room.SetRoomDefine (roomDefine));
		yield break;
	}

	private IEnumerator SequenceTimer ()
	{
		yield return new WaitForSeconds (_durartion);
		yield return StartCoroutine (SequenceTurn ());
		yield return StartCoroutine (SequenceTimer ());
		yield break;
	}

	private IEnumerator SequenceTurn ()
	{
		_player.Action ();
		yield break;
	}
}
