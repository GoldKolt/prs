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
    public class DepositContractController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public DepositContractController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: DepositAccount
        public async Task<IActionResult> Index()
        {
            //await _context.Accounts.AddAsync(new Account()
            //{
            //    Number = "101000000001",
            //    IsActive = true,
            //    Credit = 0,
            //    Debet = 0,
            //    Saldo = 0,
            //    Name = "cash"
            //});
            //await _context.Accounts.AddAsync(new Account()
            //{
            //    Number = "732700000001",
            //    IsActive = false,
            //    Credit = 10000000000,
            //    Debet = 0,
            //    Saldo = 10000000000,
            //    Name = "frb"
            //});
            //await _context.Deposits.AddAsync(new Deposit()
            //{
            //    IsRevocable = true,
            //    Name = "Белки отзывные",
            //    Rate = (decimal)0.07,
            //    Term = 12
            //});
            //await _context.Deposits.AddAsync(new Deposit()
            //{
            //    IsRevocable = false,
            //    Name = "Белки безотзывные",
            //    Rate = (decimal)0.09,
            //    Term = 12
            //});
            //await _context.SaveChangesAsync();
            IEnumerable<DepositContract> contracts = await _context.DepositContracts.Where(c => c.EndDate.CompareTo(DateTime.Today) >= 0).ToListAsync();
            IEnumerable<DepositContractViewModel> vm = _mapper.Map<IEnumerable<DepositContractViewModel>>(contracts);
            return View(vm);
        }

        // GET: DepositAccount/Details/5
        public async Task<IActionResult> Details(int id)
        {
            DepositContract contract = await _context.DepositContracts.Include(c => c.Accounts).Include(c => c.User).FirstAsync(c => c.Id == id);
            DepositContractViewModel vm = _mapper.Map<DepositContractViewModel>(contract);
            DepositAccount account = contract.Accounts.FirstOrDefault(a => !a.IsForPercents);
            DepositAccount percentAccount = contract.Accounts.FirstOrDefault(a => a.IsForPercents);
            vm.DepositAmount = account?.Credit.ToString("F");
            vm.DepositPercent = percentAccount?.Saldo.ToString("F");
            vm.UserName = string.Format("{0} {1} {2}", contract.User.Surname, contract.User.FirstName, contract.User.SecondName);
            return View(vm);
        }

        // GET: DepositAccount/Create
        public async Task<IActionResult> Create()
        {
            DepositContractViewModel vm = new DepositContractViewModel();
            IEnumerable<Deposit> deposits = await _context.Deposits.ToListAsync();
            var dep = new List<SelectListItem>();
            foreach (var item in deposits)
            {
                dep.Add(new SelectListItem() { Selected = false, Text = string.Format("Name: {0}; Rate: {1}; Contract term: {2}", item.Name, item.Rate, item.Term), Value = item.Id.ToString() });
            }
            vm.Deposits = dep;
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
            vm.Number = vm.GetHashCode().ToString("0000000000");
            return View(vm);
        }

        // POST: DepositAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepositContractViewModel vm)
        {
            try
            {
                DepositContract contract = _mapper.Map<DepositContract>(vm);
                Deposit deposit = _context.Deposits.Find(vm.DepositId);
                User user = await _context.Users.FindAsync(contract.UserId);
                int count = await _context.Accounts.Where(a => a.UserId == user.Id).CountAsync();
                contract.EndDate = contract.BeginDate.AddMonths(deposit.Term);
                await _context.DepositContracts.AddAsync(contract);
                await _context.SaveChangesAsync();
                Account cashRegisterAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == "cash");
                Account frbAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == "frb");
                decimal amount = decimal.Parse(vm.DepositAmount);
                cashRegisterAccount.Debet += amount;
                cashRegisterAccount.Credit += amount;
                frbAccount.Credit += amount;
                frbAccount.Saldo += amount;
                DepositAccount account = new DepositAccount()
                {
                    IsActive = false,
                    DepositContractId = contract.Id,
                    Credit = amount,
                    Debet = amount,
                    Saldo = 0,
                    IsForPercents = false,
                    UserId = user.Id,
                    Number = "3014" + user.Id.ToString("00000") + (count + 1).ToString("000") + '1',
                    Name = user.Surname + " " + user.FirstName + " " + user.SecondName
                };
                await _context.DepositAccounts.AddAsync(account);
                DepositAccount percentAccount = new DepositAccount()
                {
                    IsActive = false,
                    DepositContractId = contract.Id,
                    Credit = 0,
                    Debet = 0,
                    Saldo = 0,
                    IsForPercents = true,
                    UserId = user.Id,
                    Number = "3470" + user.Id.ToString("00000") + (count + 2).ToString("000") + '2',
                    Name = user.Surname + " " + user.FirstName + " " + user.SecondName
                };
                await _context.DepositAccounts.AddAsync(percentAccount);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: DepositAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepositAccount/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                DepositContract contract = await _context.DepositContracts.Include(c => c.Accounts).FirstAsync(c => c.Id == id);
                _context.DepositAccounts.RemoveRange(contract.Accounts);
                _context.DepositContracts.Remove(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> GetPercents(int id)
        {
            try
            {
                DepositContract contract = await _context.DepositContracts
                    .Include(c => c.Deposit)
                    .Where(c => c.EndDate.CompareTo(DateTime.Today) > 0 && c.Deposit.IsRevocable &&
                                c.BeginDate.AddMonths(DateTime.Today.Month - c.BeginDate.Month).CompareTo(DateTime.Today) == 0)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (contract == null)
                {
                    return RedirectToAction("Details", new {id});
                }
                Account cashRegisterAccount = await _context.Accounts.FirstAsync(a => a.Name == "cash");                
                DepositAccount percentAccount = await _context.DepositAccounts.FirstAsync(da => da.DepositContractId == contract.Id && da.IsForPercents);
                percentAccount.Debet += percentAccount.Saldo;
                cashRegisterAccount.Credit += percentAccount.Saldo;
                cashRegisterAccount.Debet += percentAccount.Saldo;
                percentAccount.Saldo = 0;
                _context.Accounts.Update(percentAccount);
                _context.Accounts.Update(cashRegisterAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> AddPercents(int id)
        {
            try
            {
                IEnumerable<DepositContract> contracts = await _context.DepositContracts.Where(c => c.EndDate.CompareTo(DateTime.Today) >= 0).Include(c => c.Deposit).ToListAsync();
                Account frbAccount = await _context.Accounts.FirstAsync(a => a.Name == "frb");
                foreach (var contract in contracts)
                {
                    IEnumerable<DepositAccount> accounts = await _context.DepositAccounts.Where(da => da.DepositContractId == contract.Id).ToListAsync();
                    DepositAccount account = accounts.First(a => !a.IsForPercents);
                    DepositAccount percentAccount = accounts.First(a => a.IsForPercents);
                    percentAccount.Credit += (account.Credit * contract.Deposit.Rate);
                    percentAccount.Saldo += (account.Credit * contract.Deposit.Rate);
                    percentAccount.Debet += (account.Credit * contract.Deposit.Rate);
                    frbAccount.Saldo -= (account.Credit * contract.Deposit.Rate);
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

        public async Task<IActionResult> EndDeposit(int id)
        {
            try
            {
                DepositContract contract = await _context.DepositContracts
                    /*.Where(c => c.EndDate.CompareTo(DateTime.Today) == 0)*/
                    .Include(c => c.Deposit).FirstOrDefaultAsync(c => c.Id == id);
                if (contract == null)
                {
                    return RedirectToAction("Details", new { id });
                }
                Account frbAccount = await _context.Accounts.FirstAsync(a => a.Name == "frb");
                Account cashRegisterAccount = await _context.Accounts.FirstAsync(a => a.Name == "cash");
                IEnumerable<DepositAccount> accounts = await _context.DepositAccounts.Where(da => da.DepositContractId == contract.Id).ToListAsync();
                DepositAccount account = accounts.First(a => !a.IsForPercents);
                DepositAccount percentAccount = accounts.First(a => a.IsForPercents);
                percentAccount.Debet += percentAccount.Saldo;
                cashRegisterAccount.Credit += percentAccount.Saldo;
                cashRegisterAccount.Debet += percentAccount.Saldo;
                percentAccount.Saldo = 0;
                frbAccount.Saldo -= account.Credit;
                cashRegisterAccount.Credit += account.Credit;
                cashRegisterAccount.Debet += account.Credit;
                account.Debet += account.Credit;
                account.Credit += account.Credit;
                account.Saldo = 0;
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


    }
}