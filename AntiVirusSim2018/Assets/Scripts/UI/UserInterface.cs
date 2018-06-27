﻿using Igor.Constants.Strings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
	public static event PauseUnpause.Pause OnPauseChange;
	private static UIScene _sceneMode;

	public static UIScene sceneMode {
		set { _sceneMode = value; }
	}

	public enum UIScene {
		MAIN_MENU,
		GAME,
		SAVES,
		OTHER
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void ListenForEscape() {
		Control.OnEscapePressed += Control_OnEscapePressed;
	}

	protected virtual void Awake() {
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
		M_Player.OnPlayerDeath += M_Player_OnPlayerDeath;
		_sceneMode = UIScene.MAIN_MENU;
	}

	private static void Control_OnEscapePressed() {
		switch (_sceneMode) {
			case UIScene.MAIN_MENU: {
				if (WindowManager.getWindowCount > 0) {
					WindowManager.CloseMostRecent();
					foreach (Button b in FindObjectOfType<MainMenu_Holder>().getButtons) {
						b.interactable = true;
					}
				}
				return;
			}

			case UIScene.GAME: {
				if (WindowManager.getWindowCount > 0) {
					WindowManager.CloseMostRecent();

					try {
						EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
					}
					catch {
						print("No Buttons are selectable.");
					}
				}
				else {
					if (OnPauseChange != null && M_Player.gameProgression != -1) {
						OnPauseChange(!PauseUnpause.isPaused);
					}
				}
				return;
			}

			case UIScene.SAVES: {

				return;
			}

			case UIScene.OTHER: {

				return;
			}

			default: {

				return;
			}
		}
	}

	private void Update() {
		switch (_sceneMode) {
			case UIScene.MAIN_MENU: {
				if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) {
					EventSystem.current.gameObject.GetComponent<EventSystemManager>().TryDeselect();
				}
				if ((Input.GetAxisRaw("HorMovement") != 0 || Input.GetAxisRaw("VertMovement") != 0) && EventSystem.current.currentSelectedGameObject == null) {
					EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
				}
				return;
			}
			case UIScene.GAME: {
				return;
			}
			case UIScene.SAVES: {
				return;
			}
			case UIScene.OTHER: {
				return;
			}
			default: {
				return;
			}
		}
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode args) {
		switch (scene.name) {
			case SceneNames.GAME1_SCENE: {
				M_Player.OnPlayerDeath += M_Player_OnPlayerDeath;
				return;
			}
		}
	}

	#region Menu Functions
	public void Deactivate(Button button) {
		button.interactable = false;
	}

	public void Activate(Button button) {
		button.interactable = true;
	}

	public void ToggleMenuButtons() {
		foreach (Button b in FindObjectOfType<MainMenu_Holder>().getButtons) {
			b.interactable = !b.interactable;
		}
	}
	#endregion

	#region GameScene functions
	private void M_Player_OnPlayerDeath(M_Player sender) {
		Animator gameOverAnim = GameObject.Find("GameOver").GetComponent<Animator>();
		gameOverAnim.Play("GameOver");
		M_Player.OnPlayerDeath -= M_Player_OnPlayerDeath;
	}

	#endregion
}
