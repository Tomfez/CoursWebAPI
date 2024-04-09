using Microsoft.AspNetCore.Identity;

namespace HoteIdentityServer.Models
{
    // Classe qui permet de rajouter des champs sur les utilisateurs. Il faut impérativement dériver de 'IdentityUser'.
    public class ApplicationUser : IdentityUser<int>
    {
        public string Fonction { get; set; } = string.Empty;
    }
}
