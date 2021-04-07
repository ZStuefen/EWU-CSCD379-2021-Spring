using Microsoft.AspNetCore.Mvc;
using UserGroup.Web.ViewModels;
using UserGroup.Web.Data;

namespace UserGroup.Web.Controllers
{
    public class SpeakersController : Controller
    {
        public IActionResult Index()
        {
            return View(MockData.Speakers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SpeakerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MockData.Speakers.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            return View(MockData.Speakers[id]);
        }

        [HttpPost]
        public IActionResult Edit(SpeakerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MockData.Speakers[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
    }
}