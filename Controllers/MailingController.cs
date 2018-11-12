using System.Linq;
using System.Web.Mvc;
using Trading.Models;

namespace Trading.Controllers
{

    public class MailingController : BaseController
    {
        

        [HttpGet]
        public ActionResult Index(int? mailingID, int? Type)
        {
            var mailings =
                DB.MailingLists.Where(x=> x.Enabled && x.Type == (Type ?? 0)).OrderBy(x => x.Name).AsEnumerable().ToList();
            ViewBag.Type = (Type ?? 0);
            mailings.Insert(0, new MailingList { ID = 0, Name = "--Выберите рассылку в списке--" });
            ViewBag.Mailings = new SelectList(mailings, "ID", "Name", mailingID ?? 0);
            var current = DB.MailingLists.FirstOrDefault(x => x.ID == mailingID);
            return View(current);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(MailingList mailing, int? mailingID, int? Type)
        {
            var mailings =
                DB.MailingLists.Where(x=> x.Enabled && x.Type == (Type ?? 0)).OrderBy(x=> x.Name).AsEnumerable().ToList();
            ViewBag.Type = (Type ?? 0);
            mailings.Insert(0, new MailingList { ID = 0, Name = "--Выберите рассылку в списке--" });
            ViewBag.Mailings = new SelectList(mailings, "ID", "Name", mailingID ?? 0);

            var mail = DB.MailingLists.FirstOrDefault(x => x.ID == mailing.ID);
            if (mail == null)
            {
                ModelState.AddModelError("", "Ошибка сохранения! Рассылка не найдена в БД");
                return View(mailing);
            }
            mail.Header = mailing.Header;
            mail.Letter = mailing.Letter;
            mail.TargetMail = mailing.TargetMail;

            if (!mail.MailingReplacements.All(x => mailing.Letter.IndexOf(x.Replacement) >= 0))
            {
                ModelState.AddModelError("", "Ошибка! Данные не сохранены - Все подстановки из списка должны быть использованы в тексте письма");
                return View(mail);
            }

            DB.SubmitChanges();

            ModelState.AddModelError("", "Данные успешно сохранены");
            return View(mail);
        }

    }
}
