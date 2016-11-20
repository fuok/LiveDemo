using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using GameData;
using Newtonsoft.Json;
using System.IO;

//通过DB操作，保存和读取Paragraph，只包含CRUD的CR
public class ParaManager : MonoBehaviour
{
	public static ParaManager Instance{ get; private set; }

	private DbAccess db;
	private SqliteDataReader sqReader;

	static ParaManager ()
	{
		//全局最先调用，这里没用
		print ("ParaManager Construct");
	}

	void Awake ()
	{
		Instance = this;

		//读取/创建数据库
		#if UNITY_EDITOR
		db = new DbAccess ("data source=" + Constants.dbPath);//数据库名//("Server=127.0.0.1;UserId=root;Password=;Database=li")
		#elif UNITY_ANDROID
		db = new DbAccess ("URI=file:" + Constants.dbPathAndroid);
		#endif
	}

	void Start ()
	{
		
	}

	void Update ()
	{

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

	//-------------------------public function-------------

	public void InitPara ()
	{
		//检查数据库版本
		int version = PlayerPrefs.GetInt ("dataBaseVersion", 0);//检查版本号
		print ("version=" + version);
		if (Constants.dataBaseVersion > version) {
			//升级数据库
			try {
				db.DeleteTable (Constants.tableName);
			} catch (System.Exception ex) {

			}
			PlayerPrefs.SetInt ("dataBaseVersion", Constants.dataBaseVersion);
			//创建数据库表，与字段
			db.CreateTable (Constants.tableName, new string[] {
				"id",
				"background", "portrait",
				"content",
				"model_0", "model_1", "model_2",
				"bgm", "function",
				"option_1", "goto_1", "option_2", "goto_2",
				"next"
			}, new string[] {
				"text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text"
			}, false);
			//初始化Para表
			StartCoroutine (WritePara2DB ());
		}
	}

	/// <summary>
	/// 写入JSON到数据库
	/// </summary>
	/// <returns>The para.</returns>
	private IEnumerator WritePara2DB ()
	{
		IEnumerator ie;
		TextAsset paraAsset = Resources.Load<TextAsset> ("paragraph/para_1");
		ParagraphData mData = JsonConvert.DeserializeObject<ParagraphData> (paraAsset.text);
		Resources.UnloadUnusedAssets ();
		ie = mData.paragraphList.GetEnumerator ();
		yield return ie;

		while (ie.MoveNext ()) {
			Paragraph para = ie.Current as Paragraph;
//			print (para.content);
			db.InsertInto (Constants.tableName, new string[] {
				"'" + para.id + "'",
				"'" + para.background + "'",
				"'" + para.portrait + "'",
				"'" + para.content + "'",
				"'" + para.model_0 + "'",
				"'" + para.model_1 + "'",
				"'" + para.model_2 + "'",
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
	public Paragraph GetNextPara (string next)
	{
//		paraIndex++;
		//通过next字段查找下一个Para
		sqReader = db.SelectWhere (Constants.tableName, new string[] {
			"id", "background", "portrait", "content", 
			"model_0", "model_1", "model_2",
			"bgm", "function",
			"option_1", "goto_1", "option_2", "goto_2", 
			"next"
		}, new string[]{ "id" }, new string[]{ "=" }, new string[]{ next });

		//声明Paragraph对象
		Paragraph currentPara = new Paragraph ();
		while (sqReader.Read ()) {//如果上边的查找没有结果，就不会进这里，我觉得最好给end一个特殊标记
//			print ("找到了");
			currentPara = new Paragraph (sqReader.GetString (sqReader.GetOrdinal ("id")), sqReader.GetString (sqReader.GetOrdinal ("background")), sqReader.GetString (sqReader.GetOrdinal ("portrait")), sqReader.GetString (sqReader.GetOrdinal ("content")), sqReader.GetString (sqReader.GetOrdinal ("model_0")), sqReader.GetString (sqReader.GetOrdinal ("model_1")), sqReader.GetString (sqReader.GetOrdinal ("model_2")), sqReader.GetString (sqReader.GetOrdinal ("bgm")), sqReader.GetString (sqReader.GetOrdinal ("function")), sqReader.GetString (sqReader.GetOrdinal ("option_1")), sqReader.GetString (sqReader.GetOrdinal ("goto_1")), sqReader.GetString (sqReader.GetOrdinal ("option_2")), sqReader.GetString (sqReader.GetOrdinal ("goto_2")), sqReader.GetString (sqReader.GetOrdinal ("next")));
		}
		return currentPara;
	}

	/// <summary>
	/// 清除数据库，用于测试
	/// </summary>
	public void CleanParaDB ()
	{
		PlayerPrefs.DeleteKey ("dataBaseVersion");
		db.DeleteTable (Constants.tableName);
	}

}
