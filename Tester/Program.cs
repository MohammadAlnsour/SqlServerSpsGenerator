using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.MsSqlServer;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var gen = new XmlTemplateReader();
            Console.Write(gen.ReadInsertStoredProcedure("Cources", "WindowsLearner"));
            Console.Write(gen.ReadUpdateStoredProcedure("Cources", "WindowsLearner"));
            Console.Write(gen.ReadSelectStoredProcedure("Cources", "WindowsLearner"));
            Console.Write(gen.ReadSelectAllStoredProcedure("Cources", "WindowsLearner"));
            Console.Write(gen.ReadDeleteStoredProcedure("Cources", "WindowsLearner"));
            Console.ReadLine();
        }
    }
}
