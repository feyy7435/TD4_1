using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using TD4.Controllers;
using TD4.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using TD4.Controllers;
using TD4.Models.EntityFramework;
using TD4.Models.Repository;

[TestClass]
public class UtilisateursControllerTests
{
    private FilmRatingsDBContext _context;
    private IDataRepository<Utilisateur> _dataRepository;
    private UtilisateursController _controller;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<FilmRatingsDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new FilmRatingsDBContext(options);
        _dataRepository = new UtilisateurManager(_context);
        _controller = new UtilisateursController(_dataRepository);

        _context.Utilisateurs.Add(new Utilisateur { UtilisateurId = 1, Nom = "Test", Prenom = "User", Mail = "test@gmail.com" });
        _context.SaveChanges();
    }
    [TestMethod]
    public void GetUtilisateur_ValidId_ShouldReturnUser()
    {
        var result = _controller.GetUtilisateur(1).Result;
        Assert.IsNotNull(result.Value);

    }
    [TestMethod]
    public void PostUtilisateur_ShouldCreateUser()
    {
        Utilisateur newUser = new Utilisateur { Nom = "New", Prenom = "User", Mail = "new@gmail.com" };
        _controller.PostUtilisateur(newUser);
        Assert.IsNotNull(_context.Utilisateurs.FirstOrDefault(u => u.Mail == "new@gmail.com"));
    }
    [TestMethod]
    public void DeleteUtilisateur_ValidId_ShouldDeleteUser()
    {
        _controller.DeleteUtilisateur(1);
        Assert.IsNull(_context.Utilisateurs.Find(1));
    }


    //[TestCleanup]
    //public void Cleanup()
    //{
    //    _context.Database.EnsureDeleted();
    //    _context.Dispose();
    //}

    //// 1. Test de GetAll()
    //[TestMethod]
    //public void GetAll_ShouldReturnAllUsers()
    //{
    //    var result = _controller.GetAll().Result;
    //    Assert.AreEqual(2, result.Value.Count());
    //}

    //// 2. Test de GetById()
    //[TestMethod]
    //public void GetById_ValidId_ShouldReturnUser()
    //{
    //    var result = _controller.GetById(1).Result;
    //    Assert.IsNotNull(result.Value);
    //    Assert.AreEqual("Jean", result.Value.Prenom);
    //}

    //[TestMethod]
    //public void GetById_InvalidId_ShouldReturnNull()
    //{
    //    var result = _controller.GetById(99).Result;
    //    Assert.IsNull(result.Value);
    //}

    //// 3. Test de GetByEmail()
    //[TestMethod]
    //public void GetByEmail_ValidEmail_ShouldReturnUser()
    //{
    //    var result = _controller.GetByEmail("jean.dupont@gmail.com").Result;
    //    Assert.IsNotNull(result.Value);
    //    Assert.AreEqual("Jean", result.Value.Prenom);
    //}

    //[TestMethod]
    //public void GetByEmail_InvalidEmail_ShouldReturnNull()
    //{
    //    var result = _controller.GetByEmail("notfound@gmail.com").Result;
    //    Assert.IsNull(result.Value);
    //}

    //// 4. Test de PostUtilisateur()
    //[TestMethod]
    //public void PostUtilisateur_ValidUser_ShouldCreateUser()
    //{
    //    Random rnd = new Random();
    //    string uniqueMail = "newuser" + rnd.Next(1, 1000000) + "@gmail.com";

    //    Utilisateur newUser = new Utilisateur
    //    {
    //        Nom = "Test",
    //        Prenom = "User",
    //        Mail = uniqueMail,
    //        Pwd = "Test1234!",
    //        Mobile = "0601020304"
    //    };

    //    var result = _controller.PostUtilisateur(newUser).Result;
    //    var userInDb = _context.Utilisateurs.FirstOrDefault(u => u.Mail == uniqueMail);
    //    Assert.IsNotNull(userInDb);
    //}

    //[TestMethod]
    //[ExpectedException(typeof(System.AggregateException))]
    //public void PostUtilisateur_DuplicateEmail_ShouldThrowException()
    //{
    //    Utilisateur newUser = new Utilisateur
    //    {
    //        Nom = "Jean",
    //        Prenom = "Dupont",
    //        Mail = "jean.dupont@gmail.com",
    //        Pwd = "Test1234!",
    //        Mobile = "0601020304"
    //    };

    //    var result = _controller.PostUtilisateur(newUser).Result;
    //}

    //// 5. Test de DeleteUtilisateur()
    //[TestMethod]
    //public void DeleteUtilisateur_ValidId_ShouldDeleteUser()
    //{
    //    var user = new Utilisateur
    //    {
    //        Nom = "ToDelete",
    //        Prenom = "User",
    //        Mail = "todelete@gmail.com",
    //        Pwd = "Test1234!"
    //    };

    //    _context.Utilisateurs.Add(user);
    //    _context.SaveChanges();

    //    int userId = user.UtilisateurId;
    //    var result = _controller.DeleteUtilisateur(userId).Result;
    //    var userInDb = _context.Utilisateurs.Find(userId);

    //    Assert.IsNull(userInDb);
    //}
}
