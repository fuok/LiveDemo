﻿using UnityEngine;
using System.Collections;
using GameData;
using Newtonsoft.Json;
using System.IO;

//通过DB操作，保存和读取Paragraph，只包含CRUD的CR
public class ParaManager : MonoBehaviour
{
	private const string tableName = "paragraph";
	private DbAccess db;

	// Use this for initialization
	void Start ()
	{
		db = new DbAccess ("data source=paragraph.db");//数据库名//("Server=127.0.0.1;UserId=root;Password=;Database=li")

		//创建数据库表，与字段
		db.CreateTable (tableName, new string[]{ "id", "content" }, new string[] {
			"text",
			"text"
		}, false);


		StartCoroutine (InitPara ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private IEnumerator InitPara ()
	{
		IEnumerator ie;
		TextAsset paraAsset = Resources.Load<TextAsset> ("paragraph/para_1");
		ParagraphData mData = JsonConvert.DeserializeObject<ParagraphData> (paraAsset.text);
		Resources.UnloadUnusedAssets ();
		ie = mData.paragraphList.GetEnumerator ();
		yield return ie;

		while (ie.MoveNext ()) {
			Paragraph para = ie.Current as Paragraph;
			print (para.content);

			db.InsertInto (tableName, new string[]{ para.id, para.content });
		}

	}




	public void SavePara ()
	{
		
	}

	public void LoadPara ()
	{
		
	}

	public void GetAllPara ()
	{
		
	}

	public void GetNextPara ()
	{
	}
}
