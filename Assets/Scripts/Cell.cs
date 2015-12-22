using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	private Transform _obj;
	private Vector3 _solidID;
	private Common.CellType _type;
	private Vector3 _margen = new Vector3 (2, 0, 2);

	// Use this for initialization
	private void Start ()
	{
		_obj = null;
	}
	
	// Update is called once per frame
	private void Update ()
	{
		
	}

	public Common.CellType GetCellType ()
	{
		return _type;
	}

	public void SetCellType (int type)
	{
		transform.gameObject.SetActive (false);

		switch (type) {
		case 0:
			_type = Common.CellType.Empty;
			transform.gameObject.SetActive (false);
			break;

		case 1:
			_type = Common.CellType.Active;
			break;

		case 2:
			_type = Common.CellType.Wall;
			break;

		case 3:
			_type = Common.CellType.Tile;
			break;

		case 4:
			_type = Common.CellType.Enter;
			break;

		case 5:
			_type = Common.CellType.Exit;
			break;

		default:
			_type = Common.CellType.Empty;
			transform.gameObject.SetActive (false);
			break;
		}
	}

	public void SetSolidID (Vector3 vec3)
	{
		_solidID = vec3;
		var pos = new Vector3 (_margen.x + (_solidID.x * 4), _solidID.y * 2, _margen.z + (_solidID.z * 4));
		transform.position = pos;
	}

	public Vector3 GetSolidID ()
	{
		return _solidID;
	}

	public void SetObject (Transform obj)
	{
		_obj = obj;
	}

	public Transform GetObject ()
	{
		return _obj;
	}

	public void RemoveObject ()
	{
		_obj = null;
	}

}
