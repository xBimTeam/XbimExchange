using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2Code.Library;

namespace Xbim.COBieLiteUK.CodeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            const string outFile = @"cobieliteuk.designer.cs";
            const string inFile = @"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.xsd";
            if (!File.Exists(inFile))
                Console.WriteLine(@"COBieLiteUK schema not found.");
            
            var generatorParams = new GeneratorParams
            {
                InputFilePath = inFile,
                CollectionObjectType = CollectionType.List,
                Language = GenerationLanguage.CSharp,
                NameSpace = "Xbim.COBieLiteUK",
                OutputFilePath = outFile,
                TargetFramework = TargetFramework.CobieLiteUk,
                EnableInitializeFields = false,
                Serialization = new SerializeParams
                {
                    DefaultEncoder = DefaultEncoder.UTF8,
                    GenerateXmlAttributes = true
                } 
            };
            generatorParams.PropertyParams = new PropertyParams(generatorParams)
            {
                GenerateShouldSerializeProperty = true, 
                GeneratePropertyNameSpecified = PropertyNameSpecifiedType.None
            };

            // Create an instance of Generator
            var generator = new GeneratorFacade(generatorParams);

            // Generate code
            var result = generator.Generate();
            if (!result.Success)
            {
                // Display the error and wait for user confirmation
                Console.WriteLine();
                Console.WriteLine(result.Messages.ToString());
                Console.WriteLine();
                Console.WriteLine(@"Press ENTER to continue...");
                Console.ReadLine();

                return;
            }

            //do textual replacement
            var outFileFullPath = Path.Combine(Path.GetDirectoryName(inFile), outFile);
            var code = File.ReadAllText(outFileFullPath);
            code = code.Replace("System.", "global::System.");
            code = code.Replace("System;", "global::System;");
            File.WriteAllText(outFileFullPath, code);


            Console.WriteLine(@"Generated code has been saved to the file {0}.", result.Entity);

            Console.WriteLine();
            Console.WriteLine(@"Finished");
            Console.WriteLine();
        }
    }
}
