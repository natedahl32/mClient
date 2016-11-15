﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.BotServer
{
    public class BotServer
    {
        #region Declarations

        // Holds all bot accounts currently registered with the server
        private List<BotAccount> mBotAccounts = new List<BotAccount>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file path the information in this class is saved to
        /// </summary>
        private string FilePath { get { return @"botaccounts.json"; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes quests to file
        /// </summary>
        public void Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(FilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, mBotAccounts);
        }

        /// <summary>
        /// Loads existing quests saved to file
        /// </summary>
        public void Load()
        {
            if (!File.Exists(FilePath)) return;

            var data = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim())) return;
            mBotAccounts = JsonConvert.DeserializeObject<List<BotAccount>>(data);
        }

        /// <summary>
        /// Adds an existing bot account.
        /// </summary>
        /// <param name="botAccount"></param>
        public void AddExistingBotAccount(BotAccount botAccount)
        {
            if (mBotAccounts.Any(a => a.AccountName == botAccount.AccountName))
                throw new ApplicationException(string.Format("Cannot add a duplicate bot account with account name {0}", botAccount.AccountName));
            if (mBotAccounts.Any(a => a.CharacterName == botAccount.CharacterName))
                throw new ApplicationException(string.Format("Cannot add a duplicate bot account with character name {0}", botAccount.CharacterName));

            mBotAccounts.Add(botAccount);
        }

        #endregion

        #region Private Methods

        private void EventHandle(Event e)
        {
            // TODO: Not sure we need this
        }

        #endregion
    }
}