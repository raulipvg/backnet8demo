using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AngularApp4.Server.Models
{
    public class Pais
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Solo se permiten letras")]
        public string Nombre { get; set; }

       
  

    }
}
