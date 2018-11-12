using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace Trading.Models.Yml
{


    [Serializable, XmlRoot("yml_catalog")]
    public class YmlCatalog
    {
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "shop")]
        public Shop Shop { get; set; }

        public YmlCatalog()
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            /*
                        Namespaces = new XmlSerializerNamespaces();
                        Namespaces.Add("", "");
            */
        }

        /*
                [XmlNamespaceDeclarations]
                public XmlSerializerNamespaces Namespaces { get; set; }
        */
    }

    [Serializable]
    public class Shop
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "company")]
        public string Company { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlArray(ElementName = "currencies")]
        [XmlArrayItem(ElementName = "currency")]
        public Collection<Currency> Currency { get; set; }

        [XmlArray(ElementName = "categories")]
        [XmlArrayItem(ElementName = "category")]
        public Collection<Category> Categories { get; set; }
        [XmlArray(ElementName = "offers")]
        [XmlArrayItem(ElementName = "offer")]
        public Collection<Offer> Offers { get; set; }

    }

    [Serializable]
    public class Currency
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "rate")]
        public string Rate { get; set; }

        public Currency()
        {
            Rate = "1";
            Id = "RUR";
        }
    }

    [XmlRoot(ElementName = "category")]
    public class Category
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "parentId")]
        public string ParentId { get; set; }

        [XmlText]
        public string Name { get; set; }

        public Category()
        {
            Id = 0;
        }
    }

    [XmlRoot(ElementName = "offer")]
    public class Offer
    {
        [XmlAttribute(AttributeName = "id")]
        public long ID { get; set; }

        [XmlAttribute(AttributeName = "available")]
        public bool Available { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "currencyId")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "categoryId")]
        public Collection<int> CategoryID { get; set; }

        [XmlElement(ElementName = "picture")]
        public string Picture { get; set; }

        /*        [XmlElement(ElementName = "publisher", Order = 7)]
                public string Publisher { get; set; }

                [XmlElement(ElementName = "author", Order = 8)]
                public string Author { get; set; }

                [XmlElement(ElementName = "year", Order = 9)]
                public string Year { get; set; }

                [XmlElement(ElementName = "ISBN", Order = 10)]
                public string ISBN { get; set; }

                [XmlElement(ElementName = "binding", Order = 11)]
                public string Binding { get; set; }

                [XmlElement(ElementName = "page_extent", Order = 12)]
                public string PageExtent { get; set; }*/

        [XmlElement(ElementName = "delivery")]
        public bool Delivery { get; set; }

        /*
                [XmlArray(ElementName = "orderingTime"), XmlArrayItem(ElementName = "ordering")]
                public Collection<string> OrderingTime { get; set; }
        */


        /*
                [XmlElement(ElementName = "vendor")]
                public string Vendor { get; set; }
        */

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        public Offer()
        {
            Price = 0;
        }
    }
}