using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Helper
{
    class RC4
    {
        private static string _key;
        private static byte[] _datas;
        static Encoding unicode = Encoding.Unicode;
        public RC4(string key, byte[] datas)
        {
            _key = key;
            _datas = datas;
        }

        public byte[] Encrypt()
        {
            return EncryptOutput(unicode.GetBytes(_key), _datas).ToArray();
        }

        public byte[] Decrypt(string encryptedFilePath)
        {
            return EncryptOutput(unicode.GetBytes(_key), System.IO.File.ReadAllBytes(encryptedFilePath)).ToArray();
        }

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
              .Select(i => (byte)i)
              .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                Swap(s, i, j);
            }

            return s;
        }

        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);

            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                Swap(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }

        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }
    }
}
