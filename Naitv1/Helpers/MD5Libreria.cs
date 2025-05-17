using System;
using System.Security.Cryptography;
using System.Text;

namespace Naitv1.Helpers
{
    public class MD5Libreria
    {
        public static string Encriptar(string input)
        {
            // Crear una instancia de MD5
            using (MD5 md5 = MD5.Create())
            {
                // Convertir la cadena de entrada en un arreglo de bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                // Calcular el hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convertir el arreglo de bytes a una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2")); // "X2" para formato hexadecimal en mayúsculas
                }
                return sb.ToString();
            }
        }
    }
}
