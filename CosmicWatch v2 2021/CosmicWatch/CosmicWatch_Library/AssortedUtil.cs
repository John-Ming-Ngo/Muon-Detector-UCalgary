using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace CosmicWatch_Library
{
    public class AssortedUtil
    {
        public static string GetEmbeddedText(Type ClassType, String resourceName)
        {
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(ClassType).Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            String text = String.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                text = String.Empty;
            }
            return text;
        }
        public static ImageSource GetEmbeddedImage(Type ClassType, String resourceName)
        {
            return ImageSource.FromResource(resourceName, ClassType.GetTypeInfo().Assembly);
        }
    }
}
