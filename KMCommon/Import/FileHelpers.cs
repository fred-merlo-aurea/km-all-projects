using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace KM.Common.Import
{
    public static class FileHelpers
    {
        public static FileConfiguration EnsureFileConfiguration(FileConfiguration fileConfig)
        {
            if (fileConfig == null)
            {
                fileConfig = new FileConfiguration();
                fileConfig.IsQuoteEncapsulated = false;
                fileConfig.FileColumnDelimiter = "comma";
            }

            return fileConfig;
        }

        public static TextFieldParser CreateTextFieldParser(
            FileInfo file,
            FileConfiguration fileConfig)
        {
            var result = CreateTextFieldParser(file, fileConfig, Encoding.UTF8);
            return result;
        }

        public static TextFieldParser CreateTextFieldParser(
            FileInfo file,
            FileConfiguration fileConfig,
            Encoding defaultEncoding)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (fileConfig == null)
            {
                throw new ArgumentNullException(nameof(fileConfig));
            }

            var textFieldParser = new TextFieldParser(file.FullName, defaultEncoding);

            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.TrimWhiteSpace = true;
            textFieldParser.HasFieldsEnclosedInQuotes = fileConfig.IsQuoteEncapsulated;
            var delimiter = Enums.GetDelimiterSymbol(fileConfig.FileColumnDelimiter.ToLower());
            if (delimiter.HasValue)
            {
                textFieldParser.SetDelimiters(delimiter.Value.ToString());
            }

            return textFieldParser;
        }
    }
}
