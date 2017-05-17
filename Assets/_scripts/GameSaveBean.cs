using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using GameData;

public class GameSaveBean : MonoBehaviour
{
	public static GameSaveBean Instance{ get; private set; }

	private DbAccess db;
	private SqliteDataReader sqReader;

	private string[] colName = new string[] {
		"savId", "savParaId", "savTime", "savText", "savImgPath"
	};
	private string[] colType = new string[] {
		"integer", "text", "text", "text", "text"
	};

	void Awake ()
	{
		Instance = this;
	}

	//-------------------------public function-------------

	public void InitSaveBean (DbAccess db)
	{
		this.db = db;
		//创建数据库表，与字段
		db.CreateTable (Constants.tableNameSave, colName, colType, false);
		//此处应该添加一个空存档id=0，这样当删除记录后取到空数据就会显示为这个,TODO
		GameSave save = new GameSave (0, "", "", "", Constants.LOCAL_PATH + "/" + "default.png");//现在只是取到一个空值，实际上获取不到这一条?而且这一条反复存入了
		AddGameSave2DB (save);
	}

	public void AddGameSave2DB (GameSave save)
	{
		db.InsertInto (Constants.tableNameSave, new object[] {
			save.savId,
			"'" + save.savParaId + "'",
			"'" + save.savTime + "'",
			"'" + save.savText + "'",
			"'" + save.savImgPath + "'"
		});
	}

	public GameSave GetGameSaveFromDB (int id)
	{
		sqReader = db.SelectWhere (Constants.tableNameSave, colName, new string[]{ "savId" }, new string[]{ "=" }, new object[]{ id });
		GameSave save = new GameSave ();
		while (sqReader.Read ()) {
			save = new GameSave (sqReader.GetInt32 (sqReader.GetOrdinal ("savId")), 
				sqReader.GetString (sqReader.GetOrdinal ("savParaId")),
				sqReader.GetString (sqReader.GetOrdinal ("savTime")),
				sqReader.GetString (sqReader.GetOrdinal ("savText")),
				sqReader.GetString (sqReader.GetOrdinal ("savImgPath")));
		}
		return save;
	}

	public void DeleteGameSave (int id)
	{
		sqReader = db.Delete (Constants.tableNameSave, new string[]{ "savId" }, new object[]{ id });
	}
}
