using UnityEngine;
using System.Collections;

public class Collectables : MonoBehaviour {

	private Rigidbody2D myBody;

	void Start () {
		myBody = GetComponent<Rigidbody2D> ();

		if(this.gameObject.tag != "InGameCollectable"){
			Invoke ("Deactivate", Random.Range (2, 6));
		}
	}

	void Deactivate(){
		this.gameObject.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D target){
		if (target.tag == "BottomBrick") {
			Vector3 temp = target.transform.position;
			temp.y += 0.8f;
			transform.position = new Vector2 (transform.position.x, temp.y);
			myBody.isKinematic = true;
		}

		if(target.tag == "Player"){

			if (this.gameObject.tag == "InGameCollectable") {
				GameController.instance.collectedItems [GameController.instance.currentLevel] = true;
				GameController.instance.Save ();

				if (GameplayController.instance != null) {
					if (GameController.instance.currentLevel == 0) {
						GameplayController.instance.score += 1 * 1000;
					} else {
						GameplayController.instance.score += GameController.instance.currentLevel * 1000;
					}
				}
			}

			this.gameObject.SetActive (false);
		}
	}
}
