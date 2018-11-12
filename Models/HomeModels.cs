using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trading.Models
{
    public class MyContractors : BaseDataObject
    {
        public int ShopID { get; set; }

        public MyContractors()
        {
            ShopID = CurrentUser.ShopOwnerID;
        }
        public MyContractors(int? ShopID)
        {
            this.ShopID = ShopID ?? CurrentUser.DefShopID;
        }
        public bool ShowBar
        {
            get { return IncomingContractors.Any(); }
        }

        private List<Contractor> _incomingContractors;
        public List<Contractor> IncomingContractors
        {
            get
            {
                return _incomingContractors ??
                       (_incomingContractors =
                           DB.Contractors.Where(x => x.UserFor == CurrentUser.ID && x.Status == 0).ToList());
            }
        }
        private List<Contractor> _incomingContractorsDenied;
        public List<Contractor> IncomingContractorsDenied
        {
            get
            {
                return _incomingContractorsDenied ??
                       (_incomingContractorsDenied =
                           DB.Contractors.Where(x => x.UserFor == CurrentUser.ID && x.Status == -1).ToList());
            }
        }
        private List<Contractor> _outcomingContractors;
        public List<Contractor> OutcomingContractors
        {
            get
            {
                return _outcomingContractors ??
                       (_outcomingContractors =
                           DB.Contractors.Where(x => x.UserBy == CurrentUser.ID && x.Status == 0).ToList());
            }
        }
        private List<Contractor> _outcomingContractorsDenied;
        public List<Contractor> OutcomingContractorsDenied
        {
            get
            {
                return _outcomingContractorsDenied ??
                       (_outcomingContractorsDenied =
                           DB.Contractors.Where(x => x.UserBy == CurrentUser.ID && x.Status == -1 && x.ShopID == ShopID).ToList());
            }
        }

        private List<Contractor> _myContactors;
        public List<Contractor> MyContactors
        {
            get
            {
                return _myContactors ??
                       (_myContactors = DB.Contractors.Where(x => x.UserBy == CurrentUser.ID && x.ShopID == ShopID && x.Status == 1).ToList());
            }
        }
        private List<Contractor> _meContactors;
        public List<Contractor> MeContactors
        {
            get
            {
                return _meContactors ??
                       (_meContactors = DB.Contractors.Where(x => x.UserFor == CurrentUser.ID /*&& x.ShopID == ShopID*/ && x.Status == 1).ToList());
            }
        }
    }
}