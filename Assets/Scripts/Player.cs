using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour
{
	[SerializeField] private Human _human;
	[SerializeField] private UserCamera _camera;

	private float _duration;
	private Animator _anim;
	private Cell _cell;
	private Vector3 _solidID;
	private KeyCode _key = KeyCode.UpArrow;
	private KeyCode _prevKey = KeyCode.UpArrow;

	private Room _activeRoom;
	private bool _isInput = false;
	private bool _isReflection = false;
	private int _moveFinger = 20;
	private Vector2 _fingerPos;

	// callback - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	private void Start ()
	{
		var common = Common.Instance;
		_duration = common.GetDuration ();
		DOTween.Init ();
	}

	private void Update ()
	{
		if (!_isInput) {

			// touch interaction
			if (Input.touchCount > 0) {
			
				Touch touch = Input.GetTouch (0);
				if (touch.phase == TouchPhase.Moved) {
					_fingerPos = Input.GetTouch (0).deltaPosition;

					if (_fingerPos.y > _moveFinger) {
						_isInput = true;
						_key = KeyCode.UpArrow;
						_fingerPos = Vector2.zero;
					} else if (_fingerPos.y < -(_moveFinger + 5)) {
						_isInput = true;
						_key = KeyCode.DownArrow;
						_fingerPos = Vector2.zero;
					} else if (_fingerPos.x < -_moveFinger) {
						_isInput = true;
						_key = KeyCode.LeftArrow;
						_fingerPos = Vector2.zero;
					} else if (_fingerPos.x > _moveFinger) {
						_isInput = true;
						_key = KeyCode.RightArrow;
						_fingerPos = Vector2.zero;
					}
				}
			} 
			// keyboard push interaction
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				_key = KeyCode.UpArrow;
				_isInput = true;
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				_key = KeyCode.DownArrow;
				_isInput = true;
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				_key = KeyCode.RightArrow;
				_isInput = true;
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				_key = KeyCode.LeftArrow;
				_isInput = true;
			}
		}
	}

	// public function  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	public IEnumerator SequenceInit (Room room)
	{
		_activeRoom = room;
//		var centerCell = _activeRoom.GetEnterCell ();
//		Debug.Log ("-----" + centerCell);

//		_cell = centerCell;
//		_cell.SetObject (transform);
		yield break;
	}

	public void Action ()
	{
//		if (_isInput && _key == _prevKey) {
//			Attack ();
//		} 

		Move ();
		_isInput = false;
		_prevKey = _key;
	}

	// private function  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	private void Move ()
	{
		Cell advanceCell = GetAdvanceCell (_key);

		var ah = GetHeight (advanceCell.GetSolidID ());
		var ph = GetHeight (_cell.GetSolidID ());

		switch (CheckCellType (advanceCell)) {

		// tile check - - - - - - - -
		case Common.CellType.Tile:
			MoveRun (CheckHeightRefrection (ph, ah, _key));
			break;

		case Common.CellType.Enter:
			MoveRun (CheckHeightRefrection (ph, ah, _key));
			break;

		case Common.CellType.Exit:
			MoveRun (CheckHeightRefrection (ph, ah, _key));
			break;
		// wall check - - - - - - - -
		case Common.CellType.Wall:
			MoveClimb (GetClimbCell ());
			break;
		}
	}

	private void MoveRun (Cell cell)
	{
		if (_isReflection) {
			_key = KeyReflection (_key);
			_isReflection = false;
		}

		_human.Run (_key);
		var v3 = cell.transform.position;
		transform.DOMove (v3, _duration);
		_camera.Move (v3, _duration * 5);

		_cell = cell;
		_cell.SetObject (transform);
	}

	private void MoveClimb (Cell cell)
	{
		if (_isReflection) {
			_key = KeyReflection (_key);
			_isReflection = false;
		}

		_human.Climb (_key);
		var v3 = cell.transform.position;
		transform.DOMove (v3, _duration);
		_camera.Move (v3, _duration * 5);

		_cell = cell;
		_cell.SetObject (transform);
	}

	private void Attack ()
	{
		_human.Attack (_key);
	}

	// private utility  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	private Common.CellType CheckCellType (Cell cell)
	{
		return cell.GetCellType ();
	}

	private Cell CheckHeightRefrection (int ph, int ah, KeyCode key)
	{
		Cell cell;
		if (ah <= ph + 2) {
			cell = GetAdvanceCell (key);
		} else {
			_key = KeyReflection (key);
			cell = GetAdvanceCell (key);
		}
		return cell;
	}

	private Cell GetAdvanceCell (KeyCode key)
	{
		var id = GetAdvanceSolidID (key);
		var h = GetHeight (id);
		var cell = _activeRoom.GetCell (new Vector3 (id.x, h, id.z));
		return cell;
	}

	private Cell GetClimbCell ()
	{
		var id = GetClimbSolidID ();
		var cell = _activeRoom.GetCell (id);
		return cell;
	}

	private KeyCode KeyReflection (KeyCode reflectionKey)
	{
		KeyCode key;
		switch (reflectionKey) {
		case KeyCode.UpArrow:
			key = KeyCode.DownArrow;
			break;
		case KeyCode.DownArrow:
			key = KeyCode.UpArrow;
			break;
		case KeyCode.RightArrow:
			key = KeyCode.LeftArrow;
			break;
		case KeyCode.LeftArrow:
			key = KeyCode.RightArrow;
			break;
		default:
			key = KeyCode.UpArrow;
			break;
		}
		return key;
	}

	private int GetHeight (Vector3 v3)
	{
		var x = v3.x;
		var z = v3.z;
		var limitY = _activeRoom.GetLimit ().y;
		var countY = 0;
		for (var i = 0; i < limitY; i++) {

			var v3h = new Vector3 (x, i, z);
			var cellH = _activeRoom.GetCell (v3h);

//			Debug.Log ("cellH" + cellH);
			var cellHType = cellH.GetCellType ();

			if (cellHType == Common.CellType.Tile) {
				countY++;
			} else if (cellHType == Common.CellType.Wall) {
				countY++;
			} else if (cellHType == Common.CellType.Enter) {
				countY++;
			} else if (cellHType == Common.CellType.Exit) {
				countY++;
			}
		}
		var fixY = countY - 1;
		return fixY;
	}

	private Vector3 GetAdvanceSolidID (KeyCode key)
	{
		var limit = _activeRoom.GetLimit ();
		var playerID = _cell.GetSolidID ();

		int x = (int)playerID.x;
		int y = (int)playerID.y;
		int z = (int)playerID.z;

		Vector3 advanceSolidID;

		switch (key) {

		case KeyCode.UpArrow:
			int front = (int)(z + 1);
			int fixFZ = GetInRoomRange (z, front, (int)limit.z);

			if (front != fixFZ) {
				_isReflection = true;
			}

			advanceSolidID = new Vector3 (x, y, fixFZ);
			break;

		case KeyCode.DownArrow:
			int back = (int)(z - 1);
			int fixBZ = GetInRoomRange (z, back, (int)limit.z);

			if (back != fixBZ) {
				_isReflection = true;
			}

			advanceSolidID = new Vector3 (x, y, fixBZ);
			break;

		case KeyCode.RightArrow:
			int right = (int)(x + 1);
			int fixRX = GetInRoomRange (x, right, (int)limit.x);

			if (right != fixRX) {
				_isReflection = true;
			}

			advanceSolidID = new Vector3 (fixRX, y, z);
			break;

		case KeyCode.LeftArrow:
			int left = (int)(x - 1);
			int fixLX = GetInRoomRange (x, left, (int)limit.x);

			if (left != fixLX) {
				_isReflection = true;
			}

			advanceSolidID = new Vector3 (fixLX, y, z);
			break;

		default:
			advanceSolidID = new Vector3 (x, y, z);
			break;

		}
		return advanceSolidID;
	}

	private Vector3 GetClimbSolidID ()
	{
		var limit = _activeRoom.GetLimit ();
		var sid = _cell.GetSolidID ();
		int x = (int)sid.x;
		int y = (int)sid.y;
		int z = (int)sid.z;
		int height = (int)(y + 1);
		int limitY = (int)limit.y;
		int fixY = GetInRoomRange (x, height, limitY);

		if (height != fixY) {
			_isReflection = true;
		}

		Vector3 climbeSolidID = new Vector3 (x, fixY, z);
		return climbeSolidID;
	}

	private int GetInRoomRange (int current, int advance, int limit)
	{
		int fix = advance;
		if (advance >= limit)
			fix = current - 1;
		else if (advance < 0)
			fix = current + 1;
		return fix;
	}
}