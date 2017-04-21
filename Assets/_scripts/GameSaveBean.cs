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
		"savId", "savParaId", "savText", "savImgPath"
	};
	private string[] colType = new string[] {
		"integer", "text", "text", "text"
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
	}

	public void AddGameSave2DB (GameSave save)
	{
		db.InsertInto (Constants.tableNameSave, new string[] {
			"'" + save.savId + "'",
			"'" + save.savParaId + "'",
			"'" + save.savText + "'",
			"'" + save.savImgPath + "'"
		});
	}

	public GameSave GetGameSaveFromDB (int id)
	{
//		sqReader = db.SelectWhere (Constants.tableNameSave, colName, new string[]{ "id" }, new string[]{ "=" }, new string[]{ id });
		return null;
	}
}
