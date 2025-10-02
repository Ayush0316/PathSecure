using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class SigningKey : BaseEntity
{
    [Required]
    public string Kid { get; set; }           // kid

    [Required]
    public string Algorithm { get; set; } = "RS256";

    [Required]
    public string PublicKeyPem { get; set; } = null!;    // public key (PEM or serialized)

    [Required]
    public string PrivateKeyEncrypted { get; set; } = null!; // encrypted private key storage

    public bool IsActive { get; set; } = false;          // only one active for signing
    public DateTime? RetiredAt { get; set; }             // when removed from active signing
}
