using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour {

	public Text scoreText, coinText;
	public bool[] levels;
	public Button[] levelBtns;
	public Text[] levelText;
	public Image[] lockIcons;
	public GameObject coinShopPanel;

	void Start(){
		Initialize ();
	}

	void Initialize(){
		scoreText.text = GameController.instance.highscore.ToString ();
		coinText.text = GameController.instance.coins.ToString ();

		levels = GameController.instance.levels;
		for(int i = 1; i < levels.Length; i++){
			if(levels[i]){
				lockIcons [i - 1].gameObject.SetActive (false);
				levelText [i - 1].gameObject.SetActive (true);
				levelBtns [i - 1].interactable = true;
			}
		}
	}

	public void LoadLevel(){
		if(GameController.instance.isMusicOn){
			MusicController.instance.GameIsLoadedTurnOffMusic ();
		}
		string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
		GameController.instance.currentLevel = int.Parse (level.Substring (6));
		LoadingScreen.instance.PlayLoadingScreen ();
		GameController.instance.isGameStartedFromLevelMenu = true;
		SceneController.LoadLevel (level);
	}

	public void OpenCoinShop(){
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop(){
		coinShopPanel.SetActive (false);
	}

	public void GoToMainMenu(){
		SceneController.LoadMainMenu ();
	}

	public void GoBack(){
		SceneController.LoadPlayerMenu ();
	}
}
