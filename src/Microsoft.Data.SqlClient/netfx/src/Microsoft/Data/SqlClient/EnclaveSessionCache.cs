﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Caching;
using System.Threading;

namespace Microsoft.Data.SqlClient
{
    // Maintains a cache of SqlEnclaveSession instances
    internal class EnclaveSessionCache
    {
        private readonly MemoryCache enclaveMemoryCache = new MemoryCache("EnclaveMemoryCache");
        private readonly Object enclaveCacheLock = new Object();

        // Nonce for each message sent by the client to the server to prevent replay attacks by the server,
        // given that for Always Encrypted scenarios, the server is considered an "untrusted" man-in-the-middle.
        private long _counter;

        // Cache timeout of 8 hours to be consistent with jwt validity.
        private static int enclaveCacheTimeOutInHours = 8;

        // Retrieves a SqlEnclaveSession from the cache
        public SqlEnclaveSession GetEnclaveSession(string servername, string attestationUrl, out long counter)
        {
            string cacheKey = GenerateCacheKey(servername, attestationUrl);
            SqlEnclaveSession enclaveSession = enclaveMemoryCache[cacheKey] as SqlEnclaveSession;
            counter = Interlocked.Increment(ref _counter);
            return enclaveSession;
        }

        // Invalidates a SqlEnclaveSession entry in the cache
        public void InvalidateSession(string serverName, string enclaveAttestationUrl, SqlEnclaveSession enclaveSessionToInvalidate)
        {
            string cacheKey = GenerateCacheKey(serverName, enclaveAttestationUrl);

            lock (enclaveCacheLock)
            {
                long counter;
                SqlEnclaveSession enclaveSession = GetEnclaveSession(serverName, enclaveAttestationUrl, out counter);

                if (enclaveSession != null && enclaveSession.SessionId == enclaveSessionToInvalidate.SessionId)
                {
                    SqlEnclaveSession enclaveSessionRemoved = enclaveMemoryCache.Remove(cacheKey) as SqlEnclaveSession;
                    if (enclaveSessionRemoved == null)
                    {
                        throw new InvalidOperationException(Strings.EnclaveSessionInvalidationFailed);
                    }
                }
            }
        }

        // Creates a new SqlEnclaveSession and adds it to the cache
        public SqlEnclaveSession CreateSession(string attestationUrl, string serverName, byte[] sharedSecret, long sessionId, out long counter)
        {
            string cacheKey = GenerateCacheKey(serverName, attestationUrl);
            SqlEnclaveSession enclaveSession = null;
            lock (enclaveCacheLock)
            {
                enclaveSession = new SqlEnclaveSession(sharedSecret, sessionId);
                enclaveMemoryCache.Add(cacheKey, enclaveSession, DateTime.UtcNow.AddHours(enclaveCacheTimeOutInHours));
                counter = Interlocked.Increment(ref _counter);
            }

            return enclaveSession;
        }

        // Generates the cache key for the enclave session cache
        private string GenerateCacheKey(string serverName, string attestationUrl)
        {
            return (serverName + attestationUrl).ToLowerInvariant();
        }
    }
}
