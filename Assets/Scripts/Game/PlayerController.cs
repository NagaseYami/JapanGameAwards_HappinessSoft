﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private GameObject Character, Body, ArmL, ArmR;

	public int GamePadNum;

	public CursorControl.CHARATYPE CharaType1, CharaType2;  // キャラクタータイプ

	//Controller
	float MoveSpeed,RotateSpeed;
	float MoveSpeedUp;
	private Rigidbody rb;

	//Attack    
	float CloseSpeed, OpenSpeed;

	private bool CloseFlag = false;
	private bool OpenFlag = false;
	private float AlreadyRotation;

	private AudioSource SE_dog;        // 効果音 イヌ
	private AudioSource SE_giraffe;    // 効果音 キリン
	private AudioSource SE_mouse;      // 効果音 イヌ
	private AudioSource SE_elephants;        // 効果音 イヌ

	// Use this for initialization
	void Start()
	{
		if (transform.Find("Dog").gameObject.activeSelf != false)
		{
			Character = transform.Find("Dog").gameObject;
		}
		else if (transform.Find("Elephants").gameObject.activeSelf != false)
		{
			Character = transform.Find("Elephants").gameObject;
		}
		else if (transform.Find("Giraffe").gameObject.activeSelf != false)
		{
			Character = transform.Find("Giraffe").gameObject;
		}
		else if (transform.Find("Mouse").gameObject.activeSelf != false)
		{
			Character = transform.Find("Mouse").gameObject;
		}
		else
		{
			Debug.Log("Cant find Character!");
		}

		Body = Character.transform.Find("Body").gameObject;
		if (Body == null)
		{
			Debug.Log("Cant find Body!");
		}
		ArmL = Character.transform.Find("ArmL").gameObject;
		if (ArmL == null)
		{
			Debug.Log("Cant find ArmL!");
		}
		ArmR = Character.transform.Find("ArmR").gameObject;
		if (ArmR == null)
		{
			Debug.Log("Cant find ArmR!");
		}

		MoveSpeed = Character.GetComponent<CharacterManager>().MoveSpeed;
		RotateSpeed = Character.GetComponent<CharacterManager>().RotateSpeed;
		CloseSpeed = Character.GetComponent<CharacterManager>().CloseSpeed;
		OpenSpeed = Character.GetComponent<CharacterManager>().OpenSpeed;

		rb = Body.GetComponent<Rigidbody>();


		//AudioSourceコンポーネントを取得し、変数に格納
		AudioSource[] audioSources = GetComponents<AudioSource>();
		SE_dog = audioSources[0];
		SE_giraffe = audioSources[1];
		SE_mouse = audioSources[2];
		SE_elephants = audioSources[3];

		// キャラクターのタイプを取得
		CharaType1 = CursorControl.GetCharaType1();
		CharaType2 = CursorControl.GetCharaType2();
	}

	void Update()
	{
		if (Time.timeScale == 1)
		{
			if (!gameObject.GetComponent<PlayerManager>().bDead)
			{
				BuffChecker();
				Controller();
				Attack();
			}
		}
	}

	void BuffChecker()
	{
		if (gameObject.GetComponent<PlayerManager>().bSpeedUp)
		{
			MoveSpeedUp = GameObject.Find("SpeedItemManager").gameObject.GetComponent<SpeedItemManager>().SpeedUpValue;
		}
		else
		{
			MoveSpeedUp = 1.0f;
		}
	}

	void Controller()
	{

		//if (Input.GetAxisRaw("Vertical" + GamePadNum) > 0 | Input.GetAxisRaw("Vertical" + GamePadNum) < 0 |
		//	Input.GetAxisRaw("Horizontal" + GamePadNum) > 0.01 |Input.GetAxisRaw("Horizontal" + GamePadNum) < -0.01)
		//{
		//	float test = Mathf.Atan2(0.0f - Input.GetAxisRaw("Vertical" + GamePadNum), 0.0f - Input.GetAxisRaw("Horizontal" + GamePadNum));
		//	Debug.Log(test);

		//	rb.AddForce(new Vector3(-Input.GetAxisRaw("Horizontal" + GamePadNum), 0, Input.GetAxisRaw("Vertical" + GamePadNum)) * MoveSpeed * MoveSpeedUp);
		//	rb.MoveRotation(Quaternion.Euler(0, test * Mathf.Rad2Deg + 90, 0));

		//	ArmR.transform.RotateAround(Body.transform.position, Vector3.up, -RotateSpeed * Time.deltaTime);
		//	ArmL.transform.RotateAround(Body.transform.position, Vector3.up, -RotateSpeed * Time.deltaTime);
		//}

		// 上方向
		if (Input.GetAxisRaw("Vertical" + GamePadNum) > 0)
		{
			rb.AddForce(Body.transform.forward * MoveSpeed * MoveSpeedUp);
		}

		// 下方向
		if (Input.GetAxisRaw("Vertical" + GamePadNum) < 0)
		{
			rb.AddForce(-Body.transform.forward * MoveSpeed * MoveSpeedUp);
		}

		Vector3 TurnLeft = new Vector3(0.0f, -RotateSpeed, 0.0f);
		Vector3 TurnRight = new Vector3(0.0f, RotateSpeed, 0.0f);

		Quaternion deltaRotation;

		// 左方向
		if (Input.GetAxisRaw("Horizontal" + GamePadNum) > 0.01)
		{
			deltaRotation = Quaternion.Euler(TurnLeft * Time.deltaTime);
			rb.MoveRotation(rb.rotation * deltaRotation);
			ArmR.transform.RotateAround(Body.transform.position, Vector3.up, -RotateSpeed * Time.deltaTime);
			ArmL.transform.RotateAround(Body.transform.position, Vector3.up, -RotateSpeed * Time.deltaTime);
		}

		// 右方向
		if (Input.GetAxisRaw("Horizontal" + GamePadNum) < -0.01)
		{
			deltaRotation = Quaternion.Euler(TurnRight * Time.deltaTime);
			rb.MoveRotation(rb.rotation * deltaRotation);
			ArmR.transform.RotateAround(Body.transform.position, Vector3.up, RotateSpeed * Time.deltaTime);
			ArmL.transform.RotateAround(Body.transform.position, Vector3.up, RotateSpeed * Time.deltaTime);
		}
	}

	void Attack()
	{
		if (Input.GetButtonDown("Fire" + GamePadNum) && !CloseFlag && !OpenFlag)
		{
			CloseFlag = true;

			switch (GamePadNum)
			{
				case 1:
					switch (CharaType1)
					{
						case CursorControl.CHARATYPE.DOG:
							SE_dog.PlayOneShot(SE_dog.clip);
							break;

						case CursorControl.CHARATYPE.GIRFFE:
							SE_giraffe.PlayOneShot(SE_giraffe.clip);
							break;

						case CursorControl.CHARATYPE.MOUSE:
							SE_mouse.PlayOneShot(SE_mouse.clip);
							break;

						case CursorControl.CHARATYPE.ELEPHANTS:
							SE_elephants.PlayOneShot(SE_elephants.clip);
							break;

					}
					break;

				case 2:
					switch (CharaType2)
					{
						case CursorControl.CHARATYPE.DOG:
							SE_dog.PlayOneShot(SE_dog.clip);
							break;

						case CursorControl.CHARATYPE.GIRFFE:
							SE_giraffe.PlayOneShot(SE_giraffe.clip);
							break;

						case CursorControl.CHARATYPE.MOUSE:
							SE_mouse.PlayOneShot(SE_mouse.clip);
							break;

						case CursorControl.CHARATYPE.ELEPHANTS:
							SE_elephants.PlayOneShot(SE_elephants.clip);
							break;

					}
					break;
			}
		}

		if (CloseFlag)
		{
			ArmR.transform.RotateAround(Body.transform.position, Vector3.up, -CloseSpeed * Time.deltaTime);
			ArmL.transform.RotateAround(Body.transform.position, Vector3.up, CloseSpeed * Time.deltaTime);
			AlreadyRotation += CloseSpeed * Time.deltaTime;
			ArmR.GetComponent<ChopsticksManager>().bHasamu = true;
			ArmL.GetComponent<ChopsticksManager>().bHasamu = true;
			if (AlreadyRotation >= 30)
			{
				ArmR.transform.eulerAngles = new Vector3(0, Body.transform.eulerAngles.y - 30, 0);
				ArmL.transform.eulerAngles = new Vector3(0, Body.transform.eulerAngles.y + 30, 0);
				CloseFlag = false;
				OpenFlag = true;
				AlreadyRotation = 0;
				ArmR.GetComponent<ChopsticksManager>().bHasamu = false;
				ArmL.GetComponent<ChopsticksManager>().bHasamu = false;
			}
		}

		if (OpenFlag)
		{
			ArmR.transform.RotateAround(Body.transform.position, Vector3.up, OpenSpeed * Time.deltaTime);
			ArmL.transform.RotateAround(Body.transform.position, Vector3.up, -OpenSpeed * Time.deltaTime);
			AlreadyRotation += OpenSpeed * Time.deltaTime;
			if (AlreadyRotation >= 30)
			{
				ArmR.transform.eulerAngles = new Vector3(0, Body.transform.eulerAngles.y, 0);
				ArmL.transform.eulerAngles = new Vector3(0, Body.transform.eulerAngles.y, 0);
				OpenFlag = false;
				AlreadyRotation = 0;
			}
		}
	}
}