using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Faq.Library
{
    public class FaqManager
    {
        const string XmlSourcePath = "faqs.xml";
        public static List<Faq> Load()
        {
            if (!File.Exists(XmlSourcePath))
            {
                return new List<Faq>();
            }

            var serializer = new XmlSerializer(typeof(List<Faq>));
            using (var reader = XmlReader.Create(XmlSourcePath))
            {
                return (List<Faq>)serializer.Deserialize(reader);
            }

        }

        public static string GetXml(List<Faq> faqs)
        {
            if (faqs == null)
            {
                throw new System.ArgumentNullException("faqs");
            }

            var serializer = new XmlSerializer(typeof(List<Faq>));
            string utf8 = null;
            using (var writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, faqs);
                utf8 = writer.ToString();
            }
            return utf8;
        }

        public static void Save(List<Faq> faqs)
        {
            if (faqs == null)
            {
                throw new System.ArgumentNullException("faqs");
            }

            string faqString = GetXml(faqs);

            File.WriteAllText(XmlSourcePath, faqString, Encoding.UTF8);

        }
    }

    class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
