using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace KMPS.Hubspot.Integration
{
	public class ExtractCSV
	{
		public static void WriteCSV<T>(IEnumerable<T> items, string path, KMPSLogger kmpsLogger, ILog logger)
		{

			Type itemType = typeof(T);
			var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			try
			{
				using (var writer = new StreamWriter(path))
				{
					// Tab delimited  
					writer.WriteLine(string.Join("	", props.Select(p => p.Name)));

					foreach (var item in items)
					{
						writer.WriteLine(string.Join("	", props.Select(p => p.GetValue(item, null))));
					}
				}
				logger.Info("File output completed ");
				kmpsLogger.MainLogWrite("File output Completed");
			}
			catch (Exception ex)
			{
				logger.Error("There is error in file " + ex.Message);
				kmpsLogger.LogMainExeception(ex, "WriteCSV");

			}
		
		}
	}
}