using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour, IEnemy
{
	private Vector3 direction;
	private float speed;
	private bool canMove = true;

	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
        if (canMove)
        {
			transform.position += direction * speed * Time.deltaTime;
		}
		
	}

	public void SetBulletMovement(Vector3 dir,float _speed)
    {
		direction = dir;
		speed = _speed;
    }
	public void stopMove()
    {
		canMove = false;
    }
	public void Repel()
    {
		//come gestisco che lo scudo respinge robe
    }

	public void OnDestroy()
	{
		
	}

}