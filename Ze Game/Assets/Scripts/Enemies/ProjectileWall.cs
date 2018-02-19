﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class ProjectileWall : MonoBehaviour {

	private RectTransform background;
	private ObjectPool pool;
	private List<GameObject> spawnedObjects = new List<GameObject>();

	public Directions origin;
	public float spawnInterval;

	private float width = 0;
	private float height = 0;
	private float spriteRotation;

	private void Start() {
		background = transform.parent.GetComponent<RectTransform>();
		width = background.sizeDelta.x / 2;
		height = background.sizeDelta.y / 2;


		switch (origin) {
			case Directions.TOP: {
				spriteRotation = 0;
				break;
			}
			case Directions.RIGHT: {
				spriteRotation = 90;
				break;
			}
			case Directions.BOTTOM: {
				spriteRotation = 180;
				break;
			}
			case Directions.LEFT: {
				spriteRotation = -90;
				break;
			}
		}

		StartCoroutine(SpawnKillerWall());
	}

	public void SetProjecileType(Enemy.EnemyType type) {
		GameObject projectilePrefab;
		switch (type) {
			case Enemy.EnemyType.PROJECTILE_SIMPLE: {
				projectilePrefab = Resources.Load(PrefabNames.ENEMY_PROJECTILE_INACCUARATE + (MapData.script.currentMapMode == MapData.MapMode.DARK ? "_Dark" : "")) as GameObject;
				break;
			}
			case Enemy.EnemyType.PROJECTILE_ACCURATE: {
				projectilePrefab = Resources.Load(PrefabNames.ENEMY_PROJECTILE_ACCURATE) as GameObject;
				break;
			}
			case Enemy.EnemyType.PROJECTILE_ICICLE: {
				projectilePrefab = Resources.Load(PrefabNames.ENEMY_PROJECTILE_ICICLE + (MapData.script.currentMapMode == MapData.MapMode.DARK ? "_Dark" : "")) as GameObject;
				break;
			}
			default: {
				throw new System.Exception("Not a valid enemy type " + type);
			}
		}
		pool = new ObjectPool(projectilePrefab);
	}

	private IEnumerator SpawnKillerWall() {
		int diff = Control.currDifficulty;

		while (gameObject) {
			yield return new WaitForSeconds(spawnInterval);
			if (diff == 0 || diff == 1) {
				Projectile wallShot = pool.getNext.GetComponent<Projectile>();
				wallShot.gameObject.tag = Tags.ENEMY;
				wallShot.transform.rotation = Quaternion.AngleAxis(spriteRotation, Vector3.back);
				wallShot.transform.position = KWProjectilePositions(origin);
				wallShot.transform.SetParent(transform);
				wallShot.gameObject.SetActive(true);
				wallShot.projectileSpeed = 15f;
				spawnedObjects.Add(wallShot.gameObject);
				wallShot.Fire();
			}
			else if (diff == 2 || diff == 3) {
				for (int i = 0; i < 2; i++) {
					Projectile wallShot = pool.getNext.GetComponent<Projectile>();
					wallShot.gameObject.tag = Tags.ENEMY;
					wallShot.transform.rotation = Quaternion.AngleAxis(spriteRotation, Vector3.back);
					wallShot.transform.position = KWProjectilePositions(origin);
					wallShot.transform.SetParent(transform);
					wallShot.gameObject.SetActive(true);
					wallShot.projectileSpeed = 15f;
					spawnedObjects.Add(wallShot.gameObject);
					wallShot.Fire();
				}
			}
			else if (diff == 4) {
				for (int i = 0; i < 3; i++) {
					Projectile wallShot = pool.getNext.GetComponent<Projectile>();
					wallShot.gameObject.tag = Tags.ENEMY;
					wallShot.transform.rotation = Quaternion.AngleAxis(spriteRotation, Vector3.back);
					wallShot.transform.position = KWProjectilePositions(origin);
					wallShot.transform.SetParent(transform);
					wallShot.gameObject.SetActive(true);
					spawnedObjects.Add(wallShot.gameObject);
					wallShot.projectileSpeed = 15;
					wallShot.Fire();
				}
			}
		}
	}


	public Vector3 KWProjectilePositions(Directions side) {
		if (side == Directions.RIGHT || side == Directions.LEFT) {
			return new Vector3(background.position.x - 5 + (side == Directions.RIGHT ? width : -width), Random.Range(background.position.y - height, background.position.y + height));
		}
		else {
			return new Vector3(Random.Range(background.position.x - width, background.position.x + width), background.position.y - 5 + (side == Directions.TOP ? height : -height));
		}
	}
}
