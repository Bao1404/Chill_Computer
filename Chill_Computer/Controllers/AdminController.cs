using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Chill_Computer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ChillComputerContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ISeriesRepository _seriesRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IOrderRepository _orderRepository;

        public AdminController(ChillComputerContext context, IOrderRepository orderRepository, IProductRepository productRepository, IProductTypeRepository productTypeRepository, IBrandRepository brandRepository, ISeriesRepository seriesRepository, IAccountService accountService)
        {
            _accountService = accountService;
            _context = context;
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _seriesRepository = seriesRepository;
            _brandRepository = brandRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult ManageInventory(int pageNumber = 1, int pageSize = 7)
        {
            List<Product> products = _productRepository.GetProductPagination(pageNumber, pageSize);
            var totalProducts = _context.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TypeList = _productTypeRepository.GetProductTypes();
            return View(products);
        }

        public IActionResult ManageOrder(int pageNumber = 1, int pageSize = 7)
        {
            var orders = _orderRepository.GetOrders(pageNumber, pageSize);
            var totalOrders = _context.Orders.Count();
            ViewBag.Dictionary = _orderRepository.GetOrderProductsMap();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(orders);
        }
        public IActionResult ManageAccount(int pageNumber = 1, int pageSize = 7)
        {
            var accounts = _accountService.GetAccounts(pageNumber, pageSize);
            var totalAccounts = _context.Accounts.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalAccounts / pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(accounts);
        }

        [HttpPost]
        public IActionResult UpdateRole()
        {
            var username = Request.Form["username"];
            var roleId = int.Parse(Request.Form["roleId"]);
            var account = _accountService.GetAccountByUserName(username);
            _accountService.UpdateRole(account, roleId);
            return RedirectToAction("ManageAccount");
        }

        [HttpPost]
        public IActionResult DeleteAccount()
        {
            string username = Request.Form["usernameDe"];

            var account = _context.Accounts.FirstOrDefault(a => a.UserName == username);

            if (account != null)
            {
                _accountService.DeleteAccount(account);
            }

            return RedirectToAction("ManageAccount");
        }

        [HttpPost]
        public IActionResult DeleteProduct()
        {
            int productId = int.Parse(Request.Form["productId"]);

            _productRepository.DeleteProduct(productId);

            return RedirectToAction("ManageInventory");
        }

        [HttpPost]
        public IActionResult UpdateProduct(IFormCollection form)
        {
            int productId = int.Parse(form["productId"]);

            var existingProduct = _productRepository.GetProductById(productId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = form["productName"];
            existingProduct.Color = form["productColor"];
            existingProduct.Price = int.Parse(form["productPrice"]);
            existingProduct.Stock = int.Parse(form["productStock"]);
            existingProduct.Img1 = form["productImg1"];
            existingProduct.Img2 = form["productImg2"];
            existingProduct.Img3 = form["productImg3"];
            existingProduct.Version = form["productVersion"];

            _context.SaveChanges();

            return RedirectToAction("ManageInventory");

        }

        [HttpPost]
        public IActionResult AddProduct(IFormCollection form)
        {
            string seriesName = form["seriesName"];
            string brandName = form["brandName"];

            var existingBrand = _brandRepository.GetBrandByName(brandName);
            var existingSeries = _seriesRepository.GetSeriesByName(seriesName);

            int? brandId = null;
            int? seriesId = null;

            if (!string.IsNullOrWhiteSpace(brandName))
            {
                if (existingBrand == null)
                {
                    var newBrand = new Brand { BrandName = brandName };
                    _brandRepository.AddBrand(newBrand);
                    brandId = newBrand.BrandId;
                }
                else
                {
                    brandId = existingBrand.BrandId;
                }
            }

            if (!string.IsNullOrWhiteSpace(seriesName))
            {
                if (existingSeries == null)
                {
                    var newSeries = new Series { SeriesName = seriesName, BrandId = (int)brandId };
                    _seriesRepository.AddSeries(newSeries);
                    seriesId = newSeries.SeriesId;
                }
                else
                {
                    seriesId = existingSeries.SeriesId;
                }
            }

            var product = new Product
            {
                ProductName = form["productName"],
                Price = int.Parse(form["productPrice"]),
                Stock = int.Parse(form["productStock"]),
                Color = form["productColor"],
                Version = form["productVersion"],
                Img1 = form["productImg1"],
                Img2 = form["productImg2"],
                Img3 = form["productImg3"],
                TypeId = string.IsNullOrEmpty(form["typeId"]) ? null : int.Parse(form["typeId"]),
                BrandId = brandId,
                SeriesId = seriesId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("ManageInventory");
        }

        [HttpPost]
        public IActionResult DeleteOrder(IFormCollection form)
        {
            string orderId = form["orderId"];

            var order = _orderRepository.GetOrderById(int.Parse(orderId));

            if (order != null)
            {
                _orderRepository.DeleteOrder(order.OrderId);
            }

            return RedirectToAction("ManageOrder");
        }

        [HttpPost]
        public IActionResult UpdateStatusOrder(IFormCollection form)
        {
            int orderId = int.Parse(form["orderIdUp"]);
            string status = form["orderStatus"];
            var order = _orderRepository.GetOrderById(orderId);
            if (order != null)
            {
                _orderRepository.UpdateOrderStatus(orderId, status);
            }
            return RedirectToAction("ManageOrder");
        }

        [HttpPost]
        public IActionResult SearchByCustomerName(IFormCollection form)
        {
            string customerName = form["customerName"];
            List<ManageOrderViewModel> orders;
            int pageNumber = 1;
            int pageSize = 7;

            if (string.IsNullOrEmpty(customerName))
            {
                int totalOrders = _context.Orders.Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize);
                ViewBag.CurrentPage = pageNumber;

                orders = _orderRepository.GetOrders(pageNumber, pageSize);
            }
            else
            {
                int totalOrders = _orderRepository.GetOrderBySearchCustomerName(customerName).Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize);
                ViewBag.CurrentPage = pageNumber;

                orders = _orderRepository.GetOrderBySearchCustomerName(customerName, pageNumber, pageSize);
            }

            ViewBag.Dictionary = _orderRepository.GetOrderProductsMap();
            return View("ManageOrder", orders);
        }


        [HttpPost]
        public IActionResult SearchInventory(IFormCollection form)
        {
            string productName = form["keySearch"];
            List<Product> products;
            int pageNumber = 1;
            int pageSize = 7;

            if (string.IsNullOrEmpty(productName))
            {
                int totalProducts = _context.Products.Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                ViewBag.CurrentPage = pageNumber;

                products = _productRepository.GetProductPagination(pageNumber, pageSize);
            }
            else
            {
                int totalProducts = _productRepository.GetProductsByName(productName).Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                ViewBag.CurrentPage = pageNumber;

                products = _productRepository.GetProductsByName(productName, pageNumber, pageSize); 
            }

            ViewBag.TypeList = _productTypeRepository.GetProductTypes();
            return View("ManageInventory", products);
        }

        [HttpPost]
        public IActionResult SearchUserName(IFormCollection form)
        {
            string userName = form["keySearch"];
            List<AccountViewModel> account;
            int pageNumber = 1;
            int pageSize = 7;

            if (string.IsNullOrEmpty(userName))
            {
                int totalAccount = _context.Accounts.Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalAccount / pageSize);
                ViewBag.CurrentPage = pageNumber;

                account = _accountService.GetAccounts(pageNumber, pageSize);
            }
            else
            {
                int totalAccount = _accountService.SearchByUsername(userName).Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalAccount / pageSize);
                ViewBag.CurrentPage = pageNumber;

                account = _accountService.SearchByUsername(userName, pageNumber, pageSize);
            }

            return View("ManageAccount", account);
        }
    }
}
