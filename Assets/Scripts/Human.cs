using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;


public class Human : MonoBehaviour
{
	[SerializeField] private Transform _diff;

	private float _duration;
	private Animator _animator;
	private int[] _listHush;
	private int _currenAttack;

	// Use this for initialization
	private void Awake ()
	{
		var common = Common.Instance;
		_duration = common.GetDuration ();

		_animator = transform.GetComponent<Animator> ();

		var result = Enum.GetValues (typeof(Common.AnimType)).Length;
		_listHush = new int[result];

		GetAnimationHush (Common.AnimType.Run);
		GetAnimationHush (Common.AnimType.Idle);
		GetAnimationHush (Common.AnimType.Climb);
		GetAnimationHush (Common.AnimType.Attack1);
		GetAnimationHush (Common.AnimType.Attack2);
		GetAnimationHush (Common.AnimType.Attack3);
		GetAnimationHush (Common.AnimType.Attack4);

		DOTween.Init ();
	}

	private void GetAnimationHush (Common.AnimType type)
	{
		var common = Common.Instance;
		_listHush [(int)type] = Animator.StringToHash (common.GetAnimTypeString (type));
	}

	public void Run (KeyCode key)
	{
		AnimationStart ((int)Common.AnimType.Run);
		Rotate (key);
	}

	public void Attack (KeyCode key)
	{
		int random = (int)UnityEngine.Random.Range (0, 4);

		if (_currenAttack == random)
			random = (random + 1) % 4;

		switch (random) {
		case 0:
			AnimationStart ((int)Common.AnimType.Attack1);
			break;
		case 1:
			AnimationStart ((int)Common.AnimType.Attack2);
			break;
		case 2:
			AnimationStart ((int)Common.AnimType.Attack3);
			break;
		case 3:
			AnimationStart ((int)Common.AnimType.Attack4);
			break;

		case 4:
			break;

		case 5:
			break;

		case 6:
			break;
		case 7:
			break;
		}

		_currenAttack = random;
		Rotate (key);
	}

	public void Climb (KeyCode key)
	{
		AnimationStart ((int)Common.AnimType.Climb);
		Rotate (key);
	}

	private void AnimationStart (int hush)
	{
		for (var i = 0; i < _listHush.Length; i++) {
			if (hush == i)
				_animator.SetBool (_listHush [i], true);
			else
				_animator.SetBool (_listHush [i], false);
		}
	}

	private void ChangeAnimation (Common.AnimType type)
	{
		
	}

	private void Rotate (KeyCode key)
	{
		Vector3 v3;
		switch (key) {
		case KeyCode.UpArrow:
			v3 = new Vector3 (0, 0, 0);
			break;
		case KeyCode.DownArrow:
			v3 = new Vector3 (0, 180, 0);
			break;
		case KeyCode.RightArrow:
			v3 = new Vector3 (0, 90, 0);
			break;
		case KeyCode.LeftArrow:
			v3 = new Vector3 (0, 270, 0);
			break;
		default:
			v3 = new Vector3 (0, 0, 0);
			break;
		}
		_diff.DOLocalRotate (v3, _duration);
	}
}
