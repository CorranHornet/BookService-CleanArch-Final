using System;
using System.ComponentModel.DataAnnotations;

namespace BookService.Api.DTOs
{
    public class MediaUnitUpdateDTO
    {
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int? DurationMinutes { get; set; }
    }
}