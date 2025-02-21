﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionAcademica.Data;
using GestionAcademica.Models;

namespace GestionAcademica.Controllers
{
    public class HorariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HorariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Horarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Horarios.Include(h => h.Alumno).Include(h => h.Curso).Include(h => h.Profesor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Horarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.Alumno)
                .Include(h => h.Curso)
                .Include(h => h.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // GET: Horarios/Create
        public IActionResult Create()
        {
            ViewBag.Alumnos = new SelectList(_context.Alumnos.ToList(), "Id", "Nombre");
            //ViewData["AlumnoID"] = new SelectList(_context.Alumnos, "Id", "Alumno");
            ViewBag.Cursos = new SelectList(_context.Cursos.ToList(), "Id", "Nombre");
            //ViewData["CursoID"] = new SelectList(_context.Cursos, "Id", "Id");
            ViewData["ProfesorID"] = new SelectList(_context.Profesores, "Id", "Id");
            ViewBag.Profesores = new SelectList(_context.Profesores.ToList(), "Id", "Nombre");
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HoraInicio,HoraFinal,Dia,CursoID,ProfesorID,AlumnoID")] Horario horario)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Alumnos = new SelectList(_context.Alumnos.ToList(), "Id", "Nombre");
            //ViewData["AlumnoID"] = new SelectList(_context.Alumnos, "Id", "Nombre", horario.AlumnoID);
            ViewBag.Cursos = new SelectList(_context.Cursos.ToList(), "Id", "Nombre");
            //ViewData["CursoID"] = new SelectList(_context.Cursos, "Id", "Id", horario.CursoID);
            ViewBag.Profesores = new SelectList(_context.Profesores.ToList(), "Id", "Nombre");
            //ViewData["ProfesorID"] = new SelectList(_context.Profesores, "Id", "Id", horario.ProfesorID);
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            ViewData["AlumnoID"] = new SelectList(_context.Alumnos, "Id", "Id", horario.AlumnoID);
            ViewData["CursoID"] = new SelectList(_context.Cursos, "Id", "Id", horario.CursoID);
            ViewData["ProfesorID"] = new SelectList(_context.Profesores, "Id", "Id", horario.ProfesorID);
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HoraInicio,HoraFinal,Dia,CursoID,ProfesorID,AlumnoID")] Horario horario)
        {
            if (id != horario.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlumnoID"] = new SelectList(_context.Alumnos, "Id", "Id", horario.AlumnoID);
            ViewData["CursoID"] = new SelectList(_context.Cursos, "Id", "Id", horario.CursoID);
            ViewData["ProfesorID"] = new SelectList(_context.Profesores, "Id", "Id", horario.ProfesorID);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.Alumno)
                .Include(h => h.Curso)
                .Include(h => h.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                _context.Horarios.Remove(horario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.Id == id);
        }
    }
}
