﻿using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public static float timeFlowMultiplier { get; private set; } = 1;

	public static Timer script { get; private set; }

	public bool isRunning { get; set; }

	private Text timerText;

	public static float getTime { get; private set; }

	public static string getTimeFormated {
		get {
			float current = getTime;
			if (current != 0) {
				float divided = current / 60;
				int modulo = (int)divided % 60;

				return string.Format("{0:00}:{1:00}.{2:000} {3}", modulo, (int)current % 60, (int)(current * 1000 % 1000), modulo == 0 ? "seconds" : "minute(s)");
			}
			else {
				return "0";
			}
		}
	}

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}

		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		Player.OnPlayerDeath += M_Player_OnPlayerDeath;
		PauseUnpause.OnPaused += OnPaused;
	}

	private void OnPaused(object sender, PauseEventArgs e) {
		isRunning = e.isPlaying;
		Time.timeScale = e.isPaused ? 0 : 1;
	}

	private void M_Player_OnPlayerDeath(object sender, PlayerDeathEventArgs e) {
		isRunning = false;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		getTime = data.core.time;
		StartTimer(1f);
		timerText = GetComponent<Text>();
		timerText.gameObject.SetActive(true);
	}

	private void Start() {
		if (timerText == null) {
			timerText = GetComponent<Text>();
			timerText.gameObject.SetActive(false);
		}
	}

	private void Update() {
		if (isRunning) {
			getTime += Time.deltaTime * timeFlowMultiplier;
			timerText.text = "Time:\t" + getTimeFormated;
		}
	}

	public static void StartTimer(float flowMultiplier) {
		script.gameObject.SetActive(true);
		script.isRunning = true;
		timeFlowMultiplier = flowMultiplier;
	}

	public static void ResetTimer() {
		getTime = 0;
		script.isRunning = false;
		timeFlowMultiplier = 1;
	}

	private void OnDestroy() {
		script = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Player.OnPlayerDeath -= M_Player_OnPlayerDeath;
	}
}
