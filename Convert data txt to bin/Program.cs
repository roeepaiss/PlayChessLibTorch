using System.Diagnostics;


namespace Convert_data_txt_to_bin
{
    internal class Program
    {
        private const string ROOT_DIR = "D:/Projects/Chess.com Project/data";
        private const string INPUT_DIR = "txt";
        private const string OUTPUT_DIR = "bin";
        private const string MARLINFLOW_EXE = "marlinflow-utils.exe";
        private const string COMMAND = "txt-to-data";

        static void Main(string[] args)
        {
            if (!Directory.Exists(ROOT_DIR))
                throw new Exception("root dir not exist");

            string marlinflow_utils = Path.Join(ROOT_DIR, MARLINFLOW_EXE);
            string inputPath = Path.Join(ROOT_DIR, INPUT_DIR);
            string outputPath = Path.Join(ROOT_DIR, OUTPUT_DIR);
            
            IEnumerable<string> inputFiles = Directory.EnumerateFiles(inputPath, "*.txt");
            Directory.CreateDirectory(outputPath);
            foreach (string inputFile in inputFiles)
            {
                Console.WriteLine($"Processing file: {inputFile}");
                string outputFile = Path.Join(outputPath, Path.ChangeExtension(Path.GetFileName(inputFile), ".bin"));
                using (Process marlinflow = new Process())
                {
                    marlinflow.StartInfo.FileName = marlinflow_utils;
                    marlinflow.StartInfo.Arguments = $"{COMMAND} \"{inputFile}\" --output=\"{outputFile}\"";
                    Console.WriteLine(marlinflow.StartInfo.FileName + " " + marlinflow.StartInfo.Arguments);
                    marlinflow.ErrorDataReceived += Marlinflow_DataReceived;
                    marlinflow.OutputDataReceived += Marlinflow_DataReceived;
                    marlinflow.Start();
                    marlinflow.WaitForExit();
                }

                Console.WriteLine($"Generated file: {outputFile}");
            }
        }

        private static void Marlinflow_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}