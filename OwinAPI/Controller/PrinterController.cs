using OwinAPI.Database;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace OwinAPI.Controller
{
    public class PrinterController : ApiController
    {
        private Font PrintFont = new Font("Arial", 10);
        private StreamReader streamToPrint;

        public List<string> Get()
        {
            var printers =  PrinterSettings.InstalledPrinters;

            string[] printerNames = new string[printers.Count];
            printers.CopyTo(printerNames, 0);

            return printerNames.ToList();
        }

        public void Post()
        {
            SQLiteConnection connection = DbFactory.GetConnection();
        }

        public void Put(string printerName, string text)
        {
            try
            {
                streamToPrint = new StreamReader(GenerateStreamFromString(text));

                try
                {
                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrintPage += new PrintPageEventHandler(HandlePrintPage);
                    printDocument.PrinterSettings.PrinterName = printerName;
                    printDocument.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void HandlePrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height / PrintFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * PrintFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, PrintFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
            {
                ev.HasMorePages = true;
            }
            else
            {
                ev.HasMorePages = false;
            }
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
