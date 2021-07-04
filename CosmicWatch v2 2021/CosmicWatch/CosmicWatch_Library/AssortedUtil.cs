using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CosmicWatch_Library
{
    public class AssortedUtil
    {
        public static string GetEmbeddedText(Type ClassType, String resourceName)
        {
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(ClassType).Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            String text = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
}
