using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;
using System.IO;

namespace BordspelApp_DAL
{
    public static class FileOperations
    {
        public static void FoutLoggen(Exception fout)
        {
            using (StreamWriter writer = new StreamWriter("foutenbestand.txt", true))
            {

                writer.WriteLine(DateTime.Now.ToString("HH:mm:ss tt"));
                writer.WriteLine(fout.GetType().Name);
                writer.WriteLine(fout.Message);
                writer.WriteLine(fout.StackTrace);
                writer.WriteLine(new String('-', 80));
                writer.WriteLine();
            }
        }

    }
}
