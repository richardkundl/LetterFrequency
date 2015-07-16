using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using LetterFrequency.Web.Helper;
using LetterFrequency.Web.Models;

namespace LetterFrequency.Web.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(TextModel model)
		{
			model.Language = Letter.Process(model.Text);

			return View(model);
		}
	}
}