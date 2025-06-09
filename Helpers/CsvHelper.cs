namespace Naitv1.Helpers
{
	using System.Text;
	using System.IO;
	using System.Linq;

	public class CsvHelper
	{
		public static byte[] ConvertirAFormatoCSV<T>(IEnumerable<T> datos)
		{
			var props = typeof(T).GetProperties();
			var csv = new StringBuilder();

			// Encabezados
			csv.AppendLine(string.Join(";", props.Select(p => p.Name)));

			// Filas de datos
			foreach (var item in datos)
			{
				var valores = props.Select(p =>
					"\"" + (p.GetValue(item)?.ToString()?.Replace("\"", "\"\"") ?? "") + "\"");
				csv.AppendLine(string.Join(";", valores));
			}

			// ✅ Usar UTF8 con BOM
			var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
			return encoding.GetBytes(csv.ToString());
		}
	}

}
