using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public void OnPointerDown(PointerEventData data){
		if (this.gameObject.tag == "MoveLeftButton") {
			PlayerScript.instance.MovePlayerLeft ();
		} else if (this.gameObject.tag == "MoveRightButton") {
			PlayerScript.instance.MovePlayerRight ();
		}
	}

	public void OnPointerUp(PointerEventData data){
		PlayerScript.instance.StopMoving ();
	}
}
