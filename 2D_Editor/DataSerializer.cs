using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2D_Editor
{
    public class DataSerializer
    {
        public DataSerializer()
        {

        }

        public void SerializeAsJson(object pData, string pFilePath)
        {
            JsonSerializer jSerializer = new JsonSerializer();
            if (File.Exists(pFilePath)) File.Delete(pFilePath);
            StreamWriter sw = new StreamWriter(pFilePath);
            JsonWriter jWriter = new JsonTextWriter(sw);
            jSerializer.Serialize(jWriter, pData);

            jWriter.Close();
            sw.Close();
        }

        public object DeserializerJson(Type pDataType, string pFilePath)
        {
            JObject obje = null;
            JsonSerializer jSerializer = new JsonSerializer();
            if (File.Exists(pFilePath))
            {
                StreamReader sr = new StreamReader(pFilePath);
                JsonReader jReader = new JsonTextReader(sr);
                obje = jSerializer.Deserialize(jReader) as JObject;

                jReader.Close();
                sr.Close();
            }
            return obje.ToObject(pDataType);
        }
    }
}
