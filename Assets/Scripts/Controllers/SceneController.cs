using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class SceneController{

	public static Scene GetCurrentScene(){
		return SceneManager.GetActiveScene ();
	}

	public static void LoadLevel(string level){
		SceneManager.LoadScene (level);
	}

	public static void LoadMainMenu(){
		LoadLevel ("MainMenu");
	}

	public static void LoadLevelMenu(){
		LoadLevel ("LevelMenu");
	}

	public static void LoadPlayerMenu(){
		LoadLevel ("PlayerMenu");
	}

	public static void LoadShopMenu(){
		LoadLevel ("ShopMenu");
	}
}
