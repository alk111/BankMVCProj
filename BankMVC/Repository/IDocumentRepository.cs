using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public interface IDocumentRepository
    {
        string Add(Document document);
        string Update(Document document);
        string Delete(Document document);
        Document GetById(int documentId);
        List<Document> GetAll();
    }
}