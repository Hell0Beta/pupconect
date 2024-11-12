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
    public class UserDashboardModel : PageModel
    {
        public string Username { get; set; }

        [BindProperty]
        public UserPets Pet { get; set; } = new UserPets();

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username") ?? "You Have No Registered Yet";
            Search(Username);
        }

        public void Search(string usersearch)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataPet", "pet.json");
            if (System.IO.File.Exists(FilePath))
            {
                var jsonData = System.IO.File.ReadAllText(FilePath);
                var jsonObj = JsonSerializer.Deserialize<List<UserPets>>(jsonData);

                var searchresult = jsonObj?.Where(pet =>
                    pet.Name?.Contains(usersearch, StringComparison.OrdinalIgnoreCase) ?? false
                ).ToList();
                
                ViewData["Userresults"] = searchresult ?? new List<UserPets>();
            }
            else
            {
                ViewData["Userresults"] = new List<UserPets>();
            }


        }

        public IActionResult OnPostEdit()
        {
            System.Console.WriteLine(Pet.PetName ?? "hmmm");
            HttpContext.Session.SetString("Pet", Pet.PetName);
            return RedirectToPage("/EditPet");
        }
    }
    public class UserPets
    {
        public string? Name { get; set; }

        public string PetName { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Description { get; set; }
    }
}
