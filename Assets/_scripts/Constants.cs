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
	//数据库地址(自行创建),//注意不是在根目录下,Application.dataPath会读取./assets/下面的
	public static string dbPath = Application.dataPath + "/" + dbName;
	//数据库地址(外部导入)
	public static string dbPathPc = "URI=file:" + Application.streamingAssetsPath + "/" + dbName;
	//Android数据库地址
	public static string dbPathAndroid = Application.persistentDataPath + "/" + dbName;

	public static Vector3 POSITION_CENTER = new Vector3 (0f, 0f, 0f);
	public static Vector3 POSITION_LEFT = new Vector3 (-7f, 0f, 0f);
	public static Vector3 POSITION_RIGHT = new Vector3 (7f, 0f, 0f);
}
