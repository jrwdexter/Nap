using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Nap.Tests.TestClasses;

namespace Nap.Tests.Formatters.Base
{
    public abstract class NapFormatterTestBase
    {
        protected static string GetFileContents(string fileName)
        {
            var assemblyPath = new FileInfo(Assembly.GetAssembly(typeof(TestClass)).Location).Directory?.FullName ?? string.Empty;
            var path = Path.Combine(assemblyPath, "Assets", fileName);
            using (var fs = File.OpenRead(path))
            using (var sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}