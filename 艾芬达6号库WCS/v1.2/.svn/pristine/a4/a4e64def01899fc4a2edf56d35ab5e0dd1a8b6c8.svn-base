using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.App
{
    public static class MonitorHelper
    {
        public static String Dir
        {
            get
            {
                var path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                path = System.IO.Path.Combine(path, "stages");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        public static void Save(String name,String content)
        {
            var filePath = Path.Combine(Dir, name);
            System.IO.File.WriteAllText(filePath, content);
        }

        public static String Read(String name)
        {
            var filePath = Path.Combine(Dir, name);

            return System.IO.File.ReadAllText(filePath);
        }

        public static Dictionary<String, String> GetList()
        {
            var files = System.IO.Directory.GetFiles(Dir);
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in files)
            {
                var name = Path.GetFileName(item);
                result.Add(name, item);
            }

            return result;
        }
    }
}
