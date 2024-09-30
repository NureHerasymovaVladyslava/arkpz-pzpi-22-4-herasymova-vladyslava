namespace FixedSTV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                var command = Console.ReadLine();

                switch (command)
                {
                    case "fcreate":
                        var file = new File();

                        var fileName = Console.ReadLine(); //getting file name
                        //check if name is available logic
                        file.FileName = fileName;

                        var fileSizeStr = Console.ReadLine(); //getting file size
                        int fileSize;
                        if (int.TryParse(fileSizeStr, out fileSize))
                        {
                            //check if size is OK logic
                            file.FileSize = fileSize;
                        } 
                        else
                        {
                            //notify user logic
                        }
                        
                        //...
                        break;
                    //...
                }
            }
        }
    }

    internal class File
    {
        public string FileName { get; set; }
        public int FileSize { get; set; }
    }
}
