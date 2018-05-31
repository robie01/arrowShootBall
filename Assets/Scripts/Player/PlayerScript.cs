using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public static PlayerScript instance;

	private float speed = 8.0f;
	private float maxVelocity = 4.0f;

	private Rigidbody2D body;
	private Animator anim;

	[SerializeField]
	private GameObject[] arrows;

	private float height;
	private bool shootOnce, shootTwice;
	private bool canWalk, moveLeft, moveRight;

	[SerializeField]
	private AudioClip shootClip;

	private Button shootBtn;

	[SerializeField]
	private GameObject shield;

	private string arrow;

	public bool hasShield, isInvincible, singleArrows, doubleArrows, singleStickyArrows, doubleStickyArrows, shootFirstArrow, shootSecondArrow;

	public delegate void Explode(bool touchedGoldBall);
	public static event Explode explode;


	void Awake(){
		MakeInstance ();
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		height = -Camera.main.orthographicSize - 0.5f;
		Initialize ();
	}

	void MakeInstance(){
		if(instance == null){
			instance = this;
		}
	}

	void Start () {
		shootBtn = GameObject.FindGameObjectWithTag ("ShootButton").GetComponent<Button> ();
		shootBtn.onClick.AddListener (() => ShootArrow ());
	}

	void FixedUpdate () {
		MovementKeyboard ();
		MovePlayer ();
	}

	void Initialize(){
		canWalk = true;

		switch(GameController.instance.selectedWeapon){
		case 0:
			arrow = "Arrow";
			shootOnce = true;
			shootTwice = false;

			singleArrows = true;
			doubleArrows = false;
			singleStickyArrows = false;
			doubleStickyArrows = false;
			break;
		case 1:
			arrow = "Arrow";
			shootOnce = true;
			shootTwice = true;

			singleArrows = false;
			doubleArrows = true;
			singleStickyArrows = false;
			doubleStickyArrows = false;
			break;
		case 2:
			arrow = "StickyArrow";
			shootOnce = true;
			shootTwice = false;

			singleArrows = false;
			doubleArrows = false;
			singleStickyArrows = true;
			doubleStickyArrows = false;
			break;
		case 3:
			arrow = "StickyArrow";
			shootOnce = true;
			shootTwice = true;

			singleArrows = false;
			doubleArrows = false;
			singleStickyArrows = false;
			doubleStickyArrows = true;
			break;
		}

		Vector3 bottomBrick = GameObject.FindGameObjectWithTag ("BottomBrick").transform.position;
		Vector3 temp = transform.position;

		switch(gameObject.name){
		
		case "Pirate(Clone)":
			temp.y = bottomBrick.y + 1.27f;
			break;
		case "Player(Clone)":
			temp.y = bottomBrick.y + 1.19f;
			break;
		}

		transform.position = temp;
	}

	public void SetShootOnce(){
		shootOnce = true;
		shootFirstArrow = false;
	}

	public void SetShootTwice(){
		if (doubleArrows || doubleStickyArrows) {
			shootTwice = true;
		}
		shootSecondArrow = false;
	}

	public void ShootArrow(){
		if(GameplayController.instance.levelInProgress){
			if (shootOnce) {
				if (arrow == "Arrow") {
					Instantiate (arrows [0], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				} else if (arrow == "StickyArrow") {
					Instantiate (arrows [2], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				}
				shootOnce = false;
				shootFirstArrow = true;
				StartCoroutine (PlayShootAnimation ());
			} else if (shootTwice) {
				if (arrow == "Arrow") {
					Instantiate (arrows [1], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				} else if (arrow == "StickyArrow") {
					Instantiate (arrows [3], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
				}
				shootTwice = false;
				shootSecondArrow = true;
				StartCoroutine (PlayShootAnimation ());
			}

		}
	}

	IEnumerator PlayShootAnimation(){
		canWalk = false;
		shootBtn.interactable = false;
		anim.SetBool ("Shoot", true);
		string clipName = name + " Shoot";
		anim.Play (clipName);
		AudioSource.PlayClipAtPoint (shootClip, transform.position);
		yield return new WaitForSeconds (0.15f);
		anim.SetBool ("Shoot", false);
		shootBtn.interactable = true;
		canWalk = true;
	}

	public void DestroyShield(){
		StartCoroutine (SetPlayerInvincible ());
		hasShield = false;
		shield.SetActive (false);
	}

	IEnumerator SetPlayerInvincible(){
		isInvincible = true;
		yield return new WaitForSeconds (3f);
		isInvincible = false;
	}

	public void StopMoving(){
		moveLeft = moveRight = false;
		anim.SetBool ("Walk", false);
	}

	public void MovePlayerLeft(){
		moveLeft = true;
		moveRight = false;
	}

	public void MovePlayerRight(){
		moveLeft = false;
		moveRight = true;
	}

	public void MovePlayer(){
		if(GameplayController.instance.levelInProgress){
			if(moveLeft){
				MoveLeft ();
			}
			if(moveRight){
				MoveRight ();
			}
		}
	}

	void MoveRight(){
		if (canWalk) {
			float force = 0.0f;
			float velocity = Mathf.Abs (body.velocity.x);

			if (velocity < maxVelocity) {
				force = speed;
			}
			Vector3 scale = transform.localScale;
			scale.x = 1.0f;
			transform.localScale = scale;

			body.AddForce (new Vector2 (force, 0));
			anim.SetBool ("Walk", velocity > 0);
		}
	}

	void MoveLeft(){
		if (canWalk) {
			float force = 0.0f;
			float velocity = Mathf.Abs (body.velocity.x);

			if (velocity < maxVelocity) {
				force = -speed;
			}
			Vector3 scale = transform.localScale;
			scale.x = -1.0f;
			transform.localScale = scale;

			body.AddForce (new Vector2 (force, 0));
			anim.SetBool ("Walk", velocity > 0);
		}
	}

	void MovementKeyboard(){
		if(canWalk){
			float force = 0.0f;
			float velocity = Mathf.Abs (body.velocity.x);

			float h = Input.GetAxis ("Horizontal");

			if(h > 0){
				if(velocity < maxVelocity){
					force = speed;
				}
				Vector3 scale = transform.localScale;
				scale.x = 1.0f;
				transform.localScale = scale;
			} else if (h < 0){
				if(velocity < maxVelocity){
					force = -speed;
				}
				Vector3 scale = transform.localScale;
				scale.x = -1.0f;
				transform.localScale = scale;
			}

			body.AddForce (new Vector2 (force, 0));

			anim.SetBool ("Walk", velocity > 0);
		}
	}

	void OnTriggerEnter2D(Collider2D target){
		if (target.tag == "SingleArrow") {
			if (!singleArrows) {
				arrow = "Arrow";
				if(!shootFirstArrow){
					shootOnce = true;
				}
				shootTwice = false;

				singleArrows = true;
				doubleArrows = false;
				singleStickyArrows = false;
				doubleStickyArrows = false;
			}
		}
		if (target.tag == "DoubleArrow") {
			if (!doubleArrows) {
				arrow = "Arrow";
				if(!shootFirstArrow){
					shootOnce = true;
				}
				if(!shootSecondArrow){
					shootTwice = true;
				}


				singleArrows = false;
				doubleArrows = true;
				singleStickyArrows = false;
				doubleStickyArrows = false;
			}
		}
		if (target.tag == "SingleStickyArrow") {
			if (!singleStickyArrows) {
				arrow = "StickyArrow";
				if(!shootFirstArrow){
					shootOnce = true;
				}
				shootTwice = false;

				singleArrows = false;
				doubleArrows = false;
				singleStickyArrows = true;
				doubleStickyArrows = false;
			}
		}
		if (target.tag == "DoubleStickyArrow") {
			if (!doubleStickyArrows) {
				arrow = "StickyArrow";
				if(!shootFirstArrow){
					shootOnce = true;
				}
				if(!shootSecondArrow){
					shootTwice = true;
				}


				singleArrows = false;
				doubleArrows = false;
				singleStickyArrows = false;
				doubleStickyArrows = true;
			}
		}
		if (target.tag == "Watch") {
			GameplayController.instance.levelTime += Random.Range (10, 20);
		}
		if (target.tag == "Shield") {
			hasShield = true;
			shield.SetActive (true);
		}
		if (target.tag == "Dynamite") {
			if (explode != null) {
				explode (false);
			}
		}
	}
}
