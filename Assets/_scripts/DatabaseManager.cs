using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
	public static DatabaseManager Instance{ get; private set; }

	public DbAccess db;

	void Awake ()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		//读取/创建数据库,目前只有android和pc后需要加入ios
		#if UNITY_ANDROID
		db = new DbAccess ("URI=file:" + Constants.dbPathAndroid);
		#else
		db = new DbAccess ("data source=" + Constants.dbPath);//数据库名//("Server=127.0.0.1;UserId=root;Password=;Database=li")
		#endif

		//检查数据库版本
		int version = PlayerPrefs.GetInt (Constants.DATABASE_VERSION, 0);//检查版本号
		print ("version=" + version);
		if (Constants.dataBaseVersion > version) {
			//升级数据库
			try {
				db.DeleteTable (Constants.tableName);
			} catch (System.Exception ex) {

			}
			PlayerPrefs.SetInt (Constants.DATABASE_VERSION, Constants.dataBaseVersion);
		}
		//初始化bean
		ParaBean.Instance.InitParaBean (db);
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
