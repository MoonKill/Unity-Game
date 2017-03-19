using UnityEngine;
using UnityEngine.SceneManagement;

public class restartOnClick : MonoBehaviour {

	public void Restart(){
		
		M_Player.doNotMove = false;
		Spike.spikesCollected = 0;
		Coins.coinsCollected = 0;
		PlayerAttack.bombs = 0;
		PlayerAttack.bullets = 0;
		M_Player.gameProgression = 0;
		Projectile.projectileSpeed = 15;
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Statics.camFade.anim.SetTrigger("UnDim");
	}

}
