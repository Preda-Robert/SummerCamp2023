using SummerCamp.DataModels.Models;
using SummerCamp.Enums;
using System.ComponentModel.DataAnnotations;

namespace SummerCamp.Models
{
    public class PlayerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati un nume!")]
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Address { get; set; }
        [EnumDataType(typeof(PositionEnum))]

        public PositionEnum? Position { get; set; }

        public int? ShirtNumber { get; set; }


        public int? TeamId { get; set; }
        public virtual Team? Team { get; set; }

    }
}
