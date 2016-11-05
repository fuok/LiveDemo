using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using GameData;
using Newtonsoft.Json;
using System.IO;

//通过DB操作，保存和读取Paragraph，只包含CRUD的CR
public class ParaManager : MonoBehaviour
{
	private static DbAccess db;
	private static SqliteDataReader sqReader;
	//当前的Para情报
	private static Paragraph currentPara = new Paragraph ("1");
	private static int paraIndex;

	void Start ()
	{
		//读取数据库
		db = new DbAccess ("data source=" + Constants.dbName);//数据库名//("Server=127.0.0.1;UserId=root;Password=;Database=li")
		//创建数据库表，与字段
		db.CreateTable (Constants.tableName, new string[] {
			"id",
			"background",
			"content",
			"model_0",
			"model_1",
			"model_2",
			"next"
		}, new string[] {
			"text", "text", "text", "text", "text",
			"text", "text"
		}, false);
		//初始化Para表
		int version = PlayerPrefs.GetInt ("dataBaseVersion", 0);//检查版本号
		print ("version=" + version);
		if (Constants.dataBaseVersion > version) {
			PlayerPrefs.SetInt ("dataBaseVersion", Constants.dataBaseVersion);
			StartCoroutine (InitPara ());
		}
	}

	//	void Update ()
	//	{
	//
	//	}

	/// <summary>
	/// 写入
	/// </summary>
	/// <returns>The para.</returns>
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
			db.InsertInto (Constants.tableName, new string[] {
				"'" + para.id + "'",
				"'" + para.background + "'",
				"'" + para.content + "'",
				"'" + para.model_0 + "'",
				"'" + para.model_1 + "'",
				"'" + para.model_2 + "'",
				"'" + para.next + "'"
			});
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

	/// <summary>
	/// 分段读取
	/// </summary>
	/// <returns>The next para.</returns>
	public static Paragraph GetNextPara ()
	{
//		paraIndex++;
		//通过next字段查找下一个Para
		sqReader = db.SelectWhere (Constants.tableName, new string[] {
			"id", "background",
			"content", "model_0", "model_1", "model_2", "next"
		}, new string[]{ "id" }, new string[]{ "=" }, new string[]{ currentPara.next });

		//声明Paragraph对象
		while (sqReader.Read ()) {//如果上边的查找没有结果，就不会进这里，我觉得最好给end一个特殊标记
//			print ("找到了");
			currentPara = new Paragraph (sqReader.GetString (sqReader.GetOrdinal ("id")), sqReader.GetString (sqReader.GetOrdinal ("background")), sqReader.GetString (sqReader.GetOrdinal ("content")), sqReader.GetString (sqReader.GetOrdinal ("model_0")), sqReader.GetString (sqReader.GetOrdinal ("model_1")), sqReader.GetString (sqReader.GetOrdinal ("model_2")), sqReader.GetString (sqReader.GetOrdinal ("next")));
		}
		return currentPara;
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy ()//认为游戏结束
	{
		print ("OnDesrtoy");
		//关闭对象
		db.CloseSqlConnection ();
	}
}
