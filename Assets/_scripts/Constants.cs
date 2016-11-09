using UnityEngine;
using System.Collections;

public class Constants
{
	//数据库名
	public const string dbName = "live db.db";
	//para表名
	public const string tableName = "paragraph";
	//数据库版本号
	public const int dataBaseVersion = 1;
	//数据库地址(自行创建)
	public const string dbPath = "./assets/" + dbName;//注意不要放在根目录下,Application.dataPath会读取assets下面的
	//PC数据库地址(外部导入)
//	public static string dbPathPc = "URI=file:" + Application.streamingAssetsPath + "/" + dbName;
	//Android数据库地址(外部导入)
//	public static string dbPathAndroid = "URI=file:" + Application.persistentDataPath + "/" + dbName;

}
