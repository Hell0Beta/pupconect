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
    public class EditPetModel : PageModel
    {
        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            Search(HttpContext.Session.GetString("Pet") ?? "well", Username);
        }

        public string Username { get; set; }
        private readonly ILogger<IndexModel> _logger;
        [BindProperty]
        public EditPetForm EditPet { get; set; }
        public EditPetModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPostEditPet()
        {
            string PETS = HttpContext.Session.GetString("Pet") ?? "Well";
            DeletePet(PETS);
            if(EditPet.Name == null){
                System.Console.WriteLine("null");
                EditPet.Name = HttpContext.Session.GetString("Username");

            }
            
            var RegPet = GetPetsFromFile();
            RegPet.Add(EditPet);
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "DataPet")); // Ensure 'Data' folder exists                                                                                                
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json"), JsonSerializer.Serialize(RegPet));
            return RedirectToPage("/UserDashboard");
        }
        public void DeletePet(string petName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json");

            var jsonData = System.IO.File.ReadAllText(filePath);
            var pets = JsonSerializer.Deserialize<List<EditPetForm>>(jsonData);

            var petToRemove = pets.FirstOrDefault(p => p.PetName == petName);



            pets.Remove(petToRemove);

            var updatedJsonData = JsonSerializer.Serialize(pets, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, updatedJsonData);

            TempData["Message"] = $"Goodbye, {petName}! We hope you find a loving new home. Don't forget to write!";


        }
        public List<EditPetForm> GetPetsFromFile()
        {
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json")))
                return new List<EditPetForm>();

            var json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json"));
            return JsonSerializer.Deserialize<List<EditPetForm>>(json);
        }

        public void Search(string usersearch, string name)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json");
            if (System.IO.File.Exists(FilePath))
            {
                var jsonData = System.IO.File.ReadAllText(FilePath);
                var jsonObj = JsonSerializer.Deserialize<List<EditPetForm>>(jsonData);
                System.Console.WriteLine(jsonObj[1]);
                var searchresult = jsonObj?.Where(pet =>

                    (pet.PetName?.Contains(usersearch, StringComparison.OrdinalIgnoreCase) ?? false) &
                    (pet.Name?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

                ViewData["EditPet"] = searchresult;
                System.Console.WriteLine(ViewData["EditPet"]);
            }
            else
            {
                ViewData["EditPet"] = new List<EditPetForm>();
            }
        }

    }
    public class EditPetForm
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


