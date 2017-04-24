using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;

public class DbAccess
{
	private SqliteConnection dbConnection;
	private SqliteCommand dbCommand;
	private SqliteDataReader reader;



	public DbAccess (string connectionString)
	{
		OpenDB (connectionString);	
	}

	public DbAccess ()
	{

	}

	/// <summary>
	/// 打开数据库
	/// </summary>
	/// <param name="connectionString">Connection string.</param>
	public void OpenDB (string connectionString)
	{
		try {
			dbConnection = new SqliteConnection (connectionString);
			dbConnection.Open ();
			Debug.Log ("version:" + dbConnection.ServerVersion);

			//				Debug.Log ("Connected to db");
		} catch (Exception e) {
			string temp1 = e.ToString ();
			//				Debug.Log(temp1);
		}

	}

	/// <summary>
	/// 关闭数据库
	/// </summary>
	public void CloseSqlConnection ()
	{

		if (dbCommand != null) {
			dbCommand.Dispose ();
		}
		dbCommand = null;

		if (reader != null) {
			reader.Dispose ();
		}
		reader = null;

		if (dbConnection != null) {
			dbConnection.Close ();
		}
		dbConnection = null;

		//			Debug.Log ("Disconnected from db.");

	}

	/// <summary>
	/// 执行SQL
	/// </summary>
	/// <returns>The query.</returns>
	/// <param name="sqlQuery">Sql query.</param>
	private SqliteDataReader ExecuteQuery (string sqlQuery)
	{
//		Debug.Log ("ExecuteQuery:" + sqlQuery);
//		dbCommand = new SqliteCommand (sqlQuery, dbConnection);
		dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = sqlQuery;
		reader = dbCommand.ExecuteReader ();
		return reader;
	}

	/// <summary>
	/// 创建数据表
	/// </summary>
	/// <returns>The table.</returns>
	/// <param name="name">Name.</param>
	/// <param name="col">Col.</param>
	/// <param name="colType">Col type.</param>
	/// <param name="autoId">id自增长.</param>
	public SqliteDataReader CreateTable (string name, string[] col, string[] colType, bool autoId)
	{
		if (col.Length != colType.Length) {
			throw new SqliteException ("columns.Length != colType.Length");
		}

		string query = "CREATE TABLE IF NOT EXISTS " + name + " (";
		if (autoId) {
			query += "id INTEGER PRIMARY KEY AUTOINCREMENT,";
		}
		query += col [0] + " " + colType [0];

		for (int i = 1; i < col.Length; ++i) {
			query += ", " + col [i] + " " + colType [i];
		}

		query += ")";

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 插入数据
	/// </summary>
	/// <returns>The into.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="values">Values.</param>
	public SqliteDataReader InsertInto (string tableName, object[] values)
	{
		string query = "INSERT INTO " + tableName + " VALUES (" + values [0];

		for (int i = 1; i < values.Length; ++i) {
			query += ", " + values [i];
		}

		query += ")";

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 在指定列插入数据
	/// </summary>
	/// <returns>The into specific.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="cols">Cols.</param>
	/// <param name="values">Values.</param>
	public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols, object[] values)
	{
		if (cols.Length != values.Length) {
			throw new SqliteException ("columns.Length != values.Length");
		}

		string query = "INSERT INTO " + tableName + "(" + cols [0];

		for (int i = 1; i < cols.Length; ++i) {
			query += ", " + cols [i];
		}

		query += ") VALUES (" + values [0];

		for (int i = 1; i < values.Length; ++i) {
			query += ", " + values [i];
		}

		query += ")";

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 清空表,并不删除表
	/// </summary>
	/// <returns>The contents.</returns>
	/// <param name="tableName">Table name.</param>
	public SqliteDataReader DeleteContents (string tableName)
	{
		string query = "DELETE FROM " + tableName;

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 删除表
	/// </summary>
	/// <returns>The table.</returns>
	/// <param name="tableName">Table name.</param>
	public SqliteDataReader DeleteTable (string tableName)
	{
		string query = "DROP TABLE " + tableName;

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 删除数据
	/// </summary>
	/// <param name="tableName">Table name.</param>
	/// <param name="cols">Cols.</param>
	/// <param name="colsvalues">Colsvalues.</param>
	public SqliteDataReader Delete (string tableName, string[] cols, object[] colsvalues)
	{
		string query = "DELETE FROM " + tableName + " WHERE " + cols [0] + " = " + colsvalues [0];

		for (int i = 1; i < colsvalues.Length; ++i) {
			query += " or " + cols [i] + " = " + colsvalues [i];
		}
		//			Debug.Log(query);
		return ExecuteQuery (query);//SQLite貌似没有ExecuteNonQuery()
	}

	/// <summary>
	/// 查询全表
	/// </summary>
	/// <returns>The full table.</returns>
	/// <param name="tableName">Table name.</param>
	public SqliteDataReader ReadFullTable (string tableName)
	{
		string query = "SELECT * FROM " + tableName;

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 按条件查询数据,2017-04-22
	/// </summary>
	/// <returns>The where.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="items">查找的列名，一般是全部</param>
	/// <param name="col">作为查找条件的列名</param>
	/// <param name="operation">Operation.</param>
	/// <param name="values">查找的值，类型是object，string型需要加单引号</param>
	public SqliteDataReader SelectWhere (string tableName, string[] searchCols, string[] selectCol, string[] operation, object[] selectvalue)
	{
		if (selectCol.Length != operation.Length || operation.Length != selectvalue.Length) {		
			throw new SqliteException ("col.Length != operation.Length != values.Length");
		}
		string query = "SELECT " + searchCols [0];

		for (int i = 1; i < searchCols.Length; ++i) {
			query += ", " + searchCols [i];
		}

		query += " FROM " + tableName + " WHERE " + selectCol [0] + operation [0] + selectvalue [0];

		for (int i = 1; i < selectCol.Length; ++i) {
			query += " AND " + selectCol [i] + operation [i] + selectvalue [0];
		}

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 更新数据
	/// </summary>
	/// <returns>The into.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="cols">Cols.</param>
	/// <param name="colsvalues">Colsvalues.</param>
	/// <param name="selectkey">Selectkey.</param>
	/// <param name="selectvalue">Selectvalue.</param>
	public SqliteDataReader UpdateInto (string tableName, string[] targetCols, object[] targetValues, string selectCol, object selectvalue)
	{
		string query = "UPDATE " + tableName + " SET " + targetCols [0] + " = " + targetValues [0];

		for (int i = 1; i < targetValues.Length; ++i) {
			query += ", " + targetCols [i] + " =" + targetValues [i];
		}

		query += " WHERE " + selectCol + " = " + selectvalue;

		return ExecuteQuery (query);
	}


}
