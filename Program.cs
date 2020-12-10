using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Lab_14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Binary");

            // Объект для сериализации.
            Document document = new Document("Fn", "Nk");
            Console.WriteLine("Объект создан.");
            // Создаем объект BinaryFormatter.
            BinaryFormatter formatter = new BinaryFormatter();
            // Получаем поток, куда будем записывать сериализованный объект.
            using (FileStream fs = new FileStream("Task_1.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, document); //Сериализ. объект с указанной вершиной (корнем) в заданный поток.
                Console.WriteLine("Объект сериализован.");
            }

            // Десериализация из файла.
            using (FileStream fs = new FileStream("Task_1.dat", FileMode.OpenOrCreate))
            {
                Document _document = (Document)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован.");
                Console.WriteLine($"OrgName:  {_document.Name}. DocName: {_document.DocName}");
            }

            Console.WriteLine("\nSOAP");

            Document document_1 = new Document("Fn", "Nk");
            Document document_2 = new Document("Fn", "Nk");
            Document[] documents = new Document[] { document_1, document_2 };

            // Создаем объект SoapFormatter.
            SoapFormatter formatter_2 = new SoapFormatter();
            // Получаем поток, куда будем записывать сериализованный объект.
            using (FileStream fs = new FileStream("documents.soap", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, documents);
                Console.WriteLine("Объект сериализован.");
            }

            // Десериализация.
            using (FileStream fs = new FileStream("documents.soap", FileMode.OpenOrCreate))
            {
                Document[] _document = (Document[])formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован.");
                foreach (Document p in _document)
                {
                    Console.WriteLine($"OrgName:  {p.Name}. DocName: {p.DocName}");
                }
            }

            Console.WriteLine("\n");

            // Объект для сериализации.
            Document p1 = new Document("as", "asfas");
            Document p2 = new Document("as", "asfas");
            Document p3 = new Document("as", "asfas");
            Document[] _p = new Document[] { p1, p2, p3 };

            XmlSerializer f = new XmlSerializer(typeof(Document[]));

            using (FileStream fs = new FileStream("p.xml", FileMode.OpenOrCreate))
            {
                f.Serialize(fs, _p);
                Console.WriteLine("Объект сериализован.");
            }

            using (FileStream fs = new FileStream("p.xml", FileMode.OpenOrCreate))
            {
                Document[] __p = (Document[])f.Deserialize(fs);

                foreach (Document p in __p)
                {
                    Console.WriteLine("Объект десериализован.");
                    Console.WriteLine($"OrgName:  {p.Name}. DocName: {p.DocName}");
                }
            }

            Console.WriteLine("\nJSON");

            // Объект для сериализации.
            Document_ document_6 = new Document_("Fn", "Nk");
            Document_ document_7 = new Document_("ssfs", "sdgsdg");
            Document_[] documents_0 = new Document_[] { document_6, document_7 };

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Document_[]));

            using (FileStream fs = new FileStream("documents_3.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, documents_0);
            }
            using (FileStream fs = new FileStream("documents_3.json", FileMode.OpenOrCreate))
            {
                Document_[] documents_1 = (Document_[])jsonFormatter.ReadObject(fs);//Выполняет чтение и возвр. десериализ. объект
                foreach (Document_ p in documents_1)
                {
                    Console.WriteLine($"OrgName:  {p.Name}. DocName: {p.DocName}");
                }
            }

            Console.WriteLine("\nXML");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("p.xml");
            XmlElement xRoot = xDoc.DocumentElement;//возвр. корень

            // Выбор всех дочерних узлов.
            XmlNodeList childnodes = xRoot.SelectNodes("*");
            foreach (XmlNode n in childnodes)
                Console.WriteLine(n.OuterXml);//возвр. разметку, содерж. данный узел

            Console.WriteLine();

            XmlNodeList childnodes_ = xRoot.SelectNodes("//Document/DocName");//Список узлов в соотв. запросу
            foreach (XmlNode n in childnodes_)
                Console.WriteLine(n.InnerText);//возвр. значение узла

            XDocument xDocument = new XDocument();
            //Элементы хмл
            XElement first = new XElement("Document");
            XElement nElem = new XElement("Name", "lll");
            XElement vElem = new XElement("DocName", "OOO");

            first.Add(nElem);
            first.Add(vElem);

            XElement second = new XElement("Document");
            XElement _nElem = new XElement("Name", "pop");
            XElement _vElem = new XElement("DocName", "ioi");

            second.Add(_nElem);
            second.Add(_vElem);

            XElement elements = new XElement("Documents");

            elements.Add(first);
            elements.Add(second);

            xDocument.Add(elements);

            xDocument.Save("Documents.xml");//сериализ. с перезаписью если уже сущ.

            XDocument doccument = XDocument.Load("Documents.xml");//созд.новый хдок  из файла

            var item = from xE in doccument.Element("Documents").Elements("Document")
                       where xE.Element("Name").Value == "pop"
                       select new Document
                       {
                           Name = xE.Element("Name").Value,
                           DocName = xE.Element("DocName").Value
                       };

            foreach (var x in item)
            {
                Console.WriteLine($"OrgName:  {x.Name}. DocName: {x.DocName}");
            }
        }
    }

    [Serializable]//Указ. на возможность сериализ. класс
    public abstract class Organization
    {
        public string Name { get; set; }
        public Organization() { }
        public Organization(string name)
        { Name = name; }
    }

    [Serializable]
    public class Document : Organization
    {
        public string DocName { get; set; }
        public Document() : base() { }
        public Document(string name, string d_name) : base(name)
        { DocName = d_name; }
        public void Display()
        {
            Console.WriteLine($"{Name} - {DocName}");
        }
    }

    [DataContract]//тип реализ. контракт
    public abstract class Organization_
    {
        [DataMember]//элемент явлю частью контракта
        public string Name { get; set; }
        public Organization_() { }
        public Organization_(string name)
        { Name = name; }
    }

    [DataContract]
    public class Document_ : Organization_
    {
        [DataMember]
        public string DocName { get; set; }
        public Document_() : base() { }
        public Document_(string name, string d_name) : base(name)
        { DocName = d_name; }
        public void Display()
        {
            Console.WriteLine($"{Name} - {DocName}");
        }
    }
}