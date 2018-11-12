using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Xml.Serialization;
using Trading.Extensions;
using Trading.Models.Yml;

namespace Trading.Models
{
    public class ImportResult
    {
        public int Added { get; set; }
        public int Updated { get; set; }
        public string Message { get; set; }
    }
    public class YmlImportModel : BaseDataObject
    {
        public string FileName { get; set; }
        public int ShopID { get; set; }

        public ImportResult Import()
        {
            var result = new ImportResult();
            try
            {
                
                var serializer = new XmlSerializer(typeof (YmlCatalog));
                using (var fs = new StreamReader(FileName))
                {
                    var catalog = (YmlCatalog) serializer.Deserialize(fs);
                    foreach (var offer in catalog.Shop.Offers)
                    {
                        string id = "";
                        if (offer.ID > 0)
                            id = offer.ID.ToString();
                        else id = offer.Name.Translit();

                        var product =
                            DB.Products.FirstOrDefault(
                                x => x.OwnerID == CurrentUser.ShopOwnerID && x.ImportID == id);
                        if (product == null)
                        {
                            product = new Product()
                            {
                                AddedDate = DateTime.Now,
                                Name = offer.Name,
                                OwnerID = CurrentUser.ShopOwnerID,
                                ImportID = id,
                                UnitCode = "796",
                                Article = offer.Name.Translit(),
                                Price = offer.Price
                            };
                            DB.Products.InsertOnSubmit(product);
                            DB.SubmitChanges();
                            result.Added++;
                        }
                        else
                        {
                            product.Price = offer.Price;
                            product.Name = offer.Name;
                            result.Updated++;
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }
            return result;
        }
    }
}