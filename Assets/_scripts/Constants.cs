#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

using UnityEngine;

public class Constants
{
	//沙盒目录
	#if ANDROID || IPHONE
	public static string SAVE_PATH = Application.persistentDataPath + "/";
	#else
	public static string SAVE_PATH = Application.dataPath + "/_save/";
	#endif

	//数据库名
	public const string dbName = "live db.db";
	//表名
	public const string tableNamePara = "paragraph";
	public const string tableNameSave = "gamesave";
	//数据库版本号
	public const int dataBaseVersion = 1;
	//数据库地址(StandAlone),//注意不是在根目录下,Application.dataPath会读取./assets/下面的
	public static string dbPath = "data source=" + Application.dataPath + "/" + dbName;
	//数据库地址(Android)
	public static string dbPathAndroid = "URI=file:" + Application.persistentDataPath + "/" + dbName;
	//数据库地址(IOS)
	public static string dbPathIos = @"data source=" + Application.persistentDataPath + "/" + dbName;

	//数据库地址(已经写好数据直接下载，应该也是Android的格式)
	public static string dbPathStreamIn = "URI=file:" + Application.streamingAssetsPath + "/" + dbName;

	//model位置，需要考虑到model高度不同，这里只使用X轴，不要使用Y轴
	public static float POSITION_CENTER = 0f;
	public static float POSITION_LEFT = -7f;
	public static float POSITION_RIGHT = 7f;

	//表示从头开始或者是从中断处开始游戏
	public static bool fromBeginning = true;

	//--------------playerprefab keys-------------------------------------------------------------------
	public const string CONTINUE_PARA_ID = "continue para id";
	public const string DATABASE_VERSION = "database version";
}
