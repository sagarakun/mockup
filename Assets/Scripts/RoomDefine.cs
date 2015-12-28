using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomDefine : ScriptableObject
{
	public string prefabName;
	public Vector3 limit;
	public List<Vector4> listCell;
	public int roomType;
}
