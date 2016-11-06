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
		Debug.Log (sqlQuery);
		dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = sqlQuery;
		//			dbCommand = new SqliteCommand (sqlQuery, dbConnection);

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
	public SqliteDataReader InsertInto (string tableName, string[] values)
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
	public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols, string[] values)
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
	public SqliteDataReader Delete (string tableName, string[]cols, string[]colsvalues)
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
	/// 按条件查询数据
	/// </summary>
	/// <returns>The where.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="items">Items.</param>
	/// <param name="col">Col.</param>
	/// <param name="operation">Operation.</param>
	/// <param name="values">Values.</param>
	public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values)
	{
		if (col.Length != operation.Length || operation.Length != values.Length) {		
			throw new SqliteException ("col.Length != operation.Length != values.Length");
		}

		string query = "SELECT " + items [0];

		for (int i = 1; i < items.Length; ++i) {
			query += ", " + items [i];
		}

		query += " FROM " + tableName + " WHERE " + col [0] + operation [0] + "'" + values [0] + "' ";

		for (int i = 1; i < col.Length; ++i) {
			query += " AND " + col [i] + operation [i] + "'" + values [0] + "' ";
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
	public SqliteDataReader UpdateInto (string tableName, string[]cols, string[]colsvalues, string selectkey, string selectvalue)
	{
		string query = "UPDATE " + tableName + " SET " + cols [0] + " = " + "'" + colsvalues [0] + "'";

		for (int i = 1; i < colsvalues.Length; ++i) {
			query += ", " + cols [i] + " =" + "'" + colsvalues [i] + "'";
		}

		query += " WHERE " + selectkey + " = " + "'" + selectvalue + "'";

		return ExecuteQuery (query);
	}


}
