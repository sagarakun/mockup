using UnityEngine;
using System.Collections;

public class Common
{
	public enum CellType
	{
		Empty,
		Active,
		Wall,
		Tile,
		Enter,
		Exit,
		Hole
	}

	public enum MapType
	{
		Basic,
		StairsUP,
		StairsDown,
		HighFloar,
		LowFloar,
	}

	public enum RoomType
	{
		In,
		Out,
		Enter,
		Exit,
	}

	public enum InType
	{
		Room,
		Corridor,
		Stairs,
	}

	public enum AnimType
	{
		Idle,
		Run,
		Climb,
		Attack1,
		Attack2,
		Attack3,
		Attack4
	}

	private static Common _instance;

	private Common ()
	{ 
		Debug.Log ("Create Common instance.");
	}

	public static Common Instance {
		get {
			if (_instance == null) {
				Application.targetFrameRate = 60;
				_instance = new Common ();
			}
			return _instance;
		}
	}

	public float GetDuration ()
	{
		return  0.2f;
	}

	public string GetDefineString ()
	{
		return "Define";
	}

	public string GetPrefabsString ()
	{
		return "Prefabs";
	}

	public string GetRoomTypeString (MapType type)
	{
		string str;
		switch (type) {

		case MapType.Basic:
			str = "Basic";
			break;
		case MapType.HighFloar:
			str = "HighFloar";
			break;
		case MapType.LowFloar:
			str = "LowFloar";
			break;
		case MapType.StairsDown:
			str = "StairsDown";
			break;
		case MapType.StairsUP:
			str = "StairsUP";
			break;
		default:
			str = "Basic";
			break;
		}
		return str;
	}

	public string GetAnimTypeString (AnimType type)
	{
		string str;
		switch (type) {

		case AnimType.Idle:
			str = "IDLE";
			break;
		case AnimType.Run:
			str = "RUN";
			break;
		case AnimType.Climb:
			str = "CLIMB";
			break;
		case AnimType.Attack1:
			str = "ATTACK1";
			break;
		case AnimType.Attack2:
			str = "ATTACK2";
			break;
		case AnimType.Attack3:
			str = "ATTACK3";
			break;
		case AnimType.Attack4:
			str = "ATTACK4";
			break;

		default:
			str = "IDLE";
			break;
		}
		return str;
	}

	public string GetCellTypeString (CellType type)
	{
		string str;
		switch (type) {
		case CellType.Empty:
			str = "Empty";
			break;
		case CellType.Active:
			str = "Active";
			break;
		case CellType.Wall:
			str = "Wall";
			break;
		case CellType.Tile:
			str = "Tile";
			break;
		case CellType.Enter:
			str = "Enter";
			break;
		case CellType.Exit:
			str = "Exit";
			break;
		case CellType.Hole:
			str = "Hole";
			break;
		default:
			str = "Empty";
			break;
		}
		return str;
	}


}