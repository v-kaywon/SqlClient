using System;
using System.Data;
// <Snippet1>
using Microsoft.Data.SqlClient;

class Program
{
    private static void AddSqlParameter(SqlCommand command, string paramValue)
    {
        SqlParameter parameter = new SqlParameter("@Description", SqlDbType.VarChar);
        parameter.IsNullable = true;
        parameter.Direction = ParameterDirection.Output;
        parameter.Size = 88;
        parameter.Value = paramValue;

        command.Parameters.Add(parameter);
    }
}
// </Snippet1>