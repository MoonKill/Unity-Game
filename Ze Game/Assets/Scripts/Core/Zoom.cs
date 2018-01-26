using UnityEngine;

public class Zoom : MonoBehaviour {

	public delegate void Zooming(bool allowed);

	public Camera cam;
	public ParticleSystem matrixA;
	public ParticleSystem matrixB;

	public static bool canZoom = true;

	public float BossMax = 107f;
	public float BossMin = 15f;

	public float NormMax = 25;
	public float NormMin = 15;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnZoomModeSwitch += M_Player_OnZoomModeSwitch;
	}

	private void M_Player_OnZoomModeSwitch(bool allowed) {
		canZoom = false;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		canZoom = data.player.canZoom;
	}

	private void LateUpdate() {
		if (CameraMovement.script.inBossRoom && canZoom) {
			float roll = Input.GetAxis("Mouse Scroll Wheel");
			
			if (roll > 0) {
				if (cam.orthographicSize < BossMax) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.2f;
				}
			}
			else if (roll < 0) {
				if (cam.orthographicSize > BossMin) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.2f;
				}
			}
			Vector3 cam_pos = new Vector3(CameraMovement.script.camX, CameraMovement.script.camY, -10);
			
			cam.transform.position = cam_pos;
			
		}
		else if (!CameraMovement.script.inBossRoom && canZoom) {
			float roll = Input.GetAxis("Mouse Scroll Wheel");
			if (roll > 0) {
				if (cam.orthographicSize < NormMax) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.08f;
				}
			}
			else if (roll < 0) {
				
				if (cam.orthographicSize > NormMin) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.08f;
				}
			}
			Vector3 cam_pos = new Vector3(CameraMovement.script.camX, CameraMovement.script.camY, -10);
			cam.transform.position = cam_pos;
		}
		if (matrixA.shape.radius != Camera.main.orthographicSize * 2 + 10 && !CameraMovement.script.inBossRoom) {

			matrixA.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + Camera.main.orthographicSize,0);
			matrixB.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - Camera.main.orthographicSize, 0);

			ParticleSystem.ShapeModule shapeA = matrixA.shape;
			ParticleSystem.ShapeModule shapeB = matrixB.shape;

			shapeA.radius = Camera.main.orthographicSize * 2 + 10;
			shapeB.radius = Camera.main.orthographicSize * 2 + 10;			
		}
	}
	private void OnDestroy() {
		M_Player.OnZoomModeSwitch += M_Player_OnZoomModeSwitch;
	}
}
