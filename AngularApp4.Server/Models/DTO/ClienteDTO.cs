namespace AngularApp4.Server.Models.DTO
{
    public class ClienteDTO
    {

        public int Id { get; set; }

        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }
        public string Pais { get; set; }

        public int PaisId {  get; set; }
        public int? TotalRecords { get; set; }
    }
}
