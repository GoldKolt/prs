using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Models;
using WebApplication15.ViewModels;

namespace WebApplication15.Controllers
{
    public class CreditController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CreditController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Credit
        public async Task<IActionResult> Index()
        {
            //await _context.Credits.AddAsync(new Credit()
            //{
            //    HasAnnuityPayments = true,
            //    Name = "Кредит аннуитентный",
            //    Rate = (decimal)0.07,
            //    Term = 12
            //});
            //await _context.Credits.AddAsync(new Credit()
            //{
            //    HasAnnuityPayments = false,
            //    Name = "Кредит дифференцированный",
            //    Rate = (decimal)0.09,
            //    Term = 12
            //});
            //await _context.SaveChangesAsync();
            IEnumerable<CreditContract> contracts = await _context.CreditContracts.Where(c => c.EndDate.CompareTo(DateTime.Today) >= 0).ToListAsync();
            IEnumerable<CreditContractViewModel> vm = _mapper.Map<IEnumerable<CreditContractViewModel>>(contracts);
            return View(vm); 
        }

        // GET: Credit/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CreditContract contract = await _context.CreditContracts.Include(c => c.Accounts).Include(c => c.User).FirstAsync(c => c.Id == id);
            CreditContractViewModel vm = _mapper.Map<CreditContractViewModel>(contract);
            CreditAccount account = contract.Accounts.FirstOrDefault(a => !a.IsForPercents);
            CreditAccount percentAccount = contract.Accounts.FirstOrDefault(a => a.IsForPercents);
            vm.CreditAmount = account?.CreditContract.CreditAmount.ToString("F");
            vm.CreditPercent = percentAccount?.Saldo.ToString("F");
            vm.Debt = (2 * account?.CreditContract.CreditAmount - account?.Debet)?.ToString("F");
            vm.UserName = string.Format("{0} {1} {2}", contract.User.Surname, contract.User.FirstName, contract.User.SecondName);
            return View(vm);
        }

        // GET: Credit/Create
        public async Task<IActionResult> Create()
        {
            CreditContractViewModel vm = new CreditContractViewModel();
            IEnumerable<Credit> credits = await _context.Credits.ToListAsync();
            var cre = new List<SelectListItem>();
            foreach (var item in credits)
            {
                cre.Add(new SelectListItem() { Selected = false, Text = string.Format("Name: {0}; Rate: {1}; Contract term: {2}", item.Name, item.Rate, item.Term), Value = item.Id.ToString() });
            }
            vm.Credits = cre;
            IEnumerable<User> users = await _context.Users.ToListAsync();
            var usr = new List<SelectListItem>();
            foreach (var item in users)
            {
                usr.Add(new SelectListItem() { Selected = false, Text = string.Concat(item.Surname, ' ', item.FirstName, ' ', item.SecondName), Value = item.Id.ToString() });
            }
            vm.Users = usr;
            var cur = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Currency)))
            {
                cur.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.Currencies = cur;
            vm.BeginDate = DateTime.Today.ToString("dd.MM.yyyy");
            vm.Number = vm.GetHashCode().ToString("0000000000").Substring(0, 10);
            return View(vm);
        }

        // POST: Credit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreditContractViewModel vm)
        {
            try
            {
                decimal amount = decimal.Parse(vm.CreditAmount);
                if (amount > 1000000 || amount < 10)
                {
                    return View("Error");
                }
                CreditContract contract = _mapper.Map<CreditContract>(vm);
                Credit credit = _context.Credits.Find(vm.CreditId);
                User user = await _context.Users.FindAsync(contract.UserId);
                int count = await _context.Accounts.Where(a => a.UserId == user.Id).CountAsync();
                contract.EndDate = contract.BeginDate.AddMonths(credit.Term);
                await _context.CreditContracts.AddAsync(contract);
                await _context.SaveChangesAsync();
                Account cashRegisterAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == "cash");
                Account frbAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == "frb");
                CreditAccount account = new CreditAccount()
                {
                    IsActive = false,
                    CreditContractId = contract.Id,
                    Credit = 0,
                    Debet = 0,
                    Saldo = 0,
                    IsForPercents = false,
                    UserId = user.Id,
                    Number = "2400" + user.Id.ToString("00000") + (count + 1).ToString("000") + '1',
                    Name = user.Surname + " " + user.FirstName + " " + user.SecondName
                };
                CreditAccount percentAccount = new CreditAccount()
                {
                    IsActive = false,
                    CreditContractId = contract.Id,
                    Credit = 0,
                    Debet = 0,
                    Saldo = 0,
                    IsForPercents = true,
                    UserId = user.Id,
                    Number = "2470" + user.Id.ToString("00000") + (count + 2).ToString("000") + '2',
                    Name = user.Surname + " " + user.FirstName + " " + user.SecondName
                };
                GiveCreditToClient(ref frbAccount, ref account, amount);
                Card card = new Card()
                {
                    Account = account,
                    Pin = Math.Abs(account.Number.GetHashCode()).ToString("0000").Substring(0, 4),
                    Number = "9112" + Math.Abs(contract.Number.GetHashCode()).ToString("00000000").Substring(0, 8),
                    Balance = amount
                };
                await _context.CreditAccounts.AddAsync(account);
                await _context.CreditAccounts.AddAsync(percentAccount);
                _context.Accounts.Update(frbAccount);
                _context.Accounts.Update(cashRegisterAccount);
                await _context.Cards.AddAsync(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: Credit/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            CreditContract contract = await _context.CreditContracts.Include(c => c.Accounts).Include(c => c.User).FirstAsync(c => c.Id == id);
            CreditContractViewModel vm = _mapper.Map<CreditContractViewModel>(contract);
            CreditAccount account = contract.Accounts.FirstOrDefault(a => !a.IsForPercents);
            CreditAccount percentAccount = contract.Accounts.FirstOrDefault(a => a.IsForPercents);
            vm.CreditAmount = account?.Credit.ToString("F");
            vm.CreditPercent = percentAccount?.Saldo.ToString("F");
            vm.UserName = string.Format("{0} {1} {2}", contract.User.Surname, contract.User.FirstName, contract.User.SecondName);
            return View(vm);
        }

        // POST: Credit/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                CreditContract contract = await _context.CreditContracts.Include(c => c.Accounts).FirstAsync(c => c.Id == id);
                _context.CreditAccounts.RemoveRange(contract.Accounts);
                _context.CreditContracts.Remove(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public void GiveCreditToClient(ref Account frbAccount, ref CreditAccount account, decimal sum)
        {
            frbAccount.Debet += sum;
            frbAccount.Saldo = Math.Abs(frbAccount.Credit - frbAccount.Debet);
            account.Debet += sum;
            account.Saldo = Math.Abs(account.Credit - account.Debet);
        }

        public void TransferCreditToCashier(ref Account cashAccount, ref CreditAccount account, decimal sum)
        {
            account.Credit += sum;
            account.Saldo = Math.Abs(account.Credit - account.Debet);
            cashAccount.Debet += sum;
            cashAccount.Saldo = Math.Abs(cashAccount.Credit - cashAccount.Debet);
        }

        public void GetCreditFromCashier(ref Account cashAccount, decimal sum)
        {
            cashAccount.Credit += sum;
            cashAccount.Saldo = Math.Abs(cashAccount.Credit - cashAccount.Debet);
        }

        public void СhargePercents(ref Account frbAccount, ref CreditAccount percentAccount, decimal sum)
        {
            frbAccount.Credit += sum;
            frbAccount.Saldo = Math.Abs(frbAccount.Credit - frbAccount.Debet);
            percentAccount.Credit += sum;
            percentAccount.Saldo = Math.Abs(percentAccount.Credit - percentAccount.Debet);
        }

        public void GivePercentsToCashier(ref Account cashAccount, decimal sum)
        {
            cashAccount.Debet += sum;
            cashAccount.Saldo = Math.Abs(cashAccount.Credit - cashAccount.Debet);
        }

        public void PaymentForPercents(ref Account cashAccount, ref CreditAccount percentAccount, decimal sum)
        {
            cashAccount.Credit += sum;
            cashAccount.Saldo = Math.Abs(cashAccount.Credit - cashAccount.Debet);
            percentAccount.Debet += sum;
            percentAccount.Saldo = Math.Abs(percentAccount.Credit - percentAccount.Debet);
        }

        public void GiveMoneyToCashier(ref Account cashAccount, decimal sum)
        {
            cashAccount.Debet += sum;
            cashAccount.Saldo = Math.Abs(cashAccount.Credit - cashAccount.Debet);
        }

        public void PaymentForCredit(ref Account cashAccount, ref CreditAccount account, decimal sum)
        {
            cashAccount.Credit += sum;
            cashAccount.Saldo = Math.Abs(cashAccount.Credit - cashAccount.Debet);
            account.Debet += sum;
            account.Saldo = Math.Abs(account.Credit - account.Debet);
        }

        public void EndCredit(ref Account frbAccount, ref CreditAccount account, decimal sum)
        {
            frbAccount.Credit += sum;
            frbAccount.Saldo = Math.Abs(frbAccount.Credit - frbAccount.Debet);
            account.Credit += sum;
            account.Saldo = Math.Abs(account.Credit - account.Debet);
        }

        public async Task<IActionResult> AddPercents()
        {
            try
            {
                IEnumerable<CreditContract> contracts = await _context.CreditContracts.Include(c => c.Credit).Include(c => c.Accounts).Where(c => c.EndDate.CompareTo(DateTime.Today) >= 0).ToListAsync();
                Account frbAccount = await _context.Accounts.FirstAsync(a => a.Name == "frb");
                foreach (var contract in contracts)
                {
                    CreditAccount account = contract.Accounts.First(a => !a.IsForPercents);
                    CreditAccount percentAccount = contract.Accounts.First(a => a.IsForPercents);
                    decimal sum = 0;
                    if (contract.Credit.HasAnnuityPayments)
                    {
                        double i = (double)contract.Credit.Rate / 12;
                        double k = (i * Math.Pow(i + 1, contract.Credit.Term)) / (Math.Pow(i + 1, contract.Credit.Term) - 1);
                        sum = (decimal)k * contract.CreditAmount - contract.CreditAmount / contract.Credit.Term;
                    }
                    else
                    {
                        // decimal osn = contract.CreditAmount / contract.Credit.Term;
                        sum = (2 * contract.CreditAmount - account.Debet) * contract.Credit.Rate / 365 * 30;
                        // sum = osn + proc;
                    }
                    СhargePercents(ref frbAccount, ref percentAccount, sum);
                    _context.Accounts.Update(percentAccount);
                }

                _context.Accounts.Update(frbAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Pay(int id)
        {
            try
            {
                CreditContract contract = await _context.CreditContracts
                    .Include(c => c.Credit).FirstOrDefaultAsync(c => c.Id == id);
                if (contract == null)
                {
                    return RedirectToAction("Details", new { id });
                }
                Account frbAccount = await _context.Accounts.FirstAsync(a => a.Name == "frb");
                Account cashRegisterAccount = await _context.Accounts.FirstAsync(a => a.Name == "cash");
                IEnumerable<CreditAccount> accounts = await _context.CreditAccounts.Where(da => da.CreditContractId == contract.Id).ToListAsync();
                CreditAccount account = accounts.First(a => !a.IsForPercents);
                CreditAccount percentAccount = accounts.First(a => a.IsForPercents);

                decimal sum = 0;
                if (contract.Credit.HasAnnuityPayments)
                {
                    double i = (double)contract.Credit.Rate / 12;
                    double k = (i * Math.Pow(i + 1, contract.Credit.Term)) / (Math.Pow(i + 1, contract.Credit.Term) - 1);
                    sum = (decimal)k * contract.CreditAmount - percentAccount.Saldo;
                }
                else
                {
                    decimal osn = contract.CreditAmount / contract.Credit.Term;
                    decimal proc = (2 * contract.CreditAmount - account.Debet) * contract.Credit.Rate / 365 * 30;
                    sum = osn + proc - percentAccount.Saldo;
                }

                PaymentForCredit(ref cashRegisterAccount, ref account, sum);
                PaymentForPercents(ref cashRegisterAccount, ref percentAccount, percentAccount.Saldo);

                _context.Accounts.Update(account);
                _context.Accounts.Update(percentAccount);
                _context.Accounts.Update(frbAccount);
                _context.Accounts.Update(cashRegisterAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // lab4

        [HttpGet]
        public IActionResult EnterCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnterCard(Card model)
        {
            Card card = _context.Cards.FirstOrDefault(c => c.Number == model.Number);
            if (card == null)
            {
                return View();
            }
            
            return RedirectToAction("EnterPin", card = model);
        }

        [HttpGet]
        public IActionResult EnterPin(Card model, int attempts = 3)
        {
            ViewBag.Attempts = attempts;
            return View(model);
        }

        [HttpPost]
        public IActionResult CheckPin(Card model, int attempts)
        {
            Card card = _context.Cards.Include(c => c.Account).FirstOrDefault(c => c.Number == model.Number);
            if (card == null)
            {
                return View("EnterCard");
            }

            if (card.Pin != model.Pin)
            {
                if (attempts - 1 <= 0)
                {
                    return View("EnterCard");
                }
                ViewBag.Attempts = attempts - 1;
                return View("EnterPin", model);
            }
            return View("Authorized", card.Id);
        }

        public IActionResult Authorized(int id)
        {
            return View(id);
        }

        public IActionResult GetMoney(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> GetMoney(int id, string amount)
        {
            Card card = _context.Cards.Include(c=>c.Account).FirstOrDefault(c => c.Id == id);
            decimal sum = decimal.Parse(amount);
            if (card == null || sum > 1000 || sum < 1 || sum > card.Balance)
            {
                return View("Error");
            }
            card.Balance -= sum;
            Account cashAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == "cash");
            CreditAccount account = await _context.CreditAccounts.FindAsync(card.Account.Id);
            TransferCreditToCashier(ref cashAccount, ref account, sum);
            GetCreditFromCashier(ref cashAccount, sum);
            _context.Accounts.Update(cashAccount);
            _context.Accounts.Update(account);
            _context.Cards.Update(card);
            await _context.SaveChangesAsync();
            ViewBag.Got = amount;
            return View("SeeRest", card);
        }

        public async Task<IActionResult> SeeRest(int id)
        {
            Card model = await _context.Cards.FindAsync(id);
            return View(model);
        }
    }
}