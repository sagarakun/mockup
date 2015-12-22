using UnityEngine;
using System.Collections;

public class Mark : MonoBehaviour
{
	[SerializeField] private Vector3 _solidID;

	private bool _isWall = false;
	private bool _isTile = false;
	private bool _isEnter = false;
	private bool _isExit = false;

	public Common.CellType _type = Common.CellType.Empty;

	public Vector3 GetSolidID ()
	{
		return _solidID;
	}

	public Common.CellType GetCellType ()
	{
		return _type;
	}

	public void CreateMarker (Vector3 v3)
	{
		_solidID = v3;
		var x = 2 + (_solidID.x * 4);
		var y = _solidID.y * 2;
		var z = 2 + (_solidID.z * 4);
		transform.position = new Vector3 (x, y, z);

		var bc = transform.GetComponent<BoxCollider> ();
		bc.isTrigger = true;
	}

	public void Initialize ()
	{
		var r = transform.GetComponent<MeshRenderer> ();
		var m = r.material;

		if (_isTile) {
			if (_isEnter)
				_type = Common.CellType.Enter;
			else if (_isExit)
				_type = Common.CellType.Exit;
			else if (_isWall)
				_type = Common.CellType.Wall;
			else
				_type = Common.CellType.Tile;
		}

		if (_isWall) {
			_type = Common.CellType.Wall;
		}

		switch (_type) {
		case Common.CellType.Empty:
			gameObject.SetActive (false);
			break;
		case Common.CellType.Tile:
			m.color = Color.blue;
			break;
		case Common.CellType.Wall:
			m.color = Color.cyan;
			break;
		default:
			break;
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		var c = Common.Instance;
		if (other.tag == c.GetCellTypeString (Common.CellType.Tile))
			_isTile = true;

		if (other.tag == c.GetCellTypeString (Common.CellType.Wall))
			_isWall = true;

		if (other.tag == c.GetCellTypeString (Common.CellType.Enter))
			_isEnter = true;

		if (other.tag == c.GetCellTypeString (Common.CellType.Exit))
			_isExit = true;
	}
}
