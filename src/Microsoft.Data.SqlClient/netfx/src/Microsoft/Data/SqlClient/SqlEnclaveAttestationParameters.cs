﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Cryptography;

namespace Microsoft.Data.SqlClient
{

    /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/SqlEnclaveAttestationParameters/*' />
    public class SqlEnclaveAttestationParameters
    {

        private static readonly string _clientDiffieHellmanKeyName = "ClientDiffieHellmanKey";
        private static readonly string _inputName = "input";
        private static readonly string _className = "EnclaveAttestationParameters";

        private readonly byte[] _input;

        /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/Protocol/*' />
        public int Protocol { get; }


        /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/ClientDiffieHellmanKey/*' />
        public ECDiffieHellmanCng ClientDiffieHellmanKey { get; }

        /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/GetInput/*' />
        public byte[] GetInput()
        {
            return Clone(_input);
        }

        /// <summary>
        /// Deep copy the array into a new array
        /// </summary>
        /// <param name="arrayToClone"></param>
        /// <returns></returns>
        private byte[] Clone(byte[] arrayToClone)
        {

            if (null == arrayToClone)
            {
                return null;
            }

            byte[] returnValue = new byte[arrayToClone.Length];

            for (int i = 0; i < arrayToClone.Length; i++)
            {
                returnValue[i] = arrayToClone[i];
            }

            return returnValue;
        }

        /// <include file='..\..\..\..\..\..\..\doc\snippets\Microsoft.Data.SqlClient\SqlEnclaveAttestationParameters.xml' path='docs/members[@name="SqlEnclaveAttestationParameters"]/ctor/*' />
        public SqlEnclaveAttestationParameters(int protocol, byte[] input, ECDiffieHellmanCng clientDiffieHellmanKey)
        {
            if (null == clientDiffieHellmanKey)
            { throw SQL.NullArgumentInConstructorInternal(_clientDiffieHellmanKeyName, _className); }
            if (null == input)
            { throw SQL.NullArgumentInConstructorInternal(_inputName, _className); }

            _input = input;
            Protocol = protocol;
            ClientDiffieHellmanKey = clientDiffieHellmanKey;
        }
    }
}
