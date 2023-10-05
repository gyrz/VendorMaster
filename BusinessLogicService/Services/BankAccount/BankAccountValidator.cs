using BusinessLogicService.Base;
using Contracts.Dto.BankAccount;
using DataAccess.Data;
using System.Text.RegularExpressions;

namespace BusinessLogicService.Services.BankAccountSvc
{
    public class BankAccountValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly BankAccountDto bankAccountDto;
        public BankAccountValidator(VendorDbContext vendorDbContext, BankAccountDto bankAccountDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.bankAccountDto = bankAccountDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(bankAccountDto.BankName))
                return "BankAccount BankName is required";

            if (string.IsNullOrEmpty(bankAccountDto.IBAN))
                return "BankAccount BankName is required";

            if (string.IsNullOrEmpty(bankAccountDto.BIC))
                return "BankAccount BankName is required";

            if (!IsValidIBAN(bankAccountDto.IBAN))
                return "IBAN format is not valid";

            if (!IsValidBIC(bankAccountDto.BIC))
                return "BIC format is not valid";

            if (CheckIfAlreadyExists(bankAccountDto))
                return "BankAccount already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(BankAccountDto bankAccountDto)
        {
            var bankAccount = vendorDbContext.BankAccounts.FirstOrDefault(x => 
            x.BankName == bankAccountDto.BankName 
            && x.IBAN == bankAccountDto.IBAN
            && x.BIC == bankAccountDto.BIC);
            if (bankAccount == null) return false;
            return bankAccount.Id != bankAccountDto.Id;
        }

        public bool IsValidIBAN(string iban)
        {
            string pattern = @"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$";
            return Regex.IsMatch(bankAccountDto.IBAN, pattern);
        }

        static bool IsValidBIC(string bic)
        {
            if (bic.Length != 8 && bic.Length != 11)
            {
                return false;
            }

            foreach (char c in bic)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}