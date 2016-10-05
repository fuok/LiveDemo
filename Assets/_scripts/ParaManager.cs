using UnityEngine;
using System.Collections;
using GameData;
using Newtonsoft.Json;
using System.IO;

//通过DB操作，保存和读取Paragraph，只包含CRUD的CR
public class ParaManager : MonoBehaviour
{
	private const string dbName = "live db.db";
	private const string tableName = "paragraph";
	private DbAccess db;

	void Start ()
	{
		//读取数据库
		db = new DbAccess ("data source=" + dbName);//数据库名//("Server=127.0.0.1;UserId=root;Password=;Database=li")
		//创建数据库表，与字段
		db.CreateTable (tableName, new string[]{ "id", "content" }, new string[] {
			"text",
			"text"
		}, false);
		//初始化数据
		StartCoroutine (InitPara ());
	}

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
			db.InsertInto (tableName, new string[]{ "'" + para.id + "'", "'" + para.content + "'" });
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
