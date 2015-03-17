using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xsd2Code.Library.Helpers;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Encapsulation of all generation process.
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Changed signature of the GeneratorFacade class constructor
    ///     Updated result types to <see cref="Result"/>
    /// 
    /// </remarks>
    public class GeneratorFacade
    {
        /// <summary>
        /// Instance of CodeDom provider
        /// </summary>
        private CodeDomProvider providerField;

        public GeneratorFacade(GeneratorParams generatorParams)
        {
            var provider = CodeDomProviderFactory.GetProvider(generatorParams.Language);
            this.Init(provider, generatorParams);
        }

        /// <summary>
        /// Generator facade class constructor
        /// </summary>
        /// <param name="provider">Code DOM provider</param>
        /// <param name="generatorParams">Generator parameters</param>
        public GeneratorFacade(CodeDomProvider provider, GeneratorParams generatorParams)
        {
            this.Init(provider, generatorParams);
        }

        /// <summary>
        /// Generator parameters
        /// </summary>
        public GeneratorParams GeneratorParams
        {
            get { return GeneratorContext.GeneratorParams; }
        }

        /// <summary>
        /// Initialize generator
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="generatorParams"></param>
        public void Init(CodeDomProvider provider, GeneratorParams generatorParams)
        {
            this.providerField = provider;
            GeneratorContext.GeneratorParams = generatorParams.Clone();

            if (string.IsNullOrEmpty(GeneratorContext.GeneratorParams.OutputFilePath))
            {
                string outputFilePath = Utility.GetOutputFilePath(generatorParams.InputFilePath, provider);
                GeneratorContext.GeneratorParams.OutputFilePath = outputFilePath;
            }
        }

        #region Methods

        /// <summary>
        /// Generates the specified buffer size.
        /// </summary>
        /// <returns>return generated code into byte array</returns>
        public Result<byte[]> GenerateBytes()
        {
            string outputFilePath = Path.GetTempFileName();
            var processResult = this.Process(outputFilePath);

            if (processResult.Success)
            {
                byte[] result = FileToByte(outputFilePath);
                try
                {
                    File.Delete(outputFilePath);
                }
                catch (Exception ex)
                {
                    processResult.Messages.Add(MessageType.Error, ex.Message);
                }

                return new Result<byte[]>(result, true, processResult.Messages);
            }

            return new Result<byte[]>(null, false, processResult.Messages);
        }

        /// <summary>
        /// Processes the code generation.
        /// </summary>
        /// <returns>true if sucess or false.</returns>
        public Result<string> Generate(GeneratorParams generatorParams)
        {
            GeneratorContext.GeneratorParams = generatorParams;
            var outputFileName = GeneratorContext.GeneratorParams.OutputFilePath;
            var processResult = this.Process(outputFileName);
            return new Result<string>(outputFileName, processResult.Success, processResult.Messages);
        }

        /// <summary>
        /// Processes the code generation.
        /// </summary>
        /// <returns>true if sucess or false.</returns>
        public Result<string> Generate()
        {
            var outputFileName = GeneratorContext.GeneratorParams.OutputFilePath;
            var processResult = this.Process(outputFileName);
            return new Result<string>(outputFileName, processResult.Success, processResult.Messages);
        }

        /// <summary>
        /// Convert file into byte[].
        /// </summary>
        /// <param name="path">The full file path to convert info byte[].</param>
        /// <returns>return file content info  byte[].</returns>
        private static byte[] FileToByte(string path)
        {
            var infoFile = new FileInfo(path);
            using (var fileSteram = infoFile.OpenRead())
            {
                var arrayOfByte = new byte[fileSteram.Length];

                fileSteram.Read(arrayOfByte, 0, (int)fileSteram.Length);
                fileSteram.Close();
                return arrayOfByte;
            }
        }

        /// <summary>
        /// Processes the specified file name.
        /// </summary>
        /// <param name="outputFilePath">The output file path.</param>
        /// <returns>true if sucess else false</returns>
        private Result Process(string outputFilePath)
        {
            #region Change CurrentDir for include schema resolution.

            string savedCurrentDir = Directory.GetCurrentDirectory();
            var inputFile = new FileInfo(GeneratorContext.GeneratorParams.InputFilePath);

            if (!inputFile.Exists)
            {
                var errorMessage = string.Format("XSD File not found at location {0}\n", GeneratorContext.GeneratorParams.InputFilePath);
                errorMessage += "Exception :\n";
                errorMessage += string.Format("Input file {0} not exist", GeneratorContext.GeneratorParams.InputFilePath);
                return new Result(false, MessageType.Error, errorMessage);
            }

            if (inputFile.Directory != null)
            {
                Directory.SetCurrentDirectory(inputFile.Directory.FullName);
                //change input file path accordinghly
                GeneratorContext.GeneratorParams.InputFilePath = inputFile.Name;
            }

            #endregion

            try
            {
                try
                {
                    var result = Generator.Process(GeneratorContext.GeneratorParams);
                    if (!result.Success) return result;

                    var ns = result.Entity;
                    using (var outputStream = new StreamWriter(outputFilePath + ".tmp", false))
                        this.providerField.GenerateCodeFromNamespace(ns, outputStream, new CodeGeneratorOptions());
                }
                catch (Exception e)
                {
                    var errorMessage = "Failed to generate code\n";
                    errorMessage += "Exception :\n";
                    errorMessage += e.Message;

                    Debug.WriteLine(string.Empty);
                    Debug.WriteLine("XSD2Code - ----------------------------------------------------------");
                    Debug.WriteLine("XSD2Code - " + e.Message);
                    Debug.WriteLine("XSD2Code - ----------------------------------------------------------");
                    Debug.WriteLine(string.Empty);
                    return new Result(false, MessageType.Error, errorMessage);
                }

                var outputFile = new FileInfo(outputFilePath);
                if (outputFile.Exists)
                {
                    if ((outputFile.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        var errorMessage = "Failed to generate code\n";
                        errorMessage += outputFilePath + " is write protect";
                        return new Result(false, MessageType.Error, errorMessage);
                    }
                }

                #region Insert tag for future generation

                using (var outputStream = new StreamWriter(outputFilePath, false))
                {


                    string commentStr = GeneratorContext.GeneratorParams.Language == GenerationLanguage.CSharp
                                            ? "// "
                                            : "'' ";

                    Assembly currentAssembly = Assembly.GetExecutingAssembly();
                    AssemblyName currentAssemblyName = currentAssembly.GetName();

                    outputStream.WriteLine(
                        "{0}------------------------------------------------------------------------------",
                            commentStr);
                    outputStream.WriteLine(string.Format("{0} <auto-generated>", commentStr));

                    outputStream.WriteLine(string.Format("{0}   Generated by Xsd2Code. Version {1} Microsoft Reciprocal License (Ms-RL) ", commentStr,
                                                  currentAssemblyName.Version));

                    string optionsLine = string.Format("{0}   {1}", commentStr,
                                                       GeneratorContext.GeneratorParams.ToXmlTag());
                    outputStream.WriteLine(optionsLine);

                    outputStream.WriteLine("{0} </auto-generated>", commentStr);

                    outputStream.WriteLine(
                            "{0}------------------------------------------------------------------------------",
                            commentStr);


                #endregion

                    using (TextReader streamReader = new StreamReader(outputFilePath + ".tmp"))
                    {
                        string line;

                        //DCM TODO Will refactor this to Not perform this last loop after verification that it works.
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            outputStream.WriteLine(line);
                        }
                    }
                    outputStream.Close();
                }
                try
                {
                    var tempFile = new FileInfo(outputFilePath + ".tmp");
                    tempFile.Delete();
                }
                catch (Exception ex)
                {
                    return new Result(false, MessageType.Error, ex.Message);
                }
            }
            finally
            {
                Directory.SetCurrentDirectory(savedCurrentDir);
            }

            return new Result(true);
        }

        #endregion
    }
}