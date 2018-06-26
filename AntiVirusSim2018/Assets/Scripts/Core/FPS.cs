using UnityEngine;

public class FPS : MonoBehaviour {

	float deltaTime = 0.0f;
	int quality;

	void OnEnable() {
		quality = QualitySettings.GetQualityLevel();
	}


	void Update() {
		if (quality == 6 || quality == 4) {
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		}
	}

	void OnGUI() {
		if (quality == 6 || quality == 4) {
			int w = Screen.width, h = Screen.height;

			GUIStyle style = new GUIStyle();

			Rect rect = new Rect(w / 2, 0, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = h * 2 / 50;
			style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			float currdelta = Time.deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps) ({2:0.0000} Delta)", msec, fps, currdelta);
			GUI.Label(rect, text, style);
		}
	}
}