using Microsoft.AspNetCore.Mvc;

namespace JournalAPI.Models.ViewModels
{
    public class SemesterViewModel
    {
        public int Year { get; set; }
        public Season Season {get;set;}
    }
}
