using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trading.Extensions;

namespace Trading.Models
{
    public partial class GraphSerialsRel : BaseDataObject
    {
        public string InitialColor
        {
            get
            {
                if (Color.IsNullOrEmpty())
                {
                    
                    var c = DB.GraphSerialsRels.First(x => x.ID == ID);
                    var exist = DB.GraphSerialsRels.Where(x => x.GraphID == c.GraphID).ToList();
                    string color = "";
                    while (true)
                    {
                        var generator = new GraphColorGenerator();
                        color = "#" + generator.NextColour();
                        if (exist.All(x => x.Color != color))
                        {
                            break;
                        }
                        
                    }
                    c.Color = color;
                    DB.SubmitChanges();
                    Color = c.Color;

                }
                return Color;
            }
        }
    }

    public partial class Graph:BaseDataObject
    {
        public int GraphTypeID { get; set; }
        public bool HasSerial(int serialID)
        {
            return GraphSerialsRels.Any(x => x.SerialID == serialID);
        }

        public string SerialListPlane { get; set; }
        
        public bool IsHidden
        {
            get
            {
                var httpCookie = HttpContext.Current.Request.Cookies["Graph_" + ID];
                return httpCookie != null && httpCookie.Value == "hidden";
            }
        }

        private List<GraphType> _typeList;
        public List<GraphType> TypeList
        {
            get { return _typeList ?? (_typeList = DB.GraphTypes.ToList()); }
        }
        public List<GraphTypeFunc> GetTypeFuncList(int typeID)
        {
            var first = DB.GraphTypeFuncs.First(x => x.ID == typeID);
            return DB.GraphTypeFuncs.Where(x => x.TypeID == first.TypeID).OrderBy(x => x.Name).ToList();
        }
    }
}