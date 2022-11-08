using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shopping.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        //Creo una propiedad que estabece el mombre, agrego requiered pra establecer los requisito de esta propiedad mediante los dataannotations
        [Display(Name = "Departamento/Estado")]
        //especifica en erro al usuario en español, el 1 se reemplaza por los caracteres y el cero por el nombre del país
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //El cero se reeemplaza por lo que tenga en la propiedad Name
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        //Aca especificamos la relación de uno a varios, la relación de un estado con un país
        public Country Country { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
