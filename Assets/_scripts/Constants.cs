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

	//model位置，需要考虑到model高度不同，这里只使用X轴，不要使用Y轴
	public static float POSITION_CENTER = 0f;
	public static float POSITION_LEFT = -7f;
	public static float POSITION_RIGHT = 7f;

	//--------------strings-------------------------------------------------------------------
	public const string CONTINUE_PARA_ID = "continue para id";
}
