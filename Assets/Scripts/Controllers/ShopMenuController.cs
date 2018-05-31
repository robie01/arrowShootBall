using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopMenuController : MonoBehaviour {

	public static ShopMenuController instance;

	public Text coinText, scoreText, ConfirmationText, watchVideoText;

	public Button weaponsTabBtn, specialTabBtn, earnCoinsTabBtn, yesBtn;

	public GameObject weaponItemsPanel, specialItemsPanel, earnCionsPanel, coinShopPanel, ConfirmationPanel;

	void Awake () {
		MakeInstance ();
	}

	void Start(){
		InitializeShopMenuController ();
	}

	void MakeInstance(){
		if(instance == null){
			instance = this;
		}
	}

	void InitializeShopMenuController(){
		coinText.text = GameController.instance.coins.ToString();
		scoreText.text = GameController.instance.highscore.ToString();
	}

	public void BuyArrows(int index){
		if(!GameController.instance.weapons[index]){
			ConfirmationPanel.SetActive (true);
			yesBtn.onClick.RemoveAllListeners ();
			if(GameController.instance.coins >= 7000){
				string arrow = "";
				switch(index){
				case 1:
					arrow = "double arrows";
					break;
				case 2:
					arrow = "sticky arrows";
					break;
				case 3:
					arrow = "double sticky arrows";
					break;
				}
				ConfirmationText.text = "Do you want to purchase " + arrow + "?";
				yesBtn.onClick.AddListener (() => ConfirmBuyArrows (index));
			} else {
				ConfirmationText.text = "You do not have enough coins. Do you want to buy coins?";
				yesBtn.onClick.AddListener (() => OpenCoinShop());
				yesBtn.onClick.AddListener (() => CancelBuy ());
			}
		}
	}

	public void ConfirmBuyArrows(int index){
		GameController.instance.weapons [index] = true;
		GameController.instance.coins -= 7000;
		GameController.instance.Save ();
		ConfirmationPanel.SetActive (false);
		coinText.text = GameController.instance.coins.ToString();
	}
		
	public void CancelBuy(){
		ConfirmationPanel.SetActive (false);
	}

	public void OpenCoinShop(){
		coinShopPanel.SetActive (true);
	}

	public void CloseCoinShop(){
		coinShopPanel.SetActive (false);
	}

	public void OpenWeaponTab(){
		weaponItemsPanel.SetActive (true);
		weaponsTabBtn.interactable = false;
		specialItemsPanel.SetActive (false);
		specialTabBtn.interactable = true;
		earnCionsPanel.SetActive (false);
		earnCoinsTabBtn.interactable = true;
	}

	public void OpenSpecialTab(){
		weaponItemsPanel.SetActive (false);
		weaponsTabBtn.interactable = true;
		specialItemsPanel.SetActive (true);
		specialTabBtn.interactable = false;
		earnCionsPanel.SetActive (false);
		earnCoinsTabBtn.interactable = true;
	}

	public void OpenEarnCoinsTab(){
		weaponItemsPanel.SetActive (false);
		weaponsTabBtn.interactable = true;
		specialItemsPanel.SetActive (false);
		specialTabBtn.interactable = true;
		earnCionsPanel.SetActive (true);
		earnCoinsTabBtn.interactable = false;
	}

	public void PlayGame(){
		SceneController.LoadPlayerMenu ();
	}

	public void GoToMainMenu(){
		SceneController.LoadMainMenu ();
	}
}
