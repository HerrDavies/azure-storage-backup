using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class AzureTableStorage
    {
        private string _accountName;
        private string _accountKey;

        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;


        public AzureTableStorage WithName(string name)
        {
            _accountName = name;
            return this;
        }

        public AzureTableStorage WithKey(string key)
        {
            _accountKey = key;
            return this;
        }

        public AzureTableStorage Initialize()
        {
            if (String.IsNullOrEmpty(_accountName))
                throw new NullReferenceException("An account name must be provided");

            if (String.IsNullOrEmpty(_accountKey))
                throw new NullReferenceException("An account key must be provided");

            _storageAccount = new CloudStorageAccount(new StorageCredentials(_accountName, _accountKey), true);
            _tableClient = _storageAccount.CreateCloudTableClient();

            return this;
        }

        public IEnumerable<CloudTable> TableListGet()
        {
            return _tableClient.ListTables();
        }

        public IEnumerable<DynamicTableEntity> TableContentsGet(string tableName)
        {
            CloudTable table = _tableClient.GetTableReference(tableName);

            TableQuery query = new TableQuery { };
            
            return table.ExecuteQuery(query);
        }
    }
}
