using BankMVC.Assemblers;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.Controllers
{
    public class DocumentController : Controller
    {// GET: User
        private readonly IDocumentService _documentService;
        private readonly DocumentAssembler _documentAssembler;

        public DocumentController(IDocumentService documentService, DocumentAssembler documentAssembler)
        {
            _documentService = documentService;
            _documentAssembler = documentAssembler;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DocumentVM documentVM)
        {
            var document = _documentAssembler.ConvertToModel(documentVM);
            var newDocument = _documentService.Add(document);
            ViewBag.Message = "Added Successfully";
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var documentData = _documentService.GetById(id);
            var documentDataVM = _documentAssembler.ConvertToViewModel(documentData);
            return View(documentDataVM);
        }

        [HttpPost]
        public ActionResult Edit(DocumentVM documentVM)
        {
            var document = _documentService.GetById(documentVM.Id);
            if (document != null)
            {
                _documentService.Update(document);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var documentData = _documentService.GetById(id);
            var documentDataVM = _documentAssembler.ConvertToViewModel(documentData);
            return View(documentDataVM);
        }

        [HttpPost]
        public ActionResult Delete(DocumentVM documentVM)
        {
            var document = _documentService.GetById(documentVM.Id);
            if (document != null)
            {
                _documentService.Delete(document);
            }
            return RedirectToAction("Index");
        }
    }
}
