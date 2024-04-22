using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

static class Program { 
    static void Main(String[] args)
    {
        if (args.Length == 0) { 
            Console.WriteLine("No arguments provided");
        }
        string directoryPath = args[0];
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Directory does not exist");
            return;
        }

        SortedDictionary<string, long> elements = new SortedDictionary<string, long>(new CustomComparer());

        string[] files = Directory.GetFiles(directoryPath);
        string[] directories = Directory.GetDirectories(directoryPath);

        DateTime oldestFileDate = DateTime.Now;
        string oldestFileName = "";

        foreach (string file in files)
        {

            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.CreationTime < oldestFileDate)
            {
                oldestFileDate = fileInfo.CreationTime;
                oldestFileName = Path.GetFileName(file);
            }

            string attributes = GetDOSAttributes(fileInfo.Attributes);
            Console.WriteLine(fileInfo.Name + " " + fileInfo.Length + " bytes " + attributes);
            elements.Add(fileInfo.Name, fileInfo.Length);
        }
        foreach (string directory in directories)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (dirInfo.CreationTime < oldestFileDate)
            {
                oldestFileDate = dirInfo.CreationTime;
                oldestFileName = Path.GetFileName(directory);
            }

            String dirName = Path.GetFileName(directory);
            int numberOfStoredFiles = Directory.GetFiles(directory).Length + Directory.GetDirectories(directory).Length;
            string attributes = GetDOSAttributes(dirInfo.Attributes);
            Console.WriteLine(dirName + " (" + numberOfStoredFiles + ") " + attributes);
            elements.Add(dirInfo.Name, numberOfStoredFiles);

            recursiveDisplay(directory, new string[] { oldestFileName }, oldestFileDate, 1);
        }
        Console.WriteLine("\nOldest file/directory: " + oldestFileName + ": " + oldestFileDate.ToString() + "\n");

        SerializeAndDeserializeCollection(elements);
    }
    static void SerializeAndDeserializeCollection(SortedDictionary<string, long> elements)
    {
        List<KeyValuePair<string, long>> keyValuePairs = new List<KeyValuePair<string, long>>(elements);

        using (FileStream fs = new FileStream("collection.bin", FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, keyValuePairs);
        }

        using (FileStream fs = new FileStream("collection.bin", FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<KeyValuePair<string, long>> deserializedList = (List<KeyValuePair<string, long>>)formatter.Deserialize(fs);

            Console.WriteLine("\nDeserialized elements:");
            foreach (var kvp in deserializedList)
            {
                Console.WriteLine($"{kvp.Key} -> {kvp.Value}");
            }
        }
    }

    static void recursiveDisplay(string directoryPath, string[] oldestFileName, DateTime oldestFileDate, int recursionLvl)
    {
        string[] files = Directory.GetFiles(directoryPath);
        string[] directories = Directory.GetDirectories(directoryPath);

        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.CreationTime < oldestFileDate)
            {
                oldestFileDate = fileInfo.CreationTime;
                oldestFileName[0] = Path.GetFileName(file);
            }

            for (int i = 0; i < recursionLvl; i++)
            {
                Console.Write("\t");
            }

            String fileName = Path.GetFileName(file);
            long fileSize = fileInfo.Length;
            string attributes = GetDOSAttributes(fileInfo.Attributes);
            Console.WriteLine(fileName + " " + fileSize + " bytes " + attributes);
        }
        foreach (string directory in directories)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (dirInfo.CreationTime < oldestFileDate)
            {
                oldestFileDate = dirInfo.CreationTime;
                oldestFileName[0] = Path.GetFileName(directory);
            }

            for (int i = 0; i < recursionLvl; i++)
            {
                Console.Write("\t");
            }
            String dirName = Path.GetFileName(directory);
            int numberOfStoredFiles = Directory.GetFiles(directory).Length + Directory.GetDirectories(directory).Length;
            string attributes = GetDOSAttributes(dirInfo.Attributes);
            Console.WriteLine(dirName + " (" + numberOfStoredFiles + ") " + attributes);
            recursiveDisplay(directory, oldestFileName, oldestFileDate,++recursionLvl);
        }
    }
    static string GetDOSAttributes(FileAttributes attributes)
    {
        string result = "";

        if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            result += "r";
        else
            result += "-";

        if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
            result += "a";
        else
            result += "-";

        if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            result += "h";
        else
            result += "-";

        if ((attributes & FileAttributes.System) == FileAttributes.System)
            result += "s";
        else
            result += "-";

        return result;
    }
    class CustomComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int result = x.Length.CompareTo(y.Length);
            if (result == 0)
            {
                result = x.CompareTo(y);
            }
            return result;
        }
    }
}