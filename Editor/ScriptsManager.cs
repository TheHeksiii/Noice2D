using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

namespace Engine.Scripts
{
    public class Script { }
}
namespace Engine.Editor
{
    public static class ScriptsManager
    {
        public static Assembly ScriptsAssembly;
        public static void LoadAllScripts()
        {
            var files = Directory.GetFiles("./Scripts/");
            /*
            for (int i = 0; i < files.Length; i++)
            {
                code += File.ReadAllText(files[i]);
                units[i] = new CodeCompileUnit() { };

            }*/
            Environment.SetEnvironmentVariable("ROSLYN_COMPILER_LOCATION", "./roslyn", EnvironmentVariableTarget.Process);
            //Create compiler object
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //Clean up
            Environment.SetEnvironmentVariable("ROSLYN_COMPILER_LOCATION", null, EnvironmentVariableTarget.Process);

            //CodeDomProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters(null, "Engine.Scripts.dll");
            parameters.ReferencedAssemblies.Add("Engine.dll");

            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = false;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;

            CompilerResults results = provider.CompileAssemblyFromFile(parameters, files);
            var x = results.PathToAssembly;

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }

            Assembly assembly = results.CompiledAssembly;
            Type[] types = assembly.GetExportedTypes();
            ScriptsAssembly = assembly;

        }
        public static void CreateScript(string className)
        {
            Type myType = CompileResultType(className);
            object myObject = Activator.CreateInstance(myType);

            Directory.CreateDirectory("Scripts");
            File.Create("./Scripts/" + myType + ".cs").Close();

            var engineNamespace = new CodeNamespace("Engine");
            var engineScriptsNamespace = new CodeNamespace("Engine.Scripts");
            var targetClass = new CodeTypeDeclaration(className)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public
            };
            targetClass.BaseTypes.Add(new CodeTypeReference((typeof(Component))));
            var codeCompileUnit = new CodeCompileUnit();
            var globalNamespace = new CodeNamespace();
            globalNamespace.Imports.Add(new CodeNamespaceImport(typeof(GameSceneView).Namespace));
            globalNamespace.Imports.Add(new CodeNamespaceImport(typeof(Engine.Scripts.Script).Namespace));

            engineScriptsNamespace.Types.Add(targetClass);
            codeCompileUnit.Namespaces.Add(globalNamespace);
            codeCompileUnit.Namespaces.Add(engineScriptsNamespace);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions { BracingStyle = "C", BlankLinesBetweenMembers = false, IndentString = "   " };
            var outputPath = "./Scripts/" + myType + ".cs";

            string pureCode;
            using (var sourceWriter = new StreamWriter(outputPath))
            {
                provider.GenerateCodeFromCompileUnit(
                            codeCompileUnit, sourceWriter, options);
            }
            using (var streamReader = new StreamReader(outputPath))
            {

                string text = streamReader.ReadToEnd();
                var sign = "//------------------------------------------------------------------------------";
                pureCode = text.Substring(text.LastIndexOf(sign) + sign.Length + 4);
                //pureCode=pureCode.Replace("\r", "");
                pureCode = pureCode.Replace("{\n", "{");
                pureCode = pureCode.Replace("\n\n", "\n");
                pureCode = pureCode.Replace("Engine.Component", "Component");
            }
            File.Create("./Scripts/" + myType + ".cs").Close();

            using (var sourceWriter = new StreamWriter(outputPath))
            {
                sourceWriter.Write(pureCode);

            }
            LoadAllScripts();
        }

        private static Type CompileResultType(string className)
        {
            TypeBuilder tb = GetTypeBuilder(className);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            /*foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType);*/

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string className)
        {
            var typeSignature = className;
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(className);
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            tb.SetParent(typeof(Component));
            return tb;
        }
    }
}