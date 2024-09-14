using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CaptureSystem.DB
{
    class DataBase
    {
        public static CaptureSystem GetServerFromDB()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CaptureSystem));
            using (FileStream fs = new FileStream("Plugins/CaptureSystem.xml", FileMode.OpenOrCreate))
            {
                CaptureSystem server = (CaptureSystem)xmlSerializer.Deserialize(fs);
                return server;

            }
        }

        public static void SerializeServ()
        {
            CaptureSystem server = new CaptureSystem();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CaptureSystem));

            using (FileStream fs = new FileStream("Plugins/CaptureSystem.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, server);
            }
        }

        public static void Save(CaptureSystem server)
        {
            FileInfo fileInfo = new FileInfo("Plugins/CaptureSystem.xml");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CaptureSystem));
            fileInfo.Delete();

            using (FileStream fs = new FileStream("Plugins/CaptureSystem.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, server);
            }
        }
    }
}
