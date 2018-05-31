using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMenuController : MonoBehaviour {

	// score, coin and confirmation panel text
	public Text scoreText, coinText, ConfirmationPanelText;

	// keeps track which player is unlocked
	public bool[] players;

	// keeps track which weapon is unlocked
	public bool[] weapons;

	// reference to price tags in the scene
	public Image[] priceTags;

	// reference to weapon icons in the scene
	public Image[] weaponIcons;

	// reference to weapon sprites to change the weapon icons in the scene
	public Sprite[] weaponSprites;

	// keeps track which weapon is selected on player
	public int selectedWeapon;

	// keeps track which player is selected
	public int selectedPlayer;

	// reference to confirmation and coin shop panels
	public GameObject ConfirmationPanel, coinShopPanel;

	// reference to yes button of confirmation panel
	public Button yesButton;

	void Start () {
		Initialize ();
	}

	void Initialize(){
		scoreText.text = "" + GameController.instance.highscore;
		coinText.text = "" + GameController.instance.coins;

		players = GameController.instance.players;
		weapons = GameController.instance.weapons;
		selectedPlayer = GameController.instance.selectedPlayer;
		selectedWeapon = GameController.instance.selectedWeapon;

		for(int i = 0; i < weaponIcons.Length; i++){
			weaponIcons [i].gameObject.SetActive(false);
		}

		for(int i = 1; i < players.Length; i++){
			if(players[i]){
				priceTags [i - 1].gameObject.SetActive (false);
			}
		}

		weaponIcons [selectedPlayer].gameObject.SetActive (true);
		weaponIcons [selectedPlayer].sprite = weaponSprites [selectedWeapon];
	}

	void ChangePlayerOrWeapon(int index){
		if(selectedPlayer != index){
			weaponIcons [selectedPlayer].gameObject.SetActive (false);

			selectedPlayer = index;

			weaponIcons [selectedPlayer].gameObject.SetActive (true);
			weaponIcons [selectedPlayer].sprite = weaponSprites [selectedWeapon];

			GameController.instance.selectedPlayer = selectedPlayer;
			GameController.instance.Save ();
		} else {
			selectedWeapon++;
			if(selectedWeapon >= weapons.Length){
				selectedWeapon = 0;
			}

			bool foundWeapon = false;
			while(!foundWeapon){
				if(weapons[selectedWeapon] == true){
					weaponIcons [selectedPlayer].sprite = weaponSprites [selectedWeapon];
					GameController.instance.selectedWeapon = selectedWeapon;
					GameController.instance.Save ();
					foundWeapon = true;
				} else {
					selectedWeapon++;
					if(selectedWeapon >= weapons.Length){
						selectedWeapon = 0;
					}
				}
			}
		}
	}

	public void SetPlayer(int index){
		if(players[index] == true){
			ChangePlayerOrWeapon (index);
		} else {
			if(GameController.instance.coins >= 7000){
				string player = "";
				switch(index){
				case 1:
					player = "Pirate";
					break;
				case 2:
					player = "Zombie";
					break;
				}
				ConfirmationPanel.SetActive (true);
				ConfirmationPanelText.text = "Do you want to purchase " + player + "?";
				yesButton.onClick.RemoveAllListeners ();
				yesButton.onClick.AddListener (() => BuyPlayer (index));
			} else {
				ConfirmationPanel.SetActive (true);
				ConfirmationPanelText.text = "You do not have enough coins. Do you want to purchase coins?";
				yesButton.onClick.RemoveAllListeners ();
				yesButton.onClick.AddListener (() => CancelBuy ());
				yesButton.onClick.AddListener (() => OpenCoinShop ());
			}
		}
	}

	public void BuyPlayer(int index){
		GameController.instance.players [index] = true;
		GameController.instance.coins -= 7000;
		selectedPlayer = index;
		GameController.instance.selectedPlayer = selectedPlayer;
		GameController.instance.Save ();
		Initialize ();
		ConfirmationPanel.SetActive (false);
	}

	public void OpenCoinShop(){
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop(){
		coinShopPanel.SetActive (false);
	}

	public void CancelBuy(){
		ConfirmationPanel.SetActive (false);
	}

	public void GoToLevelMenu(){
		SceneController.LoadLevelMenu ();
	}

	public void GoToMainMenu(){
		SceneController.LoadMainMenu ();
	}
}
