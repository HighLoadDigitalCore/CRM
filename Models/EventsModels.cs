using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions.Helpers;

namespace Trading.Models
{
    public class EventsFilter
    {
        public IEnumerable<SelectListItem> UserList { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? ActionID { get; set; }
        public IEnumerable<SelectListItem> ActionList { get; set; }

        public int? UserID { get; set; }


        public List<LogEntry> Result { get; set; }

        public EventsFilter()
        {
            var db = new DataContext();
            var users =
                db.LogEntries.Select(x => x.User)
                    .Distinct()
                    .OrderBy(x => (x.UserSurname ?? "Anonimous") + " " + (x.UserName ?? ""))
                    .ToList()
                    .Select(
                        x =>
                            new SelectListItem()
                            {
                                Selected = (x.ID == (UserID??0)),
                                Text = x.UserSurname + " " + x.UserName,
                                Value = x.ID.ToString()
                            }).ToList();

            users.Insert(0, new SelectListItem(){Selected = !UserID.HasValue, Text = "Все пользователи", Value = ""});
            UserList = users;


            var actions =
                db.LogActions.OrderBy(x => x.Name)
                    .Select(
                        x => new SelectListItem() {Selected = (ActionID??0) == x.ID, Text = x.Name, Value = x.ID.ToString()})
                    .ToList();

            actions.Insert(0, new SelectListItem(){Selected = !ActionID.HasValue, Text = "Все действия", Value = ""});
            ActionList = actions;
            Search(db);
          
        }

        public void Search(DataContext db)
        {
            var result = db.LogEntries.AsQueryable();
            if (MinDate.HasValue)
            {
                result = result.Where(x => x.ActionDate.Date >= MinDate.Value.Date);
            }
            if (MaxDate.HasValue)
            {
                result = result.Where(x => x.ActionDate.Date <= MaxDate.Value.Date);
            }
            if (ActionID.HasValue)
            {
                result = result.Where(x => x.ActionID == ActionID.Value);
            }
            if (UserID.HasValue)
            {
                result = result.Where(x => x.UserID == UserID.Value);
            }
            Result = result.OrderByDescending(x => x.ID).Take(1000).ToList();
        }
    }  
    
    public class LoginsFilter
    {
        public IEnumerable<SelectListItem> UserList { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? UserID { get; set; }


        public List<LoginHistory> Result { get; set; }

        public LoginsFilter()
        {
            var db = new DataContext();
            var users =
                db.LoginHistories.Select(x => x.User)
                    .Distinct()
                    .OrderBy(x => (x.UserSurname ?? "Anonimous") + " " + (x.UserName ?? ""))
                    .ToList()
                    .Select(
                        x =>
                            new SelectListItem()
                            {
                                Selected = (x.ID == (UserID??0)),
                                Text = x.UserSurname + " " + x.UserName,
                                Value = x.ID.ToString()
                            }).ToList();

            users.Insert(0, new SelectListItem(){Selected = !UserID.HasValue, Text = "Все пользователи", Value = ""});
            UserList = users;


            Search(db);
          
        }

        public void Search(DataContext db)
        {
            var result = db.LoginHistories.AsQueryable();
            if (MinDate.HasValue)
            {
                result = result.Where(x => x.LoginDate.Date >= MinDate.Value.Date);
            }
            if (MaxDate.HasValue)
            {
                result = result.Where(x => x.LoginDate.Date <= MaxDate.Value.Date);
            }
          
            if (UserID.HasValue)
            {
                result = result.Where(x => x.UserID == UserID.Value);
            }
            Result = result.OrderByDescending(x => x.ID).Take(1000).ToList();
        }
    }
}