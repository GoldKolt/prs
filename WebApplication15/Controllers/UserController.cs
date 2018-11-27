using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication15.Models;
using WebApplication15.ViewModels;

namespace WebApplication15.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: User
        public IActionResult Index()
        {
            IEnumerable<User> users = _context.Users;
            IEnumerable<UserViewModel> vm = _mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(vm);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int id)
        {
            User user = await _context.Users.FindAsync(id);
            UserViewModel vm = _mapper.Map<UserViewModel>(user);
            return View(vm);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            UserViewModel vm = new UserViewModel();
            var sex = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Sex)))
            {
                sex.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.Sexes = sex;
            var cit = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Cities)))
            {
                cit.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.CitiesOfResidence = cit;
            var regCit = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Cities)))
            {
                regCit.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.RegistrationCities = regCit;
            var dis = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(DisabilityGroup)))
            {
                dis.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.Disabilities = dis;
            var ms = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(MaritalStatus)))
            {
                ms.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.MaritalStatuses = ms;
            var nat = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Nationality)))
            {
                nat.Add(new SelectListItem() { Selected = false, Text = item, Value = item });
            }
            vm.Nationalities = nat;
            return View(vm);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel vm)
        {
            try
            {
                // TODO: Add insert logic here
                User user = _mapper.Map<User>(vm);
                if (user.BirthDate.AddYears(14) > DateTime.Today || user.IssueDate > DateTime.Today || user.BirthDate >= user.IssueDate)
                {
                    return View("Error");
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            User user = await _context.Users.FindAsync(id);
            UserViewModel vm = _mapper.Map<UserViewModel>(user);
            var sex = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Sex)))
            {
                sex.Add(new SelectListItem() { Selected = item == vm.Sex.ToString(), Text = item, Value = item });
            }
            vm.Sexes = sex;
            var cit = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Cities)))
            {
                cit.Add(new SelectListItem() { Selected = item == vm.CityOfResidence.ToString(), Text = item, Value = item });
            }
            vm.CitiesOfResidence = cit;
            var regCit = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Cities)))
            {
                regCit.Add(new SelectListItem() { Selected = item == vm.RegistrationCity.ToString(), Text = item, Value = item });
            }
            vm.RegistrationCities = regCit;
            var dis = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(DisabilityGroup)))
            {
                dis.Add(new SelectListItem() { Selected = item == vm.Nationality.ToString(), Text = item, Value = item });
            }
            vm.Disabilities = dis;
            var ms = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(MaritalStatus)))
            {
                ms.Add(new SelectListItem() { Selected = item == vm.Nationality.ToString(), Text = item, Value = item });
            }
            vm.MaritalStatuses = ms;
            var nat = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Nationality)))
            {
                nat.Add(new SelectListItem() { Selected = item == vm.Nationality.ToString(), Text = item, Value = item });
            }
            vm.Nationalities = nat;
            return View(vm);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel vm)
        {
            try
            {
                User user = _mapper.Map<User>(vm);
                if (user.BirthDate > DateTime.Today || user.IssueDate > DateTime.Today)
                {
                    return View("Error");
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: User/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await _context.Users.FindAsync(id);
            UserViewModel vm = _mapper.Map<UserViewModel>(user);
            return View(vm);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                User user = await _context.Users.FindAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}