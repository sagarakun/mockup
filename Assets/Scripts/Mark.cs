using UnityEngine;
using System.Collections;

public class Mark : MonoBehaviour
{
	[SerializeField] private Vector3 _solidID;

	private bool _isWall = false;
	private bool _isTile = false;

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
		var x = _solidID.x;
		var y = _solidID.y;
		var z = _solidID.z;

		transform.position = new Vector3 (x, y, z);

		var bc = transform.GetComponent<BoxCollider> ();
		bc.isTrigger = true;
	}

	public void Initialize ()
	{
		var r = transform.GetComponent<MeshRenderer> ();
		var m = r.material;

		if (_isTile) {
			if (_isWall)
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
	}
}
