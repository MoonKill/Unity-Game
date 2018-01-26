using UnityEngine;
using System.Collections;
using Igor.Constants.Strings;


public class TurretAttack : MonoBehaviour {

	public GameObject projectile;
	public GameObject target;
	public float turretSpawnRateStart;
	public float turretSpawnRateEnd;
	private float currSpawnRate;

	public bool useDefaultTiming;
	public bool applyRandomness;
	public float randomnessMultiplier;

	private Vector3 targetPos;
	private ObjectPool pool_EnemyProjectile;
	private Transform enemy;
	private Coroutine ChangeFireRate;

	private void Start() {
		pool_EnemyProjectile = new ObjectPool(Resources.Load("Enemies/" + projectile.name + (MapData.script.currentMapMode == MapData.MapMode.LIGHT ? "" : "_Dark")) as GameObject);
		enemy = GameObject.Find("Enemies").transform;
		targetPos = target.transform.position;
		if (useDefaultTiming) {
			switch (Control.currDifficulty) {
				case 0: {
					turretSpawnRateStart = 1.6f;
					turretSpawnRateEnd = 1.4f;
					break;
				}
				case 1: {
					turretSpawnRateStart = 1.5f;
					turretSpawnRateEnd = 1.3f;
					break;
				}
				case 2: {
					turretSpawnRateStart = 1.4f;
					turretSpawnRateEnd = 1.2f;
					break;
				}
				case 3: {
					turretSpawnRateStart = 1.4f;
					turretSpawnRateEnd = 1.2f;
					break;
				}
				case 4: {
					turretSpawnRateStart = 1.3f;
					turretSpawnRateEnd = 1.1f;
					break;
				}
			}
			ChangeFireRate = StartCoroutine(CurrentSpawnRate(turretSpawnRateStart, turretSpawnRateEnd));
			StartCoroutine(WaitForAttack(turretSpawnRateStart));
		}
		else {
			ChangeFireRate = StartCoroutine(CurrentSpawnRate(turretSpawnRateStart, turretSpawnRateEnd));
			StartCoroutine(WaitForAttack(turretSpawnRateStart));
		}
	}

	private IEnumerator CurrentSpawnRate(float startSpeed, float endSpeed) {
		for (float f = 0; f <= 1; f += Time.deltaTime * 0.03f) {
			currSpawnRate = Mathf.Lerp(startSpeed, endSpeed, f);
			yield return null;
		}
	}


	private IEnumerator WaitForAttack(float spawnRate) {
		while (true) {
			yield return new WaitForSeconds(spawnRate);
			targetPos = target.transform.position;
			spawnRate = currSpawnRate;

			int diff = Control.currDifficulty;

			if (diff <= 2) {
				Projectile bullet = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
				if (applyRandomness) {
					Vector3 rnd = RandomVec(diff);
					bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((targetPos + rnd) - gameObject.transform.position));
				}
				else {
					bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, (targetPos - gameObject.transform.position));
				}
				bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
				bullet.transform.SetParent(enemy);
				bullet.gameObject.SetActive(true);
				bullet.projectileSpeed = 20;
				bullet.Fire();
			}
			else {
				for (int i = 0; i < 2; i++) {
					Projectile bullet = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
					if (applyRandomness) {
						Vector3 rnd = RandomVec(diff);
						bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((targetPos + rnd) - (gameObject.transform.position)));
					}
					else {
						bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, (targetPos - gameObject.transform.position));
					}
					bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
					bullet.transform.SetParent(enemy);
					bullet.gameObject.SetActive(true);
					bullet.projectileSpeed = 25;
					bullet.Fire();
				}
			}
		}
	}

	public Vector3 RandomVec(int difficulty) {
		float r = 0;
		if (difficulty <= 2) {
			r = Random.Range(-1 * randomnessMultiplier, 1 * randomnessMultiplier);
			return Vector2.one * r;
		}
		else {
			r = Random.Range(-2 * randomnessMultiplier, 2 * randomnessMultiplier);
			return Vector2.one * r;
		}
	}

	void OnDestroy() {
		StopAllCoroutines();
	}

	public float getCurrentSpawnRate {
		get { return currSpawnRate; }
	}
}
