using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trading.Models
{
    public class BaseFormModel
    {
        public string Message { get; set; }
        public string RedirectURL { get; set; }
        public bool IsPost { get; set; }
    }

    public class LoginModel:BaseFormModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        
    }
    public class RegisterModelPartnerStep1:BaseFormModel
    {
        public int Type { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patrinomic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool Agree { get; set; }
    } 
    public class RegisterModelCustomerStep1:BaseFormModel
    {
        public string NickCSS { get; set; }
        public int Type { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patrinomic { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Agree { get; set; }
        public bool HasNick { get; set; }
    }

    public class SavePassModel : BaseFormModel
    {
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public string DigitID { get; set; }
    }
}