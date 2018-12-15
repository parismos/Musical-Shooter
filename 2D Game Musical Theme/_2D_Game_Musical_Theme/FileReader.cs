using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace _2D_Game_Musical_Theme
{
    public class FileReader
    {
        private Stream mFileStream = null;

        public FileReader(Stream stream)
        {
            mFileStream = stream;
        }

        public List<string> ReadLinesFromTextFile()
        {
            //Reading one string at a time
            string line = "";

            // Initialise a list to contain the results
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(mFileStream))
                {
                    //Read until end of file and store each line in the list created
                    while ((line = reader.ReadLine()) != null)
                    {
                       lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: File could not be read!");
                Console.WriteLine("Exception Message: " + e.Message);
            }

            return lines;
        }
    }
}
