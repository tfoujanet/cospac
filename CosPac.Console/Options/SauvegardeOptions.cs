using CommandLine;

namespace CosPac.Console.Options
{
    [Verb("pack", HelpText = "Sauvegarde le contenu d'une collection Cosmos")]
    public class SauvegardeOptions
    {
        [Option('s', "serveur", Required = true, HelpText = "Url du serveur Cosmos")]
        public string ServeurUrl { get; set; }

        [Option('a', "auth", Required = true, HelpText = "Clé d'authentification")]
        public string AuthKey { get; set; }

        [Option('b', "base", Required = true, HelpText = "Nom de la base de données")]
        public string Base { get; set; }

        [Option('c', "collection", Required = true, HelpText = "Nom de la collection")]
        public string Collection { get; set; }

        [Option('o', "sortie", Required = false, Default = "Output.cospac", HelpText = "Nom du fichier de sortie")]
        public string FichierSortie { get; set; }
        
        [Option('p', "propriete", Required = false, HelpText = "Propriété pour le nommage des documents")]
        public string Propriete { get; set; }
    }
}
