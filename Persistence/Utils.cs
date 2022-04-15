﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal class Utils
    {
        public static string FindFile(string path, string pattern)
        {
            DirectoryInfo dirInfo = new(path);
            var dirs = dirInfo.GetDirectories().OrderBy(x => x.Name);

            foreach (var dir in dirs)
            {
                string[] files = Directory.GetFiles(dir.FullName, pattern,
                    new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        RecurseSubdirectories = true
                    });

                if (files.Length != 0)
                {
                    return $"{dir.FullName}\\{pattern}";
                }
            }

            return String.Empty;
        }
    }
}
