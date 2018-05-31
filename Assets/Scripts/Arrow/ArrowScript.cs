using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	private float speed = 4f;
	private bool isSticky;
	[SerializeField]
	private AudioClip clip;

	void Start () {
		if(gameObject.tag.Contains("Sticky")){
			isSticky = true;
		}
	}

	void Update () {
		if (gameObject.tag == "FirstStickyArrow") {
			if (isSticky) {
				ShootArrow ();
			}
		} else if (gameObject.tag == "SecondStickyArrow") {
			if (isSticky) {
				ShootArrow ();
			}
		} else {
			ShootArrow ();
		}
	}

	void ShootArrow(){
		Vector3 temp = transform.position;
		temp.y += speed * Time.deltaTime;
		transform.position = temp;
	}

	IEnumerator ResetStickyArrow(){
		yield return new WaitForSeconds (2.5f);
		if (gameObject.tag == "FirstStickyArrow") {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().SetShootOnce ();
			gameObject.SetActive (false);
		} else if (gameObject.tag == "SecondStickyArrow") {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().SetShootTwice ();
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "LargestBall" || other.tag == "LargeBall" || other.tag == "MediumBall" || other.tag == "SmallBall" || other.tag == "SmallestBall"
			|| other.tag == "BreakableBrickTop" || other.tag == "BreakableBrickBottom" || other.tag == "BreakableBrickLeft" || other.tag == "BreakableBrickRight"){
			if (gameObject.tag == "FirstArrow" || gameObject.tag == "FirstStickyArrow") {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().SetShootOnce ();
			} else if (gameObject.tag == "SecondArrow" || gameObject.tag == "SecondStickyArrow") {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().SetShootTwice ();
			}
			if(other.tag == "BreakableBrickTop" || other.tag == "BreakableBrickBottom" || other.tag == "BreakableBrickLeft" || other.tag == "BreakableBrickRight"){
				other.GetComponentInParent<BrickScript> ().BreakBrick ();
			}
			gameObject.SetActive (false);
		}
		if(other.tag == "TopBrick" || other.tag == "UnbreakableBrickTop" || other.tag == "UnbreakableBrickBottom" || other.tag == "UnbreakableBrickLeft" 
			|| other.tag == "UnbreakableBrickRIght" || other.tag == "UnbreakableBrickBottomVertical"){
			if (gameObject.tag == "FirstArrow") {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().SetShootOnce ();
				gameObject.SetActive (false);
			} else if (gameObject.tag == "SecondArrow") {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ().SetShootTwice ();
				gameObject.SetActive (false);
			} else {
				speed = 0f;
				AudioSource.PlayClipAtPoint (clip, transform.position);
				StartCoroutine (ResetStickyArrow ());
			}
		}
	}
}
