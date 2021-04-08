using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserGroup.Web.ViewModels;

namespace UserGroup.Web.Controllers
{
    public class EventsController : Controller
    {
        public static List<EventViewModel> Events = new List<EventViewModel>{
            new EventViewModel {Id = 0, Title = "First Presentation", Description = "Simple Description", Date = DateTime.Now.AddMonths(-2), Location="IntelliTect Office", SpeakerId = 0},
            new EventViewModel {Id = 1, Title = "Second Presentation", Description = "Another simple description", Date = DateTime.Now.AddMonths(-1), Location="IntelliTect Office", SpeakerId = 1},
        };

        public IActionResult Index()
        {
            return View(Events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Events.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            return View(Events[id]);
        }

        [HttpPost]
        public IActionResult Edit(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Events[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Events.RemoveAt(id);
            return RedirectToAction(nameof(Index));
        }
    }
}