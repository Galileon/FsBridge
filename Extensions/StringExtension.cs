using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FsConnect
{
    public static class StringExtension
    {
     
        public static string FirstLine(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            var newLinePos = str.IndexOf(Environment.NewLine, StringComparison.CurrentCulture);
            return newLinePos > 0 ? str.Substring(0, newLinePos) : str;
        }

        /// <summary>
        /// Take a string value from comma separater freeswitch parameters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetFreeswitchCommandParamsValue(this string input, string paramName)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var indexOfParameter = input.IndexOf(paramName);
            if (indexOfParameter < 0) return string.Empty;
            var indexOfComma = input.IndexOf(',', indexOfParameter + paramName.Length);
            if (indexOfParameter < 0) return string.Empty;

            var ret = input.Substring(indexOfParameter + paramName.Length + 1, indexOfComma - indexOfParameter - 1 - paramName.Length);
            return ret;
        }


        public static string RelativePath(this string absPath, string relTo)
        {
            string[] absDirs = absPath.Split('\\');
            string[] relDirs = relTo.Split('\\');
            // Get the shortest of the two paths 
            int len = absDirs.Length < relDirs.Length ? absDirs.Length : relDirs.Length;
            // Use to determine where in the loop we exited 
            int lastCommonRoot = -1; int index;
            // Find common root 
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index])
                    lastCommonRoot = index;
                else break;
            }
            // If we didn't find a common prefix then throw 
            if (lastCommonRoot == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }
            // Build up the relative path 
            StringBuilder relativePath = new StringBuilder();
            // Add on the .. 
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0) relativePath.Append("..\\");
            }
            // Add on the folders 
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index] + "\\");
            }
            relativePath.Append(relDirs[relDirs.Length - 1]);
            return relativePath.ToString();
        }

    }


}
