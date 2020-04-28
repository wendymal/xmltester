using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Timers;

namespace xmlconsoleappp
{
    class Program
    {
        static void Main(string[] args)
        {
            //** Before running, add your local machine directories here **//

            // add any path on your local machine to add file, file does NOT need to exist
            string filepath = @"C:\Users\wendym\Downloads\xml-tester.csv";

            // location of any xml file you want to parse
            string xmlFilePath = @"C:\Users\wendym\Downloads\SampleTranscript\44168.8.xml";



            XElement xmlfile = XElement.Load(xmlFilePath);

            string intCode = Path.GetFileName(xmlFilePath);

            int index = intCode.IndexOf(".");
            if (index > 0)
            {
                intCode = intCode.Substring(0, index);
            }
                
            // parsing xml and adding words to dictionary

            Dictionary<string, WordInfoContainer> dict = new Dictionary<string, WordInfoContainer>(StringComparer.InvariantCultureIgnoreCase);

            foreach (XText text in (IEnumerable)XDocument.Parse(xmlfile.ToString()).XPathEvaluate("//*/text()"))
            {
                if (dict.ContainsKey(text.Value))
                {
                    dict[text.Value].Count++;
                    dict[text.Value].IntCode = intCode;
                }
                else
                {
                    dict.Add(text.Value, new WordInfoContainer { Count = 1, IntCode = intCode });
                }
            }

            // creating csv file -- not the best method, might use library instead
            using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
            FileMode.Create, FileAccess.Write)))
            {
                foreach (var item in dict)
                {
                    // word
                    string line1 = item.Key;
                    // count of words
                    int line2 = item.Value.Count;
                    // int code
                    string line3 = item.Value.IntCode;

                    string csvRow = string.Format("{0},{1},{2}", line1, line2, line3);
                    writer.WriteLine(csvRow);
                }
            }
        }

        public class WordInfoContainer
        {
            public int Count { get; set; }
            public string IntCode { get; set; }
        }

    }
}
