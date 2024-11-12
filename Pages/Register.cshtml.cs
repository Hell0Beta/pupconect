using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using pupconect.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace pupconect.Pages
{
    public class RegisterModel : PageModel
    {
        public string Username { get; set; }

        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public RegisterPetForm RegisterPet { get; set; }

        public RegisterModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

        }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
           
        }

        public IActionResult OnPostRegisterPet()
        {
            RegisterPet.Name = HttpContext.Session.GetString("Username") ?? "Well";
            var RegPet = GetPetsFromFile();
            RegPet.Add(RegisterPet);
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "DataPet")); // Ensure 'Data' folder exists
            // HttpContext.Session.SetString("Dogname", RegPet.Name);
            // HttpContext.Session.SetString("Dogname", RegPet.Name);
            // HttpContext.Session.SetString("Dogname", RegPet.Name);
            // HttpContext.Session.SetString("Dogname", RegPet.Name);
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json"), JsonSerializer.Serialize(RegPet));
            return RedirectToPage("/UserDashboard");


        }


        
        public List<RegisterPetForm> GetPetsFromFile()
        {
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json")))
                return new List<RegisterPetForm>();

            var json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json"));
            return JsonSerializer.Deserialize<List<RegisterPetForm>>(json);
        }


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
        public string Name { get; set; }
    }
}