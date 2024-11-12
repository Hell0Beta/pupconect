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
    public class LostPetModel : PageModel
    {
        public List<ReportPetForm> Dog { get; set; }
        public ReportPetForm ReportPet { get; set; }
        public void OnGet()
        {

        }
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
}