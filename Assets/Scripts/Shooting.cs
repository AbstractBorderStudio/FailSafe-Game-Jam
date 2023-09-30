using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
	{
		public GameObject playerBulletPrefab;
		public Transform playerTransform;
		public Transform shootingPoint;
		private GameObject bullet;

		public float waitToFire=0.3f;
		public float shootPower = 10f;
		private bool canFire=true;
		//private Vector3 offset = (1f, 0f, 0f);

		
		// Use this for initialization
		void Start()
		{
		canFire = true;

		}

    private void Update()
    {
        if (canFire)
        {
			/*if (Input.GetKey(KeyCode.DownArrow))
			{
				//Debug.Log("giu");
				bullet =CreatePlayerBullet();
				bullet.GetComponent<BulletMovement>().SetBulletMovement(-playerTransform.up, shootPower);
				
				StartCoroutine(WaitToShoot());
			}
			else */if (Input.GetKey(KeyCode.UpArrow))
			{
				//Debug.Log("su");
				bullet = CreatePlayerBullet();
				bullet.GetComponent<BulletMovement>().SetBulletMovement(playerTransform.up, shootPower);
				StartCoroutine(WaitToShoot());
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				//Debug.Log("sx");
				bullet = CreatePlayerBullet();
				bullet.GetComponent<BulletMovement>().SetBulletMovement(-playerTransform.right+playerTransform.up, shootPower);
				StartCoroutine(WaitToShoot());
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				//Debug.Log("dx");
				bullet = CreatePlayerBullet();
				bullet.GetComponent<BulletMovement>().SetBulletMovement(playerTransform.right + playerTransform.up, shootPower);
				StartCoroutine(WaitToShoot());
			}
		}
       
	}


	GameObject CreatePlayerBullet()
	{
		return Instantiate(playerBulletPrefab, shootingPoint.position, Quaternion.identity);
	}
	
	private IEnumerator WaitToShoot()
    {
		canFire = false;
		yield return new WaitForSeconds(waitToFire);
		canFire = true;
    }

		
	}
