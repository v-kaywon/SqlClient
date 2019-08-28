﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Data.SqlClient.ManualTesting.Tests
{
    public class ConnectionBehaviorTest
    {
        [ConditionalFact(typeof(DataTestUtility), nameof(DataTestUtility.AreConnStringsSetup))]
        public void ConnectionBehaviorClose()
        {
            using (SqlConnection sqlConnection = new SqlConnection((new SqlConnectionStringBuilder(DataTestUtility.TcpConnStr) { MaxPoolSize = 1 }).ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT '1'", sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            string result = reader[0].ToString();
                        }
                    }

                    Assert.Equal(ConnectionState.Closed, sqlConnection.State);
                }
            }
        }

        [ConditionalFact(typeof(DataTestUtility), nameof(DataTestUtility.AreConnStringsSetup))]
        public void ConnectionBehaviorCloseAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection((new SqlConnectionStringBuilder(DataTestUtility.TcpConnStr) { MaxPoolSize = 1 }).ConnectionString))
            {
                Task<bool> result = VerifyConnectionBehaviorCloseAsync(sqlConnection);
                bool value = result.Result;
            }
        }

        private async Task<bool> VerifyConnectionBehaviorCloseAsync(SqlConnection sqlConnection)
        {
            using (SqlCommand command = new SqlCommand("SELECT '1'", sqlConnection))
            {
                await sqlConnection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        string result = reader[0].ToString();
                    }
                }

                Assert.Equal(ConnectionState.Closed, sqlConnection.State);
            }

            return true;
        }
    }
}
