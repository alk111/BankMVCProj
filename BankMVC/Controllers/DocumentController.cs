﻿using BankMVC.Assemblers;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.Controllers
{
    [AllowAnonymous]
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
        public ActionResult Index()
        {
            var documents = _documentService.GetAll();
            var documentVMs = documents.Select(d => _documentAssembler.ConvertToViewModel(d)).ToList();
            return View(documentVMs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DocumentVM documentVM ,HttpPostedFileBase file)
        {
            documentVM.PostedFile = file;
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
        [HttpGet]
        public ActionResult ViewDocument(int id, string fileName)
        {
            // Retrieve the document based on the ID
            var document = _documentService.GetById(id);

            if (document != null)
            {
                // Set the Content-Disposition header to "inline" to instruct the browser to display the content
                Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);

                // Set the content type appropriately based on your document type
                Response.ContentType = "application/pdf"; // Set to "application/pdf" for PDF documents

                // Write the document content to the response stream
                return File(document.DocumentFile, "application/pdf",fileName);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult Verify(int id)
        {
            var document = _documentService.GetById(id);
            if (document != null)
            {
                document.IsVerified = true;
                _documentService.Update(document);
            }
            return RedirectToAction("Index");
        }

    }
}
