using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace quanlykhachsan.MaHoa
{
    internal class Mahoa
    {
        public static string ComputeSHA256Hash(string input)
        {
            // Chuyển đổi chuỗi đầu vào thành mảng byte
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Chuyển mảng byte thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static void Main()
        {
            // Ví dụ chuỗi cần mã hóa
            string input = "123";

            // Tính toán hash SHA-256 của chuỗi
            string hash = ComputeSHA256Hash(input);
            Console.WriteLine($"SHA-256 hash: {hash}");
        }
    }
}
