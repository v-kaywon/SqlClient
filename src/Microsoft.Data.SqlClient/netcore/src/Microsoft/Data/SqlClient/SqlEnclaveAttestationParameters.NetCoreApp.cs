﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Cryptography;

namespace Microsoft.Data.SqlClient
{
    /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/SqlEnclaveAttestationParameters/*' />
    public partial class SqlEnclaveAttestationParameters
    {
        private static readonly string _clientDiffieHellmanKeyName = "ClientDiffieHellmanKey";
        private static readonly string _inputName = "input";
        private static readonly string _className = "EnclaveAttestationParameters";

        /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/ClientDiffieHellmanKey/*' />
        public ECDiffieHellmanCng ClientDiffieHellmanKey { get; }

        /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/ctor/*' />
        public SqlEnclaveAttestationParameters(int protocol, byte[] input, ECDiffieHellmanCng clientDiffieHellmanKey)
        {
            _input = input ?? throw SQL.NullArgumentInConstructorInternal(_inputName, _className);
            Protocol = protocol;
            ClientDiffieHellmanKey = clientDiffieHellmanKey ?? throw SQL.NullArgumentInConstructorInternal(_clientDiffieHellmanKeyName, _className);
        }
    }
}
