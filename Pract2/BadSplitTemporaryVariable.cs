namespace BadSTV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input;
            bool isRunning = true;

            while (isRunning)
            {
                input = Console.ReadLine();

                switch (input)
                {
                    case "fcreate":
                        var file = new File();

                        input = Console.ReadLine(); //getting file name
                        //check if name is available logic
                        file.FileName = input;

                        input = Console.ReadLine(); //getting file size
                        int fileSize;
                        if (int.TryParse(input, out fileSize))
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
