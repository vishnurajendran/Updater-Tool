using System.Buffers.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UpdatorTool.Scripts.Runtime
{
    public static class Encoder
    {
        public static byte[] Encode<T>(T data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, data);
            return ms.ToArray();
        }
        
        public static T Decode<T>(byte[] bArr)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bArr, 0, bArr.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T) binForm.Deserialize(memStream);

            return obj;
        }
    }
}