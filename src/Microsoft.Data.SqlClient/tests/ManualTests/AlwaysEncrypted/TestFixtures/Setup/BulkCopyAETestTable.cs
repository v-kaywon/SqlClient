﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Data.SqlClient.ManualTesting.Tests.AlwaysEncrypted.Setup
{
	public class BulkCopyAETestTable : Table
	{
		private const string ColumnEncryptionAlgorithmName = @"AEAD_AES_256_CBC_HMAC_SHA_256";
		private ColumnEncryptionKey columnEncryptionKey1;
		private ColumnEncryptionKey columnEncryptionKey2;

		public BulkCopyAETestTable(string tableName, ColumnEncryptionKey columnEncryptionKey1, ColumnEncryptionKey columnEncryptionKey2) : base(tableName)
		{
			this.columnEncryptionKey1 = columnEncryptionKey1;
			this.columnEncryptionKey2 = columnEncryptionKey2;
		}

		public override void Create(SqlConnection sqlConnection)
		{
			string sql =
				$@"CREATE TABLE [dbo].[{Name}]
                (
                    [c1] varchar(2000) COLLATE Latin1_General_BIN2 ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = [{columnEncryptionKey1.Name}], ENCRYPTION_TYPE = RANDOMIZED, ALGORITHM = '{ColumnEncryptionAlgorithmName}')
                )";

			using (SqlCommand command = sqlConnection.CreateCommand())
			{
				command.CommandText = sql;
				command.ExecuteNonQuery();
			}
		}
	}
}
