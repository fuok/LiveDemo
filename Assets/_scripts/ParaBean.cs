using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using GameData;
using Newtonsoft.Json;
using System.IO;

//通过DB操作，保存和读取Paragraph，只包含CRUD的CR
public class ParaBean : MonoBehaviour
{
	public static ParaBean Instance{ get; private set; }

	private DbAccess db;
	private SqliteDataReader sqReader;

	private string[] colName = new string[] {
		"id",
		"background", "portrait",
		"content",
		"model_0", "motion_0", "model_1", "motion_1", "model_2", "motion_2",
		"bgm", "function",
		"option_1", "goto_1", "option_2", "goto_2",
		"next"
	};
	private string[] colType = new string[] {
		"text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text"
	};

	static ParaBean ()
	{
		//全局最先调用，这里没用
		print ("ParaManager Construct");
	}

	void Awake ()
	{
		Instance = this;
	}

	//-------------------------public function-------------

	public void InitParaBean (DbAccess db)
	{
		this.db = db;
		//创建数据库表，与字段
		db.CreateTable (Constants.tableNamePara, colName, colType, false);
		//初始化Para表
		WritePara2DB ();
	}

	/// <summary>
	/// 写入JSON到数据库,5.5之后List的GetEnumerator无法再获取IEnumerator类型，所以不能再用Coroutine
	/// </summary>
	/// <returns>The para.</returns>
	private void WritePara2DB ()
	{
		TextAsset paraAsset = Resources.Load<TextAsset> ("paragraph/para_1");
		ParagraphData mData = JsonConvert.DeserializeObject<ParagraphData> (paraAsset.text);
		Resources.UnloadUnusedAssets ();
		List<Paragraph>.Enumerator ie = mData.paragraphList.GetEnumerator ();

		while (ie.MoveNext ()) {
			Paragraph para = ie.Current as Paragraph;
//			print (para.content);
			db.InsertInto (Constants.tableNamePara, new string[] {
				"'" + para.id + "'",
				"'" + para.background + "'",
				"'" + para.portrait + "'",
				"'" + para.content + "'",
				"'" + para.model_0 + "'",
				"'" + para.motion_0 + "'",
				"'" + para.model_1 + "'",
				"'" + para.motion_1 + "'",
				"'" + para.model_2 + "'",
				"'" + para.motion_2 + "'",
				"'" + para.bgm + "'",
				"'" + para.function + "'",
				"'" + para.option_1 + "'",
				"'" + para.goto_1 + "'",
				"'" + para.option_2 + "'",
				"'" + para.goto_2 + "'",
				"'" + para.next + "'"
			});
		}
	}

	//	public void SavePara ()
	//	{
	//
	//	}
	//
	//	public void LoadPara ()
	//	{
	//
	//	}
	//
	//	public void GetAllPara ()
	//	{
	//
	//	}

	/// <summary>
	/// 分段读取
	/// </summary>
	/// <returns>The next para.</returns>
	public Paragraph GetParaFromDB (string id)
	{
		//通过next字段查找下一个Para
		sqReader = db.SelectWhere (Constants.tableNamePara, colName, new string[]{ "id" }, new string[]{ "=" }, new string[]{ "'" + id + "'" });

		//声明Paragraph对象
		Paragraph currentPara = new Paragraph ();
		while (sqReader.Read ()) {//如果上边的查找没有结果，就不会进这里，我觉得最好给end一个特殊标记
//			print ("找到了");
			currentPara = new Paragraph (sqReader.GetString (sqReader.GetOrdinal ("id")), 
				sqReader.GetString (sqReader.GetOrdinal ("background")),
				sqReader.GetString (sqReader.GetOrdinal ("portrait")),
				sqReader.GetString (sqReader.GetOrdinal ("content")),
				sqReader.GetString (sqReader.GetOrdinal ("model_0")),
				sqReader.GetString (sqReader.GetOrdinal ("motion_0")),
				sqReader.GetString (sqReader.GetOrdinal ("model_1")),
				sqReader.GetString (sqReader.GetOrdinal ("motion_1")),
				sqReader.GetString (sqReader.GetOrdinal ("model_2")),
				sqReader.GetString (sqReader.GetOrdinal ("motion_2")),
				sqReader.GetString (sqReader.GetOrdinal ("bgm")),
				sqReader.GetString (sqReader.GetOrdinal ("function")),
				sqReader.GetString (sqReader.GetOrdinal ("option_1")),
				sqReader.GetString (sqReader.GetOrdinal ("goto_1")),
				sqReader.GetString (sqReader.GetOrdinal ("option_2")),
				sqReader.GetString (sqReader.GetOrdinal ("goto_2")),
				sqReader.GetString (sqReader.GetOrdinal ("next")));
		}
		return currentPara;
	}

}
