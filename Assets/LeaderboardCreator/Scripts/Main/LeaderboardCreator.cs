﻿using System;
using System.Collections;
using Dan.Enums;
using Dan.Models;
using UnityEngine;

using static Dan.ConstantVariables;

namespace Dan.Main
{
    public static class LeaderboardCreator
    {
        public static bool LoggingEnabled { get; set; } = true;
        
        private static LeaderboardCreatorBehaviour _behaviour;

        internal static string UserGuid;

        public static string UserID => UserGuid;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Log("Initializing...");
            _behaviour = new GameObject("[LeaderboardCreator]").AddComponent<LeaderboardCreatorBehaviour>();
            UnityEngine.Object.DontDestroyOnLoad(_behaviour.gameObject);

            _behaviour.Authorize(OnAuthorizationAttempted);
        }

        private static void OnAuthorizationAttempted(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                Log("<b><color=#FF0000>Failed to connect to server, trying again...</color></b>");

                IEnumerator Co()
                {
                    yield return new WaitForSeconds(5f);
                    _behaviour.Authorize(OnAuthorizationAttempted);
                }

                _behaviour.StartCoroutine(Co());
                return;
            }
            UserGuid = guid;
            Log("<b><color=#009900>Initialized!</color></b>");
        }

        /// <summary>
        /// Pings the server to check if a connection can be established.
        /// </summary>
        /// <param name="isOnline">If true, the server is online, else connection failed.</param>
        public static void Ping(Action<bool> isOnline) => _behaviour.SendGetRequest(GetServerURL(), isOnline);

        /// <summary>
        /// Fetches a leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">The public key of the leaderboard
        /// (retrieve from https://lcv2.danqzq.games).</param>
        /// <param name="callback">Returns entries of the leaderboard if the request was successful.</param>
        public static void GetLeaderboard(string publicKey, Action<Entry[]> callback) => 
            GetLeaderboard(publicKey, false, LeaderboardSearchQuery.Default, callback);

        /// <summary>
        /// Fetches a leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">The public key of the leaderboard
        /// (retrieve from https://lcv2.danqzq.games).</param>
        /// <param name="isInAscendingOrder">If true, the leaderboard will be sorted in ascending order.</param>
        /// <param name="callback">Returns entries of the leaderboard if the request was successful.</param>
        public static void GetLeaderboard(string publicKey, bool isInAscendingOrder, Action<Entry[]> callback) => 
            GetLeaderboard(publicKey, isInAscendingOrder, LeaderboardSearchQuery.Default, callback);
        
        /// <summary>
        /// Fetches a leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">The public key of the leaderboard
        /// (retrieve from https://lcv2.danqzq.games).</param>
        /// <param name="searchQuery">A struct with additional search parameters for filtering entries.</param>
        /// <param name="callback">Returns entries of the leaderboard if the request was successful.</param>
        public static void GetLeaderboard(string publicKey, LeaderboardSearchQuery searchQuery, Action<Entry[]> callback) => 
            GetLeaderboard(publicKey, false, searchQuery, callback);

        /// <summary>
        /// Fetches a leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">The public key of the leaderboard
        /// (retrieve from https://lcv2.danqzq.games).</param>
        /// <param name="isInAscendingOrder">If true, the leaderboard will be sorted in ascending order.</param>
        /// <param name="searchQuery">A struct with additional search parameters for filtering entries.</param>
        /// <param name="callback">Returns entries of the leaderboard if the request was successful.</param>
        public static void GetLeaderboard(string publicKey, bool isInAscendingOrder, LeaderboardSearchQuery searchQuery, Action<Entry[]> callback)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                LogError("Public key cannot be null or empty!");
                return;
            }

            var query = $"?publicKey={publicKey}&userGuid={UserGuid}&isInAscendingOrder={(isInAscendingOrder ? 1 : 0)}";
            query += searchQuery.ChainQuery();
            
            _behaviour.SendGetRequest(GetServerURL(Routes.Get, query), callback);
        }
        
        /// <summary>
        /// Uploads a new entry to the leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">The public key of the leaderboard</param>
        /// <param name="username">The username of the player</param>
        /// <param name="score">The highscore of the player</param>
        /// <param name="callback">Returns true if the request was successful.</param>
        /// <param name="errorCallback">Returns an error message if the request failed.</param>
        public static void UploadNewEntry(string publicKey, string username, int score, Action<bool> callback = null, Action<string> errorCallback = null) => 
            UploadNewEntry(publicKey, username, score, " ", callback, errorCallback);

        /// <summary>
        /// Uploads a new entry to the leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">The public key of the leaderboard</param>
        /// <param name="username">The username of the player</param>
        /// <param name="score">The highscore of the player</param>
        /// <param name="extra">Extra data to be stored with the entry (max length of 100, unless using an advanced leaderboard)</param>
        /// <param name="callback">Returns true if the request was successful.</param>
        /// <param name="errorCallback">Returns an error message if the request failed.</param>
        public static void UploadNewEntry(string publicKey, string username, int score, string extra, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                LogError("Public key cannot be null or empty!");
                return;
            }

            if (string.IsNullOrEmpty(username))
            {
                LogError("Username cannot be null or empty!");
                return;
            }

            callback += isSuccessful =>
            {
                if (!isSuccessful)
                    LogError("Uploading entry data failed!");
                else
                    Log("Successfully uploaded entry data to leaderboard!");
            };
            
            _behaviour.SendPostRequest(GetServerURL(Routes.Upload), Requests.Form(
                Requests.Field("publicKey", publicKey),
                Requests.Field("username", username),
                Requests.Field("score", score.ToString()),
                Requests.Field("extra", UserGuid),
                Requests.Field("userGuid", UserGuid)), callback, errorCallback);
        }

        /// <summary>
        /// Updates the username of the entry in a leaderboard with the given public key.
        /// </summary>
        /// <param name="publicKey">Public key of the leaderboard.</param>
        /// <param name="username">The new username of the player.</param>
        /// <param name="callback">Returns true if the request was successful.</param>
        /// <param name="errorCallback">Returns an error message if the request failed.</param>
        public static void UpdateEntryUsername(string publicKey, string username, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                LogError("Public key cannot be null or empty!");
                return;
            }
            
            if (string.IsNullOrEmpty(username))
            {
                LogError("Username cannot be null or empty!");
                return;
            }
            
            callback += isSuccessful =>
            {
                if (!isSuccessful)
                    LogError("Updating entry's username failed!");
                else
                    Log("Successfully updated player's username!");
            };
            
            _behaviour.SendPostRequest(GetServerURL(Routes.UpdateUsername), Requests.Form(
                Requests.Field("publicKey", publicKey),
                Requests.Field("username", username),
                Requests.Field("userGuid", UserGuid)), callback, errorCallback);
        }
        
        /// <summary>
        /// Deletes the entry in a leaderboard, with the given public key.
        /// </summary>
        /// <param name="publicKey">Public key of the leaderboard.</param>
        /// <param name="callback">Returns true if the request was successful.</param>
        /// <param name="errorCallback">Returns an error message if the request failed.</param>
        public static void DeleteEntry(string publicKey, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                LogError("Public key cannot be null or empty!");
                return;
            }
            
            callback += isSuccessful =>
            {
                if (!isSuccessful)
                    LogError("Deleting entry failed!");
                else
                    Log("Successfully deleted player's entry!");
            };
            
            _behaviour.SendPostRequest(GetServerURL(Routes.DeleteEntry), Requests.Form(
                Requests.Field("publicKey", publicKey),
                Requests.Field("userGuid", UserGuid)), callback, errorCallback);
        }
        
        public static void DeleteEntry(string publicKey, string userName, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                LogError("Public key cannot be null or empty!");
                return;
            }

            callback += isSuccessful =>
            {
                if (!isSuccessful)
                    LogError("Deleting entry failed!");
                else
                    Log("Successfully deleted player's entry!");
            };

            _behaviour.SendPostRequest(GetServerURL(Routes.DeleteEntry), Requests.Form(
                Requests.Field("publicKey", publicKey),
                Requests.Field("username", userName)), callback, errorCallback);
        }

        /// <summary>
        /// Resets a player's unique identifier and allows them to submit a new entry to the leaderboard.
        /// </summary>
        public static void ResetPlayer(Action onReset = null)
        {
            _behaviour.ResetAndAuthorize(OnAuthorizationAttempted, onReset);
        }

        internal static void Log(string message)
        {
            if (!LoggingEnabled) return;
            Debug.Log($"[LeaderboardCreator] {message}");
        }
        
        internal static void LogError(string message)
        {
            if (!LoggingEnabled) return;
            Debug.LogError($"[LeaderboardCreator] {message}");
        }
    }
}