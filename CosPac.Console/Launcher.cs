using CosPac.Backup;
using CosPac.Console.Options;
using ShellProgressBar;
using System;
using System.Threading.Tasks;

namespace CosPac.Console
{
    public class Launcher
    {
        public Launcher()
        {
            ProgressBarOptions = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593'
            };
        }

        public ProgressBarOptions ProgressBarOptions { get; }

        public ProgressBar ProgressBar { get; set; }

        public async Task Backup(SauvegardeOptions options)
        {
            var backup = new CosmosBackup();
            backup.SauvegardeCommencee += InitialiserProgressBar;
            backup.DocumentSauvegarde += NotifierAvancement;
            backup.SauvegardeTerminee += TerminerSauvegarde;

            var reponse = await backup.Sauvegarder(new BackupRequete
            {
                FichierSortie = options.FichierSortie,
                Propriete = options.Propriete,
                Serveur = new CosmosServeur
                {
                    Url = options.ServeurUrl,
                    Cle = options.AuthKey,
                    NomBase = options.Base,
                    Collection = options.Collection
                }
            });

            backup.SauvegardeCommencee -= InitialiserProgressBar;
            backup.DocumentSauvegarde -= NotifierAvancement;
            backup.SauvegardeTerminee -= TerminerSauvegarde;
        }

        private void InitialiserProgressBar(int total)
        {
            ProgressBar = new ProgressBar(total, "Sauvegarde commencée", ProgressBarOptions);
        }

        private void NotifierAvancement(int index, int total, string nomFichier)
        {
            ProgressBar.Tick($"{index + 1} / {total} documents sauvegardés.");
        }

        private void TerminerSauvegarde(int total)
        {
            ProgressBar.Dispose();
        }
    } 
}
