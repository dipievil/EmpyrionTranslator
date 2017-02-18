using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mergecsv
{
    class Program
    {
        static void Main(string[] args)
        {
            string arquivo_csv_traducoes = "Localization.csv";
            string arquivo_csv_original = "Localization.csv";
            string arquivo_csv_destino = "Localization.csv";
            string pathEmpyrion = @"C:\Program Files (x86)\Steam\steamapps\common\Empyrion - Galactic Survival\";
            string pastaContent = @"Content\Extras";

            Console.WriteLine("#################################");
            Console.WriteLine(" CSV Empyrion localization Merge ");

            if (args.Length > 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine("mergecsv help - exibe esta ajuda");
                    Console.WriteLine("mergecsv - junta os as traduções no arquivo '"+ arquivo_csv_destino + "' na pasta '"+pathEmpyrion+ pastaContent+"'");
                    Console.WriteLine("mergecsv pasta - junta as traduções no arquivo '" + arquivo_csv_destino + pastaContent+ "' na pasta 'pasta'");
                    Console.WriteLine("mergecsv pasta arquivo - junta as traduções no arquivo 'arquivo' na pasta '" + pathEmpyrion+ pastaContent+"'");
                } else
                    arquivo_csv_destino = args[0];

                if (args.Count() > 1)
                    pathEmpyrion = args[1];
            }
            
            if (Directory.Exists(pathEmpyrion + pastaContent))
            {
                if (File.Exists(arquivo_csv_traducoes))
                {
                    string linha = string.Empty;
                    int count = 0;
                
                    List<string> linhasCSV = new List<string>();
                
                    Dictionary<int, string> dicLinhasArquivo = new Dictionary<int, string>();

                    Dictionary<int, string> dicEng = new Dictionary<int, string>();
                    Dictionary<int, string> dicPt = new Dictionary<int, string>();
                    Dictionary<int, string> dicAle = new Dictionary<int, string>();
                    
                    #region Copia o CSV - removendo a última coluna
                    StreamReader file = new StreamReader(arquivo_csv_original);
                    while ((linha = file.ReadLine()) != null)
                    {
                        string[] arDadosLinha = linha
                            .Replace("English (United States)","English")
                            .Replace("\"String Identifier\"","KEY")
                            .Replace("German", "Deutsch")
                            .Replace("Portuguese (Brazil)","Português Brasileiro")
                            .Split(',');
                        arDadosLinha = arDadosLinha.Take(arDadosLinha.Count() - 1).ToArray();
                        linhasCSV.Add(string.Join(",",arDadosLinha));
                        count++;
                    }
                    file.Close();
                    #endregion

                    #region Faz o backup
                    string arquivoBackup = pathEmpyrion + pastaContent + @"\" + arquivo_csv_destino+ ".BAK";
                    string arquivoDestino = pathEmpyrion + pastaContent + @"\" + arquivo_csv_destino;

                    Console.WriteLine("Realizando backup do arquivo original ");

                    if (File.Exists(arquivoBackup)){
                        File.Delete(arquivoBackup + ".BAK");
                    }

                    File.Copy(arquivoDestino, arquivoBackup, true);
                    #endregion

                    #region Copia a tradução
                    using (StreamWriter csvNovo = new StreamWriter(arquivoDestino))
                            foreach (var entry in linhasCSV)
                                csvNovo.WriteLine("{0}", entry);
                    #endregion


                } else {
                    Console.WriteLine("Arquivo não encontrado "+arquivo_csv_traducoes);
                }

            }
            else
            {
                Console.WriteLine("Pasta de destino "+pathEmpyrion + pastaContent + " não existe.");
            }
            
            Console.WriteLine("#################################");

        }
    }
}
