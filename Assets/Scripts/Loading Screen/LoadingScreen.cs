using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public static LoadingScreen instance;

	[SerializeField]
	private GameObject bg, logo, text, fadePanel;
	[SerializeField]
	private Animator fadeAnim;

	void Start () {
		MakeSingleton ();
		Hide ();
	}

	void MakeSingleton(){
		if(instance != null){
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void OnLevelWasLoaded(){
		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
	}

	public void PlayLoadingScreen(){
		StartCoroutine (ShowLoadingScreen ());
	}

	public void PlayFadeIn(){
		StartCoroutine (FadeIn ());
	}

	IEnumerator FadeIn(){
		fadeAnim.Play ("FadeIn");
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.4f));

		if(GameplayController.instance != null){
			GameplayController.instance.SetHasLevelBegan (true);
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.9f));
		fadePanel.SetActive (false);
	}

	public void FadeOut(){
		fadePanel.SetActive (true);
		fadeAnim.Play ("FadeOut");
	}

	IEnumerator ShowLoadingScreen(){
		Show ();
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1f));
		Hide ();

		if(GameplayController.instance != null){
			GameplayController.instance.SetHasLevelBegan (true);
		}
	}

	void Show(){
		bg.SetActive (true);
		logo.SetActive (true);
		text.SetActive (true);
	}

	void Hide(){
		bg.SetActive (false);
		logo.SetActive (false);
		text.SetActive (false);
	}
}
