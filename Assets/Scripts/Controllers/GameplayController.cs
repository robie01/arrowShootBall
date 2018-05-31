using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	[SerializeField]
	private GameObject[] topAndBottomBricks, leftBricks, rightBricks;

	public GameObject panelsBG, levelFinishedPanel, playerDiedPanel, pausePanel;

	private GameObject topBrick, bottomBrick, leftBrick, rightBrick;

	private Vector3 coordinates;

	[SerializeField]
	private GameObject[] players;

	public float levelTime;

	public Text livesText, scoreText, levelTimerText, endLevelScoreText, coundownText, watchVideoText;

	private float countdownTimer = 3.0f;

	public static int smallBallsCount = 0;

	public int lives, score, coins;

	public bool isGamePaused, hasLevelBegan, levelInProgress, countdownLevel;

	[SerializeField]
	private GameObject[] endOfLevelRewards;

	[SerializeField]
	private Button pauseBtn;

	void Awake(){
		MakeInstance ();
		InitBricksAndPlayer ();
	}

	void MakeInstance(){
		if(instance == null){
			instance = this;
		}
	}

	void InitBricksAndPlayer(){

		coordinates = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));

		int index = Random.Range (0, topAndBottomBricks.Length);
		topBrick = Instantiate (topAndBottomBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, -180))) as GameObject;
		bottomBrick = Instantiate (topAndBottomBricks [index]);
		leftBrick = Instantiate (leftBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, -90))) as GameObject;
		rightBrick = Instantiate (rightBricks [index],  new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, 90))) as GameObject;

		topBrick.tag = "TopBrick";

		topBrick.transform.position = new Vector3 (-coordinates.x + 8, coordinates.y + 0.17f, 0);
		bottomBrick.transform.position = new Vector3 (-coordinates.x + 8, -coordinates.y, 0);
		leftBrick.transform.position = new Vector3 (-coordinates.x - 0.17f, coordinates.y + 5, 0);
		rightBrick.transform.position = new Vector3 (coordinates.x + 0.17f, coordinates.y + 5, 0);

		Instantiate (players [GameController.instance.selectedPlayer]);
	}

	void Start(){
		Initialize ();
	}

	void Update(){
		UpdateGameplayController ();
	}

	void Initialize(){
		if(GameController.instance.isGameStartedFromLevelMenu){
			score = 0;
			lives = 2;
			GameController.instance.currentScore = score;
			GameController.instance.currentLives = lives;
			GameController.instance.isGameStartedFromLevelMenu = false;
		} else {
			score = GameController.instance.currentScore;
			lives = GameController.instance.currentLives;
		}

		levelTimerText.text = levelTime.ToString ("F0");
		scoreText.text = "Score x" + score;
		livesText.text = "x" + lives;
		Time.timeScale = 0;
		coundownText.text = countdownTimer.ToString ("F0");
	}

	void UpdateGameplayController(){
		scoreText.text = "Score x" + score;
		if(hasLevelBegan){
			Countdown ();
		}

		if(countdownLevel){
			LevelCountDownTimer ();
		}
	}

	public void SetHasLevelBegan(bool hasLevelBegan){
		this.hasLevelBegan = hasLevelBegan;
	}

	void Countdown(){
		countdownTimer -= 0.01f;
		coundownText.text = countdownTimer.ToString ("F0");
		if(countdownTimer <= 0){
			Time.timeScale = 1;
			hasLevelBegan = false;
			levelInProgress = true;
			countdownLevel = true;
			coundownText.gameObject.SetActive (false);
		}
	}

	void LevelCountDownTimer(){
		if(Time.timeScale == 1){
			levelTime -= Time.deltaTime;
			levelTimerText.text = levelTime.ToString ("F0");

			if(levelTime <= 0){
				lives--;
				GameController.instance.currentScore = score;
				GameController.instance.currentLives = lives;

				if(lives <= 0){
					StartCoroutine (PromptToWatchVideo ());
				} else {
					StartCoroutine (RestartLevel ());
				}
			}
		}
	}

	IEnumerator RestartLevel(){
		levelInProgress = false;
		coins = 0;
		smallBallsCount = 0;
		Time.timeScale = 0;
		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.FadeOut ();
		}
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		SceneController.LoadLevel (SceneController.GetCurrentScene ().name);
		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayFadeIn ();
		}
	}

	public void PlayerDied(){
		countdownLevel = false;
		pauseBtn.interactable = false;
		levelInProgress = false;

		smallBallsCount = 0;
		lives--;
		GameController.instance.currentScore = score;
		GameController.instance.currentLives = lives;
		if(lives <= 0){
			StartCoroutine (PromptToWatchVideo ());
		} else {
			StartCoroutine (RestartLevel ());
		}
	}

	IEnumerator PromptToWatchVideo(){
		countdownLevel = false;
		pauseBtn.interactable = false;
		levelInProgress = false;

		Time.timeScale = 0;

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.7f));

		playerDiedPanel.SetActive (true);
	}

	IEnumerator GiveLivesForVideo() {
		watchVideoText.text = "Thank you for watching.";

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (2f));

		coins = 0;
		lives = 2;
		smallBallsCount = 0;

		GameController.instance.currentLives = lives;
		GameController.instance.currentScore = score;

		Time.timeScale = 0;

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.FadeOut ();
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		SceneController.LoadLevel (SceneController.GetCurrentScene ().name);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	IEnumerator LevelCompleted(){
		countdownLevel = false;
		pauseBtn.interactable = false;

		int unlockedLevel = GameController.instance.currentLevel;
		unlockedLevel++;
		if(!(unlockedLevel >= GameController.instance.levels.Length)){
			GameController.instance.levels [unlockedLevel] = true;
		}

		Instantiate (endOfLevelRewards [GameController.instance.currentLevel], new Vector3(0, Camera.main.orthographicSize, 0), Quaternion.identity);

		if(GameController.instance.doubleCoins){
			coins *= 2;
		}
		GameController.instance.coins = coins;
		GameController.instance.Save ();

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (4f));
		levelInProgress = false;
		PlayerScript.instance.StopMoving ();
		Time.timeScale = 0;

		levelFinishedPanel.SetActive (true);
		endLevelScoreText.text = score.ToString ();
	}

	public void CountSmallBalls(){
		smallBallsCount--;

		if(smallBallsCount == 0){
			StartCoroutine (LevelCompleted ());
		}
	}

	public void GoToMapButton(){
		SetNewHighScore ();

		if(Time.timeScale == 0){
			Time.timeScale = 1;
		}

		SceneController.LoadLevelMenu ();

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	public void RestartLevelButton(){

		smallBallsCount = 0;
		coins = 0;

		GameController.instance.currentLives = lives;
		GameController.instance.currentScore = score;

		SceneController.LoadLevel (SceneController.GetCurrentScene ().name);

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}

	static void SetNewHighScore ()
	{
		if (GameController.instance.highscore < GameController.instance.currentScore) {
			GameController.instance.highscore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}
	}

	public void NextLevelButton(){
		GameController.instance.currentScore = score;
		GameController.instance.currentLives = lives;
		SetNewHighScore ();

		int nextLevel = GameController.instance.currentLevel;
		nextLevel++;

		if (!(nextLevel >= GameController.instance.levels.Length)) {
			GameController.instance.currentLevel = nextLevel;
			SceneController.LoadLevel ("Level " + nextLevel);
			if (LoadingScreen.instance != null) {
				LoadingScreen.instance.PlayLoadingScreen ();
			}
		}
	}

	public void PauseGame(){
		if (!hasLevelBegan) {
			if (levelInProgress) {
				if(!isGamePaused){
					countdownLevel = false;
					levelInProgress = false;
					isGamePaused = true;

					panelsBG.SetActive (true);
					pausePanel.SetActive (true);

					Time.timeScale = 0;
				}
			}
		}
	}

	public void ResumeGameButton(){
		countdownLevel = true;
		levelInProgress = true;
		isGamePaused = false;

		panelsBG.SetActive (false);
		pausePanel.SetActive (false);

		Time.timeScale = 1;
	}

	public void QuitOnVideoPrompt(){
		GameController.instance.currentScore = score;

		SetNewHighScore ();

		Time.timeScale = 1;

		SceneController.LoadLevelMenu ();

		if (LoadingScreen.instance != null) {
			LoadingScreen.instance.PlayLoadingScreen ();
		}
	}
}
