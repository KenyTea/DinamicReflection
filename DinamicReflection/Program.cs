using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DinamicReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            Assembly assemblay = Assembly.LoadFile(@"\\dc\Студенты\ПКО\SEB-171.2\C#\Exception\GeneratorName.dll");
            Type[] types = assemblay.GetTypes();

            foreach (Type item in types)
            {
                Console.WriteLine("-> {0} ({1})", item.Name, item.IsClass);
                foreach (MethodInfo method in item.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    Console.WriteLine("  -> {0} - {1}", method.Name, method.ReturnType);

                    foreach (ParameterInfo param in method.GetParameters())
                    {
                        Console.WriteLine("    -> {0} ({1})", param.Name, param.ParameterType.BaseType);
                    }
                }
            }
            #endregion

            Type tGenerator = types.FirstOrDefault(f => f.IsClass && f.Name == "Generator");

            Object metGenerator = Activator.CreateInstance(tGenerator);
            // метод Generator
            MethodInfo GenerateDafault = metGenerator.GetType().GetMethod("GenerateDefault");
            // парамктры метода
            ParameterInfo piGender = GenerateDafault.GetParameters()[0];

            object gender = null;
            if (piGender.ParameterType.BaseType == typeof(Enum))
            {
                gender = Enum.ToObject(piGender.ParameterType, 0);
                // так, как енум, то есть список енум
                FieldInfo[] fiGender = piGender.ParameterType.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var item in fiGender)
                {
                    Console.WriteLine(item.Name);
                }
            }
            object[] para = new object[] { gender };
            var result = GenerateDafault.Invoke(metGenerator, para);
            Console.WriteLine("\n\n\n\n\n {0}", result);
        }
    }
}
