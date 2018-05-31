using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	[SerializeField]
	private Animator settingsButtonsAnim;

	private bool hidden;
	private bool canTouchSettingsButton;

	[SerializeField]
	private Button musicButton, facebookButton;

	[SerializeField]
	private Sprite[] musicSprites, facebookSprites, infoSprites;

	[SerializeField]
	private GameObject infoPanel;

	[SerializeField]
	private Image infoImage;

	private int infoIndex;

	void Start () {
		canTouchSettingsButton = true;
		hidden = true;
		if (GameController.instance.isMusicOn) {
			MusicController.instance.PlayBGMusic ();
			musicButton.image.sprite = musicSprites [0];
		} else {
			MusicController.instance.StopBGMusic ();
			musicButton.image.sprite = musicSprites [1];
		}
		infoIndex = 0;
		infoImage.sprite = infoSprites [infoIndex];
	}

	public void SettingsButton(){
		StartCoroutine (DisableSettingsButton ());
	}

	IEnumerator DisableSettingsButton(){
		if(canTouchSettingsButton){
			if(hidden){
				canTouchSettingsButton = false;
				settingsButtonsAnim.Play ("SlideIn");
				hidden = false;
				yield return new WaitForSeconds (0.5f);
				canTouchSettingsButton = true;
			} else {
				canTouchSettingsButton = false;
				settingsButtonsAnim.Play ("SlideOut");
				hidden = true;
				yield return new WaitForSeconds (0.5f);
				canTouchSettingsButton = true;
			}
		}
	}

	public void MusicButton(){
		if(GameController.instance.isMusicOn){
			musicButton.image.sprite = musicSprites [1];
			MusicController.instance.StopBGMusic ();
			GameController.instance.isMusicOn = false;
			GameController.instance.Save();
		} else {
			musicButton.image.sprite = musicSprites [0];
			MusicController.instance.PlayBGMusic ();
			GameController.instance.isMusicOn = true;
			GameController.instance.Save();
		}
	}

	public void OpenInfoPanel(){
		infoPanel.SetActive (true);
	}

	public void CloseInfoPanel(){
		infoPanel.SetActive (false);
	}

	public void NextInfo(){
		infoIndex = ++infoIndex % infoSprites.Length;
		infoImage.sprite = infoSprites [infoIndex];
	}

	public void PlayButton(){
		MusicController.instance.PlayClickClip ();
		SceneController.LoadPlayerMenu ();
	}

	public void ShopButton(){
		MusicController.instance.PlayClickClip ();
		SceneController.LoadShopMenu ();
	}
}
