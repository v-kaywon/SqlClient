﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Security;
using System;

namespace Microsoft.Data.SqlClient.Tests
{
    public class AADAccessTokenTest
    {
        private SqlConnectionStringBuilder _builder;
        private SqlCredential _credential = null;
        [Theory]
        [InlineData("Test combination of Access Token and IntegratedSecurity", new object[] { "Integrated Security", true })]
        [InlineData("Test combination of Access Token and User Id", new object[] { "UID", "sampleUserId" })]
        [InlineData("Test combination of Access Token and Password", new object[] { "PWD", "samplePassword" })]
        [InlineData("Test combination of Access Token and Credentials", new object[] { "sampleUserId" })]
        public void InvalidCombinationOfAccessToken(string description, object[] Params)
        {
            string _ = description; // Using C# Discards as workaround to the XUnit warning.
            _builder = new SqlConnectionStringBuilder
            {
                ["Data Source"] = "sample.database.windows.net"
            };

            if (Params.Length == 1)
            {
                SecureString password = new SecureString();
                password.MakeReadOnly();
                _credential = new SqlCredential(Params[0] as string, password);
            }
            else
            {
                _builder[Params[0] as string] = Params[1];
            }
            InvalidCombinationCheck(_credential);
        }

        private void InvalidCombinationCheck(SqlCredential credential)
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString, credential))
            {
                Assert.Throws<InvalidOperationException>(() => connection.AccessToken = "SampleAccessToken");
            }
        }
    }
}
