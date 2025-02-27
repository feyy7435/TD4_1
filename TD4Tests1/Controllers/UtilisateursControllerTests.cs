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
using Humanizer;
using Microsoft.AspNetCore.Mvc;

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
    [TestMethod]
    public void Postutilisateur_ModelValidated_CreationOK()
    {
        // Arrange
        Random rnd = new Random();
        int chiffre = rnd.Next(1, 1000000000);
        // Le mail doit être unique donc 2 possibilités :
        // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
        // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
         Utilisateur userAtester = new Utilisateur()
         {
             Nom = "MACHIN",
             Prenom = "Luc",
             Mobile = "0606070809",
             Mail = "machin" + chiffre + "@gmail.com",
             Pwd = "Toto1234!",
             Rue = "Chemin de Bellevue",
             CodePostal = "74940",
             Ville = "Annecy-le-Vieux",
             Pays = "France",
             Latitude = null,
             Longitude = null
         };
        // Act
        var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
 // Assert
        Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault(); 
      //  On récupère l'utilisateur créé directement dans la BD grace à son mail unique
 // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
 // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
        userAtester.UtilisateurId = userRecupere.UtilisateurId;
        Assert.AreEqual(userAtester, userRecupere, "Utilisateurs pas identiques");
    }


    [TestMethod]
    public void Postutilisateur_ModelValidated_CreationOK_AvecMoq()
    {
        // Arrange
        var mockRepository = new Mock<IDataRepository<Utilisateur>>();
        var userController = new UtilisateursController(mockRepository.Object);
        Utilisateur user = new Utilisateur
        {
            Nom = "POISSON",
            Prenom = "Pascal",
            Mobile = "1",
            Mail = "poisson@gmail.com",
            Pwd = "Toto12345678!",
            Rue = "Chemin de Bellevue",
            CodePostal = "74940",
            Ville = "Annecy-le-Vieux",
            Pays = "France",
            Latitude = null,
            Longitude = null
        };
        // Act
        var actionResult = userController.PostUtilisateur(user).Result;
        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
        Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
        var result = actionResult.Result as CreatedAtActionResult;
        Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
        user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
        Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
    }

    [TestMethod]
    public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
    {
        // Arrange
        Utilisateur user = new Utilisateur
        {
            UtilisateurId = 1,
            Nom = "Calida",
            Prenom = "Lilley",
            Mobile = "0653930778",
            Mail = "clilleymd@last.fm",
            Pwd = "Toto12345678!",
            Rue = "Impasse des bergeronnettes",
            CodePostal = "74200",
            Ville = "Allinges",
            Pays = "France",
            Latitude = 46.344795F,
            Longitude = 6.4885845F
        };
        var mockRepository = new Mock<IDataRepository<Utilisateur>>();
        mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
        var userController = new UtilisateursController(mockRepository.Object);
        // Act
        var actionResult = userController.GetUtilisateurById(1).Result;
        // Assert
        Assert.IsNotNull(actionResult);
        Assert.IsNotNull(actionResult.Value);
        Assert.AreEqual(user, actionResult.Value as Utilisateur);
    }

    [TestMethod]
    public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
    {
        var mockRepository = new Mock<IDataRepository<Utilisateur>>();
        var userController = new UtilisateursController(mockRepository.Object);
        // Act
        var actionResult = userController.GetUtilisateurById(0).Result;
        // Assert
        Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
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
