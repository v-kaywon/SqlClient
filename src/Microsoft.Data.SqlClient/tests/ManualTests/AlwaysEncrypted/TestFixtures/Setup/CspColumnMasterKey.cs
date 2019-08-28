﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Data.SqlClient.ManualTesting.Tests.AlwaysEncrypted.Setup
{
    public class CspColumnMasterKey : ColumnMasterKey
    {
        public StoreLocation CertificateStoreLocation { get; set; } = StoreLocation.CurrentUser;
        public StoreName CertificateStoreName { get; set; } = StoreName.My;
        public string Thumbprint { get; }
        public override string KeyPath { get; }

        public CspColumnMasterKey(string name, string certificateThumbprint) : base(name)
        {
            KeyStoreProviderName = @"MSSQL_CERTIFICATE_STORE";
            Thumbprint = certificateThumbprint;
            KeyPath = string.Concat(CertificateStoreLocation.ToString(), "/", CertificateStoreName.ToString(), "/", Thumbprint);
        }

        public CspColumnMasterKey(string name, string providerName, string cspKeyPath) : base(name)
        {
            KeyStoreProviderName = providerName;
            KeyPath = cspKeyPath;
        }

    }
}
