using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeitourApi.Models;

[Table("tbl_usuario")]
public class User : BaseModel
{
    [Key]
    [Column("pk_usuario_id",TypeName = "int")]
    [Required]
    public int Id { get; set; }

    [Column("usuario_nome",TypeName = "varchar(30)")]
    [Required]
    public required string NameUser { get; set;}

    [Column("usuario_email",TypeName = "varchar(100)")]
    [Required]
    public required string Email { get; set; } = null!;

    [Column("usuario_senha",TypeName = "varchar(64)")]
    [Required]
    public required string Password { get; set; } = null!;
    
    [Column("usuario_biografia",TypeName = "varchar(300)")]
    public string? BioUser { get; set;}

    [Column("usuario_foto_perfil")]
    public string? ProfilePhoto { get; set; }

    [Column("usuario_acesso", TypeName="Enum")]
    public string Access {get; set;} = null!;
}