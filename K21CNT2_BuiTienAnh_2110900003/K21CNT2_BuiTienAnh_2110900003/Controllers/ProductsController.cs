using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using K21CNT2_BuiTienAnh_2110900003.Models;

namespace K21CNT2_BuiTienAnh_2110900003.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DsmmvcContext _context;

        public ProductsController(DsmmvcContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách sản phẩm có Cid = 1
            var productsCid1 = await _context.Products
                .Where(p => p.Cid == 1)
                .ToListAsync();

            // Lấy danh sách sản phẩm có Cid = 2
            var productsCid2 = await _context.Products
                .Where(p => p.Cid == 2)
                .ToListAsync();

            // Lấy danh sách sản phẩm có Cid = 3
            var productsCid3 = await _context.Products
                .Where(p => p.Cid == 3)
                .ToListAsync();

            // Sử dụng ViewBag để truyền danh sách sản phẩm sang View
            ViewBag.ProductsCid1 = productsCid1;
            ViewBag.ProductsCid2 = productsCid2;
            ViewBag.ProductsCid3 = productsCid3;

            return View();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
