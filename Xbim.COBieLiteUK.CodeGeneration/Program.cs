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
            if (!File.Exists(@"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.xsd"))
                Console.WriteLine(@"COBieLiteUK schema not found.");
            
            var generatorParams = new GeneratorParams
            {
                InputFilePath = @"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.xsd",
                CollectionObjectType = CollectionType.List,
                Language = GenerationLanguage.CSharp,
                NameSpace = "Xbim.COBieLiteUK2",
                OutputFilePath = @"cobieliteuk.designer.cs",
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
                GenerateShouldSerializeProperty = false, 
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

            Console.WriteLine(@"Generated code has been saved into the file {0}.", result.Entity);

            Console.WriteLine();
            Console.WriteLine(@"Finished");
            Console.WriteLine();
        }
    }
}
