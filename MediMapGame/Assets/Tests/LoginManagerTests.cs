using NUnit.Framework;
using UnityEngine;

public class LoginManagerTests
{
    private LoginManager loginManager;

    [SetUp]
    public void Voorbereiding()
    {
        // Maak een nieuw LoginManager object voor elke test
        GameObject loginManagerObject = new GameObject("LoginManager");
        loginManager = loginManagerObject.AddComponent<LoginManager>();
    }

    [TearDown]
    public void Opruimen()
    {
        // Ruim op na elke test
        Object.DestroyImmediate(loginManager.gameObject);
    }

    // Test 1: Geldige inloggegevens
    [Test]
    public void ValideerInloggegevens_GeldigeInvoer_GeenFoutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker123";
        string wachtwoord = "GeldigWachtwoord1!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.IsNull(resultaat); // Geen foutmelding betekent dat de gegevens geldig zijn
    }

    // Test 2: Lege gebruikersnaam
    [Test]
    public void ValideerInloggegevens_LegeGebruikersnaam_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "";
        string wachtwoord = "GeldigWachtwoord1!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Gebruikersnaam mag niet leeg zijn.", resultaat);
    }

    // Test 3: Gebruikersnaam met speciale tekens
    [Test]
    public void ValideerInloggegevens_GebruikersnaamMetSpecialeTekens_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "test@gebruiker";
        string wachtwoord = "GeldigWachtwoord1!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Gebruikersnaam mag alleen letters en cijfers bevatten.", resultaat);
    }

    // Test 4: Wachtwoord te kort
    [Test]
    public void ValideerInloggegevens_WachtwoordTeKort_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "Kort1!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Wachtwoord moet minimaal 10 tekens lang zijn.", resultaat);
    }

    // Test 5: Wachtwoord zonder hoofdletter
    [Test]
    public void ValideerInloggegevens_WachtwoordZonderHoofdletter_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "geenhoofdletter1!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Wachtwoord moet minimaal één hoofdletter bevatten.", resultaat);
    }

    // Test 6: Wachtwoord zonder kleine letter
    [Test]
    public void ValideerInloggegevens_WachtwoordZonderKleineLetter_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "ALLESHOOFDLETTERS1!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Wachtwoord moet minimaal één kleine letter bevatten.", resultaat);
    }

    // Test 7: Wachtwoord zonder cijfer
    [Test]
    public void ValideerInloggegevens_WachtwoordZonderCijfer_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "GeenCijferHier!";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Wachtwoord moet minimaal één cijfer bevatten.", resultaat);
    }

    // Test 8: Wachtwoord zonder speciaal teken
    [Test]
    public void ValideerInloggegevens_WachtwoordZonderSpeciaalTeken_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "GeenSpeciaalTeken1";
        string email = "test@voorbeeld.nl";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Wachtwoord moet minimaal één speciaal teken bevatten.", resultaat);
    }

    // Test 9: Lege e-mail (voor registratie)
    [Test]
    public void ValideerInloggegevens_LegeEmail_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "GeldigWachtwoord1!";
        string email = "";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Ongeldig e-mailformaat.", resultaat);
    }

    // Test 10: Ongeldig e-mailformaat
    [Test]
    public void ValideerInloggegevens_OngeldigEmailFormaat_Foutmelding()
    {
        // Arrange
        string gebruikersnaam = "testgebruiker";
        string wachtwoord = "GeldigWachtwoord1!";
        string email = "ongeldig-email";

        // Act
        string resultaat = loginManager.ValidateCredentials(gebruikersnaam, wachtwoord, email, isRegistering: true);

        // Assert
        Assert.AreEqual("Ongeldig e-mailformaat.", resultaat);
    }
}