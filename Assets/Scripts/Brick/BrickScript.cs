using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	private Animator anim;

	void Awake(){
		anim = GetComponent<Animator> ();
	}

	public void BreakBrick(){
		anim.Play ("Break Brick");
	}

	public void DeactivateBrick(){
		gameObject.SetActive (false);
	}
}
