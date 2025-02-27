using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TD4.Models.EntityFramework;
using TD4.Models.Repository;

public class UtilisateurManager : IDataRepository<Utilisateur>
{
    private readonly FilmRatingsDBContext _context;

    public UtilisateurManager(FilmRatingsDBContext context)
    {
        _context = context;
    }

    public ActionResult<IEnumerable<Utilisateur>> GetAll()
    {
        return _context.Utilisateurs.ToList();
    }

    public ActionResult<Utilisateur> GetById(int id)
    {
        return _context.Utilisateurs.FirstOrDefault(u => u.UtilisateurId == id);
    }

    public ActionResult<Utilisateur> GetByString(string mail)
    {
        return _context.Utilisateurs.FirstOrDefault(u => u.Mail.ToUpper() == mail.ToUpper());
    }

    public void Add(Utilisateur entity)
    {
        _context.Utilisateurs.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Utilisateur utilisateur, Utilisateur entity)
    {
        _context.Entry(utilisateur).State = EntityState.Modified;
        utilisateur.Nom = entity.Nom;
        utilisateur.Prenom = entity.Prenom;
        utilisateur.Mail = entity.Mail;
        utilisateur.Pwd = entity.Pwd;
        utilisateur.Mobile = entity.Mobile;
        _context.SaveChanges();
    }

    public void Delete(Utilisateur utilisateur)
    {
        _context.Utilisateurs.Remove(utilisateur);
        _context.SaveChanges();
    }
}
