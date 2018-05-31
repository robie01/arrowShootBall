using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	private float forceX, forceY;
	private bool moveLeft = true;
	private Rigidbody2D body;

	[SerializeField]
	private GameObject originalBall;

	private GameObject ball1, ball2;

	private BallScript ball1Script, ball2Script;

	[SerializeField]
	private AudioClip[] clip;

	[SerializeField]
	private GameObject[] collectables;

	void Awake(){
		if (this.gameObject.tag == "SmallestBall") {
			GameplayController.smallBallsCount++;
		}

		body = GetComponent<Rigidbody2D> ();
		SetBallSpeed ();
		InstantiateBalls ();

	}

	void OnEnable(){
		PlayerScript.explode += Explode;
	}

	void OnDisable(){
		PlayerScript.explode -= Explode;
	}

	void Start () {
	
	}

	void Update () {
		MoveBall ();
	}

	void InstantiateBalls(){
		if(this.gameObject.tag != "SmallestBall"){
			ball1 = Instantiate (originalBall);
			ball2 = Instantiate (originalBall);

			ball1Script = ball1.GetComponent<BallScript> ();
			ball2Script = ball2.GetComponent<BallScript> ();

			ball1.SetActive (false);
			ball2.SetActive (false);
		}
	}

	public void SetMoveLeft(bool flag){
		moveLeft = flag;
	}

	void MoveBall(){
		Vector3 temp = transform.position;
		temp.x += moveLeft ? -(forceX * Time.deltaTime) : (forceX * Time.deltaTime);
		transform.position = temp;
	}

	void AddScoreAndCoins(string tag){
		switch(tag){
		case "LargestBall":
			GameplayController.instance.coins += Random.Range (15, 20);
			GameplayController.instance.score += Random.Range (600, 700);
			break;
		case "LargeBall":
			GameplayController.instance.coins += Random.Range (13, 18);
			GameplayController.instance.score += Random.Range (500, 600);
			break;
		case "MediumBall":
			GameplayController.instance.coins += Random.Range (11, 16);
			GameplayController.instance.score += Random.Range (400, 500);
			break;
		case "SmallBall":
			GameplayController.instance.coins += Random.Range (10, 15);
			GameplayController.instance.score += Random.Range (300, 400);
			break;
		case "SmallestBall":
			GameplayController.instance.coins += Random.Range (9, 14);
			GameplayController.instance.score += Random.Range (200, 300);
			break;
		}
	}

	void InitNewBalls(){
		Vector3 position = transform.position;

		ball1.transform.position = position;
		ball1Script.SetMoveLeft(true);
		ball1.SetActive (true);

		ball2.transform.position = position;
		ball2Script.SetMoveLeft(false);
		ball2.SetActive (true);

		if(this.gameObject.tag != "SmallestBall"){
			if(transform.position.y > 1 && transform.position.y < 1.3f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if(transform.position.y > 1.3f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
			} else if (transform.position.y > 1f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}

		InitializeCollectables (transform.position);
		AddScoreAndCoins (this.gameObject.tag);
		gameObject.SetActive (false);
	}

	public void Explode(bool touchedGoldBall){
		StartCoroutine (ExplodeBalls (touchedGoldBall));
	}

	IEnumerator ExplodeBalls(bool touchedGoldBall){
		if(this.gameObject.tag == "LargestBall"){
			yield return null;
		} else {
			yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.5f));
		}

		if(this.gameObject.tag != "SmallestBall"){
			Vector3 position = transform.position;

			ball1.transform.position = position;
			ball1Script.SetMoveLeft(true);
			ball1.SetActive (true);

			ball2.transform.position = position;
			ball2Script.SetMoveLeft(false);
			ball2.SetActive (true);

			if(transform.position.y > 1 && transform.position.y < 1.3f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if(transform.position.y > 1.3f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
			} else if (transform.position.y > 1f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}

		if(touchedGoldBall){
			if(this.gameObject.tag != "SmallestBall"){
				ball1Script.Explode (true);
				ball2Script.Explode (true);
			} else {
				GameplayController.instance.CountSmallBalls ();
			}
			this.gameObject.SetActive (false);
		} else {
			if(this.gameObject.tag != "SmallestBall"){
				ball1Script.Explode (false);
				ball2Script.Explode (false);
				this.gameObject.SetActive (false);
			}
		}
	}

	void InitializeCollectables(Vector3 position){
		if(this.gameObject.tag != "SmallestBall"){
			int chance = Random.Range (0, 60);
			if (chance >= 0 && chance < 21) {
				Instantiate (collectables [Random.Range (4, collectables.Length)], position, Quaternion.identity);
			} else if (chance >= 21 && chance < 36) {
				Instantiate (collectables [Random.Range (0, 4)], position, Quaternion.identity);
			}
		}
	}

	void SetBallSpeed(){
		forceX = 2.5f;
		switch(gameObject.tag){
		case "LargestBall":
			forceY = 11.5f;
			break;
		case "LargeBall":
			forceY = 10.5f;
			break;
		case "MediumBall":
			forceY = 9.5f;
			break;
		case "SmallBall":
			forceY = 9f;
			break;
		case "SmallestBall":
			forceY = 8.5f;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "FirstArrow" || other.tag == "SecondArrow" || other.tag == "FirstStickyArrow" || other.tag == "SecondStickyArrow"){
			if(gameObject.tag != "SmallestBall"){
				InitNewBalls ();
			} else {
				GameplayController.instance.CountSmallBalls ();
			}
			AudioSource.PlayClipAtPoint (clip[Random.Range(0,clip.Length)], transform.position);
			this.gameObject.SetActive (false);
		}

		if(other.tag == "UnbreakableBrickTop" || other.tag == "BreakableBrickTop" || other.tag == "UnbreakableBrickTopVertical"){
			body.velocity = new Vector2 (0, 5);
		} else if(other.tag == "UnbreakableBrickBottom" || other.tag == "BreakableBrickBottom" || other.tag == "UnbreakableBrickBottomVertical"){
			body.velocity = new Vector2 (0, -2);
		} else if(other.tag == "UnbreakableBrickLeft" || other.tag == "BreakableBrickLeft" || other.tag == "UnbreakableBrickLeftVertical"){
			moveLeft = false;
		} else if(other.tag == "UnbreakableBrickRight" || other.tag == "BreakableBrickRight" || other.tag == "UnbreakableBrickRightVertical"){
			moveLeft = true;
		}

		if(other.tag == "BottomBrick"){
			body.velocity = new Vector2 (0, forceY);
		}
		if(other.tag == "LeftBrick"){
			moveLeft = false;
		}
		if(other.tag == "RightBrick"){
			moveLeft = true;
		}

		if (other.gameObject.tag == "Player") {
			if (PlayerScript.instance.hasShield) {
				PlayerScript.instance.DestroyShield ();
			} else {
				if (!PlayerScript.instance.isInvincible) {
					Destroy (other.gameObject);
					GameplayController.instance.PlayerDied ();
				}
			}
		}
	}
}
