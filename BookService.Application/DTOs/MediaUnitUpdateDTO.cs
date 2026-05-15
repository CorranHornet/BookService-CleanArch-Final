using System;
using System.ComponentModel.DataAnnotations;

namespace BookService.Application.DTOs
{
    public class MediaUnitUpdateDTO
    {
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int? PageCount { get; set; }
        public int? DurationMinutes { get; set; }
    }
}