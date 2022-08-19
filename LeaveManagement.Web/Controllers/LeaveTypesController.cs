using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Web.Data;

namespace LeaveManagement.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        //Dependency injection. This will allow the model to communicate with the database.
        private readonly ApplicationDbContext _context;
        //Constructor
        public LeaveTypesController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Dependency injection end

        //The start of the actions
        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            //"Return a view having made the call to the database". The await is necessary because it is an Async function
            //:Go to database, Got to LeaveTypes table and everything that is in it convert it to a list"
            //In SQL: Select * from LeaveTypes
              return _context.LeaveTypes != null ? 
                          View(await _context.LeaveTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.LeaveTypes'  is null.");
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //If we could not find the record. You know.
            if (id == null || _context.LeaveTypes == null)
            {
                return NotFound();
            }

            //Find the first record that matches the given ID
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            //If the query came back empty, say the record was not found
            if (leaveType == null)
            {
                return NotFound();
            }
            //Otherwise if nothing has gone bad, return the view with the found record from the database
            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        //This one will just create a form (I'm assuming we go make it in the view file)
        public IActionResult Create()
        {
            return View();
        }

        //Second create that takes the data from the form
        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Bind each field coming from the data form to the matching fields in the model and storing it in the object of LeaveType
        public async Task<IActionResult> Create([Bind("Name,DefaultDays,Id,DateCreated,DateModified")] LeaveType leaveType)
        {
            if (ModelState.IsValid)
            {
                //State the type of modifications
                _context.Add(leaveType);
                //Save the changes
                await _context.SaveChangesAsync();
                //Afterwards, send us back to the index page (Index action)
                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        // GET: LeaveTypes/Edit/5
        //Will look very identical to details. Must first find record with the matching ID
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LeaveTypes == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,DefaultDays,Id,DateCreated,DateModified")] LeaveType leaveType)
        {
            if (id != leaveType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveType);
                    await _context.SaveChangesAsync();
                }
                //Example: 2 people are editing the same record at the same time
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Take us back to the Index page
                return RedirectToAction(nameof(Index));
            }
            return View(leaveType);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LeaveTypes == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        //Make sure no one is trying to spoof a delete request
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LeaveTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LeaveTypes'  is null.");
            }
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Check if a certain leaveType exists
        private bool LeaveTypeExists(int id)
        {
          return (_context.LeaveTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //Chain "database.table.whatdoiwant"... _context.LeaveTypes.Any for example
    }
}
