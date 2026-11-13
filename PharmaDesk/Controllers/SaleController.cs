using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.Services;
using PharmaDesk.ViewModels;

namespace PharmaDesk.Controllers
{
    public class SaleController : Controller
    {
        private readonly PharmaDeskContext _context;
        private readonly SaleService _saleService;

        public SaleController(PharmaDeskContext context, SaleService saleService)
        {
            _context = context;
            _saleService = saleService;
        }

        // GET: /Sale/New
        public async Task<IActionResult> New()
        {
            // bring the list of active medicines to the view for selection
            var medicines = await _context.Medicines
                .Where(m => m.IsActive)
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.Barcode,
                    m.SellingPrice
                })
                .ToListAsync();

            ViewBag.MedicinesJson = System.Text.Json.JsonSerializer.Serialize(medicines);
            return View();
        }

        // POST: /Sale/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
      public async Task<IActionResult> Save([FromBody] SaleViewModel vm)
      
        {
            if (vm == null || vm.Items == null || vm.Items.Count == 0)
            {
                return Json(new { success = false, message = "Cart is empty." });
            }

            var result = await _saleService.ProcessSaleAsync(vm);

            if (result.Success)
            {
                return Json(new { success = true, message = result.Message, saleId = result.SaleId });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }

        // GET: /Sale/Receipt/5  (page after sale)
        public async Task<IActionResult> Receipt(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Medicine)
                .Include(s => s.TeamMember)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return NotFound();

            return View(sale);
        }

        // GET: /Sale/History
        public async Task<IActionResult> History()
        {
            var sales = await _context.Sales
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Medicine)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            return View(sales);
        }

    }
}