using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Helldiver
{
    public class HarmonyLoad
    {
        public static Assembly Load0Harmony()
        {
            Assembly exAssembly = Assembly.GetExecutingAssembly();
            String currentNamespace = typeof(HarmonyLoad).Namespace;
            using (Stream stream = exAssembly.GetManifestResourceStream($"{currentNamespace}.0Harmony.dll"))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream?.CopyTo(memoryStream);
                Assembly assembly = Assembly.Load(memoryStream.ToArray());
                return assembly;
            }

        }
    }
}
