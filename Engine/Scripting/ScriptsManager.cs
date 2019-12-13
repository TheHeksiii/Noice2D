using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Microsoft.Xna.Framework;
using Scripts;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Scripts
{
      public class Script { }
}
namespace Engine
{
      public static class ScriptsManager
      {
            public static Assembly ScriptsAssembly;
            static ScriptsManager()
            {
                  //CompileScriptsAssembly();
            }
            public static void CompileScriptsAssembly()
            {
                  if (Directory.Exists("./Scripts/") == false)
                  {
                        Directory.CreateDirectory("./Scripts/");
                  }
                  var files = Directory.GetFiles("./Scripts/");

                  Environment.SetEnvironmentVariable("ROSLYN_COMPILER_LOCATION", "./roslyn", EnvironmentVariableTarget.Process);
                  CSharpCodeProvider provider = new CSharpCodeProvider();
                  Environment.SetEnvironmentVariable("ROSLYN_COMPILER_LOCATION", null, EnvironmentVariableTarget.Process);

                  //CodeDomProvider provider = new CSharpCodeProvider();
                  CompilerParameters parameters = new CompilerParameters(null, "Scripts.dll");
                  parameters.ReferencedAssemblies.Add("Engine.dll");
                  // parameters.ReferencedAssemblies.Add("Assemblies/Microsoft.Xna.Framework.dll");
                  parameters.ReferencedAssemblies.Add("MonoGame.Extended.dll");
                  parameters.ReferencedAssemblies.Add("MonoGame.Framework.dll");
                  parameters.ReferencedAssemblies.Add("System.Drawing.dll");
                  parameters.ReferencedAssemblies.Add("System.dll");
                  parameters.ReferencedAssemblies.Add("System.Xml.dll");

                  parameters.GenerateInMemory = true;
                  parameters.GenerateExecutable = false;

                  //System.ComponentModel.TypeDescriptor.AddAttributes
                  /*var components = typeof(Component)
                          .Assembly.GetTypes()
                          .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract).ToList();
                  components.AddRange(ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component))));
                  List<PropertyInfo> properties = new List<PropertyInfo>();
                  for (int i = 0; i < components.Count; i++)
                  {
                      properties.AddRange(components[i].GetProperties().Where(p => (p.GetCustomAttribute<ShowInEditor>() != null)).ToArray());
                  }
                  for(int i = 0; i < properties.Count; i++)
                  {
                      if (properties[i].PropertyType == typeof(bool))
                      {
                          properties[i].att
                      }
                  }*/
                  System.ComponentModel.TypeDescriptor.AddAttributes(typeof(UInt16), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.BoolEditor), typeof(System.Drawing.Design.UITypeEditor)) });
                  //System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Microsoft.Xna.Framework.Vector2), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.BoolEditor), typeof(System.Drawing.Design.UITypeEditor)) });
                  System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Action), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.MethodEditor), typeof(System.Drawing.Design.UITypeEditor)) });
                  System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Color), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.ColorPickerEditor), typeof(System.Drawing.Design.UITypeEditor)) });




                  CompilerResults results = provider.CompileAssemblyFromFile(parameters, files);

                  if (results.Errors.HasErrors)
                  {
                        StringBuilder sb = new StringBuilder();

                        foreach (CompilerError error in results.Errors)
                        {
                              sb.AppendLine(String.Format("Line [{0}] in {1}: \n Error ({2}): {3}", error.Line, error.FileName, error.ErrorNumber, error.ErrorText));

                        }
                        //EditorConsole.Log(sb.ToString());
                  }

                  Assembly assembly = results.CompiledAssembly;

                  ScriptsAssembly = assembly;
            }
            public static Type CreateScript(string className)
            {
                  Type myType = CompileResultType(className);
                  object myObject = Activator.CreateInstance(myType);

                  Directory.CreateDirectory("Scripts");
                  File.Create("./Scripts/" + myType + ".cs").Close();

                  var engineNamespace = new CodeNamespace("Engine");
                  var engineScriptsNamespace = new CodeNamespace("Scripts");
                  var targetClass = new CodeTypeDeclaration(className)
                  {
                        IsClass = true,
                        TypeAttributes = TypeAttributes.Public
                  };
                  targetClass.BaseTypes.Add(new CodeTypeReference((typeof(Component))));
                  var codeCompileUnit = new CodeCompileUnit();
                  var globalNamespace = new CodeNamespace();

                  globalNamespace.Imports.Add(new CodeNamespaceImport("Engine"));
                  globalNamespace.Imports.Add(new CodeNamespaceImport("Scripts"));
                  globalNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.Xna.Framework"));
                  globalNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.Xna.Framework.Input"));

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
                        pureCode = pureCode.Replace("{\n", "{");
                        pureCode = pureCode.Replace("\n\n", "\n");
                        pureCode = pureCode.Replace("Engine.Component", "Component");
                  }
                  File.Create("./Scripts/" + myType + ".cs").Close();

                  using (var sourceWriter = new StreamWriter(outputPath))
                  {
                        sourceWriter.Write(pureCode);

                  }
                  var p = new Microsoft.Build.Evaluation.Project(@"./Project.csproj", null, null, new Microsoft.Build.Evaluation.ProjectCollection());
                  p.AddItem("Compile", outputPath);
                  p.Save();

                  CompileScriptsAssembly();

                  return myType;
            }

            private static Type CompileResultType(string className)
            {
                  TypeBuilder tb = GetTypeBuilder(className);
                  ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);


                  Type objectType = tb.CreateType();
                  return objectType;
            }

            private static TypeBuilder GetTypeBuilder(string className)
            {
                  var typeSignature = className;
                  var an = new AssemblyName(typeSignature);
                  AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndCollect);
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