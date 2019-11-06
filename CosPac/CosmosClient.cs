using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosPac
{
    public class CosmosClient
    {
        public CosmosClient(CosmosServeur serveur)
        {
            Serveur = new Uri(serveur.Url);
            Cle = serveur.Cle;
            CollectionUri = UriFactory.CreateDocumentCollectionUri(serveur.NomBase, serveur.Collection);
        }

        public Uri Serveur { get; }

        public string Cle { get; }

        public Uri CollectionUri { get; }

        public async Task<List<Document>> Lister()
        {
            using (var client = new DocumentClient(Serveur, Cle))
            {
                var query = client.CreateDocumentQuery(CollectionUri).AsDocumentQuery();

                var documentList = new List<Document>();
                while (query.HasMoreResults)
                {
                    var results = await query.ExecuteNextAsync<Document>();
                    documentList.AddRange(results);
                }

                return documentList;
            }
        }
    }
}
