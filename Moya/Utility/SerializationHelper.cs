namespace Moya.Utility
{
    using System.IO;
    using System.Xml.Serialization;

    public class SerializationHelper
    {
        public static string Serialize<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, toSerialize);
                return stringWriter.ToString();
            }
        }

        public static T Deserialize<T>(string serializedData)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(serializedData.GetType());

            T deserializedObject;
            using (TextReader tr = new StringReader(serializedData))
            {
                deserializedObject = (T)xmlSerializer.Deserialize(tr);
            }
            return deserializedObject;
        }
    }
}