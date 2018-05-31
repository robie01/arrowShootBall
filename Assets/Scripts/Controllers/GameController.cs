using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {

	private GameData data;

	public static GameController instance;
	[HideInInspector]
	public int currentLevel = -1, currentScore, selectedPlayer, selectedWeapon, coins, highscore, currentLives;
	[HideInInspector]
	public bool isGameStartedFirstTime, isMusicOn, isGameStartedFromLevelMenu, doubleCoins;
	[HideInInspector]
	public bool[] players, levels, weapons, achievements,collectedItems;

	void Awake(){
		MakeSingletone ();
		InitializeGameVariables ();
	}

	void Start () {
		
	}

	void MakeSingletone(){
		if(instance != null){
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void InitializeGameVariables(){
		Load ();
		if(data != null){
			isGameStartedFirstTime = data.IsGameStartedFirstTime;
		} else {
			isGameStartedFirstTime = true;
		}

		if(isGameStartedFirstTime) {
			highscore = 0;
			coins = 0;
			selectedPlayer = 0;
			selectedWeapon = 0;
			isGameStartedFirstTime = false;
			isMusicOn = false;
			doubleCoins = false;
			players = new bool[2];
			levels = new bool[40];
			weapons = new bool[4];
			achievements = new bool[8];
			collectedItems = new bool[40];

			players [0] = true;
			for(int i = 1; i < players.Length; i++){
				players [i] = false;
			}

			levels [0] = true;
			for(int i = 1; i < levels.Length; i++){
				levels [i] = false;
			}

			weapons [0] = true;
			for(int i = 1; i < weapons.Length; i++){
				weapons [i] = false;
			}

			for(int i = 1; i < achievements.Length; i++){
				achievements [i] = false;
			}

			for(int i = 1; i < collectedItems.Length; i++){
				collectedItems [i] = false;
			}

			data = new GameData ();

			data.IsGameStartedFirstTime = isGameStartedFirstTime;

			data.IsMusicOn = isMusicOn;
			data.DoubleCoins = doubleCoins;
			data.SelectedPlayer = selectedPlayer;
			data.SelectedWeapon = selectedWeapon;
			data.Coins = coins;
			data.Highscore = highscore;

			data.Players = players;
			data.Levels = levels;
			data.Weapons = weapons;
			data.Achievements = achievements;
			data.CollectedItems = collectedItems;

			Save ();

			Load ();
		} else {
			isGameStartedFirstTime = data.IsGameStartedFirstTime;

			isMusicOn = data.IsMusicOn;
			doubleCoins = data.DoubleCoins;
			selectedPlayer = data.SelectedPlayer;
			selectedWeapon = data.SelectedWeapon;
			coins = data.Coins;
			highscore = data.Highscore;

			players = data.Players;
			levels = data.Levels;
			weapons = data.Weapons;
			achievements = data.Achievements;
			collectedItems = data.CollectedItems;
		}
	}

	public void Save(){
		FileStream file = null;

		try{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Create(Application.persistentDataPath + "/GameData.dat");
			if(data != null){
				data.IsGameStartedFirstTime = isGameStartedFirstTime;

				data.IsMusicOn = isMusicOn;
				data.DoubleCoins = doubleCoins;
				data.SelectedPlayer = selectedPlayer;
				data.SelectedWeapon = selectedWeapon;
				data.Coins = coins;
				data.Highscore = highscore;

				data.Players = players;
				data.Levels = levels;
				data.Weapons = weapons;
				data.Achievements = achievements;
				data.CollectedItems = collectedItems;

				bf.Serialize(file, data);
			}
		}catch(Exception e){
			
		} finally {
			if (file != null) {
				file.Close ();
			}
		}
	}

	public void Load(){
		FileStream file = null;

		try{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
			data = (GameData)bf.Deserialize(file);
		}catch(Exception e){

		} finally {
			if (file != null) {
				file.Close ();
			}
		}
	}
}

[Serializable]
class GameData{

	private bool isGameStartedFirstTime;

	private bool isMusicOn;
	private bool doubleCoins;
	private int selectedPlayer;
	private int selectedWeapon;
	private int coins;
	private int highscore;

	private bool[] players;
	private bool[] levels;
	private bool[] weapons;
	private bool[] achievements;
	private bool[] collectedItems;

	public bool IsGameStartedFirstTime {
		get {
			return this.isGameStartedFirstTime;
		}
		set {
			isGameStartedFirstTime = value;
		}
	}

	public bool IsMusicOn {
		get {
			return this.isMusicOn;
		}
		set {
			isMusicOn = value;
		}
	}

	public bool DoubleCoins {
		get {
			return this.doubleCoins;
		}
		set {
			doubleCoins = value;
		}
	}

	public int SelectedPlayer {
		get {
			return this.selectedPlayer;
		}
		set {
			selectedPlayer = value;
		}
	}

	public int SelectedWeapon {
		get {
			return this.selectedWeapon;
		}
		set {
			selectedWeapon = value;
		}
	}

	public int Coins {
		get {
			return this.coins;
		}
		set {
			coins = value;
		}
	}

	public int Highscore {
		get {
			return this.highscore;
		}
		set {
			highscore = value;
		}
	}

	public bool[] Players {
		get {
			return this.players;
		}
		set {
			players = value;
		}
	}

	public bool[] Levels {
		get {
			return this.levels;
		}
		set {
			levels = value;
		}
	}

	public bool[] Weapons {
		get {
			return this.weapons;
		}
		set {
			weapons = value;
		}
	}

	public bool[] Achievements {
		get {
			return this.achievements;
		}
		set {
			achievements = value;
		}
	}

	public bool[] CollectedItems {
		get {
			return this.collectedItems;
		}
		set {
			collectedItems = value;
		}
	}
}