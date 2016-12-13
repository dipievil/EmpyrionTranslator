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
            string arquivo_ptbr = "Localization_ptbr.csv";
            string arquivo_csv_original = "Localization.csv";
            string arquivo_csv_destino = "Localization.csv";
            string arquivo_fr = "Localization_fr.csv";
            string pastaEmpyrion = @"C:\Program Files (x86)\Steam\steamapps\common\Empyrion - Galactic Survival\Content\Extras";

            Console.WriteLine("#################################");
            Console.WriteLine(" CSV Empyrion localization Merge ");

            if (args.Length > 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine("mergecsv help - exibe esta ajuda");
                    Console.WriteLine("mergecsv - junta os as traduções no arquivo '"+ arquivo_csv_destino + "' na pasta '"+pastaEmpyrion+"'");
                    Console.WriteLine("mergecsv pasta - junta as traduções no arquivo '" + arquivo_csv_destino + "' na pasta 'pasta'");
                    Console.WriteLine("mergecsv pasta arquivo - junta as traduções no arquivo 'arquivo' na pasta '" + pastaEmpyrion+"'");
                } else
                {
                    arquivo_csv_destino = args[0];
                }

                if (args.Count() > 1)
                {
                    pastaEmpyrion = args[1];
                }
            }
            
            if (Directory.Exists(pastaEmpyrion))
            {
                if (File.Exists(arquivo_ptbr))
                {
                    string linha = string.Empty;
                    int count = 0;
                
                    List<string> linhasCSV = new List<string>();
                
                    Dictionary<int, string> dicLinhasArquivo = new Dictionary<int, string>();

                    Dictionary<int, string> dicEng = new Dictionary<int, string>();
                    Dictionary<int, string> dicPt = new Dictionary<int, string>();
                    Dictionary<int, string> dicAle = new Dictionary<int, string>();

                    #region Lista Inicial
                    StreamReader file = new StreamReader(arquivo_csv_original);
                    while ((linha = file.ReadLine()) != null)
                    {
                        dicLinhasArquivo.Add(count,linha);
                        count++;
                    }
                    file.Close();
                    #endregion

                    #region Adiciona portugues
                    Console.WriteLine(" Adicionando Portguês(BR) ");
                    count = 0;
                    file = new StreamReader(arquivo_ptbr);
                    while ((linha = file.ReadLine()) != null)
                    {
                        if (count > 0) {
                            string[] valores = linha.Split(',');

                            var linhadados = dicLinhasArquivo.FirstOrDefault(x => x.Value.Contains(valores[0] + ","));

                            int chaveLinha = linhadados.Key;
                            if (chaveLinha > 0)
                                dicLinhasArquivo[chaveLinha] = linhadados.Value + "," + valores[1].Replace("\"","");
                        
                        } else
                        {
                             dicLinhasArquivo[0] = dicLinhasArquivo[0] + @",Portugues" ;
                        }
                        count++;
                    }
                    file.Close();
                    #endregion

                    #region Adiciona o ingles onde não teve tradução

                    Dictionary<int, string> dicTemp = new Dictionary<int, string>();

                    foreach (var linhaDados in dicLinhasArquivo)
                    {
                        int key = linhaDados.Key;
                        string linhaCorreta = linhaDados.Value;
                        if (key > 0){
                            string[] colunas = linhaDados.Value.Split(',');
                            if (colunas.Length < 4)
                            {
                                linhaCorreta = linhaDados.Value + "," + colunas[1];
                            } 
                        }
                        dicTemp[key] = linhaCorreta;
                    }

                    dicLinhasArquivo = dicTemp;

                    #endregion

                    if (File.Exists(arquivo_fr))
                    {
                        count = 0;
                        file = new StreamReader(arquivo_fr);

                        #region Adiciona frances
                        Console.WriteLine(" Adicionando Francês(CA) ");

                        while ((linha = file.ReadLine()) != null)
                        {
                            if (count > 0)
                            {
                                string[] valores = linha.Split(',');

                                var linhadados = dicLinhasArquivo.FirstOrDefault(x => x.Value.Contains(valores[0] + ","));

                                int chaveLinha = linhadados.Key;
                                if (chaveLinha > 0)
                                    dicLinhasArquivo[chaveLinha] = linhadados.Value + "," + valores[1].Replace("\"", "");

                            }
                            else
                            {
                                dicLinhasArquivo[0] = dicLinhasArquivo[0] + @",French";
                            }
                            count++;
                        }
                        file.Close();
                        #endregion

                        #region Adiciona o ingles onde não teve tradução
                        dicTemp = new Dictionary<int, string>();

                        foreach (var linhaDados in dicLinhasArquivo)
                        {
                            int key = linhaDados.Key;
                            string linhaCorreta = linhaDados.Value;
                            if (key > 0)
                            {
                                string[] colunas = linhaDados.Value.Split(',');
                                if (colunas.Length < 4)
                                {
                                    linhaCorreta = linhaDados.Value + "," + colunas[1];
                                }
                            }
                            dicTemp[key] = linhaCorreta;
                        }

                        dicLinhasArquivo = dicTemp;
                        #endregion

                        #region Backup do localization original

                        string arquivoBackup = pastaEmpyrion + @"\" + arquivo_csv_destino+ ".BAK";
                        string arquivoDestino = pastaEmpyrion + @"\" + arquivo_csv_destino;

                        Console.WriteLine(" Realizando backup do arquivo original ");

                        if (File.Exists(arquivoBackup)){
                            File.Delete(arquivoBackup + ".BAK");
                        }

                        File.Copy(arquivoDestino, arquivoBackup, true);

                        #endregion
                        
                        using (StreamWriter csvNovo = new StreamWriter(arquivoDestino))
                            foreach (var entry in dicLinhasArquivo)
                                csvNovo.WriteLine("{0}", entry.Value);
                        
                        Console.WriteLine("Merge finished");
                    }else {
                    Console.WriteLine("Arquivo não encontrado "+arquivo_fr);
                    }

                } else {
                    Console.WriteLine("Arquivo não encontrado "+arquivo_ptbr);
                }

            }
            else
            {
                Console.WriteLine("Pasta de destino "+pastaEmpyrion+" não existe.");
            }
            
            Console.WriteLine("#################################");

        }
    }
}
