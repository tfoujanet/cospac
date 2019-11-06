using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CosPac.Backup
{
    public class CosmosBackup
    {
        public event Action<int> SauvegardeCommencee;
        public event Action<int, int, string> DocumentSauvegarde;
        public event Action<int> SauvegardeTerminee;

        public async Task<BackupReponse> Sauvegarder(BackupRequete requete)
        {
            var client = new CosmosClient(requete.Serveur);
            var documents = await client.Lister();

            SauvegardeCommencee?.Invoke(documents.Count);

            CompresserDocuments(documents.ToList(), requete.FichierSortie, requete.Propriete);

            SauvegardeTerminee?.Invoke(documents.Count);

            return new BackupReponse { FichierSauvegarde = requete.FichierSortie, NombreDocumentsSauvegardes = documents.Count };
        }

        private void CompresserDocuments(List<Document> documents, string fichier, string propriete = null)
        {
            var nombreDocuments = documents.Count;
            using (var zip = new FileStream(fichier, FileMode.OpenOrCreate))
            {
                using (var archive = new ZipArchive(zip, ZipArchiveMode.Update))
                {
                    for (var i = 0; i < nombreDocuments; i++)
                    {
                        var document = documents[i];
                        var nomDocument = string.IsNullOrEmpty(propriete) ? document.Id : document.GetPropertyValue<string>(propriete);
                        var entree = archive.CreateEntry($"{nomDocument}.json", CompressionLevel.Optimal);
                        using (var writer = new StreamWriter(entree.Open()))
                        {
                            writer.Write(JsonConvert.SerializeObject(document));
                        }
                        DocumentSauvegarde?.Invoke(i, nombreDocuments, nomDocument);
                    }
                }
            }
        }
    }
}
