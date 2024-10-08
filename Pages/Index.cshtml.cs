using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using pupconect.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;


namespace pupconect;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public List<ReportPetForm> Dogs { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        Dogs = new List<ReportPetForm>();
        Dogs = GetLostFromFile();

    }




    [BindProperty]
    public LoginForm Login { get; set; }


    private readonly string jsonFilePath;

    [BindProperty]
    public RegisterPetForm RegisterPet { get; set; }

    [BindProperty]
    public ReportPetForm ReportPet { get; set; }

    public string Message { get; set; }



    public void OnGet()
    {
        Dogs = GetLostFromFile();
    }


    public IActionResult OnPostSearch(string usersearch)
    {
        var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "LostPet", "lost.json");
        if (System.IO.File.Exists(FilePath))
        {
            var jsonData = System.IO.File.ReadAllText(FilePath);
            var jsonObj = JsonSerializer.Deserialize<List<ReportPetForm>>(jsonData);

            var searchresult = jsonObj?.Where(pet =>
                (pet.Breed?.Contains(usersearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (pet.Location?.Contains(usersearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (pet.ContactInfo?.Contains(usersearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (pet.AdditionalInfo?.Contains(usersearch, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();

            ViewData["results"] = searchresult ?? new List<ReportPetForm>();
        }
        else
        {
            ViewData["results"] = new List<ReportPetForm>();
        }

        return Page();
    }
    
    public IActionResult OnPostLogin()
    {
        var users = GetUsersFromFile();
        users.Add(Login);
        // {
        //     PetName = RegisterPet.PetName,
        //     Breed = RegisterPet.Breed,
        //     Age = RegisterPet.Age,
        //     Description = RegisterPet.Description
        // };
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "loginData.json");


        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data")); // Ensure 'Data' folder exists
        System.IO.File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(users));
        return RedirectToPage("/Index");

        if (!ModelState.IsValid)
        {
            return Page();
        }
    }

    private List<LoginForm> GetUsersFromFile()
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "loginData.json");

        if (!System.IO.File.Exists(jsonFilePath))
            return new List<LoginForm>();

        var json = System.IO.File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<LoginForm>>(json);
    }

    public IActionResult OnPostRegisterPet()
    {
        var RegPet = GetPetsFromFile();
        RegPet.Add(RegisterPet);
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "DataPet")); // Ensure 'Data' folder exists

        System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json"), JsonSerializer.Serialize(RegPet));
        return RedirectToPage("/Index");


        Message = $"Pet {RegisterPet.PetName} registered successfully";
        return Page();
    }

    public List<RegisterPetForm> GetPetsFromFile()
    {
        if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json")))
            return new List<RegisterPetForm>();

        var json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json"));
        return JsonSerializer.Deserialize<List<RegisterPetForm>>(json);
    }

    public IActionResult OnPostReportPet()
    {
        var repPet = GetLostFromFile();
        repPet.Add(ReportPet);
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "LostPet"));

        System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "LostPet", "Lost.json"), JsonSerializer.Serialize(repPet));
        return RedirectToPage("/Index");


        // TODO: Add actual pet reporting logic here
        Message = $"Pet report submitted for {ReportPet.ReportType}";
        return Page();
    }

    public List<ReportPetForm> GetLostFromFile()
    {

        if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "LostPet", "lost.json")))
            return new List<ReportPetForm>();

        var json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "LostPet", "lost.json"));
        return JsonSerializer.Deserialize<List<ReportPetForm>>(json);

    }
}

public class LoginForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class RegisterPetForm
{
    [Required]
    public string PetName { get; set; }

    [Required]
    public string Breed { get; set; }

    [Required]
    [Range(0, 30)]
    public int Age { get; set; }

    public string Description { get; set; }
}

public class ReportPetForm
{
    [Required]
    public string ReportType { get; set; }

    [Required]
    public string Breed { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public string Date { get; set; }

    [Required]
    [Phone]
    public string ContactInfo { get; set; }

    public string AdditionalInfo { get; set; }
}
